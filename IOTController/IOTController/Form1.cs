using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using LBSoft.IndustrialCtrls;
using LBSoft.IndustrialCtrls.Meters;
using LBSoft.IndustrialCtrls.Utils;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using System.Globalization;
using System.Text.RegularExpressions;
namespace IOTController
{
    public partial class Form1 : Form
    {

        Thread th, autodisconnectorthread;
        volatile bool serviceRunning = false;
        volatile bool autoRunning = false;
        Db obj;
        MqttClient client = null;
        string clientId;
        string mqttserverip = "";
        string mqttserverfallbackip = "";
        int mqttport = 1883;
        string mqttSubTopic = "";
        string mqttPubTopic = "";
        string mqttStartTopic = "energy/control/start";
        string mqttStartPayloadTopic = "energy/control/start";
        string controlMeterNo = "7700";
        double eVal = 0; // live electricity-consumption value
        double current = 0;
        readonly object valueLock = new object();
        bool autoStartOnLoad = false;
        bool enableMockMqtt = false;
        string mockMeterNo = "";
        double mockRawValue = 650;
        int mockIntervalMs = 3000;
        bool serviceStarted = false;
        bool? lastAutoDisconnectState = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            StopThreads();

            Application.ExitThread();
        }

        //dip2_espsend
        //dip2_esprecv

        private void BtnStartStop_Click(object sender, EventArgs e)
        {
            if (BtnStartStop.Text.StartsWith("Start"))
            {
                StartService();
            }
            else
            {
                StopService();
            }
        }

        // this code runs when a message was received
        void client_MqttMsgPublishReceived(object sender, MqttMsgPublishEventArgs e)
        {
            string ReceivedMessage = Encoding.UTF8.GetString(e.Message);
            try
            {
                Logger.Info("MQTT RX [" + e.Topic + "] " + ReceivedMessage);
                ProcessIncomingPayload(e.Topic, ReceivedMessage);
            }
            catch (Exception err)
            {
                Logger.Error("MQTT message handler failed", err);
            }
           // obj.insert(ReceivedMessage);
        }

        private void AutoDisconnectLoop()
        {
            while (autoRunning)
            {
                try
                {
                    auto();
                }
                catch (Exception ex)
                {
                    Logger.Error("AutoDisconnectLoop failed", ex);
                }
                Thread.Sleep(10000);
            }
        }
 

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                mqttserverip = System.Configuration.ConfigurationManager.AppSettings["MqttServer"] ?? "broker.emqx.io";
                mqttserverfallbackip = System.Configuration.ConfigurationManager.AppSettings["MqttServerFallbackIp"] ?? "";
                mqttport = ReadIntSetting("MqttPort", 1883);
                mqttSubTopic = System.Configuration.ConfigurationManager.AppSettings["MqttSubscribeTopic"] ?? "aebpublish2";
                mqttPubTopic = System.Configuration.ConfigurationManager.AppSettings["MqttPublishTopic"] ?? "aebsubscribe2";
                mqttStartTopic = System.Configuration.ConfigurationManager.AppSettings["MqttStartTopic"] ?? "energy/control/start";
                mqttStartPayloadTopic = mqttStartTopic;
                controlMeterNo = System.Configuration.ConfigurationManager.AppSettings["ControlMeterNo"] ?? "7700";
                autoStartOnLoad = false; // Demo mode: always start manually from UI button.
                enableMockMqtt = ReadBoolSetting("EnableMockMqtt", false);
                mockMeterNo = System.Configuration.ConfigurationManager.AppSettings["MockMeterNo"] ?? "";
                mockRawValue = ReadDoubleSetting("MockRawValue", 650);
                mockIntervalMs = ReadIntSetting("MockIntervalMs", 3000);

