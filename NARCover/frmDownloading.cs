using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace NARCover {
	public partial class frmDownloading : Form {
		Downloader downloader;
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
			downloader.PhaseChange += Downloader_PhaseChange;
			downloader.ExceptionThrown += Downloader_ExceptionThrown;
		}

		private void Downloader_ExceptionThrown(Exception e) {
			Invoke(new MethodInvoker(() => {
				if (e.GetType() == typeof(APIException))
					switch (MessageBox.Show("API request error, the games DB might be offline or API outdated, check https://thegamesdb.net or " +
						"contact me at https://github.com/SimplyBLGDev/NARCover.", "API Exception", MessageBoxButtons.OKCancel, MessageBoxIcon.Error)) {
						case DialogResult.Cancel:
							Close(); // TODO
							break;
						case DialogResult.OK:
							break;
					}
			}));
		}

		private void Downloader_PhaseChange(Downloader.PhaseInfo info) {
			Invoke(new MethodInvoker(() => {
				switch (info.phase) {
					case Downloader.PhaseInfo.Phase.Start:
						UpdateStateLabels(0);
						break;
					case Downloader.PhaseInfo.Phase.FindImages:
						UpdateStateLabels(1);
						break;
					case Downloader.PhaseInfo.Phase.DownloadImages:
						UpdateStateLabels(2);
						pbProgress.Maximum = info.gamesFound;
						break;
					case Downloader.PhaseInfo.Phase.Finish:
						MessageBox.Show("Done", "Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
						btnExportNotFound.Enabled = true;
						break;
				}
			}));
		}

		private void Downloader_Update(Downloader.UpdateInfo info) {
			Invoke(new MethodInvoker(() => {
				switch (info.type) {
					case Downloader.UpdateInfo.UpdateType.GameFound:
						lblCurrentDownload.Text = "Found game: " + info.game.name;
						break;
					case Downloader.UpdateInfo.UpdateType.GameNotFound:
						missingGames.Add(info.game.name);
						lvMissingGames.Items.Add(info.game.name);
						break;
					case Downloader.UpdateInfo.UpdateType.ImageDownload:
						pbProgress.Value++;
						lblPreviewGameName.Text = info.game.name;
						pbImagePreview.ImageLocation = info.game.imageAddress;
						lblCurrentDownload.Text = "Downloaded " + info.game.name + " to " + info.game.imageAddress;
						break;
				}
			}));
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

		private void UpdateStateLabels(int newState) {
			Label[] stateLabels = new Label[] { lblState0, lblState1, lblState2};

			for (int i = 0; i < stateLabels.Length; i++)
				if (i == newState)
					stateLabels[i].Font = new Font(stateLabels[i].Font.FontFamily, stateLabels[i].Font.Size, FontStyle.Bold);
				else
					stateLabels[i].Font = new Font(stateLabels[i].Font.FontFamily, stateLabels[i].Font.Size, FontStyle.Regular);
		}

		private void frmDownloading_FormClosing(object sender, FormClosingEventArgs e) {
			DialogResult = DialogResult.OK;
		}
	}
}
