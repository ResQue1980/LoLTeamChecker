namespace LoLTeamChecker.Gui.Controls
{
    partial class PlayerControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlayerControl));
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.LevelLabel = new System.Windows.Forms.Label();
            this.LoadingPicture = new System.Windows.Forms.PictureBox();
            this.InfoTabs = new System.Windows.Forms.TabControl();
            this.TeamLabel = new System.Windows.Forms.Label();
            this.NameLabel = new System.Windows.Forms.LinkLabel();
            this.totalKda = new System.Windows.Forms.Label();
            this.champKda = new System.Windows.Forms.Label();
            this.champIcon1 = new System.Windows.Forms.PictureBox();
            this.champIcon2 = new System.Windows.Forms.PictureBox();
            this.champIcon3 = new System.Windows.Forms.PictureBox();
            this.champIcon4 = new System.Windows.Forms.PictureBox();
            this.champIcon5 = new System.Windows.Forms.PictureBox();
            this.totalGames = new System.Windows.Forms.Label();
            this.totalPercentWon = new System.Windows.Forms.Label();
            this.currChampIcon = new System.Windows.Forms.PictureBox();
            this.division = new System.Windows.Forms.Label();
            this.champGames = new System.Windows.Forms.Label();
            this.champPercentWon = new System.Windows.Forms.Label();
            this.acceptedStatus = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.LoadingPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.champIcon1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.champIcon2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.champIcon3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.champIcon4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.champIcon5)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.currChampIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.acceptedStatus)).BeginInit();
            this.SuspendLayout();
            // 
            // LevelLabel
            // 
            this.LevelLabel.AutoSize = true;
            this.LevelLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LevelLabel.Location = new System.Drawing.Point(25, 11);
            this.LevelLabel.Name = "LevelLabel";
            this.LevelLabel.Size = new System.Drawing.Size(21, 13);
            this.LevelLabel.TabIndex = 2;
            this.LevelLabel.Text = "30";
            this.LevelLabel.Visible = false;
            // 
            // LoadingPicture
            // 
            this.LoadingPicture.Image = ((System.Drawing.Image)(resources.GetObject("LoadingPicture.Image")));
            this.LoadingPicture.Location = new System.Drawing.Point(10, 126);
            this.LoadingPicture.Name = "LoadingPicture";
            this.LoadingPicture.Size = new System.Drawing.Size(16, 16);
            this.LoadingPicture.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.LoadingPicture.TabIndex = 0;
            this.LoadingPicture.TabStop = false;
            // 
            // InfoTabs
            // 
            this.InfoTabs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InfoTabs.ItemSize = new System.Drawing.Size(0, 15);
            this.InfoTabs.Location = new System.Drawing.Point(422, 14);
            this.InfoTabs.Multiline = true;
            this.InfoTabs.Name = "InfoTabs";
            this.InfoTabs.SelectedIndex = 0;
            this.InfoTabs.Size = new System.Drawing.Size(200, 138);
            this.InfoTabs.TabIndex = 3;
            this.InfoTabs.Visible = false;
            // 
            // TeamLabel
            // 
            this.TeamLabel.AutoSize = true;
            this.TeamLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TeamLabel.Location = new System.Drawing.Point(25, -2);
            this.TeamLabel.Name = "TeamLabel";
            this.TeamLabel.Size = new System.Drawing.Size(14, 13);
            this.TeamLabel.TabIndex = 4;
            this.TeamLabel.Text = "1";
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameLabel.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.NameLabel.Location = new System.Drawing.Point(38, -2);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(98, 13);
            this.NameLabel.TabIndex = 5;
            this.NameLabel.TabStop = true;
            this.NameLabel.Text = "NAMEEEEEEEE";
            this.NameLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.NameLabel_LinkClicked);
            // 
            // totalKda
            // 
            this.totalKda.AutoSize = true;
            this.totalKda.Location = new System.Drawing.Point(218, -2);
            this.totalKda.Name = "totalKda";
            this.totalKda.Size = new System.Drawing.Size(28, 13);
            this.totalKda.TabIndex = 6;
            this.totalKda.Text = "?.??";
            // 
            // champKda
            // 
            this.champKda.AutoSize = true;
            this.champKda.Location = new System.Drawing.Point(218, 11);
            this.champKda.Name = "champKda";
            this.champKda.Size = new System.Drawing.Size(28, 13);
            this.champKda.TabIndex = 7;
            this.champKda.Text = "?.??";
            // 
            // champIcon1
            // 
            this.champIcon1.Location = new System.Drawing.Point(246, 0);
            this.champIcon1.Margin = new System.Windows.Forms.Padding(0);
            this.champIcon1.Name = "champIcon1";
            this.champIcon1.Size = new System.Drawing.Size(22, 22);
            this.champIcon1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.champIcon1.TabIndex = 8;
            this.champIcon1.TabStop = false;
            // 
            // champIcon2
            // 
            this.champIcon2.Location = new System.Drawing.Point(268, 0);
            this.champIcon2.Margin = new System.Windows.Forms.Padding(0);
            this.champIcon2.Name = "champIcon2";
            this.champIcon2.Size = new System.Drawing.Size(22, 22);
            this.champIcon2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.champIcon2.TabIndex = 9;
            this.champIcon2.TabStop = false;
            // 
            // champIcon3
            // 
            this.champIcon3.Location = new System.Drawing.Point(290, 0);
            this.champIcon3.Margin = new System.Windows.Forms.Padding(0);
            this.champIcon3.Name = "champIcon3";
            this.champIcon3.Size = new System.Drawing.Size(22, 22);
            this.champIcon3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.champIcon3.TabIndex = 10;
            this.champIcon3.TabStop = false;
            // 
            // champIcon4
            // 
            this.champIcon4.Location = new System.Drawing.Point(312, 0);
            this.champIcon4.Margin = new System.Windows.Forms.Padding(0);
            this.champIcon4.Name = "champIcon4";
            this.champIcon4.Size = new System.Drawing.Size(22, 22);
            this.champIcon4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.champIcon4.TabIndex = 11;
            this.champIcon4.TabStop = false;
            // 
            // champIcon5
            // 
            this.champIcon5.Location = new System.Drawing.Point(334, 0);
            this.champIcon5.Margin = new System.Windows.Forms.Padding(0);
            this.champIcon5.Name = "champIcon5";
            this.champIcon5.Size = new System.Drawing.Size(22, 22);
            this.champIcon5.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.champIcon5.TabIndex = 12;
            this.champIcon5.TabStop = false;
            // 
            // totalGames
            // 
            this.totalGames.AutoSize = true;
            this.totalGames.ForeColor = System.Drawing.SystemColors.ControlText;
            this.totalGames.Location = new System.Drawing.Point(156, -2);
            this.totalGames.Name = "totalGames";
            this.totalGames.Size = new System.Drawing.Size(31, 13);
            this.totalGames.TabIndex = 13;
            this.totalGames.Text = "????";
            // 
            // totalPercentWon
            // 
            this.totalPercentWon.AutoSize = true;
            this.totalPercentWon.Location = new System.Drawing.Point(184, -2);
            this.totalPercentWon.Name = "totalPercentWon";
            this.totalPercentWon.Size = new System.Drawing.Size(36, 13);
            this.totalPercentWon.TabIndex = 14;
            this.totalPercentWon.Text = "??.?%";
            // 
            // currChampIcon
            // 
            this.currChampIcon.Location = new System.Drawing.Point(136, 0);
            this.currChampIcon.Margin = new System.Windows.Forms.Padding(0);
            this.currChampIcon.Name = "currChampIcon";
            this.currChampIcon.Size = new System.Drawing.Size(22, 22);
            this.currChampIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.currChampIcon.TabIndex = 15;
            this.currChampIcon.TabStop = false;
            // 
            // division
            // 
            this.division.AutoSize = true;
            this.division.ForeColor = System.Drawing.SystemColors.ControlText;
            this.division.Location = new System.Drawing.Point(52, 11);
            this.division.Name = "division";
            this.division.Size = new System.Drawing.Size(31, 13);
            this.division.TabIndex = 16;
            this.division.Text = "????";
            // 
            // champGames
            // 
            this.champGames.AutoSize = true;
            this.champGames.ForeColor = System.Drawing.SystemColors.ControlText;
            this.champGames.Location = new System.Drawing.Point(156, 11);
            this.champGames.Name = "champGames";
            this.champGames.Size = new System.Drawing.Size(31, 13);
            this.champGames.TabIndex = 17;
            this.champGames.Text = "????";
            // 
            // champPercentWon
            // 
            this.champPercentWon.AutoSize = true;
            this.champPercentWon.Location = new System.Drawing.Point(184, 11);
            this.champPercentWon.Name = "champPercentWon";
            this.champPercentWon.Size = new System.Drawing.Size(36, 13);
            this.champPercentWon.TabIndex = 18;
            this.champPercentWon.Text = "??.?%";
            // 
            // acceptedStatus
            // 
            this.acceptedStatus.Location = new System.Drawing.Point(0, 2);
            this.acceptedStatus.Margin = new System.Windows.Forms.Padding(0);
            this.acceptedStatus.Name = "acceptedStatus";
            this.acceptedStatus.Size = new System.Drawing.Size(22, 22);
            this.acceptedStatus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.acceptedStatus.TabIndex = 19;
            this.acceptedStatus.TabStop = false;
            // 
            // PlayerControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.acceptedStatus);
            this.Controls.Add(this.currChampIcon);
            this.Controls.Add(this.champPercentWon);
            this.Controls.Add(this.champGames);
            this.Controls.Add(this.division);
            this.Controls.Add(this.totalPercentWon);
            this.Controls.Add(this.totalGames);
            this.Controls.Add(this.champIcon5);
            this.Controls.Add(this.champIcon4);
            this.Controls.Add(this.champIcon3);
            this.Controls.Add(this.champIcon2);
            this.Controls.Add(this.champIcon1);
            this.Controls.Add(this.champKda);
            this.Controls.Add(this.totalKda);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.TeamLabel);
            this.Controls.Add(this.InfoTabs);
            this.Controls.Add(this.LoadingPicture);
            this.Controls.Add(this.LevelLabel);
            this.Name = "PlayerControl";
            this.Size = new System.Drawing.Size(366, 28);
            ((System.ComponentModel.ISupportInitialize)(this.LoadingPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.champIcon1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.champIcon2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.champIcon3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.champIcon4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.champIcon5)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.currChampIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.acceptedStatus)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

		private System.Windows.Forms.ToolTip ToolTip;
		private System.Windows.Forms.Label LevelLabel;
		private System.Windows.Forms.PictureBox LoadingPicture;
		public System.Windows.Forms.TabControl InfoTabs;
		private System.Windows.Forms.Label TeamLabel;
        private System.Windows.Forms.LinkLabel NameLabel;
        private System.Windows.Forms.Label totalKda;
        private System.Windows.Forms.Label champKda;
        private System.Windows.Forms.PictureBox champIcon1;
        private System.Windows.Forms.PictureBox champIcon2;
        private System.Windows.Forms.PictureBox champIcon3;
        private System.Windows.Forms.PictureBox champIcon4;
        private System.Windows.Forms.PictureBox champIcon5;
        private System.Windows.Forms.Label totalGames;
        private System.Windows.Forms.Label totalPercentWon;
        private System.Windows.Forms.PictureBox currChampIcon;
        private System.Windows.Forms.Label division;
        private System.Windows.Forms.Label champGames;
        private System.Windows.Forms.Label champPercentWon;
        private System.Windows.Forms.PictureBox acceptedStatus;
    }
}
