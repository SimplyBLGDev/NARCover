﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

			// <Game code, file path>
			Dictionary<int, string> gameImages = new Dictionary<int, string>();

			foreach (string game in games)
				gameImages.Add(ProcessGame(game), "");

			FindCovers(gameImages);
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
		public void FindCovers(Dictionary<int, string> gameCodes) {
			string gameCodesString = "";

			foreach (int code in gameCodes.Keys)
				gameCodesString += (gameCodesString != "" ? ", ": "") + code.ToString();

			// Get all games images in a single request
			string responseString = Get("https://api.thegamesdb.net/v1/Games/Images?apikey=" + PUBLICKEY + "&games_id=" + gameCodesString);
			JObject response = JObject.Parse(responseString);

			if (response.Value<int>("code") != 200)// No success code
				throw new APIException("API Request Error.", response.Value<int>("code"));

			foreach (int code in gameCodes.Keys) {
				var availableGameImages = response["data"]["images"][code.ToString()];

				if (availableGameImages.Children().Count() == 0)
					gameCodes[code] = "";
				else
					gameCodes[code] = availableGameImages[0].Value<string>("filename");
			}
		}

		public int FindGameCode(string name) {
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
