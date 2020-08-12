using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NARCover {
	public partial class frmDownloading : Form {
		Downloader downloader;
		bool done = false;
		List<string> missingGames;

		public frmDownloading(string[] files, List<string> priorityImageTypes, string saveDir, int console,
			string imgURLBase, bool useFilename) {

			InitializeComponent();
			missingGames = new List<string>();
			UpdateStateLabels(0);

			downloader = new Downloader();
			downloader.gameFiles = new List<string>(files);
			downloader.priorityImageTypes = priorityImageTypes;
			downloader.consoleId = console;
			downloader.saveDir = saveDir;
			downloader.imgURLBase = imgURLBase;
			downloader.useFileNameForImage = useFilename;

			downloader.Update += Downloader_Update;
		}

		private void Downloader_Update(Downloader.DownloaderUpdateInfo info) {
			throw new NotImplementedException();
		}

		private void frmDownloading_Shown(object sender, EventArgs e) {
			downloader.Start();
		}

		private void btnExportNotFound_Click(object sender, EventArgs e) {
			string path = Path.Combine(Application.StartupPath, "'Games Not Found_" + DateTime.Now.ToString("u") + ".txt'");
			string export = "Games not found:\n";

			foreach (string entry in missingGames)
				export += "\n" + entry;

			File.WriteAllText(@path, export);

			lblExportStatus.Text = "Games Not Found list written to " + path;
			btnExportNotFound.Enabled = false;
		}

		private void Downloader_OnDone() {
			Invoke(new MethodInvoker(() => {
				MessageBox.Show("Done", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
				btnExportNotFound.Enabled = true;
				done = true;
			}));
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

		private void Downloader_OnStartFindingCovers(int gamesFound) {
			Invoke(new MethodInvoker(() => {
				UpdateStateLabels(1);
			}));
		}

		private void Downloader_OnGameFound(GameInfo gameFound) {
			Invoke(new MethodInvoker(() => {
				lblCurrentDownload.Text = "Found game: " + gameFound.name;
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
				missingGames.Add(e.game);
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

		private void frmDownloading_FormClosing(object sender, FormClosingEventArgs e) {
			DialogResult = DialogResult.OK;
		}
	}
}
