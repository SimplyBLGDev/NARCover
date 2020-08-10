using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace NARCover {
	public partial class frmDownloading : Form {
		const string PUBLICKEY = "97b9ec2c3dd0573d0d03f832c98041320383bfbb7294452d19431bd728b5557a";
		const string OGIMAGESBASEADDRESS = "https://cdn.thegamesdb.net/images/original/";

		public /*heh*/ string publicKey { get { return publicKey; } } // TODO: Allow a 'override public key' field
		public string romsPath = "";
		public string saveDir = "";
		public List<string> extensions;
		public List<string> priorityImageTypes;
		public List<string> games;

		public frmDownloading(string romsPath, List<string> extensions, List<string> priorityImageTypes, string saveDir) {
			InitializeComponent();
			this.romsPath = romsPath;
			this.extensions = extensions;
			this.priorityImageTypes = priorityImageTypes;
			if (saveDir == "")
				this.saveDir = Application.StartupPath + "/images/";
			else
				this.saveDir = saveDir;
		}

		private void frmDownloading_Shown(object sender, EventArgs e) {
			SearchAndDownloadGames();
		}

		void SearchAndDownloadGames() {
			string[] names = Directory.GetFiles(romsPath);
			games = new List<string>();

			foreach (string name in names)
				if (extensions.Contains(Path.GetExtension(name)))
					games.Add(GetSimplifiedGameName(Path.GetFileNameWithoutExtension(name)));

			// <Game code, GameInfo>
			Dictionary<int, GameInfo> gamesData = new Dictionary<int, GameInfo>();

			foreach (string game in games)
				gamesData.Add(ProcessGame(game), new GameInfo(game));

			FindCovers(gamesData);

			pbProgress.Value = 0;
			pbProgress.Maximum = gamesData.Values.Count;
			Task.Run(() => DownloadImages(gamesData.Values.ToArray()));
		}

		private int ProcessGame(string game) {
			try {
				return FindGameCode(game);
			} catch (APIException e) {
				switch (MessageBox.Show("API request error, the games DB might be offline or API outdated, check https://thegamesdb.net or " +
					"contact me at https://github.com/SimplyBLGDev/NARCover.", "API Exception", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)) {
					case DialogResult.Cancel:
						Close(); // TODO
						break;
					case DialogResult.OK:
						break;
				}
			}

			return -1;
		}

		// Populates a dictionary of game codes with their respective images' urls
		private void FindCovers(Dictionary<int, GameInfo> gameCodes) {
			string gameCodesString = "";

			foreach (int code in gameCodes.Keys)
				gameCodesString += (gameCodesString != "" ? ", ": "") + code.ToString();

			// Get all games images in a single request
			string responseString = Get("https://api.thegamesdb.net/v1/Games/Images?apikey=" + PUBLICKEY + "&games_id=" + gameCodesString);
			JObject response = JObject.Parse(responseString);

			if (response.Value<int>("code") != 200)// No success code
				throw new APIException("API Request Error.", response.Value<int>("code"));

			int[] keys = gameCodes.Keys.ToArray(); // Avoid on-the-fly conflicts due to dictionary modification
			foreach (int code in keys) {
				var availableGameImages = response["data"]["images"][code.ToString()];

				foreach (string imageType in priorityImageTypes) {
					for (int i = 0; i < availableGameImages.Count(); i++)
						if ((availableGameImages[i].Value<string>("type") == imageType)
						&& (imageType != "boxart" || availableGameImages[i].Value<string>("side") == "front")) { // If looking for boxart only take the front side

							gameCodes[code].imageAddress = availableGameImages[i].Value<string>("filename");
							break;
						}

					if (gameCodes[code].imageAddress != "")
						break; // break when we found the image
				}

			}
		}

		private int FindGameCode(string name) {
			string responseString = Get("https://api.thegamesdb.net/v1.1/Games/ByGameName?apikey=" + PUBLICKEY + "&name=" + name);
			JObject response = JObject.Parse(responseString);

			if (response.Value<int>("code") != 200) { // No success code
				throw new APIException("API Request Error.", response.Value<int>("code"));
			} else {
				if (response["data"].Value<int>("count") == 0)
					throw new GameNotFoundException("Game not found.", name);
				else
					return response["data"]["games"][0].Value<int>("id");
			}
		}

		async void DownloadImages(GameInfo[] gameCodes) {
			foreach (GameInfo game in gameCodes) {
				using (var client = new WebClient()) {
					Uri uri = new Uri(OGIMAGESBASEADDRESS + game.imageAddress);
					client.DownloadDataCompleted += Client_DownloadDataCompleted;
					await Task.Run(() => {
						client.DownloadFileAsync(uri, saveDir + game.name + Path.GetExtension(game.imageAddress)); return true;
					});
				}
			}
		}

		private void Client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e) {
			pbProgress.Value++;
		}

		public string GetSimplifiedGameName(string name) {
			string r = "";
			int parentheses = 0;
			int brackets = 0;
			int chevrons = 0;

			for (int i = 0; i < name.Length; i++) {
				switch (name[i]) {
					case '(': parentheses++; break;
					case '[': brackets++; break;
					case '<': chevrons++; break;
				}

				if (parentheses <= 0 && brackets <= 0 && chevrons <= 0)
					r += name[i];

				switch (name[i]) {
					case ')': parentheses--; break;
					case ']': brackets--; break;
					case '>': chevrons--; break;
				}
			} // Remove anything between parentheses, brackets or chevrons

			for (int i = 0; i < r.Length - 1; i++) {
				// Titles starting with 'The' are usually rewritten with ', The' at the end to better sort their names, this re-inserts that ending
				//  into the beggining this method generalizes any such occurences
				if (r[i] == ',' && r[i + 1] == ' ') {
					r = r.Substring(i + 2, r.Length - (i + 2)).Trim() + " " + r.Substring(0, i);
					break;
				}
			}

			r = r.Trim();

			return r;
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

	class GameInfo {
		public string name;
		public string imageAddress;

		public GameInfo(string name) {
			this.name = name;
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
