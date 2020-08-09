using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VidoixFTPUploader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            FTPUploader ftpu = new FTPUploader();
            //ftpu.Upload(@"C:\Users\Acand\source\repos\Videoix\Videoix\bin\x86\Debug", "176.53.34.178", "admin", "1sw13f3f5g5n6nk");
            ftpu.Upload(@"C:\Users\Acand\Desktop\New folder\", "176.53.34.178", "admin", "1sw13f3f5g5n6nk");
            //FtpDosyaUpload("176.53.34.178", "admin", "1sw13f3f5g5n6nk", @"C:\Users\Acand\Desktop\New folder\testDoc.txt");
        }
        public void FtpDosyaUpload(string ip, string username, string pass, string aktarilacak_dosya_yolu)
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
