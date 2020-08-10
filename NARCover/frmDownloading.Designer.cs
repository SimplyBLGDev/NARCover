namespace NARCover {
	partial class frmDownloading {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.lblCurrentDownload = new System.Windows.Forms.Label();
			this.pbProgress = new System.Windows.Forms.ProgressBar();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.pbImagePreview = new System.Windows.Forms.PictureBox();
			this.lblPreviewGameName = new System.Windows.Forms.Label();
			this.tableLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pbImagePreview)).BeginInit();
			this.SuspendLayout();
			// 
			// lblCurrentDownload
			// 
			this.lblCurrentDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblCurrentDownload.AutoEllipsis = true;
			this.lblCurrentDownload.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.lblCurrentDownload.Location = new System.Drawing.Point(9, 380);
			this.lblCurrentDownload.Name = "lblCurrentDownload";
			this.lblCurrentDownload.Size = new System.Drawing.Size(296, 13);
			this.lblCurrentDownload.TabIndex = 3;
			this.lblCurrentDownload.Text = "-";
			this.lblCurrentDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pbProgress
			// 
			this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pbProgress.Location = new System.Drawing.Point(9, 396);
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new System.Drawing.Size(296, 23);
			this.pbProgress.TabIndex = 2;
			this.pbProgress.Value = 50;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 68.82662F));
			this.tableLayoutPanel1.Controls.Add(this.pbImagePreview, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.lblPreviewGameName, 0, 1);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 13);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80.54393F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 19F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 19.45607F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(296, 364);
			this.tableLayoutPanel1.TabIndex = 4;
			// 
			// pbImagePreview
			// 
			this.pbImagePreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pbImagePreview.Location = new System.Drawing.Point(3, 3);
			this.pbImagePreview.Name = "pbImagePreview";
			this.pbImagePreview.Size = new System.Drawing.Size(290, 255);
			this.pbImagePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbImagePreview.TabIndex = 0;
			this.pbImagePreview.TabStop = false;
			// 
			// lblPreviewGameName
			// 
			this.lblPreviewGameName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblPreviewGameName.AutoEllipsis = true;
			this.lblPreviewGameName.Location = new System.Drawing.Point(3, 261);
			this.lblPreviewGameName.Name = "lblPreviewGameName";
			this.lblPreviewGameName.Size = new System.Drawing.Size(290, 19);
			this.lblPreviewGameName.TabIndex = 1;
			this.lblPreviewGameName.Text = "-";
			this.lblPreviewGameName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// frmDownloading
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(317, 431);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.lblCurrentDownload);
			this.Controls.Add(this.pbProgress);
			this.Name = "frmDownloading";
			this.Text = "Downloading...";
			this.Shown += new System.EventHandler(this.frmDownloading_Shown);
			this.tableLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pbImagePreview)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblCurrentDownload;
		private System.Windows.Forms.ProgressBar pbProgress;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.PictureBox pbImagePreview;
		private System.Windows.Forms.Label lblPreviewGameName;
	}
}