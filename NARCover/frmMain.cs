using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NARCover {
	public partial class frmMain : Form {
		public Dictionary<string, int> platformIds = new Dictionary<string, int>();

		public frmMain() {
			InitializeComponent();
			PopulatePlatformCMB();
		}

		private void PopulatePlatformCMB() {
			string responseString = Utils.Get("https://api.thegamesdb.net/v1/Platforms?apikey=" + Downloader.PUBLICKEY); //TODO
			JObject response = JObject.Parse(responseString);

			if (response.Value<int>("code") != 200) { // No success code
				throw new APIException("API Request Error.", response.Value<int>("code"));
			} else {
				foreach (JToken platform in response["data"]["platforms"].Children()) {
					cmbConsole.Items.Add(platform.First.Value<string>("name"));
					platformIds.Add(platform.First.Value<string>("name"), platform.First.Value<int>("id"));
				}
			}
		}

		private void btnOK_Click(object sender, EventArgs e) {
			string romsPath = txtROMsPath.Text;
			List<string> extensions = new List<string>(txtExtensions.Text.Split(';'));
			List<string> priorityList = new List<string>();
			foreach (string type in lbPriority.Items)
				priorityList.Add(type);
			string saveDir = txtSaveDir.Text;
			int console = platformIds[cmbConsole.Text];

			frmDownloading downloading = new frmDownloading(romsPath, extensions, priorityList, saveDir, console);
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
			if (index >= lbPriority.Items.Count-1)
				return;

			string item = lbPriority.Items[index] as string;
			lbPriority.Items[index] = lbPriority.Items[index + 1];
			lbPriority.Items[index + 1] = item;

			lbPriority.SelectedIndex++;
		}
	}
}
