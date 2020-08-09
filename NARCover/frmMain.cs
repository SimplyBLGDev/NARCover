using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NARCover {
	public partial class frmMain : Form {
        public frmMain() {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e) {
            frmDownloading downloading = new frmDownloading(txtROMsPath.Text, new List<string>(txtExtensions.Text.Split(';')), txtSaveDir.Text);
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
	}
}
