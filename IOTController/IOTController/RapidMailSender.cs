using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.IO;
using System.Net.Mime;
using System.Configuration;
namespace IOTController
{
    class RapidMailSender
    {
        public static void Send(string Id , string meterno , string amount , string units, string name, string contype )
        {
            SmtpClient smtp = new SmtpClient();
            int port;
            int.TryParse(ConfigurationManager.AppSettings["SmtpPort"], out port);
            smtp.Port = port == 0 ? 587 : port;
            smtp.Host = ConfigurationManager.AppSettings["SmtpHost"] ?? "smtp.gmail.com";
            string smtpUser = ConfigurationManager.AppSettings["SmtpUser"] ?? "mailserviceforproject@gmail.com";
            string smtpPassword = ConfigurationManager.AppSettings["SmtpPassword"] ?? ConfigurationManager.AppSettings["*****"];
            smtp.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPassword);
            smtp.EnableSsl = true;

            MailAddress _from = new MailAddress(smtpUser);
            MailAddress _to = new MailAddress(Id);

            MailMessage mail = new MailMessage(_from, _to);
            mail.Subject = "Electricity Bill";

            string body = "<table border=2> <tr><th> Electricity Bill </th></tr>";
            body += "<tr><td>Meterno</td><td>" + meterno +"</td></tr>";
            body += "<tr><td>Name</td><td>" + name +"</td></tr>";
            body += "<tr><td>Connection type</td><td>" + contype+ "</td></tr>";
            body += "<tr><td>Units consumed</td><td>" + units + "</td></tr>";
            body += "<tr><td>Amount to pay</td><td>" + amount + "</td></tr>";
            body += "</table>Please pay in time to avoid disconnection";

            mail.Body = body; //  string.Format("<font size=3> Dear customer : Units consumed {0} for meterno {1} , please pay {2}/-Rs  before due date to avoid disconnection </font>", units, meterno, amount);

            mail.IsBodyHtml = true;
            try
            {
                smtp.Send(mail);
            }
            catch (Exception e)
            {
                Logger.Error("Email send failed", e);
                throw;
            }
        }
    }
}
