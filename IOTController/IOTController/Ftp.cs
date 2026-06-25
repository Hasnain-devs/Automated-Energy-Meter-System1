using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace IOTController
{
    class Ftp
    {
        public static void UploadFileToFTP(string cmd, string filename)
        {
                 
                string ftpfullpath = "ftp://njsempire.com/iotspace/" + filename;
                FtpWebRequest ftp = (FtpWebRequest)FtpWebRequest.Create(ftpfullpath);
                ftp.Credentials = new NetworkCredential("njsuser", "Njsuser123");

                ftp.KeepAlive = true;
                ftp.UseBinary = true;
                ftp.Method = WebRequestMethods.Ftp.UploadFile;

                byte[] buffer = ASCIIEncoding.ASCII.GetBytes(cmd);

                Stream ftpstream = ftp.GetRequestStream();
                ftpstream.Write(buffer, 0, buffer.Length);
                ftpstream.Close();
            
            
        }
    }
}
