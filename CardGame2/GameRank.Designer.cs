﻿namespace CardGame2
{
    partial class GameRank
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnToExcelSave = new MaterialSkin.Controls.MaterialRaisedButton();
            this.btnExcelOpen = new MaterialSkin.Controls.MaterialRaisedButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(5, 141);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.Size = new System.Drawing.Size(569, 329);
            this.dataGridView1.TabIndex = 2;
            // 
            // btnToExcelSave
            // 
            this.btnToExcelSave.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnToExcelSave.Depth = 0;
            this.btnToExcelSave.Location = new System.Drawing.Point(75, 75);
            this.btnToExcelSave.MaximumSize = new System.Drawing.Size(185, 60);
            this.btnToExcelSave.MinimumSize = new System.Drawing.Size(185, 60);
            this.btnToExcelSave.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnToExcelSave.Name = "btnToExcelSave";
            this.btnToExcelSave.Primary = true;
            this.btnToExcelSave.Size = new System.Drawing.Size(185, 60);
            this.btnToExcelSave.TabIndex = 4;
            this.btnToExcelSave.Text = "게임순위 파일 저장하기";
            this.btnToExcelSave.UseVisualStyleBackColor = true;
            this.btnToExcelSave.Click += new System.EventHandler(this.btnToExcelSave_Click);
            // 
            // btnExcelOpen
            // 
            this.btnExcelOpen.AutoSize = true;
            this.btnExcelOpen.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.btnExcelOpen.Depth = 0;
            this.btnExcelOpen.Location = new System.Drawing.Point(333, 75);
            this.btnExcelOpen.MaximumSize = new System.Drawing.Size(185, 60);
            this.btnExcelOpen.MinimumSize = new System.Drawing.Size(185, 60);
            this.btnExcelOpen.MouseState = MaterialSkin.MouseState.HOVER;
            this.btnExcelOpen.Name = "btnExcelOpen";
            this.btnExcelOpen.Primary = true;
            this.btnExcelOpen.Size = new System.Drawing.Size(185, 60);
            this.btnExcelOpen.TabIndex = 5;
            this.btnExcelOpen.Text = "게임순위 파일 열어보기";
            this.btnExcelOpen.UseVisualStyleBackColor = true;
            this.btnExcelOpen.Click += new System.EventHandler(this.btnExcelOpen_Click);
            // 
            // GameRank
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 480);
            this.Controls.Add(this.btnExcelOpen);
            this.Controls.Add(this.btnToExcelSave);
            this.Controls.Add(this.dataGridView1);
            this.MaximumSize = new System.Drawing.Size(580, 480);
            this.MinimumSize = new System.Drawing.Size(580, 480);
            this.Name = "GameRank";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "게임순위";
            this.Load += new System.EventHandler(this.GameRank_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView1;
        private MaterialSkin.Controls.MaterialRaisedButton btnToExcelSave;
        private MaterialSkin.Controls.MaterialRaisedButton btnExcelOpen;
    }
}