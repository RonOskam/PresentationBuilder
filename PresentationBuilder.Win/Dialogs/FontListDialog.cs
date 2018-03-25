using PEC.Windows.Common.Dialogs;
using PresentationBuilder.BLL.PowerPoint;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PresentationBuilder.Win.Dialogs
{
  public class FontListDialog : OkCancelDialog
  {
    private IContainer components = (IContainer) null;
    private FontItem _songFont;
    private FontItem _indicatorFont;
    private FontItem _messageFont;
    private Label songFontLabel;
    private Label label1;
    private Button songButton;
    private Button messageButton;
    private Label label2;
    private Label messageFontLabel;
    private Button numberButton;
    private Label label4;
    private Label numberFontLabel;
    private Button defaultButton;

    public FontItem SongFont
    {
      get
      {
        return this._songFont;
      }
      set
      {
        this._songFont = value;
        if (value == null)
          return;
        this.songFontLabel.Font = this._songFont.SetFont();
        this.songFontLabel.ForeColor = this._songFont.FontColor;
      }
    }

    public FontItem IndicatorFont
    {
      get
      {
        return this._indicatorFont;
      }
      set
      {
        this._indicatorFont = value;
        if (value == null)
          return;
        this.numberFontLabel.Font = this._indicatorFont.SetFont();
        this.numberFontLabel.ForeColor = this._indicatorFont.FontColor;
      }
    }

    public FontItem MessageFont
    {
      get
      {
        return this._messageFont;
      }
      set
      {
        this._messageFont = value;
        if (value == null)
          return;
        this.messageFontLabel.Font = this._messageFont.SetFont();
        this.messageFontLabel.ForeColor = this._messageFont.FontColor;
      }
    }

    public FontListDialog()
    {
      this.InitializeComponent();
    }

    private void songButton_Click(object sender, EventArgs e)
    {
      FontSelectDialog fontSelectDialog = new FontSelectDialog();
      fontSelectDialog.SelectedFont = SongFont;
      if (fontSelectDialog.ShowDialog() != DialogResult.OK)
        return;
      this.SongFont = fontSelectDialog.SelectedFont;
    }

    private void messageButton_Click(object sender, EventArgs e)
    {
      FontSelectDialog fontSelectDialog = new FontSelectDialog();
      fontSelectDialog.SelectedFont = this.MessageFont;
      if (fontSelectDialog.ShowDialog() != DialogResult.OK)
        return;
      this.MessageFont = fontSelectDialog.SelectedFont;
    }

    private void numberButton_Click(object sender, EventArgs e)
    {
      FontSelectDialog fontSelectDialog = new FontSelectDialog();
      fontSelectDialog.SelectedFont = this.IndicatorFont;
      if (fontSelectDialog.ShowDialog() != DialogResult.OK)
        return;
      this.IndicatorFont = fontSelectDialog.SelectedFont;
    }

    private void defaultButton_Click(object sender, EventArgs e)
    {
      this._songFont.SaveFont("SongFont");
      this._messageFont.SaveFont("MessageFont");
      this._indicatorFont.SaveFont("IndicatorFont");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.songFontLabel = new Label();
      this.label1 = new Label();
      this.songButton = new Button();
      this.messageButton = new Button();
      this.label2 = new Label();
      this.messageFontLabel = new Label();
      this.numberButton = new Button();
      this.label4 = new Label();
      this.numberFontLabel = new Label();
      this.defaultButton = new Button();
      this.SuspendLayout();
      this._okButton.Location = new Point(223, 417);
      this._cancelButton.Location = new Point(304, 417);
      this.songFontLabel.BackColor = Color.LightGray;
      this.songFontLabel.BorderStyle = BorderStyle.FixedSingle;
      this.songFontLabel.Font = new Font("Microsoft Sans Serif", 32f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.songFontLabel.Location = new Point(12, 37);
      this.songFontLabel.Name = "songFontLabel";
      this.songFontLabel.Size = new Size(361, 90);
      this.songFontLabel.TabIndex = 4;
      this.songFontLabel.Text = "AaBbCc";
      this.songFontLabel.TextAlign = ContentAlignment.MiddleCenter;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 17);
      this.label1.Name = "label1";
      this.label1.Size = new Size(56, 13);
      this.label1.TabIndex = 5;
      this.label1.Text = "Song Font";
      this.songButton.Location = new Point(298, 11);
      this.songButton.Name = "songButton";
      this.songButton.Size = new Size(75, 23);
      this.songButton.TabIndex = 6;
      this.songButton.Text = "Select";
      this.songButton.UseVisualStyleBackColor = true;
      this.songButton.Click += new EventHandler(this.songButton_Click);
      this.messageButton.Location = new Point(298, 145);
      this.messageButton.Name = "messageButton";
      this.messageButton.Size = new Size(75, 23);
      this.messageButton.TabIndex = 9;
      this.messageButton.Text = "Select";
      this.messageButton.UseVisualStyleBackColor = true;
      this.messageButton.Click += new EventHandler(this.messageButton_Click);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(12, 150);
      this.label2.Name = "label2";
      this.label2.Size = new Size(74, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "Message Font";
      this.messageFontLabel.BackColor = Color.LightGray;
      this.messageFontLabel.BorderStyle = BorderStyle.FixedSingle;
      this.messageFontLabel.Font = new Font("Microsoft Sans Serif", 32f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.messageFontLabel.Location = new Point(12, 170);
      this.messageFontLabel.Name = "messageFontLabel";
      this.messageFontLabel.Size = new Size(361, 90);
      this.messageFontLabel.TabIndex = 7;
      this.messageFontLabel.Text = "AaBbCc";
      this.messageFontLabel.TextAlign = ContentAlignment.MiddleCenter;
      this.numberButton.Location = new Point(298, 276);
      this.numberButton.Name = "numberButton";
      this.numberButton.Size = new Size(75, 23);
      this.numberButton.TabIndex = 12;
      this.numberButton.Text = "Select";
      this.numberButton.UseVisualStyleBackColor = true;
      this.numberButton.Click += new EventHandler(this.numberButton_Click);
      this.label4.AutoSize = true;
      this.label4.Location = new Point(12, 281);
      this.label4.Name = "label4";
      this.label4.Size = new Size(96, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "Song Number Font";
      this.numberFontLabel.BackColor = Color.LightGray;
      this.numberFontLabel.BorderStyle = BorderStyle.FixedSingle;
      this.numberFontLabel.Font = new Font("Microsoft Sans Serif", 32f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.numberFontLabel.Location = new Point(12, 301);
      this.numberFontLabel.Name = "numberFontLabel";
      this.numberFontLabel.Size = new Size(361, 90);
      this.numberFontLabel.TabIndex = 10;
      this.numberFontLabel.Text = "AaBbCc";
      this.numberFontLabel.TextAlign = ContentAlignment.MiddleCenter;
      this.defaultButton.Location = new Point(12, 417);
      this.defaultButton.Name = "defaultButton";
      this.defaultButton.Size = new Size(75, 23);
      this.defaultButton.TabIndex = 13;
      this.defaultButton.Text = "Set Default";
      this.defaultButton.UseVisualStyleBackColor = true;
      this.defaultButton.Click += new EventHandler(this.defaultButton_Click);
      this.ClientSize = new Size(391, 452);
      this.Controls.Add((Control) this.defaultButton);
      this.Controls.Add((Control) this.numberButton);
      this.Controls.Add((Control) this.label4);
      this.Controls.Add((Control) this.numberFontLabel);
      this.Controls.Add((Control) this.messageButton);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.messageFontLabel);
      this.Controls.Add((Control) this.songButton);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.songFontLabel);
      this.Name = "FontListDialog";
      this.Text = "Fonts";
      this.Controls.SetChildIndex((Control) this._okButton, 0);
      this.Controls.SetChildIndex((Control) this._cancelButton, 0);
      this.Controls.SetChildIndex((Control) this.songFontLabel, 0);
      this.Controls.SetChildIndex((Control) this.label1, 0);
      this.Controls.SetChildIndex((Control) this.songButton, 0);
      this.Controls.SetChildIndex((Control) this.messageFontLabel, 0);
      this.Controls.SetChildIndex((Control) this.label2, 0);
      this.Controls.SetChildIndex((Control) this.messageButton, 0);
      this.Controls.SetChildIndex((Control) this.numberFontLabel, 0);
      this.Controls.SetChildIndex((Control) this.label4, 0);
      this.Controls.SetChildIndex((Control) this.numberButton, 0);
      this.Controls.SetChildIndex((Control) this.defaultButton, 0);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
