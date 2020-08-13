using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace NARCover {
	public partial class frmMain : Form {
		public Dictionary<string, int> platformIds = new Dictionary<string, int>();
		public Dictionary<string, string> imageSourceQualities = new Dictionary<string, string>() {
			{ "Original (Max quality)", "https://cdn.thegamesdb.net/images/original/" },
			{ "Large", "https://cdn.thegamesdb.net/images/large/" },
			{ "Medium", "https://cdn.thegamesdb.net/images/medium/" },
			{ "Small", "https://cdn.thegamesdb.net/images/small/" },
			{ "Thumb", "https://cdn.thegamesdb.net/images/thumb/" },
			{ "Cropped center thumb", "https://cdn.thegamesdb.net/images/cropped_center_thumb/" }
		};

		string quality { get { return imageSourceQualities[cmbQuality.Text]; } }
		bool useFolderName { get { return chkUseFolderAsName.Checked; } }
		int console { get { return platformIds[cmbConsole.Text]; } }
		bool useFilename { get { return rbROMName.Checked; } }
		string romsPath { get { return txtROMsPath.Text; } }
		bool subdirs { get { return chkSubdir.Checked; } }

		public frmMain() {
			InitializeComponent();
			fbdSaveDir.SelectedPath = Path.Combine(Application.StartupPath, "images");
			txtSaveDir.Text = fbdSaveDir.SelectedPath;
		}

		private void frmMain_Shown(object sender, EventArgs e) {
			try {
				PopulatePlatformCMB();
			} catch (APIException ex) {
				if (MessageBox.Show("API request error, the games DB might be offline or API outdated, check https://thegamesdb.net or " +
					"contact me at https://github.com/SimplyBLGDev/NARCover.", "API Exception", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
					Close();
			}

			PopulateQualityCMB();
		}

		private void OpenDownloader() {
			if (!ValidateUserValues())
				return;

			string[] validFiles = Utils.GetValidFiles(romsPath, GetExtensions(), useFolderName, subdirs);

			frmDownloading downloading = new frmDownloading(validFiles, GetPriorityList(), GetSaveDir(), console, quality, useFilename);
			Hide();
			if (downloading.ShowDialog() == DialogResult.OK)
				Close();
			else
				Show();
		}

		string GetSaveDir() {
			if (txtSaveDir.Text == "")
				return Path.Combine(Application.StartupPath, "images");
			return txtSaveDir.Text;
		}

		List<string> GetExtensions() {
			return new List<string>(txtExtensions.Text.Split(';'));
		}

		List<string> GetPriorityList() {
			List<string> r = new List<string>();
			foreach (string entry in lbPriority.Items)
				r.Add(entry);
			return r;
		}

		private bool ValidateUserValues() {
			string errorMsg = "";
			if (!platformIds.ContainsKey(cmbConsole.Text))
				errorMsg += "Invalid console, pick one from the dropdown list.\n";
			else if (!Directory.Exists(txtROMsPath.Text))
				errorMsg += "ROMs Path invalid.\n";
			else if (!Directory.Exists(GetSaveDir()))
				errorMsg += "Images' save dir invalid.\n";
			else if (txtExtensions.Text.Split(';').Length == 0)
				errorMsg += "Select at least one file extension.\n";
			else if (!imageSourceQualities.ContainsKey(cmbQuality.Text))
				errorMsg += "Quality not valid.\n";

			if (errorMsg != "") {
				MessageBox.Show(errorMsg.Trim(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
			return true;
		}

		private void PopulatePlatformCMB() {
			string responseString = Utils.Get("https://api.thegamesdb.net/v1/Platforms?apikey=" + Downloader.PUBLICKEY); //TODO
			JObject response = JObject.Parse(responseString);

			if (response.Value<int>("code") != 200) { // No success code
				throw new APIException(response.Value<int>("code"));
			} else {
				cmbConsole.Items.Clear();
				foreach (JToken platform in response["data"]["platforms"].Children()) {
					cmbConsole.Items.Add(platform.First.Value<string>("name"));
					platformIds.Add(platform.First.Value<string>("name"), platform.First.Value<int>("id"));
				}
			}

			cmbConsole.SelectedIndex = 0;
		}

		private void PopulateQualityCMB() {
			cmbQuality.Items.Clear();
			foreach (string entry in imageSourceQualities.Keys)
				cmbQuality.Items.Add(entry);
			cmbQuality.SelectedIndex = 0;
		}

		private void btnOK_Click(object sender, EventArgs e) {
			OpenDownloader();
		}

		private void btnCancel_Click(object sender, EventArgs e) {
			Close();
		}

		private void btnRomsPath_Click(object sender, EventArgs e) {
			if (fbdROMsPath.ShowDialog() == DialogResult.OK)
				txtROMsPath.Text = fbdROMsPath.SelectedPath;
		}

		private void btnSaveDir_Click(object sender, EventArgs e) {
			if (fbdSaveDir.ShowDialog() == DialogResult.OK)
				txtSaveDir.Text = fbdSaveDir.SelectedPath;
		}

		private void btnPriorityUp_Click(object sender, EventArgs e) {
			int index = lbPriority.SelectedIndex;
			if (index <= 0)
				return;

			string item = lbPriority.Items[index] as string;
			lbPriority.Items[index] = lbPriority.Items[index - 1];
			lbPriority.Items[index - 1] = item;

			lbPriority.SelectedIndex--;
		}

		private void btnPriorityDown_Click(object sender, EventArgs e) {
			int index = lbPriority.SelectedIndex;
			if (index == -1) // No selection
				return;
			if (index >= lbPriority.Items.Count-1)
				return;

			string item = lbPriority.Items[index] as string;
			lbPriority.Items[index] = lbPriority.Items[index + 1];
			lbPriority.Items[index + 1] = item;

			lbPriority.SelectedIndex++;
		}

		private void chkUseFolderAsName_CheckedChanged(object sender, EventArgs e) {
			chkSubdir.Enabled = !chkUseFolderAsName.Checked;
			chkSubdir.Checked = false;
			txtExtensions.Enabled = !chkUseFolderAsName.Checked;
			lblExtensions.Enabled = !chkUseFolderAsName.Checked;
		}

		private void chkSubdir_CheckedChanged(object sender, EventArgs e) {
			chkUseFolderAsName.Enabled = !chkSubdir.Checked;
			chkUseFolderAsName.Checked = false;
		}
	}
}
