using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VidoixFTPUploader
{
    public class FTPUploader
    {
        public void Upload(string p, string i, string un, string up)
        {
            ip = i;
            username = un;
            pass = up;
            rootPath = p;
            _p = System.Windows.Forms.Application.StartupPath;
            CreateNewAccessDatabaseOrOpen(_p);
            connection.Open();
            Founding();
            connection.Close();
        }
        public void FtpFileUpload(string aktarilacak_dosya_yolu)
        {
            FileInfo FI = new FileInfo(aktarilacak_dosya_yolu);

            string uri = $"ftp://{ip}//" + "" + FI.Name;

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
        public class Files
        {
            public string Id { get; set; }
            public string Path { get; set; }
            public string LastWriteTime { get; set; }
        }
        private List<string> ChangeFilePaths { get; set; }
        private DataTable FileLogs { get; set; }
        OleDbConnection connection = null;
        OleDbCommand command = null;
        string rootPath, _p, ip, username, pass;
        private void FileLogsRestart()
        {
            string query = $@"Select * From {nameof(Files)}";
            command.CommandText = query;
            var asd = connection.GetSchema();
            var reader = command.ExecuteReader();
            reader.Read();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            FileLogs = dataTable;
        }
        private void Founding()
        {
            FileLogsRestart();
            ChangeFilePaths = new List<string>();
            var allFilesInFromPath = Directory.GetFiles(rootPath, "*", SearchOption.AllDirectories);
            foreach (var fromFilePath in allFilesInFromPath)
            {
                var fromFileInfo = new FileInfo(fromFilePath);

                //var fileLog = FileLogs?.FirstOrDefault(fl => fl.Path == fromFilePath);
                var fileLog = (from log in FileLogs.Rows.Cast<DataRow>().ToList()
                               where log["Path"].ToString() == fromFilePath
                               select log).FirstOrDefault();

                bool IsCorrect = false;
                if (fileLog != null)
                    IsCorrect = fileLog["LastWriteTime"].ToString() != fromFileInfo.LastWriteTime.ToString("yyyyMMddHHmmss");

                string queryForUpdate = string.Empty;
                if (fileLog != null)
                    queryForUpdate = $@"
                    Update
                        {nameof(Files)}
                    Set
                        LastWriteTime = '{fromFileInfo.LastWriteTime.ToString("yyyyMMddHHmmss")}'
                    Where
                        Id = '{fileLog["Id"]}'
                    ";
                string queryForInsert = string.Empty;
                if (fileLog is null)
                    queryForInsert = $@"
                    Insert Into {nameof(Files)}
                    (
                    Id
                    ,Path
                    ,LastWriteTime
                    )
                    Values
                    (
                    '{Guid.NewGuid().ToString()}'
                    ,'{fromFilePath}'
                    ,'{fromFileInfo.LastWriteTime.ToString("yyyyMMddHHmmss")}'
                    )
                    ";

                if (IsCorrect)
                {
                    AddList();
                    command.CommandText = queryForUpdate;
                    command.ExecuteNonQuery();
                }
                else if (fileLog is null)
                {
                    AddList();
                    command.CommandText = queryForInsert;
                    command.ExecuteNonQuery();
                }

                void AddList()
                {
                    ChangeFilePaths.Add(fromFilePath);
                }
            }
            if (ChangeFilePaths.Count != 0)
                CreateZip();
        }
        private void CreateZip()
        {
            FileLogsRestart();
            foreach (var filePath in ChangeFilePaths)
            {
                FtpFileUpload(filePath);
            }

        }
        private string GetRootPath(string path, string root) => path.Replace(root, "");
        public bool CreateNewAccessDatabaseOrOpen(string fileName)
        {
            fileName += @"\\DB.accdb";
            string connectionString = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + fileName + "; Jet OLEDB:Engine Type=5";
            if (!File.Exists(fileName))
                if (!Create())
                    return false;
            connection = new OleDbConnection(connectionString);
            command = new OleDbCommand("", connection);
            return true;
            bool Create()
            {
                bool result = false;

                ADOX.Catalog cat = new ADOX.Catalog();
                try
                {
                    cat.Create(connectionString);


                    ADODB.Connection con = cat.ActiveConnection as ADODB.Connection;


                    con.Execute("CREATE TABLE Files( [Id] Text, [Path] Text, [LastWriteTime] Text)", out _);

                    if (con != null)
                        con.Close();

                    result = true;
                }
                catch (Exception ex)
                {
                    result = false;
                }
                cat = null;
                return result;
            }
        }
    }

}