                obj = new Db();
                DataTable buffer = obj.GetCustomers();
                if (buffer.Rows.Count != 0)
                {
                    DrpCustomerList.DataSource = buffer;
                    DrpCustomerList.DisplayMember = "cName";
                    DrpCustomerList.ValueMember = "cId";
                }
                else
                {
                    BtnStartStop.Enabled = false;
                    MessageBox.Show("No customers found in database. Add at least one customer first.", "IOT Controller", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }

                // Keep controller idle on launch for clean demo sequence.
                BtnStartStop.Text = "Start service";
                serviceRunning = false;
                autoRunning = false;
                serviceStarted = false;
                lastAutoDisconnectState = null;

                // Connect MQTT on launch, but keep automation idle until Start Service is clicked.
                EnsureMqttConnectedIdle();
            }
            catch (Exception ex)
            {
                Logger.Error("Form1_Load failed", ex);
                BtnStartStop.Enabled = false;
                BtnGenerate.Enabled = false;
                BtnService.Enabled = false;
                MessageBox.Show("Startup failed: " + ex.Message + "\n\nCheck IOTController.log in the EXE folder.", "IOT Controller", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnGenerate_Click(object sender, EventArgs e)
        {
            int cid;
            if (!TryGetSelectedCustomerId(out cid))
            {
                MessageBox.Show("Select a customer first.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool ok = obj.GenerateBill(cid);
            if (ok)
            {
                MessageBox.Show("Bill generated and sent to customer email", "Bill alert", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No bill generated. The customer may have zero units.", "Billing", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnService_Click(object sender, EventArgs e)
        {
            BtnService.Enabled = false;
            int cid;
            if (!TryGetSelectedCustomerId(out cid))
            {
                MessageBox.Show("Select a customer first.", "Controller", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                BtnService.Enabled = true;
                return;
            }

            SendControlCommand(obj.DisconnectService(cid));
            MessageBox.Show("Remote command sent to IOT", "Controller", MessageBoxButtons.OK, MessageBoxIcon.Information);
            BtnService.Enabled = true;
        }

        private void auto()
        {
            if (!serviceStarted)
            {
                return;
            }

            int cid;
            if (!TryGetSelectedCustomerId(out cid))
            {
                return;
            }
            bool shouldDisconnect = obj.DisconnectService(cid);
            if (!lastAutoDisconnectState.HasValue || lastAutoDisconnectState.Value != shouldDisconnect)
            {
                if (!shouldDisconnect && lastAutoDisconnectState.HasValue && lastAutoDisconnectState.Value)
                {
                    Logger.Info("Payment detected (or no pending dues). Reconnect command will be sent.");
                }
                SendControlCommand(shouldDisconnect);
                lastAutoDisconnectState = shouldDisconnect;
            }
        }

        private void ServiceLoop()
        {
            double mockRaw = mockRawValue;
            while (serviceRunning)
            {
                if (enableMockMqtt)
                {
                    string payload = string.IsNullOrWhiteSpace(mockMeterNo)
                        ? mockRaw.ToString(CultureInfo.InvariantCulture)
                        : string.Format("{0}|{1}", mockMeterNo, mockRaw.ToString(CultureInfo.InvariantCulture));
                    ProcessIncomingPayload(mqttSubTopic, payload);
                    mockRaw += 1;
                    Thread.Sleep(mockIntervalMs);
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private void StopThreads()
        {
            serviceRunning = false;
            autoRunning = false;
            try
            {
                if (th != null && th.IsAlive)
                    th.Join(1000);
                if (autodisconnectorthread != null && autodisconnectorthread.IsAlive)
                    autodisconnectorthread.Join(1000);
            }
            catch (Exception ex)
            {
                Logger.Error("StopThreads failed", ex);
            }

            try
            {
                if (client != null && client.IsConnected)
                {
                    client.Unsubscribe(new[] { mqttSubTopic });
                    client.Disconnect();
                }
            }
            catch (Exception ex)
            {
                Logger.Error("MQTT disconnect failed", ex);
            }
        }

        private void client_ConnectionClosed(object sender, EventArgs e)
        {
            Logger.Info("MQTT connection closed.");
        }

        private bool TryGetSelectedCustomerId(out int cid)
        {
            cid = 0;
            if (DrpCustomerList.IsDisposed)
            {
                return false;
            }

            if (DrpCustomerList.InvokeRequired)
            {
                try
                {
                    string selectedValue = (string)DrpCustomerList.Invoke(new Func<string>(() =>
                    {
                        if (DrpCustomerList.SelectedValue == null)
                        {
                            return null;
                        }
                        return DrpCustomerList.SelectedValue.ToString();
                    }));

                    if (string.IsNullOrWhiteSpace(selectedValue))
                    {
                        cid = 0;
                        return false;
                    }
                    return int.TryParse(selectedValue, out cid);
                }
                catch
                {
                    cid = 0;
                    return false;
                }
            }

            if (DrpCustomerList.SelectedValue == null)
            {
                return false;
            }
            return int.TryParse(DrpCustomerList.SelectedValue.ToString(), out cid);
        }

        private void SendControlCommand(bool disconnect)
        {
            if (!serviceStarted)
            {
                Logger.Info("Control publish skipped: service not started.");
                return;
            }

            if (client == null || !client.IsConnected)
            {
                return;
            }
            string controlTopic = mqttPubTopic.TrimEnd('/') + "/" + controlMeterNo;
            string payload = disconnect ? "1" : "0";
            byte[] data = Encoding.UTF8.GetBytes(payload);
            client.Publish(controlTopic, data, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            client.Publish(controlTopic, data, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            client.Publish(controlTopic, data, MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
            Logger.Info(disconnect ? "Sending OFF (1)" : "Sending ON (0)");
            Logger.Info("MQTT TX [" + controlTopic + "] " + payload + " x3");
        }

        private void UpdateMeterUi(double delta)
        {
            if (lbAnalogMeter1.IsDisposed)
            {
                return;
            }

            if (lbAnalogMeter1.InvokeRequired)
            {
                lbAnalogMeter1.BeginInvoke((MethodInvoker)delegate
                {
                    lbAnalogMeter1.Value = Math.Min(lbAnalogMeter1.MaxValue, lbAnalogMeter1.Value + delta);
                });
            }
            else
            {
                lbAnalogMeter1.Value = Math.Min(lbAnalogMeter1.MaxValue, lbAnalogMeter1.Value + delta);
            }
        }

        private bool TryExtractReading(string payload, out double reading)
        {
            reading = 0;
            if (string.IsNullOrWhiteSpace(payload))
            {
                return false;
            }

            if (double.TryParse(payload.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out reading) ||
                double.TryParse(payload.Trim(), NumberStyles.Any, CultureInfo.CurrentCulture, out reading))
            {
                return true;
            }

            var match = Regex.Match(payload, @"(-?\d+(\.\d+)?)");
            if (!match.Success)
            {
                return false;
            }

            return double.TryParse(match.Value, NumberStyles.Any, CultureInfo.InvariantCulture, out reading);
        }

        private void StartService()
        {
            bool mqttConnected = false;
            try
            {
                if (string.IsNullOrWhiteSpace(mqttserverip) || string.IsNullOrWhiteSpace(mqttSubTopic) || string.IsNullOrWhiteSpace(mqttPubTopic))
                {
                    MessageBox.Show("MQTT settings are missing in App.config.", "MQTT", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                EnsureMqttConnectedIdle();
                mqttConnected = true;
            }
            catch (Exception ex)
            {
                Logger.Error("MQTT start failed", ex);
                if (!enableMockMqtt)
                {
                    MessageBox.Show("Failed to start MQTT service. Check log.", "MQTT", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                Logger.Info("Proceeding in mock mode without MQTT broker.");
            }

            try
            {
                serviceStarted = true;
                serviceRunning = true;
                autoRunning = true;
                lastAutoDisconnectState = null;

                th = new Thread(new ThreadStart(ServiceLoop));
                autodisconnectorthread = new Thread(new ThreadStart(AutoDisconnectLoop));
                th.IsBackground = true;
                autodisconnectorthread.IsBackground = true;
                th.Start();
                autodisconnectorthread.Start();

                // Explicit start marker topic for ESP/UI observers.
                try
                {
                    if (client != null && client.IsConnected)
                    {
                        string startTopic = mqttStartPayloadTopic.TrimEnd('/') + "/" + controlMeterNo;
                        client.Publish(startTopic, Encoding.UTF8.GetBytes("1"), MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE, false);
                        Logger.Info("MQTT TX [" + startTopic + "] 1");
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error("Failed to publish start marker.", ex);
                }

                BtnStartStop.Text = "Stop service";
                Logger.Info("Service Started");
                Logger.Info(mqttConnected
                    ? "MQTT service started. Subscribed to: " + mqttSubTopic
                    : "Service started in mock-only mode.");
            }
            catch (Exception ex)
            {
                Logger.Error("Service thread start failed", ex);
                MessageBox.Show("Failed to start service threads. Check log.", "Controller", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private MqttClient CreateConnectedMqttClient()
        {
            List<string> hosts = new List<string>();
            if (!string.IsNullOrWhiteSpace(mqttserverip))
            {
                hosts.Add(mqttserverip.Trim());
            }
            if (!string.IsNullOrWhiteSpace(mqttserverfallbackip))
            {
                hosts.Add(mqttserverfallbackip.Trim());
            }

            Exception lastEx = null;
            foreach (string host in hosts)
            {
                for (int i = 0; i < 3; i++)
                {
                    try
                    {
                        var c = new MqttClient(host, mqttport, false, null);
                        c.MqttMsgPublishReceived += client_MqttMsgPublishReceived;
                        c.ConnectionClosed += client_ConnectionClosed;
                        string attemptClientId = "IOTController-" + Guid.NewGuid().ToString("N").Substring(0, 12);
                        c.Connect(attemptClientId);
                        Logger.Info("MQTT client connected with host " + host + ":" + mqttport);
                        return c;
                    }
                    catch (Exception ex)
                    {
                        lastEx = ex;
                        Logger.Error("MQTT connect attempt failed for host " + host + " (attempt " + (i + 1) + ")", ex);
                        Thread.Sleep(1500);
                    }
                }
            }

            throw lastEx ?? new Exception("Unable to initialize/connect MQTT client.");
        }

        private void StopService()
        {
            StopThreads();
            serviceStarted = false;
            lastAutoDisconnectState = null;
            BtnStartStop.Text = "Start service";
            Logger.Info("MQTT service stopped.");
        }

        private void EnsureMqttConnectedIdle()
        {
            if (client != null && client.IsConnected)
            {
                return;
            }

            try
            {
                if (client != null)
                {
                    client.MqttMsgPublishReceived -= client_MqttMsgPublishReceived;
                    client.ConnectionClosed -= client_ConnectionClosed;
                    if (client.IsConnected) client.Disconnect();
                }
            }
            catch { }

            client = CreateConnectedMqttClient();
            client.Subscribe(new[] { mqttSubTopic }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
            Logger.Info("MQTT idle connected. Subscribed to: " + mqttSubTopic);
        }

        private void ProcessIncomingPayload(string topic, string payload)
        {
            int cid;
            double parsed;
            string meterNo;

            if (!TryResolveIncomingPayload(topic, payload, out cid, out parsed, out meterNo))
            {
                Logger.Info("Ignoring payload (unresolved): " + payload);
                return;
            }

            current = parsed - 540;
            current = current / 100.0;
            current = Math.Abs(current);
            lock (valueLock)
            {
                eVal += current;
            }

            obj.InsertReading(cid, current);
            obj.UpdateData(cid, current);
            Logger.Info(string.Format("Reading applied: meter={0}, cid={1}, raw={2}, delta={3:0.###}", meterNo ?? "(selected)", cid, parsed, current));
            UpdateMeterUi(current);
        }

        private bool TryResolveIncomingPayload(string topic, string payload, out int cid, out double readingRaw, out string meterNo)
        {
            cid = 0;
            readingRaw = 0;
            meterNo = null;

            if (string.IsNullOrWhiteSpace(payload))
            {
                return false;
            }

            // Format supported for simulation: METERNO|RAWVALUE (example: 333666|650)
            string[] parts = payload.Split('|');
            if (parts.Length == 2)
            {
                meterNo = parts[0].Trim();
                if (!TryExtractReading(parts[1], out readingRaw))
                {
                    return false;
                }

                if (!obj.TryGetCustomerIdByMeter(meterNo, out cid))
                {
                    return false;
                }
                return true;
            }

            // Topic format support: energy/meter/{meterNo} with payload as RAWVALUE.
            if (!string.IsNullOrWhiteSpace(topic) &&
                topic.StartsWith("energy/meter/", StringComparison.OrdinalIgnoreCase))
            {
                meterNo = topic.Substring("energy/meter/".Length).Trim();
                if (string.IsNullOrWhiteSpace(meterNo))
                {
                    return false;
                }
                if (!TryExtractReading(payload, out readingRaw))
                {
                    return false;
                }
                return obj.TryGetCustomerIdByMeter(meterNo, out cid);
            }

            // Fallback: treat payload as raw reading and apply to currently selected customer.
            if (!TryExtractReading(payload, out readingRaw))
            {
                return false;
            }

            return TryGetSelectedCustomerId(out cid);
        }

        private bool ReadBoolSetting(string key, bool fallback)
        {
            bool value;
            return bool.TryParse(System.Configuration.ConfigurationManager.AppSettings[key], out value) ? value : fallback;
        }

        private int ReadIntSetting(string key, int fallback)
        {
            int value;
            return int.TryParse(System.Configuration.ConfigurationManager.AppSettings[key], out value) ? value : fallback;
        }

        private double ReadDoubleSetting(string key, double fallback)
        {
            double value;
            return double.TryParse(System.Configuration.ConfigurationManager.AppSettings[key], NumberStyles.Any, CultureInfo.InvariantCulture, out value) ? value : fallback;
        }
 
    }
}
