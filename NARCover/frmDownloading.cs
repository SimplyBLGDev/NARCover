using Newtonsoft.Json.Linq;
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

namespace NARCover {
	public partial class frmDownloading : Form {
        const string PUBLICKEY = "97b9ec2c3dd0573d0d03f832c98041320383bfbb7294452d19431bd728b5557a";

        public string romsPath = "";
        public List<string> games;

        public frmDownloading(string romsPath, List<string> extensions) {
            InitializeComponent();
            this.romsPath = romsPath;

            string[] names = Directory.GetFiles(romsPath);
            games = new List<string>();

            foreach (string name in names)
                if (extensions.Contains(Path.GetExtension(name)))
                    games.Add(Path.GetFileNameWithoutExtension(name));

            foreach (string game in games)
                Download(game);
        }

        public string Get(string uri) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }
        }

        public void Download(string name) {
            string response = Get("https://api.thegamesdb.net/v1.1/Games/ByGameName?apikey=" + PUBLICKEY + "&name=" + GetSimplifiedGameName(name));
            var values = JObject.Parse(response);
        }

        public string GetSimplifiedGameName(string name) {
            return name.Replace("(U)", "").Replace("(E)", "").Replace("(J)", "").Replace("(UE)", "").Replace(", The", "");
        }
    }
}
