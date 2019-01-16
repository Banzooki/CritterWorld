﻿namespace CritterWorld
{
    partial class CritterScorePanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CritterScorePanel));
            this.labelScorePrompt = new System.Windows.Forms.Label();
            this.labelScore = new System.Windows.Forms.Label();
            this.progressBarEnergy = new System.Windows.Forms.ProgressBar();
            this.progressBarHealth = new System.Windows.Forms.ProgressBar();
            this.labelName = new System.Windows.Forms.Label();
            this.labelAuthor = new System.Windows.Forms.Label();
            this.labelHealth = new System.Windows.Forms.Label();
            this.labelEnergy = new System.Windows.Forms.Label();
            this.labelEscaped = new System.Windows.Forms.Label();
            this.labelDead = new System.Windows.Forms.Label();
            this.spriteSurfaceCritter = new SCG.TurboSprite.SpriteSurface(this.components);
            this.labelNumber = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelScorePrompt
            // 
            this.labelScorePrompt.AutoSize = true;
            this.labelScorePrompt.Font = new System.Drawing.Font("Arial Narrow", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelScorePrompt.Location = new System.Drawing.Point(79, 1);
            this.labelScorePrompt.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelScorePrompt.Name = "labelScorePrompt";
            this.labelScorePrompt.Size = new System.Drawing.Size(26, 13);
            this.labelScorePrompt.TabIndex = 1;
            this.labelScorePrompt.Text = "Score:";
            // 
            // labelScore
            // 
            this.labelScore.AutoSize = true;
            this.labelScore.Font = new System.Drawing.Font("Arial Narrow", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelScore.Location = new System.Drawing.Point(109, 2);
            this.labelScore.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelScore.Name = "labelScore";
            this.labelScore.Size = new System.Drawing.Size(33, 13);
            this.labelScore.TabIndex = 2;
            this.labelScore.Text = "999/999";
            // 
            // progressBarEnergy
            // 
            this.progressBarEnergy.Location = new System.Drawing.Point(264, 7);
            this.progressBarEnergy.Margin = new System.Windows.Forms.Padding(2);
            this.progressBarEnergy.Name = "progressBarEnergy";
            this.progressBarEnergy.Size = new System.Drawing.Size(34, 7);
            this.progressBarEnergy.TabIndex = 6;
            // 
            // progressBarHealth
            // 
            this.progressBarHealth.Location = new System.Drawing.Point(264, 17);
            this.progressBarHealth.Margin = new System.Windows.Forms.Padding(2);
            this.progressBarHealth.Name = "progressBarHealth";
            this.progressBarHealth.Size = new System.Drawing.Size(34, 7);
            this.progressBarHealth.TabIndex = 7;
            // 
            // labelName
            // 
            this.labelName.Font = new System.Drawing.Font("Arial Narrow", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelName.Location = new System.Drawing.Point(48, 14);
            this.labelName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(186, 11);
            this.labelName.TabIndex = 8;
            this.labelName.Text = "Name";
            // 
            // labelAuthor
            // 
            this.labelAuthor.Font = new System.Drawing.Font("Arial Narrow", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelAuthor.Location = new System.Drawing.Point(48, 25);
            this.labelAuthor.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelAuthor.Name = "labelAuthor";
            this.labelAuthor.Size = new System.Drawing.Size(186, 13);
            this.labelAuthor.TabIndex = 9;
            this.labelAuthor.Text = "Author";
            // 
            // labelHealth
            // 
            this.labelHealth.Image = ((System.Drawing.Image)(resources.GetObject("labelHealth.Image")));
            this.labelHealth.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.labelHealth.Location = new System.Drawing.Point(240, 16);
            this.labelHealth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelHealth.Name = "labelHealth";
            this.labelHealth.Size = new System.Drawing.Size(21, 19);
            this.labelHealth.TabIndex = 5;
            // 
            // labelEnergy
            // 
            this.labelEnergy.Image = ((System.Drawing.Image)(resources.GetObject("labelEnergy.Image")));
            this.labelEnergy.ImageAlign = System.Drawing.ContentAlignment.BottomRight;
            this.labelEnergy.Location = new System.Drawing.Point(240, 2);
            this.labelEnergy.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelEnergy.Name = "labelEnergy";
            this.labelEnergy.Size = new System.Drawing.Size(21, 19);
            this.labelEnergy.TabIndex = 4;
            // 
            // labelEscaped
            // 
            this.labelEscaped.Image = ((System.Drawing.Image)(resources.GetObject("labelEscaped.Image")));
            this.labelEscaped.Location = new System.Drawing.Point(200, 0);
            this.labelEscaped.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelEscaped.Name = "labelEscaped";
            this.labelEscaped.Size = new System.Drawing.Size(34, 24);
            this.labelEscaped.TabIndex = 3;
            this.labelEscaped.Visible = false;
            // 
            // labelDead
            // 
            this.labelDead.Image = ((System.Drawing.Image)(resources.GetObject("labelDead.Image")));
            this.labelDead.Location = new System.Drawing.Point(162, -1);
            this.labelDead.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelDead.Name = "labelDead";
            this.labelDead.Size = new System.Drawing.Size(34, 24);
            this.labelDead.TabIndex = 10;
            this.labelDead.Visible = false;
            // 
            // spriteSurfaceCritter
            // 
            this.spriteSurfaceCritter.Active = false;
            this.spriteSurfaceCritter.AutoBlank = true;
            this.spriteSurfaceCritter.AutoBlankColor = System.Drawing.Color.Black;
            this.spriteSurfaceCritter.DesiredFPS = 10;
            this.spriteSurfaceCritter.Location = new System.Drawing.Point(2, -1);
            this.spriteSurfaceCritter.Margin = new System.Windows.Forms.Padding(2);
            this.spriteSurfaceCritter.Name = "spriteSurfaceCritter";
            this.spriteSurfaceCritter.OffsetX = 0;
            this.spriteSurfaceCritter.OffsetY = 0;
            this.spriteSurfaceCritter.Size = new System.Drawing.Size(38, 39);
            this.spriteSurfaceCritter.TabIndex = 0;
            this.spriteSurfaceCritter.ThreadPriority = System.Threading.ThreadPriority.Normal;
            this.spriteSurfaceCritter.UseVirtualSize = false;
            this.spriteSurfaceCritter.VirtualHeight = 39;
            this.spriteSurfaceCritter.VirtualSize = new System.Drawing.Size(38, 39);
            this.spriteSurfaceCritter.VirtualWidth = 0;
            this.spriteSurfaceCritter.WraparoundEdges = false;
            // 
            // labelNumber
            // 
            this.labelNumber.AutoSize = true;
            this.labelNumber.Location = new System.Drawing.Point(45, 2);
            this.labelNumber.Name = "labelNumber";
            this.labelNumber.Size = new System.Drawing.Size(25, 13);
            this.labelNumber.TabIndex = 11;
            this.labelNumber.Text = "999";
            // 
            // CritterScorePanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelNumber);
            this.Controls.Add(this.labelDead);
            this.Controls.Add(this.labelEscaped);
            this.Controls.Add(this.labelAuthor);
            this.Controls.Add(this.labelName);
            this.Controls.Add(this.progressBarHealth);
            this.Controls.Add(this.progressBarEnergy);
            this.Controls.Add(this.labelHealth);
            this.Controls.Add(this.labelEnergy);
            this.Controls.Add(this.labelScore);
            this.Controls.Add(this.labelScorePrompt);
            this.Controls.Add(this.spriteSurfaceCritter);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CritterScorePanel";
            this.Size = new System.Drawing.Size(300, 39);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SCG.TurboSprite.SpriteSurface spriteSurfaceCritter;
        private System.Windows.Forms.Label labelScorePrompt;
        private System.Windows.Forms.Label labelScore;
        private System.Windows.Forms.Label labelEscaped;
        private System.Windows.Forms.Label labelEnergy;
        private System.Windows.Forms.Label labelHealth;
        private System.Windows.Forms.ProgressBar progressBarEnergy;
        private System.Windows.Forms.ProgressBar progressBarHealth;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label labelAuthor;
        private System.Windows.Forms.Label labelDead;
        private System.Windows.Forms.Label labelNumber;
    }
}
