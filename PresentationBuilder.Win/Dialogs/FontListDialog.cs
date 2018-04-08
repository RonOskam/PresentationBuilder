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
        return _songFont;
      }
      set
      {
        _songFont = value;
        if (value == null)
          return;
        songFontLabel.Font = _songFont.SetFont();
        songFontLabel.ForeColor = _songFont.FontColor;
      }
    }

    public FontItem IndicatorFont
    {
      get
      {
        return _indicatorFont;
      }
      set
      {
        _indicatorFont = value;
        if (value == null)
          return;
        numberFontLabel.Font = _indicatorFont.SetFont();
        numberFontLabel.ForeColor = _indicatorFont.FontColor;
      }
    }

    public FontItem MessageFont
    {
      get
      {
        return _messageFont;
      }
      set
      {
        _messageFont = value;
        if (value == null)
          return;
        messageFontLabel.Font = _messageFont.SetFont();
        messageFontLabel.ForeColor = _messageFont.FontColor;
      }
    }

    public FontListDialog()
    {
      InitializeComponent();
    }

    private void songButton_Click(object sender, EventArgs e)
    {
      FontSelectDialog fontSelectDialog = new FontSelectDialog();
      fontSelectDialog.SelectedFont = SongFont;
      if (fontSelectDialog.ShowDialog() != DialogResult.OK)
        return;
      SongFont = fontSelectDialog.SelectedFont;
    }

    private void messageButton_Click(object sender, EventArgs e)
    {
      FontSelectDialog fontSelectDialog = new FontSelectDialog();
      fontSelectDialog.SelectedFont = MessageFont;
      if (fontSelectDialog.ShowDialog() != DialogResult.OK)
        return;
      MessageFont = fontSelectDialog.SelectedFont;
    }

    private void numberButton_Click(object sender, EventArgs e)
    {
      FontSelectDialog fontSelectDialog = new FontSelectDialog();
      fontSelectDialog.SelectedFont = IndicatorFont;
      if (fontSelectDialog.ShowDialog() != DialogResult.OK)
        return;
      IndicatorFont = fontSelectDialog.SelectedFont;
    }

    private void defaultButton_Click(object sender, EventArgs e)
    {
      _songFont.SaveFont("SongFont");
      _messageFont.SaveFont("MessageFont");
      _indicatorFont.SaveFont("IndicatorFont");
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      songFontLabel = new Label();
      label1 = new Label();
      songButton = new Button();
      messageButton = new Button();
      label2 = new Label();
      messageFontLabel = new Label();
      numberButton = new Button();
      label4 = new Label();
      numberFontLabel = new Label();
      defaultButton = new Button();
      SuspendLayout();
      _okButton.Location = new Point(223, 417);
      _cancelButton.Location = new Point(304, 417);
      songFontLabel.BackColor = Color.LightGray;
      songFontLabel.BorderStyle = BorderStyle.FixedSingle;
      songFontLabel.Font = new Font("Microsoft Sans Serif", 32f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      songFontLabel.Location = new Point(12, 37);
      songFontLabel.Name = "songFontLabel";
      songFontLabel.Size = new Size(361, 90);
      songFontLabel.TabIndex = 4;
      songFontLabel.Text = "AaBbCc";
      songFontLabel.TextAlign = ContentAlignment.MiddleCenter;
      label1.AutoSize = true;
      label1.Location = new Point(12, 17);
      label1.Name = "label1";
      label1.Size = new Size(56, 13);
      label1.TabIndex = 5;
      label1.Text = "Song Font";
      songButton.Location = new Point(298, 11);
      songButton.Name = "songButton";
      songButton.Size = new Size(75, 23);
      songButton.TabIndex = 6;
      songButton.Text = "Select";
      songButton.UseVisualStyleBackColor = true;
      songButton.Click += new EventHandler(songButton_Click);
      messageButton.Location = new Point(298, 145);
      messageButton.Name = "messageButton";
      messageButton.Size = new Size(75, 23);
      messageButton.TabIndex = 9;
      messageButton.Text = "Select";
      messageButton.UseVisualStyleBackColor = true;
      messageButton.Click += new EventHandler(messageButton_Click);
      label2.AutoSize = true;
      label2.Location = new Point(12, 150);
      label2.Name = "label2";
      label2.Size = new Size(74, 13);
      label2.TabIndex = 8;
      label2.Text = "Message Font";
      messageFontLabel.BackColor = Color.LightGray;
      messageFontLabel.BorderStyle = BorderStyle.FixedSingle;
      messageFontLabel.Font = new Font("Microsoft Sans Serif", 32f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      messageFontLabel.Location = new Point(12, 170);
      messageFontLabel.Name = "messageFontLabel";
      messageFontLabel.Size = new Size(361, 90);
      messageFontLabel.TabIndex = 7;
      messageFontLabel.Text = "AaBbCc";
      messageFontLabel.TextAlign = ContentAlignment.MiddleCenter;
      numberButton.Location = new Point(298, 276);
      numberButton.Name = "numberButton";
      numberButton.Size = new Size(75, 23);
      numberButton.TabIndex = 12;
      numberButton.Text = "Select";
      numberButton.UseVisualStyleBackColor = true;
      numberButton.Click += new EventHandler(numberButton_Click);
      label4.AutoSize = true;
      label4.Location = new Point(12, 281);
      label4.Name = "label4";
      label4.Size = new Size(96, 13);
      label4.TabIndex = 11;
      label4.Text = "Song Number Font";
      numberFontLabel.BackColor = Color.LightGray;
      numberFontLabel.BorderStyle = BorderStyle.FixedSingle;
      numberFontLabel.Font = new Font("Microsoft Sans Serif", 32f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      numberFontLabel.Location = new Point(12, 301);
      numberFontLabel.Name = "numberFontLabel";
      numberFontLabel.Size = new Size(361, 90);
      numberFontLabel.TabIndex = 10;
      numberFontLabel.Text = "AaBbCc";
      numberFontLabel.TextAlign = ContentAlignment.MiddleCenter;
      defaultButton.Location = new Point(12, 417);
      defaultButton.Name = "defaultButton";
      defaultButton.Size = new Size(75, 23);
      defaultButton.TabIndex = 13;
      defaultButton.Text = "Set Default";
      defaultButton.UseVisualStyleBackColor = true;
      defaultButton.Click += new EventHandler(defaultButton_Click);
      ClientSize = new Size(391, 452);
      Controls.Add((Control) defaultButton);
      Controls.Add((Control) numberButton);
      Controls.Add((Control) label4);
      Controls.Add((Control) numberFontLabel);
      Controls.Add((Control) messageButton);
      Controls.Add((Control) label2);
      Controls.Add((Control) messageFontLabel);
      Controls.Add((Control) songButton);
      Controls.Add((Control) label1);
      Controls.Add((Control) songFontLabel);
      Name = "FontListDialog";
      Text = "Fonts";
      Controls.SetChildIndex((Control) _okButton, 0);
      Controls.SetChildIndex((Control) _cancelButton, 0);
      Controls.SetChildIndex((Control) songFontLabel, 0);
      Controls.SetChildIndex((Control) label1, 0);
      Controls.SetChildIndex((Control) songButton, 0);
      Controls.SetChildIndex((Control) messageFontLabel, 0);
      Controls.SetChildIndex((Control) label2, 0);
      Controls.SetChildIndex((Control) messageButton, 0);
      Controls.SetChildIndex((Control) numberFontLabel, 0);
      Controls.SetChildIndex((Control) label4, 0);
      Controls.SetChildIndex((Control) numberButton, 0);
      Controls.SetChildIndex((Control) defaultButton, 0);
      ResumeLayout(false);
      PerformLayout();
    }
  }
}
