using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace NARCover {
	public partial class frmDownloading : Form {
		Downloader downloader;

		public frmDownloading(string romsPath, List<string> extensions, List<string> priorityImageTypes, string saveDir, int console) {
			InitializeComponent();
			UpdateStateLabels(0);

			downloader = new Downloader();
			downloader.romsPath = romsPath;
			downloader.extensions = extensions;
			downloader.priorityImageTypes = priorityImageTypes;
			downloader.consoleId = console;
			if (saveDir == "")
				downloader.saveDir = Application.StartupPath + "/images/";
			else
				downloader.saveDir = saveDir;
		}

		private void UpdateStateLabels(int newState) {
			Label currentStateLabel;
			Label[] stateLabels = new Label[] { lblState0, lblState1, lblState2};

			currentStateLabel = stateLabels[newState];

			for (int i = 0; i < stateLabels.Length; i++)
				if (i == newState)
					stateLabels[i].Font = new Font(stateLabels[i].Font.FontFamily, stateLabels[i].Font.Size, FontStyle.Bold);
				else
					stateLabels[i].Font = new Font(stateLabels[i].Font.FontFamily, stateLabels[i].Font.Size, FontStyle.Regular);
		}

		private void frmDownloading_Shown(object sender, EventArgs e) {
			downloader.OnAPIException += Downloader_APIException;
			downloader.OnGameNotFound += Downloader_GameNotFound;
			downloader.OnImageDownloaded += Downloader_ImageDownloaded;
			downloader.OnStartDownload += Downloader_OnStartDownload;
			downloader.OnStartFindingCovers += Downloader_OnStartFindingCovers;
			downloader.Start();
		}

		private void Downloader_OnStartFindingCovers(int gamesFound) {
			Invoke(new MethodInvoker(() => {
				UpdateStateLabels(1);
			}));
		}

		private void Downloader_OnStartDownload(int gamesFound) {
			Invoke(new MethodInvoker(() => {
				UpdateStateLabels(2);
				pbProgress.Maximum = gamesFound;
			}));
		}

		private void Downloader_ImageDownloaded(GameInfo game, string imagePath) {
			Invoke(new MethodInvoker(() => {
				pbProgress.Value++;
				lblPreviewGameName.Text = game.name;
				pbImagePreview.ImageLocation = imagePath;
				lblCurrentDownload.Text = "Downloaded " + game.name + " to " + imagePath;
			}));
		}

		private void Downloader_GameNotFound(GameNotFoundException e) {
			Invoke(new MethodInvoker(() => {
				lvMissingGames.Items.Add(e.game);
			}));
		}

		private void Downloader_APIException(APIException e) {
			switch (MessageBox.Show("API request error, the games DB might be offline or API outdated, check https://thegamesdb.net or " +
					"contact me at https://github.com/SimplyBLGDev/NARCover.", "API Exception", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)) {
				case DialogResult.Cancel:
					Close(); // TODO
					break;
				case DialogResult.OK:
					break;
			}
		}
	}
}
