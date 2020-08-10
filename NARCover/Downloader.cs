using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NARCover {
	public class Downloader {
		public const string PUBLICKEY = "97b9ec2c3dd0573d0d03f832c98041320383bfbb7294452d19431bd728b5557a";
		const string OGIMAGESBASEADDRESS = "https://cdn.thegamesdb.net/images/original/";

		public /*heh*/ string publicKey {
			get { return publicKeyOverride == null ? PUBLICKEY : publicKeyOverride; }
			set { publicKeyOverride = value; }
		}
		string publicKeyOverride;
		public string romsPath = "";
		public string saveDir = "";
		public int consoleId;
		public List<string> extensions;
		public List<string> priorityImageTypes;
		public List<string> games;

		public delegate void GameNotFoundDel(GameNotFoundException e);
		public event GameNotFoundDel OnGameNotFound;
		public delegate void APIExceptionDel(APIException e);
		public event APIExceptionDel OnAPIException;
		public delegate void GameInfoDel(GameInfo game);
		public event GameInfoDel OnImageDownloaded;

		public void Start() {
			Task.Run(() => SearchAndDownloadGames());
		}

		public void SearchAndDownloadGames() {
			string[] names = Directory.GetFiles(romsPath);
			games = new List<string>();

			foreach (string name in names)
				if (extensions.Contains(Path.GetExtension(name)))
					games.Add(Utils.GetSimplifiedGameName(Path.GetFileNameWithoutExtension(name)));

			// <Game code, GameInfo>
			Dictionary<int, GameInfo> gamesData = new Dictionary<int, GameInfo>();

			foreach (string game in games)
				gamesData.Add(ProcessGame(game), new GameInfo(game));

			FindCovers(gamesData);

			DownloadImages(gamesData.Values.ToArray());
		}

		private int ProcessGame(string game) {
			try {
				return FindGameCode(game);
			} catch (APIException e) {
				OnAPIException(e);
			} catch (GameNotFoundException e) {
				OnGameNotFound(e);
			}

			return -1;
		}

		// Populates a dictionary of game codes with their respective images' urls
		private void FindCovers(Dictionary<int, GameInfo> gameCodes) {
			string gameCodesString = "";

			foreach (int code in gameCodes.Keys)
				gameCodesString += (gameCodesString != "" ? ", " : "") + code.ToString();

			// Get all games images in a single request
			string responseString = Utils.Get("https://api.thegamesdb.net/v1/Games/Images?apikey=" + publicKey + "&games_id=" + gameCodesString);
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
			string responseString = Utils.Get("https://api.thegamesdb.net/v1.1/Games/ByGameName?apikey=" + publicKey + "&name=" + name + "&filter[platform]=" + consoleId.ToString());
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

		void DownloadImages(GameInfo[] gameCodes) {
			foreach (GameInfo game in gameCodes) {
				using (var client = new WebClient()) {
					Uri uri = new Uri(OGIMAGESBASEADDRESS + game.imageAddress);
					client.DownloadFile(uri, saveDir + game.name + Path.GetExtension(game.imageAddress));
					OnImageDownloaded(game);
				}
			}
		}
	}
}
