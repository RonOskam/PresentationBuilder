// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.Win.Dialogs.SongSaveAsDialog
// Assembly: PresentationBuilder, Version=1.0.0.28120, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 295F5AD1-A97E-4830-A536-CA2F8525E5B1
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.exe

using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using PEC.Windows.Common.Controls;
using PEC.Windows.Common.Dialogs;
using PresentationBuilder.BLL;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PresentationBuilder.Win.Dialogs
{
  public class SongSaveAsDialog : OkCancelDialog
  {
    private IContainer components = (IContainer) null;
    private Song _song;
    private Label label2;
    private LookUpEdit bookLookup;
    private TextBox numberTextBox;
    private Label label1;
    private Label disclaimerLabel;

    public SongSaveAsDialog(Song song, bool showDisclaimer)
    {
      InitializeComponent();
      _song = song;
      disclaimerLabel.Visible = showDisclaimer;
      if (!showDisclaimer)
        Height = Height - disclaimerLabel.Height;
      LookupEdit.Initialize(bookLookup, "Title", (string) null, (IEnumerable) DataSource.GetBooks());
    }

    private void _okButton_Click(object sender, EventArgs e)
    {
      string text = (string) null;
      short result;
      if (!short.TryParse(numberTextBox.Text, out result))
        text = "Not a valid number.";
      else if ((int) result == 0)
      {
        text = "Not a valid number.";
      }
      else
      {
        _song.Book = (Book) bookLookup.EditValue;
        _song.Number = result;
        if (!_song.ValidNumber())
          text = "This song number has already been used for this book.";
        else
          DataSource.AddSong(_song);
      }
      if (text == null)
        return;
      int num = (int) MessageBox.Show(text, Application.ProductName);
      DialogResult = DialogResult.None;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      label2 = new Label();
      bookLookup = new LookUpEdit();
      numberTextBox = new TextBox();
      label1 = new Label();
      disclaimerLabel = new Label();
      bookLookup.Properties.BeginInit();
      SuspendLayout();
      _okButton.Location = new Point(82, 134);
      _okButton.Click += new EventHandler(_okButton_Click);
      _cancelButton.Location = new Point(163, 134);
      label2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      label2.AutoSize = true;
      label2.Location = new Point(33, 79);
      label2.Name = "label2";
      label2.Size = new Size(32, 13);
      label2.TabIndex = 11;
      label2.Text = "Book";
      bookLookup.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      bookLookup.Location = new Point(71, 76);
      bookLookup.Name = "bookLookup";
      bookLookup.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      bookLookup.Size = new Size(119, 20);
      bookLookup.TabIndex = 10;
      numberTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      numberTextBox.Location = new Point(71, 102);
      numberTextBox.Name = "numberTextBox";
      numberTextBox.Size = new Size(100, 20);
      numberTextBox.TabIndex = 12;
      label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
      label1.AutoSize = true;
      label1.Location = new Point(21, 105);
      label1.Name = "label1";
      label1.Size = new Size(44, 13);
      label1.TabIndex = 13;
      label1.Text = "Number";
      disclaimerLabel.Location = new Point(12, 9);
      disclaimerLabel.Name = "disclaimerLabel";
      disclaimerLabel.Size = new Size(226, 53);
      disclaimerLabel.TabIndex = 14;
      disclaimerLabel.Text = "Saving the song will save it to a database where it will be available to all users of this program.  It's not required to save the song to use it in your presentation.";
      ClientSize = new Size(250, 169);
      Controls.Add((Control) disclaimerLabel);
      Controls.Add((Control) label1);
      Controls.Add((Control) numberTextBox);
      Controls.Add((Control) label2);
      Controls.Add((Control) bookLookup);
      Name = "SongSaveAsDialog";
      Text = "Save Song As";
      Controls.SetChildIndex((Control) _okButton, 0);
      Controls.SetChildIndex((Control) _cancelButton, 0);
      Controls.SetChildIndex((Control) bookLookup, 0);
      Controls.SetChildIndex((Control) label2, 0);
      Controls.SetChildIndex((Control) numberTextBox, 0);
      Controls.SetChildIndex((Control) label1, 0);
      Controls.SetChildIndex((Control) disclaimerLabel, 0);
      bookLookup.Properties.EndInit();
      ResumeLayout(false);
      PerformLayout();
    }
  }
}
