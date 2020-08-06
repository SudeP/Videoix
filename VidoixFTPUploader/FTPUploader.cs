using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VidoixFTPUploader
{
    public class FTPUploader
    {
        public void Upload()
        {

        }
        public void FtpFileUpload(string ip, string username, string pass, string aktarilacak_dosya_yolu)
        {
            FileInfo FI = new FileInfo(aktarilacak_dosya_yolu);

            string uri = $"ftp://{ip}/" + "" + "testDoc.txt";

            FtpWebRequest FTP;
            FTP = (FtpWebRequest)WebRequest.Create(new Uri(uri));
            FTP.Credentials = new NetworkCredential(username, pass);
            FTP.KeepAlive = false;
            FTP.Method = WebRequestMethods.Ftp.UploadFile;
            FTP.UseBinary = true;
            FTP.ContentLength = FI.Length;
            FTP.UsePassive = false;
            int buffLength = 2048;
            byte[] buff = new byte[buffLength];
            int contentLen;
            FileStream FS = FI.OpenRead();
            try
            {
                Stream strm = FTP.GetRequestStream();
                contentLen = FS.Read(buff, 0, buffLength);
                while (contentLen != 0)
                {
                    strm.Write(buff, 0, contentLen);
                    contentLen = FS.Read(buff, 0, buffLength);
                }
                strm.Close();
                FS.Close();
            }
#pragma warning disable
            catch (Exception ex)
#pragma warning restore
            {

            }
        }
    }
}
