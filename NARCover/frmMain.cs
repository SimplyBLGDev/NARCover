﻿using Newtonsoft.Json.Linq;
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

		private void PopulatePlatformCMB() {
			string responseString = Utils.Get("https://api.thegamesdb.net/v1/Platforms?apikey=" + Downloader.PUBLICKEY); //TODO
			JObject response = JObject.Parse(responseString);

			if (response.Value<int>("code") != 200) { // No success code
				throw new APIException("API Request Error.", response.Value<int>("code"));
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
			string errorMsg = "";
			if (!platformIds.ContainsKey(cmbConsole.Text))
				errorMsg += "Invalid console, pick one from the dropdown list.\n";
			else if (!Directory.Exists(txtROMsPath.Text))
				errorMsg += "ROMs Path invalid.\n";
			else if (!Directory.Exists(txtSaveDir.Text))
				errorMsg += "Images' save dir invalid.\n";
			else if (txtExtensions.Text.Split(';').Length == 0)
				errorMsg += "Select at least one file extension.\n";
			else if (!imageSourceQualities.ContainsKey(cmbQuality.Text))
				errorMsg += "Quality not valid.\n";

			if (errorMsg != "") {
				MessageBox.Show(errorMsg.Trim(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			string romsPath = txtROMsPath.Text;
			List<string> extensions = new List<string>(txtExtensions.Text.Split(';'));
			List<string> priorityList = new List<string>();
			string saveDir = txtSaveDir.Text;

			foreach (string type in lbPriority.Items)
				priorityList.Add(type);

			int console = platformIds[cmbConsole.Text];
			string quality = imageSourceQualities[cmbQuality.Text];
			bool useFilename = rbROMName.Checked;

			frmDownloading downloading = new frmDownloading(romsPath, extensions, priorityList, saveDir, console, quality, useFilename);
			downloading.ShowDialog();
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
	}
}
