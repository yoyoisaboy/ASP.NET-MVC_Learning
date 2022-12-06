
using HtmlAgilityPack;
using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Web.Services;

namespace Day16
{
    public partial class Index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //catch_json_url(); 
            //catch_json_url_with_python();

        }
        public String catch_json_url_with_binding()
        {
            string Product_Name = "";
            string reply = "null";
            HtmlWeb webClient = new HtmlWeb(); // 建立呼叫網站Client端，模擬請求
            HtmlDocument doc = webClient.Load("https://data.gov.tw/dataset/31897"); // 載入網站
            HtmlNodeCollection item = doc.DocumentNode.SelectNodes($"/ html / head / script[1] / text()"); // 根據要的那段html右鍵複製XPath
            if (item != null)
            {
                Product_Name = item[0].InnerText.ToString();
                string json_url_left = "\"encodingFormat\":\"JSON\",\"contentUrl\":\"";
                string json_url_right = "},{\"@type\":\"DataDownload\",\"encodingFormat\":\"WEBSERVICES\",";
                int loc_left = Product_Name.IndexOf(json_url_left);
                int loc_right = Product_Name.IndexOf(json_url_right);
                string cut_substr = "\"},{\"@type\":\"DataDownload\",\"encodingFor";
                string json_url = Product_Name.Substring(loc_left + json_url_left.Length, loc_right - loc_left - cut_substr.Length); //切出json的URL
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                reply = client.DownloadString(json_url);

            }
            //Label1.Text = json_url;
            return reply;
        }

        [WebMethod]
        public static String catch_json_url_with_ajax()
        {
            string Product_Name = "";
            string reply = "null";
            HtmlWeb webClient = new HtmlWeb(); // 建立呼叫網站Client端，模擬請求
            HtmlDocument doc = webClient.Load("https://data.gov.tw/dataset/31897"); // 載入網站
            HtmlNodeCollection item = doc.DocumentNode.SelectNodes($"/ html / head / script[1] / text()"); // 根據要的那段html右鍵複製XPath
            if (item != null)
            {
                Product_Name = item[0].InnerText.ToString();
                string json_url_left = "\"encodingFormat\":\"JSON\",\"contentUrl\":\"";
                string json_url_right = "},{\"@type\":\"DataDownload\",\"encodingFormat\":\"WEBSERVICES\",";
                int loc_left = Product_Name.IndexOf(json_url_left);
                int loc_right = Product_Name.IndexOf(json_url_right);
                string cut_substr = "\"},{\"@type\":\"DataDownload\",\"encodingFor";
                string json_url = Product_Name.Substring(loc_left + json_url_left.Length, loc_right - loc_left - cut_substr.Length); //切出json的URL
                WebClient client = new WebClient();
                client.Encoding = Encoding.UTF8;
                reply = client.DownloadString(json_url);
            }
            return reply;
        }

        public String catch_json_url_with_python()
        {
            var path = Server.MapPath("~/assets/python/");
            ProcessStartInfo start = new ProcessStartInfo();
            start.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            start.FileName = $"C:/Users/{Environment.UserName}/AppData/Local/Programs/Python/Python37/python.exe";
            start.Arguments = path + "catch_json_url.py";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            start.RedirectStandardError = true;
            start.CreateNoWindow = true;
            start.ErrorDialog = true;

            string text = "";
            using (Process process = Process.Start(start))
            {
                process.BeginErrorReadLine();
                while (!process.StandardOutput.EndOfStream)
                {
                    text = process.StandardOutput.ReadLine();

                }
                process.WaitForExit();
            }
            //Label1.Text = text;
            return text;
        }
        
    }
}