namespace RevitPBP_AddIn
{
    partial class SPdigital_tools
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
            this.butPBPErstellen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // butPBPErstellen
            // 
            this.butPBPErstellen.Location = new System.Drawing.Point(64, 28);
            this.butPBPErstellen.Name = "butPBPErstellen";
            this.butPBPErstellen.Size = new System.Drawing.Size(90, 48);
            this.butPBPErstellen.TabIndex = 0;
            this.butPBPErstellen.Text = "PBP.ifc georeferenziert erstellen";
            this.butPBPErstellen.UseVisualStyleBackColor = true;
            this.butPBPErstellen.Click += new System.EventHandler(this.butPBPErstellen_Click);
            // 
            // SPdigital_tools
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(216, 293);
            this.Controls.Add(this.butPBPErstellen);
            this.Name = "SPdigital_tools";
            this.Text = "SPdigital_tools";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button butPBPErstellen;
    }
}