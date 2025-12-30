using SQLLiteRollArtWin.Forms;
using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace SQLLiteRollArtWin
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        static bool UrlExists(string url)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "HEAD";
                request.UserAgent = "SQLLiteRollArtWin";
                request.AllowAutoRedirect = true;

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (WebException)
            {
                return false;
            }
        }

    /*
    static void Main()
    {
        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        var dbUrl = "https://github.com/LunelRoller/Toolbox/releases/download/v1.0.0/sqlite.s3db";
        var sqlUrl = "https://github.com/LunelRoller/Toolbox/releases/download/v1.0.0/init.sql";

        Download(dbUrl, "sqlite.s3db");
        Download(sqlUrl, "init.sql");

        Console.WriteLine("OK");
        Console.ReadLine();
    }

    static void Download(string url, string file)
    {
        if (File.Exists(file))
            return;

        using (var wc = new WebClient())
        {
            wc.Headers.Add("User-Agent", "SQLLiteRollArtWin");
            wc.DownloadFile(url, file);
        }
    }*/
}
}