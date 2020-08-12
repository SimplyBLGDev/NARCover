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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDownloading));
			this.lblCurrentDownload = new System.Windows.Forms.Label();
			this.pbProgress = new System.Windows.Forms.ProgressBar();
			this.lblPreviewGameName = new System.Windows.Forms.Label();
			this.pbImagePreview = new System.Windows.Forms.PictureBox();
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.lblState2 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.lblState1 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.lblState0 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.lvMissingGames = new System.Windows.Forms.ListView();
			this.btnExportNotFound = new System.Windows.Forms.Button();
			this.lblExportStatus = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.pbImagePreview)).BeginInit();
			this.tableLayoutPanel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.SuspendLayout();
			// 
			// lblCurrentDownload
			// 
			this.lblCurrentDownload.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblCurrentDownload.AutoEllipsis = true;
			this.lblCurrentDownload.Cursor = System.Windows.Forms.Cursors.Arrow;
			this.lblCurrentDownload.Location = new System.Drawing.Point(9, 360);
			this.lblCurrentDownload.Name = "lblCurrentDownload";
			this.lblCurrentDownload.Size = new System.Drawing.Size(402, 13);
			this.lblCurrentDownload.TabIndex = 2;
			this.lblCurrentDownload.Text = "-";
			this.lblCurrentDownload.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// pbProgress
			// 
			this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pbProgress.Location = new System.Drawing.Point(9, 376);
			this.pbProgress.Maximum = 10;
			this.pbProgress.Name = "pbProgress";
			this.pbProgress.Size = new System.Drawing.Size(402, 23);
			this.pbProgress.TabIndex = 0;
			// 
			// lblPreviewGameName
			// 
			this.lblPreviewGameName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblPreviewGameName.AutoEllipsis = true;
			this.lblPreviewGameName.Location = new System.Drawing.Point(183, 317);
			this.lblPreviewGameName.Margin = new System.Windows.Forms.Padding(3);
			this.lblPreviewGameName.Name = "lblPreviewGameName";
			this.lblPreviewGameName.Size = new System.Drawing.Size(216, 24);
			this.lblPreviewGameName.TabIndex = 5;
			this.lblPreviewGameName.Text = "-";
			this.lblPreviewGameName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// pbImagePreview
			// 
			this.pbImagePreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pbImagePreview.Location = new System.Drawing.Point(183, 33);
			this.pbImagePreview.Name = "pbImagePreview";
			this.tableLayoutPanel1.SetRowSpan(this.pbImagePreview, 2);
			this.pbImagePreview.Size = new System.Drawing.Size(216, 278);
			this.pbImagePreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pbImagePreview.TabIndex = 0;
			this.pbImagePreview.TabStop = false;
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.ColumnCount = 3;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.Controls.Add(this.pbImagePreview, 2, 1);
			this.tableLayoutPanel1.Controls.Add(this.lblPreviewGameName, 2, 3);
			this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.label3, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.lvMissingGames, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.btnExportNotFound, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.lblExportStatus, 1, 3);
			this.tableLayoutPanel1.Location = new System.Drawing.Point(9, 13);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 4;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(402, 344);
			this.tableLayoutPanel1.TabIndex = 1;
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel2.ColumnCount = 5;
			this.tableLayoutPanel1.SetColumnSpan(this.tableLayoutPanel2, 3);
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5F));
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this.tableLayoutPanel2.Controls.Add(this.lblState2, 4, 0);
			this.tableLayoutPanel2.Controls.Add(this.label2, 3, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblState1, 2, 0);
			this.tableLayoutPanel2.Controls.Add(this.label1, 1, 0);
			this.tableLayoutPanel2.Controls.Add(this.lblState0, 0, 0);
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 1;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(402, 30);
			this.tableLayoutPanel2.TabIndex = 0;
			// 
			// lblState2
			// 
			this.lblState2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblState2.AutoSize = true;
			this.lblState2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblState2.Location = new System.Drawing.Point(283, 0);
			this.lblState2.Name = "lblState2";
			this.lblState2.Size = new System.Drawing.Size(116, 30);
			this.lblState2.TabIndex = 4;
			this.lblState2.Text = "Downloading images";
			this.lblState2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(263, 0);
			this.label2.MinimumSize = new System.Drawing.Size(10, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(14, 30);
			this.label2.TabIndex = 3;
			this.label2.Text = ">";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblState1
			// 
			this.lblState1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblState1.AutoSize = true;
			this.lblState1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblState1.Location = new System.Drawing.Point(143, 0);
			this.lblState1.Name = "lblState1";
			this.lblState1.Size = new System.Drawing.Size(114, 30);
			this.lblState1.TabIndex = 2;
			this.lblState1.Text = "Finding images URLs";
			this.lblState1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(123, 0);
			this.label1.MinimumSize = new System.Drawing.Size(10, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(14, 30);
			this.label1.TabIndex = 1;
			this.label1.Text = ">";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// lblState0
			// 
			this.lblState0.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblState0.AutoSize = true;
			this.lblState0.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.lblState0.Location = new System.Drawing.Point(3, 0);
			this.lblState0.Name = "lblState0";
			this.lblState0.Size = new System.Drawing.Size(114, 30);
			this.lblState0.TabIndex = 0;
			this.lblState0.Text = "Finding game titles";
			this.lblState0.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoEllipsis = true;
			this.tableLayoutPanel1.SetColumnSpan(this.label3, 2);
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(3, 33);
			this.label3.Margin = new System.Windows.Forms.Padding(3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(174, 24);
			this.label3.TabIndex = 1;
			this.label3.Text = "Games not found:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// lvMissingGames
			// 
			this.lvMissingGames.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tableLayoutPanel1.SetColumnSpan(this.lvMissingGames, 2);
			this.lvMissingGames.FullRowSelect = true;
			this.lvMissingGames.HideSelection = false;
			this.lvMissingGames.LabelWrap = false;
			this.lvMissingGames.Location = new System.Drawing.Point(3, 63);
			this.lvMissingGames.MultiSelect = false;
			this.lvMissingGames.Name = "lvMissingGames";
			this.lvMissingGames.Size = new System.Drawing.Size(174, 248);
			this.lvMissingGames.TabIndex = 2;
			this.lvMissingGames.UseCompatibleStateImageBehavior = false;
			this.lvMissingGames.View = System.Windows.Forms.View.List;
			// 
			// btnExportNotFound
			// 
			this.btnExportNotFound.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.btnExportNotFound.Enabled = false;
			this.btnExportNotFound.Location = new System.Drawing.Point(3, 317);
			this.btnExportNotFound.Name = "btnExportNotFound";
			this.btnExportNotFound.Size = new System.Drawing.Size(84, 24);
			this.btnExportNotFound.TabIndex = 3;
			this.btnExportNotFound.Text = "Save as .txt";
			this.btnExportNotFound.UseVisualStyleBackColor = true;
			this.btnExportNotFound.Click += new System.EventHandler(this.btnExportNotFound_Click);
			// 
			// lblExportStatus
			// 
			this.lblExportStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lblExportStatus.AutoSize = true;
			this.lblExportStatus.Location = new System.Drawing.Point(93, 317);
			this.lblExportStatus.Margin = new System.Windows.Forms.Padding(3);
			this.lblExportStatus.Name = "lblExportStatus";
			this.lblExportStatus.Size = new System.Drawing.Size(84, 24);
			this.lblExportStatus.TabIndex = 4;
			this.lblExportStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// frmDownloading
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(423, 411);
			this.Controls.Add(this.tableLayoutPanel1);
			this.Controls.Add(this.lblCurrentDownload);
			this.Controls.Add(this.pbProgress);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MinimumSize = new System.Drawing.Size(370, 400);
			this.Name = "frmDownloading";
			this.Text = "Downloading...";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmDownloading_FormClosing);
			this.Shown += new System.EventHandler(this.frmDownloading_Shown);
			((System.ComponentModel.ISupportInitialize)(this.pbImagePreview)).EndInit();
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.tableLayoutPanel2.ResumeLayout(false);
			this.tableLayoutPanel2.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label lblCurrentDownload;
		private System.Windows.Forms.ProgressBar pbProgress;
		private System.Windows.Forms.Label lblPreviewGameName;
		private System.Windows.Forms.PictureBox pbImagePreview;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
		private System.Windows.Forms.Label lblState2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label lblState1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label lblState0;
		private System.Windows.Forms.ListView lvMissingGames;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnExportNotFound;
		private System.Windows.Forms.Label lblExportStatus;
	}
}