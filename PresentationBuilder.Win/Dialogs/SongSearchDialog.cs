using DevExpress.XtraEditors;
using PEC.Windows.Common.Dialogs;
using PresentationBuilder.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PresentationBuilder.Win.Dialogs
{
  public class SongSearchDialog : OkCancelDialog
  {
    private Dictionary<string, string> _songList = (Dictionary<string, string>) null;
    private Song _song = (Song) null;
    private IContainer components = (IContainer) null;
    private TextBox songText;
    private Label label1;
    private Button searchButton;
    private ListBoxControl songListBox;

    public Song SelectedSong
    {
      get
      {
        return this._song;
      }
    }

    public SongSearchDialog()
    {
      this.InitializeComponent();
    }

    private void searchButton_Click(object sender, EventArgs e)
    {
      this.Cursor = Cursors.WaitCursor;
      bool flag = true;
      this._songList = new CyberhymnalReader().SearchByName(this.songText.Text);
      if (flag)
      {
        this.songListBox.DisplayMember = "Key";
        this.songListBox.Items.Clear();
        foreach (KeyValuePair<string, string> keyValuePair in this._songList)
          this.songListBox.Items.Add((object) keyValuePair.Key);
      }
      this.Cursor = Cursors.Default;
    }

    private void _okButton_Click(object sender, EventArgs e)
    {
      if (this._songList == null || this.songListBox.SelectedIndex == -1)
      {
        this.DialogResult = DialogResult.None;
      }
      else
      {
        string url = this._songList[Convert.ToString(this.songListBox.SelectedItem)];
        CyberhymnalReader cyberhymnalReader = new CyberhymnalReader();
        try
        {
          this._song = cyberhymnalReader.GetSong(url);
        }
        catch (Exception ex)
        {
          int num = (int) MessageBox.Show("The following error occured while accessing www.nethymnal.org:\r\n\r\n" + ex.Message, Application.ProductName);
          this.DialogResult = DialogResult.None;
        }
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.songText = new TextBox();
      this.label1 = new Label();
      this.searchButton = new Button();
      this.songListBox = new ListBoxControl();
     // this.songListBox.BeginInit();
      this.SuspendLayout();
      this._okButton.Location = new Point(134, 303);
      this._okButton.TabIndex = 3;
      this._okButton.Click += new EventHandler(this._okButton_Click);
      this._cancelButton.Location = new Point(215, 303);
      this._cancelButton.TabIndex = 4;
      this.songText.Location = new Point(12, 25);
      this.songText.Name = "songText";
      this.songText.Size = new Size(209, 20);
      this.songText.TabIndex = 0;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(9, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(263, 13);
      this.label1.TabIndex = 3;
      this.label1.Text = "Enter a song title to search for on www.NetHymnal.org";
      this.searchButton.Location = new Point(227, 23);
      this.searchButton.Name = "searchButton";
      this.searchButton.Size = new Size(63, 23);
      this.searchButton.TabIndex = 1;
      this.searchButton.Text = "Search";
      this.searchButton.UseVisualStyleBackColor = true;
      this.searchButton.Click += new EventHandler(this.searchButton_Click);
      this.songListBox.Location = new Point(12, 51);
      this.songListBox.Name = "songListBox";
      this.songListBox.Size = new Size(278, 246);
      this.songListBox.TabIndex = 2;
      this.ClientSize = new Size(302, 338);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.songText);
      this.Controls.Add((Control) this.songListBox);
      this.Controls.Add((Control) this.searchButton);
      this.Name = "SongSearchDialog";
      this.Text = "Song Search";
      this.Controls.SetChildIndex((Control) this.searchButton, 0);
      this.Controls.SetChildIndex((Control) this.songListBox, 0);
      this.Controls.SetChildIndex((Control) this.songText, 0);
      this.Controls.SetChildIndex((Control) this.label1, 0);
      this.Controls.SetChildIndex((Control) this._okButton, 0);
      this.Controls.SetChildIndex((Control) this._cancelButton, 0);
      //this.songListBox.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
