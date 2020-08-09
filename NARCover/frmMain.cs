using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NARCover {
	public partial class frmMain : Form {
        public frmMain() {
            InitializeComponent();
        }

        private void btnRomsPath_Click(object sender, EventArgs e) {
            if (fbdROMsPath.ShowDialog() == DialogResult.OK)
                txtROMsPath.Text = fbdROMsPath.SelectedPath;
        }

        private void btnOK_Click(object sender, EventArgs e) {
            frmDownloading downloading = new frmDownloading(txtROMsPath.Text, new List<string>(txtExtensions.Text.Split(';')));
            downloading.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e) {
            Close();
		}
	}
}
