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
        public List<string> extensions;
        public List<string> games;

        public frmDownloading(string romsPath, List<string> extensions) {
            InitializeComponent();
            this.romsPath = romsPath;
            this.extensions = extensions;
        }

        private void frmDownloading_Load(object sender, EventArgs e) {
            SearchAndDownloadGames();
        }

        void SearchAndDownloadGames() {
            string[] names = Directory.GetFiles(romsPath);
            games = new List<string>();

            foreach (string name in names)
                if (extensions.Contains(Path.GetExtension(name)))
                    games.Add(GetSimplifiedGameName(Path.GetFileNameWithoutExtension(name)));

            foreach (string game in games)
                ProcessGame(game);
        }

        void ProcessGame(string game) {
            try {
                int gameCode = FindGameCode(game);
                DownloadCover(gameCode);
            } catch (APIException e) {
                switch (MessageBox.Show(e.Message, "API Exception", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error)) {
                    case DialogResult.Abort:
                        Close();
                        break;
                    case DialogResult.Ignore:
                        return;
                    case DialogResult.Retry:
                        // Retry
                        break;
                }
            }
        }

        public void DownloadCover(int gameCode) {

		}

        public int FindGameCode(string name) {
            string responseString = Get("https://api.thegamesdb.net/v1.1/Games/ByGameName?apikey=" + PUBLICKEY + "&name=" + GetSimplifiedGameName(name));
            JObject response = JObject.Parse(responseString);

            if (response.Value<int>("code") != 200) { // No success code
                throw new APIException("API request error, the games DB might be offline or API outdated, check https://thegamesdb.net or" +
                    "contact me at https://github.com/SimplyBLGDev/NARCover.", response.Value<int>("code"));
            } else {
                if (response["data"].Value<int>("count") == 0)
                    throw new GameNotFoundException("Game not found.", name);
                else
                    return response["data"]["games"][0].Value<int>("id");
            }
        }

        public string GetSimplifiedGameName(string name) {
            return name.Replace("(U)", "").Replace("(E)", "").Replace("(J)", "").Replace("(UE)", "").Replace(", The", "");
        }

        public static string Get(string uri) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream)) {
                return reader.ReadToEnd();
            }
        }

    }

    class APIException : Exception {
        int errorCode;

        public APIException(string message, int code) : base(message) {
            errorCode = code;
        }
    }

    class GameNotFoundException : Exception {
        string game;

        public GameNotFoundException(string message, string game) : base(message) {
            this.game = game;
        }
    }
}
