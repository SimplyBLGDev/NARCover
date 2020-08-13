using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NARCover {
	public class Downloader {
		public const string PUBLICKEY = "97b9ec2c3dd0573d0d03f832c98041320383bfbb7294452d19431bd728b5557a";

		public delegate void DownloaderUpdate(UpdateInfo info);
		public delegate void DownloaderPhaseChange(PhaseInfo info);
		public delegate void DownloaderExceptionThrown(Exception e);
		public event DownloaderUpdate Update;
		public event DownloaderPhaseChange PhaseChange;
		public event DownloaderExceptionThrown ExceptionThrown;

		public /*heh*/ string publicKey {
			get { return publicKeyOverride == null ? PUBLICKEY : publicKeyOverride; }
			set { publicKeyOverride = value; }
		}
		string publicKeyOverride;
		public string saveDir = "";
		public int consoleId;
		public List<string> extensions;
		public List<string> priorityImageTypes;
		public List<string> gameFiles;
		public string imgURLBase;
		public bool useFileNameForImage;

		public void Start() {
			Task.Run(() => SearchAndDownloadGames());
		}

		private void SearchAndDownloadGames() {
			PhaseChange(new PhaseInfo(PhaseInfo.Phase.Start, 0));
			Dictionary<int, GameInfo> gamesData = SearchGames();

			if (gamesData.Count > 0) {
				PhaseChange(new PhaseInfo(PhaseInfo.Phase.FindImages, gamesData.Count));
				FindCovers(gamesData);

				PhaseChange(new PhaseInfo(PhaseInfo.Phase.DownloadImages, gamesData.Count));
				DownloadImages(gamesData.Values.ToArray());
			}

			PhaseChange(new PhaseInfo(PhaseInfo.Phase.Finish, gamesData.Count));
		}

		private Dictionary<int, GameInfo> SearchGames() {
			// <Game code, GameInfo>
			Dictionary<int, GameInfo> gamesData = new Dictionary<int, GameInfo>();

			foreach (string game in gameFiles) {
				string simplifiedName = Utils.GetSimplifiedGameName(game);
				GameInfo gameInfo = new GameInfo(game, simplifiedName);
				int gameId = ProcessGame(simplifiedName);

				if (gamesData.ContainsKey(gameId) || gameId == -1) { // Duplicate game or game not found
					Update(new UpdateInfo(UpdateInfo.UpdateType.GameNotFound, gameInfo));
				} else {
					gamesData.Add(gameId, gameInfo);
					Update(new UpdateInfo(UpdateInfo.UpdateType.GameFound, gameInfo));
				}
			}

			return gamesData;
		}

		private int ProcessGame(string game) {
			try {
				return FindGameCode(game);
			} catch (APIException e) {
				ExceptionThrown(e);
			}

			return -1;
		}

		private int FindGameCode(string name) {
			string responseString = Utils.Get("https://api.thegamesdb.net/v1.1/Games/ByGameName?apikey=" + publicKey + "&name=" + name + "&filter[platform]=" + consoleId.ToString());
			JObject response = JObject.Parse(responseString);

			if (response.Value<int>("code") != 200) { // No success code
				throw new APIException(response.Value<int>("code"));
			} else {
				if (response["data"].Value<int>("count") > 0)
					return response["data"]["games"][0].Value<int>("id");
				else
					return -1;
			}
		}

		// Populates a dictionary of game codes with their respective images' urls
		private void FindCovers(Dictionary<int, GameInfo> gameCodes) {
			string gameCodesString = string.Concat(gameCodes.Keys);

			// Get all games images in a single request
			string responseString = Utils.Get("https://api.thegamesdb.net/v1/Games/Images?apikey=" + publicKey + "&games_id=" + gameCodesString);
			JObject response = JObject.Parse(responseString);

			if (response.Value<int>("code") != 200)// No success code
				throw new APIException(response.Value<int>("code"));

			int[] keys = gameCodes.Keys.ToArray(); // Avoid on-the-fly conflicts due to dictionary modification
			foreach (int code in keys) {
				var availableGameImages = response["data"]["images"][code.ToString()];

				if (availableGameImages == null) {
					Update(new UpdateInfo(UpdateInfo.UpdateType.GameNotFound, gameCodes[code]));
					gameCodes.Remove(code);
					continue;
				}

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
			try {
				foreach (GameInfo game in gameCodes) {
					using (var client = new WebClient()) {
						Uri uri = new Uri(imgURLBase + game.imageAddress);
						string _name = useFileNameForImage ? game.sourceFile : game.name;
						game.imageSaveAddress = Path.Combine(saveDir, _name + Path.GetExtension(game.imageAddress));
						client.DownloadFile(uri, game.imageSaveAddress);
						Update(new UpdateInfo(UpdateInfo.UpdateType.ImageDownload, game));
					}
				}
			} catch (WebException e) {
				ExceptionThrown(e);
			}
		}

		public class UpdateInfo {
			public enum UpdateType {
				GameFound, GameNotFound, ImageDownload
			}
			public UpdateType type;

			public GameInfo game;

			public UpdateInfo(UpdateType type, GameInfo game = null) {
				this.type = type;
				this.game = game;
			}
		}

		public class PhaseInfo {
			public enum Phase {
				Start, FindImages, DownloadImages, Finish
			}
			public Phase phase;

			public int gamesFound;

			public PhaseInfo(Phase phase, int gamesFound) {
				this.phase = phase;
				this.gamesFound = gamesFound;
			}
		}
	}
}
