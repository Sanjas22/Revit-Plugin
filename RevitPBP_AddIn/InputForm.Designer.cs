namespace RevitPBP_AddIn
{
    partial class InputForm
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
            this.components = new System.ComponentModel.Container();
            this.labelName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelX = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.labelZ = new System.Windows.Forms.Label();
            this.labelLage = new System.Windows.Forms.Label();
            this.textBoxX = new System.Windows.Forms.TextBox();
            this.textBoxY = new System.Windows.Forms.TextBox();
            this.textBoxZ = new System.Windows.Forms.TextBox();
            this.textBoxLage = new System.Windows.Forms.TextBox();
            this.buttonStandardFamilie = new System.Windows.Forms.Button();
            this.labelHöhe = new System.Windows.Forms.Label();
            this.textBoxHöhe = new System.Windows.Forms.TextBox();
            this.buttonFamilieHerunterladen = new System.Windows.Forms.Button();
            this.Hinweis1 = new System.Windows.Forms.ToolTip(this.components);
            this.labelHinweisClip = new System.Windows.Forms.Label();
            this.buttonNeuePBP = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(47, 52);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(35, 13);
            this.labelName.TabIndex = 0;
            this.labelName.Text = "Name";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(111, 49);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(100, 20);
            this.textBoxName.TabIndex = 1;
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(56, 81);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(14, 13);
            this.labelX.TabIndex = 2;
            this.labelX.Text = "X";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(56, 115);
            this.labelY.Name = "labelY";
            this.labelY.Size = new System.Drawing.Size(14, 13);
            this.labelY.TabIndex = 3;
            this.labelY.Text = "Y";
            // 
            // labelZ
            // 
            this.labelZ.AutoSize = true;
            this.labelZ.Location = new System.Drawing.Point(56, 143);
            this.labelZ.Name = "labelZ";
            this.labelZ.Size = new System.Drawing.Size(14, 13);
            this.labelZ.TabIndex = 4;
            this.labelZ.Text = "Z";
            // 
            // labelLage
            // 
            this.labelLage.AutoSize = true;
            this.labelLage.Location = new System.Drawing.Point(29, 181);
            this.labelLage.Name = "labelLage";
            this.labelLage.Size = new System.Drawing.Size(63, 13);
            this.labelLage.TabIndex = 5;
            this.labelLage.Text = "EPSG Lage";
            // 
            // textBoxX
            // 
            this.textBoxX.Location = new System.Drawing.Point(111, 81);
            this.textBoxX.Name = "textBoxX";
            this.textBoxX.Size = new System.Drawing.Size(100, 20);
            this.textBoxX.TabIndex = 6;
            // 
            // textBoxY
            // 
            this.textBoxY.Location = new System.Drawing.Point(111, 115);
            this.textBoxY.Name = "textBoxY";
            this.textBoxY.Size = new System.Drawing.Size(100, 20);
            this.textBoxY.TabIndex = 7;
            // 
            // textBoxZ
            // 
            this.textBoxZ.Location = new System.Drawing.Point(111, 148);
            this.textBoxZ.Name = "textBoxZ";
            this.textBoxZ.Size = new System.Drawing.Size(100, 20);
            this.textBoxZ.TabIndex = 8;
            // 
            // textBoxLage
            // 
            this.textBoxLage.Location = new System.Drawing.Point(111, 181);
            this.textBoxLage.Name = "textBoxLage";
            this.textBoxLage.Size = new System.Drawing.Size(100, 20);
            this.textBoxLage.TabIndex = 9;
            this.textBoxLage.Text = "7837";
            // 
            // buttonStandardFamilie
            // 
            this.buttonStandardFamilie.Location = new System.Drawing.Point(19, 256);
            this.buttonStandardFamilie.Name = "buttonStandardFamilie";
            this.buttonStandardFamilie.Size = new System.Drawing.Size(86, 41);
            this.buttonStandardFamilie.TabIndex = 10;
            this.buttonStandardFamilie.Text = "Standard Familie";
            this.buttonStandardFamilie.UseVisualStyleBackColor = true;
            this.buttonStandardFamilie.Click += new System.EventHandler(this.buttonStandardFamilie_Click);
            // 
            // labelHöhe
            // 
            this.labelHöhe.AutoSize = true;
            this.labelHöhe.Location = new System.Drawing.Point(29, 219);
            this.labelHöhe.Name = "labelHöhe";
            this.labelHöhe.Size = new System.Drawing.Size(65, 13);
            this.labelHöhe.TabIndex = 11;
            this.labelHöhe.Text = "EPSG Höhe";
            // 
            // textBoxHöhe
            // 
            this.textBoxHöhe.Location = new System.Drawing.Point(111, 219);
            this.textBoxHöhe.Name = "textBoxHöhe";
            this.textBoxHöhe.Size = new System.Drawing.Size(100, 20);
            this.textBoxHöhe.TabIndex = 12;
            this.textBoxHöhe.Text = "5684";
            // 
            // buttonFamilieHerunterladen
            // 
            this.buttonFamilieHerunterladen.Location = new System.Drawing.Point(127, 256);
            this.buttonFamilieHerunterladen.Name = "buttonFamilieHerunterladen";
            this.buttonFamilieHerunterladen.Size = new System.Drawing.Size(91, 41);
            this.buttonFamilieHerunterladen.TabIndex = 13;
            this.buttonFamilieHerunterladen.Text = "Familie herunterladen";
            this.buttonFamilieHerunterladen.UseVisualStyleBackColor = true;
            this.buttonFamilieHerunterladen.Click += new System.EventHandler(this.buttonFamilieHerunterladen_Click);
            // 
            // labelHinweisClip
            // 
            this.labelHinweisClip.Location = new System.Drawing.Point(0, 0);
            this.labelHinweisClip.MaximumSize = new System.Drawing.Size(273, 356);
            this.labelHinweisClip.Name = "labelHinweisClip";
            this.labelHinweisClip.Size = new System.Drawing.Size(255, 52);
            this.labelHinweisClip.TabIndex = 14;
            this.labelHinweisClip.Text = "Hinweis: Bitte achten Sie darauf, die Klammer (Clip) beim Vermessungspunkt zu lös" +
    "en, bevor Sie fortfahren!";
            // 
            // buttonNeuePBP
            // 
            this.buttonNeuePBP.Location = new System.Drawing.Point(132, 324);
            this.buttonNeuePBP.Name = "buttonNeuePBP";
            this.buttonNeuePBP.Size = new System.Drawing.Size(86, 41);
            this.buttonNeuePBP.TabIndex = 15;
            this.buttonNeuePBP.Text = "Neue PBP anlegen";
            this.buttonNeuePBP.UseVisualStyleBackColor = true;
            this.buttonNeuePBP.Click += new System.EventHandler(this.buttonNeuePBP_Click);
            // 
            // InputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 421);
            this.Controls.Add(this.buttonNeuePBP);
            this.Controls.Add(this.labelHinweisClip);
            this.Controls.Add(this.buttonFamilieHerunterladen);
            this.Controls.Add(this.textBoxHöhe);
            this.Controls.Add(this.labelHöhe);
            this.Controls.Add(this.buttonStandardFamilie);
            this.Controls.Add(this.textBoxLage);
            this.Controls.Add(this.textBoxZ);
            this.Controls.Add(this.textBoxY);
            this.Controls.Add(this.textBoxX);
            this.Controls.Add(this.labelLage);
            this.Controls.Add(this.labelZ);
            this.Controls.Add(this.labelY);
            this.Controls.Add(this.labelX);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelName);
            this.Name = "InputForm";
            this.Text = "InputForm";
            this.Load += new System.EventHandler(this.InputForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label labelZ;
        private System.Windows.Forms.Label labelLage;
        private System.Windows.Forms.TextBox textBoxX;
        private System.Windows.Forms.TextBox textBoxY;
        private System.Windows.Forms.TextBox textBoxZ;
        private System.Windows.Forms.TextBox textBoxLage;
        private System.Windows.Forms.Button buttonStandardFamilie;
        private System.Windows.Forms.Label labelHöhe;
        private System.Windows.Forms.TextBox textBoxHöhe;
        private System.Windows.Forms.Button buttonFamilieHerunterladen;
        private System.Windows.Forms.ToolTip Hinweis1;
        private System.Windows.Forms.Label labelHinweisClip;
        private System.Windows.Forms.Button buttonNeuePBP;
    }
}