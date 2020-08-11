using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication.ExtendedProtection;
using System.Threading.Tasks;

namespace NARCover {
	public class Downloader {
		public const string PUBLICKEY = "97b9ec2c3dd0573d0d03f832c98041320383bfbb7294452d19431bd728b5557a";

		public /*heh*/ string publicKey {
			get { return publicKeyOverride == null ? PUBLICKEY : publicKeyOverride; }
			set { publicKeyOverride = value; }
		}
		string publicKeyOverride;
		public string romsPath = "";
		public bool searchSubdirs;
		public bool useFolderName;
		public string saveDir = "";
		public int consoleId;
		public List<string> extensions;
		public List<string> priorityImageTypes;
		public List<string> gameFiles;
		public string imgURLBase;
		public bool useFileNameForImage;

		public delegate void GameNotFoundDel(GameNotFoundException e);
		public delegate void APIExceptionDel(APIException e);
		public delegate void GameInfoDel(GameInfo game, string imagePath);
		public delegate void StartDownloadingDel(int gamesFound);
		public delegate void StartFindingCoversDel(int gamesFound);
		public delegate void Done();
		public event GameNotFoundDel OnGameNotFound;
		public event APIExceptionDel OnAPIException;
		public event GameInfoDel OnImageDownloaded;
		public event StartFindingCoversDel OnStartFindingCovers;
		public event StartDownloadingDel OnStartDownload;
		public event Done OnDone;

		public void Start() {
			Task.Run(() => SearchAndDownloadGames());
		}

		private void SearchAndDownloadGames() {
			Dictionary<int, GameInfo> gamesData = SearchGames();
			DownloadGames(gamesData);
		}

		private Dictionary<int, GameInfo> SearchGames() {
			if (useFolderName) {
				gameFiles = new List<string>(Directory.GetDirectories(romsPath));
			} else {
				SearchOption searchOption = searchSubdirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
				string[]names = Directory.GetFiles(romsPath, "*", searchOption);
				gameFiles = new List<string>();

				foreach (string name in names)
					if (extensions.Contains(Path.GetExtension(name)))
						gameFiles.Add(Path.GetFileNameWithoutExtension(name));
			}

			// <Game code, GameInfo>
			Dictionary<int, GameInfo> gamesData = new Dictionary<int, GameInfo>();

			foreach (string game in gameFiles) {
				string simplifiedName = Utils.GetSimplifiedGameName(game);
				int gameId = ProcessGame(simplifiedName);
				if (gameId != -1)
					gamesData.Add(gameId, new GameInfo(game, simplifiedName));
			}

			return gamesData;
		}

		private void DownloadGames(Dictionary<int, GameInfo> gamesData) {
			OnStartFindingCovers(gamesData.Count);
			FindCovers(gamesData);

			OnStartDownload(gamesData.Count);
			DownloadImages(gamesData.Values.ToArray());

			OnDone();
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

				if (gameCodes[code].imageAddress == "") // If no image could be found
					gameCodes.Remove(code);
			}
		}

		void DownloadImages(GameInfo[] gameCodes) {
			foreach (GameInfo game in gameCodes) {
				using (var client = new WebClient()) {
					Uri uri = new Uri(imgURLBase + game.imageAddress);
					string _name = useFileNameForImage ? game.filename : game.filename;
					client.DownloadFile(uri, Path.Combine(saveDir, _name + Path.GetExtension(game.imageAddress)));
					OnImageDownloaded(game, Path.Combine(saveDir, _name + Path.GetExtension(game.imageAddress)));
				}
			}
		}
	}
}
