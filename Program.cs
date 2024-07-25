using System;
using System.IO;
using FluentFTP;
using Newtonsoft;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

class Program
{
    class FtpConfig
    {
        public string? Host { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? RemoteDirectory { get; set; }
        public string? LocalDirectory { get; set; }
    }

    static  void Main(string[] args)
    {
        string configFilePath = "ftp_config.json";

        string json = File.ReadAllText(configFilePath);
        var ftpConfigs = JsonConvert.DeserializeObject<FtpConfig[]>(json);
        if (ftpConfigs == null) return;
        foreach(FtpConfig config in ftpConfigs) 
        {
            Console.WriteLine($"Downloading from  {config.Host}{config.RemoteDirectory}");
            if (config.Host == null || config.Username==null || config.Password==null || config.RemoteDirectory==null || config.LocalDirectory == null)
            {
                Console.WriteLine("Json file info incomplete");
                return;
            }
                

            string ftpHost = config.Host;
            string ftpUsername = config.Username; 
            string ftpPassword = config.Password; 
            string remoteFolderPath = config.RemoteDirectory; 
            string localFolderPath = config.LocalDirectory;  

            try
            {
                if (!Directory.Exists(localFolderPath))
                {
                    Directory.CreateDirectory(localFolderPath);
                }

                using var ftpClient = new FtpClient(ftpHost, ftpUsername, ftpPassword);
                ftpClient.Connect();

                var items = ftpClient.GetListing(remoteFolderPath);

                foreach (var item in items)
                {
                    if (item.Type == FtpObjectType.File)
                    {
                        string localFilePath = Path.Combine(localFolderPath, item.Name);

                        if (!File.Exists(localFilePath))
                        {
                            Console.Write($"Downloading {item.Name}...");
                            ftpClient.DownloadFile(localFilePath, item.FullName);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine(" V");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.WriteLine($"{item.Name} already exists. Skipping download.");
                        }
                    }
                }

                ftpClient.Disconnect();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
