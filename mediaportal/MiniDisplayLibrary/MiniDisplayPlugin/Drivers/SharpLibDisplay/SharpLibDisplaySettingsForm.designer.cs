namespace MediaPortal.ProcessPlugins.MiniDisplayPlugin.Drivers
{
  partial class SharpLibDisplaySettingsForm
    {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

 
      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
      this.checkBoxSingleLine = new System.Windows.Forms.CheckBox();
      this.buttonOk = new System.Windows.Forms.Button();
      this.label1 = new System.Windows.Forms.Label();
      this.linkLabelDocumentation = new System.Windows.Forms.LinkLabel();
      this.SuspendLayout();
      // 
      // checkBoxSingleLine
      // 
      this.checkBoxSingleLine.AutoSize = true;
      this.checkBoxSingleLine.Location = new System.Drawing.Point(16, 32);
      this.checkBoxSingleLine.Name = "checkBoxSingleLine";
      this.checkBoxSingleLine.Size = new System.Drawing.Size(113, 24);
      this.checkBoxSingleLine.TabIndex = 0;
      this.checkBoxSingleLine.Text = "Single Line";
      this.checkBoxSingleLine.UseVisualStyleBackColor = true;
      // 
      // buttonOk
      // 
      this.buttonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonOk.AutoSize = true;
      this.buttonOk.Location = new System.Drawing.Point(449, 199);
      this.buttonOk.Name = "buttonOk";
      this.buttonOk.Size = new System.Drawing.Size(138, 45);
      this.buttonOk.TabIndex = 1;
      this.buttonOk.Text = "&OK";
      this.buttonOk.UseVisualStyleBackColor = true;
      this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(448, 20);
      this.label1.TabIndex = 2;
      this.label1.Text = "Use Single Line mode to improve readability on smaller display.";
      // 
      // linkLabelDocumentation
      // 
      this.linkLabelDocumentation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.linkLabelDocumentation.AutoSize = true;
      this.linkLabelDocumentation.Location = new System.Drawing.Point(12, 224);
      this.linkLabelDocumentation.Name = "linkLabelDocumentation";
      this.linkLabelDocumentation.Size = new System.Drawing.Size(167, 20);
      this.linkLabelDocumentation.TabIndex = 6;
      this.linkLabelDocumentation.TabStop = true;
      this.linkLabelDocumentation.Text = "Online Documentation";
      this.linkLabelDocumentation.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDocumentation_LinkClicked);
      // 
      // SharpLibDisplaySettingsForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
      this.ClientSize = new System.Drawing.Size(599, 256);
      this.Controls.Add(this.linkLabelDocumentation);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.buttonOk);
      this.Controls.Add(this.checkBoxSingleLine);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "SharpLibDisplaySettingsForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "MiniDisplay - Setup - Advanced Settings";
      this.ResumeLayout(false);
      this.PerformLayout();

      }

      #endregion

    private System.Windows.Forms.CheckBox checkBoxSingleLine;
    private System.Windows.Forms.Button buttonOk;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.LinkLabel linkLabelDocumentation;
    }
}