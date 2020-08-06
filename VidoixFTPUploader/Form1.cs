using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            using (var client = new WebClient())
            {
                client.Credentials = new NetworkCredential("admin", "1sw13f3f5g5n6nk");
                client.UploadFile("176.53.34.178:21", WebRequestMethods.Ftp.UploadFile, @"C:\Users\Acand\Desktop\New folder\testDoc.txt");
            }
        }
    }
}
