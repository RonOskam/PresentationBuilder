using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using PEC.Windows.Common.Controls;
using PresentationBuilder.BLL;
using PresentationBuilder.BLL.PowerPoint;
using PresentationBuilder.Win.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using System.Xml;

namespace PresentationBuilder.Win.Controls
{
  public class SlideControl : UserControl
  {
    private Song _internetSong = (Song)null;
    private IContainer components = (IContainer)null;
    private Panel songPanel;
    private Label label3;
    private Label label2;
    private Label label1;
    private CheckedListBoxControl verseCheckList;
    private LookUpEdit songLookup;
    private LookUpEdit bookLookup;
    private Panel messagePanel;
    private Label label5;
    private Label label4;
    private LookUpEdit messageLookup;
    private LookUpEdit messageTypeLookup;
    private Panel passagePanel;
    private TextBox passageTextBox;
    private Label label7;
    private Panel panel1;
    private Button openButton;
    internal LookUpEdit typeLookup;

    internal SlideListBox ParentBox { get; set; }

    public event SlideControl.ChangedEventHandler Changed;

    public SlideControl()
    {
      InitializeComponent();
      LookupEdit.Initialize(typeLookup, typeof(SlideType));
      songLookup.Properties.DropDownRows = 15;
      bookLookup.Properties.DropDownRows = 15;
      messagePanel.Top = songPanel.Top;
      passagePanel.Top = songPanel.Top;
      typeLookup.Properties.ForceInitialize();
      typeLookup.ItemIndex = 0;
      typeLookup_EditValueChanged(null, EventArgs.Empty);
      SetClickEvent(Controls);
    }

    private void SetClickEvent(Control.ControlCollection controls)
    {
      foreach (Control control in (ArrangedElementCollection)controls)
      {
        control.Click += new EventHandler(SlideControl_Click);
        SetClickEvent(control.Controls);
      }
    }

    public void GotFocus()
    {
      BackColor = Color.LightCyan;
    }

    public void LostFocus()
    {
      BackColor = Color.FromName("Control");
    }

    private void typeLookup_EditValueChanged(object sender, EventArgs e)
    {
      Changed?.Invoke(this, EventArgs.Empty);
      if (typeLookup.EditValue == null)
        return;
      SlideType slideType = (SlideType)typeLookup.EditValue;
      songPanel.Visible = slideType == SlideType.Song;
      messagePanel.Visible = slideType == SlideType.Message;
      passagePanel.Visible = slideType == SlideType.BiblePassage;
      if (slideType == SlideType.Song)
      {
        List<Book> books = DataSource.GetBooks();
        books.Add(new Book()
        {
          Title = "(Search Internet)",
          BookID = -1
        });
        LookupEdit.Initialize(bookLookup, "Title", (string)null, (IEnumerable)books);
        Height = 50;
      }
      else
        Height = 33;
      if (slideType != SlideType.Message)
        return;
      LookupEdit.Initialize(messageTypeLookup, "Description", (string)null, (IEnumerable)DataSource.GetMessageTypes());
    }

    private void bookLookup_EditValueChanged(object sender, EventArgs e)
    {
      if (bookLookup.EditValue == null || bookLookup.Tag != null)
        return;
      // ISSUE: reference to a compiler-generated field
      Changed?.Invoke(this, EventArgs.Empty);
      Book book = (Book)bookLookup.EditValue;
      if (book.BookID == -1)
      {
        SongSearchDialog songSearchDialog = new SongSearchDialog();
        if (songSearchDialog.ShowDialog() == DialogResult.OK)
        {
          Song selectedSong = songSearchDialog.SelectedSong;
          songSearchDialog.Close();
          SongEditDialog songEditDialog = new SongEditDialog();
          songEditDialog.EditSong(selectedSong);
          DialogResult dialogResult = songEditDialog.ShowDialog();
          if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Retry)
          {
            int? selectedSongId = songEditDialog.SelectedSongID;
            if (selectedSongId.HasValue)
            {
              bookLookup.EditValue = selectedSong.Book;
              foreach (SongItem song in (List<SongItem>)songLookup.Properties.DataSource)
              {
                if (song.SongID == selectedSongId.Value)
                {
                  songLookup.EditValue = song;
                  ConfigureVerses(song);
                  break;
                }
              }
            }
            else
            {
              _internetSong = selectedSong;
              ConfigureVerses(_internetSong);
              songLookup.EditValue = null;
            }
          }
          else
            bookLookup.EditValue = null;
        }
        else
          bookLookup.EditValue = null;
      }
      else
        LoadSongLookup(book);
    }

    private void LoadSongLookup(Book book)
    {
      songLookup.Properties.Columns.Clear();
      LookupEdit.Initialize(songLookup, "Number", (string)null, DataSource.GetSongItems(book));
      songLookup.Properties.Columns.Add(new LookUpColumnInfo("Name", 150));
    }

    public void UpdateSongList()
    {
      if ((SlideType)typeLookup.EditValue != SlideType.Song || bookLookup.EditValue == null)
        return;
      Book book = (Book)bookLookup.EditValue;
      if (book.BookID != -1)
      {
        object editValue = songLookup.EditValue;
        LoadSongLookup(book);
        if (editValue != null)
        {
          songLookup.Text = ((SongItem)editValue).Number.ToString();
          songLookup.ClosePopup();
        }
      }
    }

    private void messageTypeLookup_EditValueChanged(object sender, EventArgs e)
    {
      if (messageTypeLookup.EditValue == null)
        return;
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      Changed?.Invoke(this, EventArgs.Empty);
      MessageType messageType = (MessageType)messageTypeLookup.EditValue;
      messageLookup.Properties.Columns.Clear();
      LookupEdit.Initialize(messageLookup, "Text", "MessageID", (IEnumerable)messageType.Messages);
      messageLookup.Properties.Columns.Add(new LookUpColumnInfo("Code", 5));
    }

    private void songLookup_EditValueChanged(object sender, EventArgs e)
    {
      if (songLookup.EditValue == null)
        return;
      ConfigureVerses(songLookup.EditValue as SongItem);
      // ISSUE: reference to a compiler-generated field
      Changed?.Invoke(this, EventArgs.Empty);
    }

    private void ConfigureVerses(SongItem song)
    {
      verseCheckList.Items.Clear();
      for (int index = 1; index <= song.VerseCount; index++)
        verseCheckList.Items.Add(index, index == 1);
    }

    private void ConfigureVerses(Song song)
    {
      verseCheckList.Items.Clear();
      foreach (Verse verse in song.Verses)
        verseCheckList.Items.Add(Convert.ToInt32(verse.VerseNumber), (int)verse.VerseNumber == 1);
    }

    private void verseCheckList_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) && (int)e.KeyChar != 48)
      {
        int num = Convert.ToInt32(e.KeyChar.ToString());
        if (num > verseCheckList.Items.Count)
          return;
        CheckedListBoxItem checkedListBoxItem = verseCheckList.Items[num - 1];
        checkedListBoxItem.CheckState = checkedListBoxItem.CheckState != CheckState.Unchecked ? CheckState.Unchecked : CheckState.Checked;
      }
      else
      {
        if ((int)e.KeyChar != 97)
          return;
        foreach (CheckedListBoxItem checkedListBoxItem in (CollectionBase)verseCheckList.Items)
          checkedListBoxItem.CheckState = CheckState.Checked;
      }
    }

    private void SlideControl_Click(object sender, EventArgs e)
    {
      ParentBox.SetFocusControl(this);
    }

    public SlideItem CreateSlide()
    {
      switch ((SlideType)typeLookup.EditValue)
      {
        case SlideType.Song:
          Song song = (Song)null;
          if (_internetSong != null)
            song = _internetSong;
          else if (songLookup.EditValue != null)
            song = DataSource.GetSong(((SongItem)songLookup.EditValue).SongID);
          List<int> verses = new List<int>();
          foreach (CheckedListBoxItem checkedListBoxItem in (CollectionBase)verseCheckList.Items)
          {
            if (checkedListBoxItem.CheckState == CheckState.Checked)
              verses.Add((int)checkedListBoxItem.Value);
          }
          return (SlideItem)new SongSlide(song, verses);
        case SlideType.Message:
          return (SlideItem)new MessageSlide(messageLookup.Text);
        case SlideType.BiblePassage:
          return (SlideItem)new PassageSlide(passageTextBox.Text);
        default:
          return (SlideItem)new BlankSlide();
      }
    }

    private void openButton_Click(object sender, EventArgs e)
    {
      SongEditDialog songEditDialog = new SongEditDialog();
      if (songLookup.EditValue != null)
      {
        SongItem songItem = (SongItem)songLookup.EditValue;
        songEditDialog.EditSong(songItem.SongID);
      }
      else if (_internetSong != null)
        songEditDialog.EditSong(_internetSong);
      DialogResult dialogResult = songEditDialog.ShowDialog();
      int num;
      switch (dialogResult)
      {
        case DialogResult.Abort:
          bookLookup_EditValueChanged(null, EventArgs.Empty);
          return;
        case DialogResult.OK:
          num = 1;
          break;
        default:
          num = dialogResult == DialogResult.Retry ? 1 : 0;
          break;
      }
      if (num == 0)
        return;
      // ISSUE: reference to a compiler-generated field
      if (Changed != null)
      {
        // ISSUE: reference to a compiler-generated field
        Changed(this, EventArgs.Empty);
      }
      int? selectedSongId = songEditDialog.SelectedSongID;
      if (selectedSongId.HasValue)
      {
        bookLookup.EditValue = DataSource.GetBookBySong(selectedSongId.Value);
        UpdateSongList();
        foreach (SongItem song in (List<SongItem>)songLookup.Properties.DataSource)
        {
          if (song.SongID == selectedSongId.Value)
          {
            songLookup.EditValue = song;
            songLookup.ClosePopup();
            ConfigureVerses(song);
            _internetSong = (Song)null;
            break;
          }
        }
      }
      else
        ConfigureVerses(_internetSong);
    }

    public void WriteToXML(XmlWriter writer)
    {
      writer.WriteStartElement("Slide");
      writer.WriteAttributeString("Type", typeLookup.Text);
      switch ((SlideType)typeLookup.EditValue)
      {
        case SlideType.Song:
          if (_internetSong != null)
          {
            _internetSong.WriteToXML(writer);
          }
          else
          {
            if (bookLookup.EditValue != null)
              writer.WriteAttributeString("Book", ((Book)bookLookup.EditValue).BookID.ToString());
            if (songLookup.EditValue != null)
              writer.WriteAttributeString("Song", ((SongItem)songLookup.EditValue).SongID.ToString());
          }
          writer.WriteStartElement("SelectedVerses");
          foreach (CheckedListBoxItem checkedListBoxItem in (CollectionBase)verseCheckList.Items)
          {
            if (checkedListBoxItem.CheckState == CheckState.Checked)
            {
              writer.WriteStartElement("Verse");
              writer.WriteAttributeString("Number", Convert.ToString(checkedListBoxItem.Value));
              writer.WriteEndElement();
            }
          }
          writer.WriteEndElement();
          break;
        case SlideType.Message:
          if (messageTypeLookup.EditValue != null)
            writer.WriteAttributeString("MessageType", ((MessageType)messageTypeLookup.EditValue).MessageTypeID.ToString());
          if (messageLookup.EditValue != null)
          {
            writer.WriteAttributeString("Message", Convert.ToString(messageLookup.EditValue));
            break;
          }
          break;
        case SlideType.BiblePassage:
          writer.WriteAttributeString("Passage", passageTextBox.Text);
          break;
      }
      writer.WriteEndElement();
    }

    public void ReadFromXML(XmlReader reader)
    {
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "Type")
          typeLookup.Text = reader.Value;
        else if (reader.Name == "Book")
          bookLookup.EditValue = Enumerable.FirstOrDefault<Book>((IEnumerable<Book>)bookLookup.Properties.DataSource, (Func<Book, bool>)(t => t.BookID == Convert.ToInt32(reader.Value)));
        else if (reader.Name == "Song")
        {
          SongItem song = Enumerable.FirstOrDefault<SongItem>((IEnumerable<SongItem>)songLookup.Properties.DataSource, (Func<SongItem, bool>)(t => t.SongID == Convert.ToInt32(reader.Value)));
          songLookup.EditValue = song;
          reader.Read();
          if (song != null)
            ConfigureVerses(song);
          ReadSelectedVerses(reader);
        }
        else if (reader.Name == "InternetSong")
        {
          _internetSong = new Song();
          bookLookup.Tag = "X";
          bookLookup.EditValue = bookLookup.Properties.GetDataSourceRowByDisplayValue("(Search Internet)");
          bookLookup.Tag = null;
        }
        else if (reader.Name == "Name")
          _internetSong.Name = reader.Value;
        else if (reader.Name == "Refrain")
          _internetSong.Refrain = reader.Value;
        else if (reader.Name == "RefrainFirst")
        {
          _internetSong.IsRefrainFirst = Convert.ToBoolean(reader.Value);
          reader.Read();
          if (reader.Name == "Verses")
          {
            while (true)
            {
              reader.Read();
              if (!(reader.Name != "Verse"))
              {
                Verse entity = new Verse();
                _internetSong.Verses.Add(entity);
                if (reader.MoveToAttribute("Number"))
                  entity.VerseNumber = Convert.ToByte(reader.Value);
                if (reader.MoveToAttribute("Text"))
                  entity.Text = reader.Value;
              }
              else
                break;
            }
            ConfigureVerses(_internetSong);
          }
          ReadSelectedVerses(reader);
        }
        else if (reader.Name == "Passage")
          passageTextBox.Text = reader.Value;
        else if (reader.Name == "MessageType")
          messageTypeLookup.EditValue = Enumerable.FirstOrDefault<MessageType>((IEnumerable<MessageType>)messageTypeLookup.Properties.DataSource, (Func<MessageType, bool>)(t => t.MessageTypeID == Convert.ToInt32(reader.Value)));
        else if (reader.Name == "Message")
          messageLookup.EditValue = Convert.ToInt32(reader.Value);
      }
    }

    private void ReadSelectedVerses(XmlReader reader)
    {
      if (!(reader.Name == "SelectedVerses"))
        return;
      reader.Read();
      while (reader.Name == "Verse")
      {
        if (reader.MoveToAttribute("Number"))
        {
          int num = Convert.ToInt32(reader.Value);
          if (num <= verseCheckList.Items.Count)
            verseCheckList.Items[num - 1].CheckState = CheckState.Checked;
        }
        reader.Read();
      }
    }

    private void verseCheckList_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (Changed == null)
        return;
      // ISSUE: reference to a compiler-generated field
      Changed(this, EventArgs.Empty);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      typeLookup = new LookUpEdit();
      songPanel = new Panel();
      openButton = new Button();
      label3 = new Label();
      label2 = new Label();
      label1 = new Label();
      verseCheckList = new CheckedListBoxControl();
      songLookup = new LookUpEdit();
      bookLookup = new LookUpEdit();
      messagePanel = new Panel();
      label5 = new Label();
      label4 = new Label();
      messageLookup = new LookUpEdit();
      messageTypeLookup = new LookUpEdit();
      passagePanel = new Panel();
      passageTextBox = new TextBox();
      label7 = new Label();
      panel1 = new Panel();
      typeLookup.Properties.BeginInit();
      songPanel.SuspendLayout();
      //verseCheckList.BeginInit();
      songLookup.Properties.BeginInit();
      bookLookup.Properties.BeginInit();
      messagePanel.SuspendLayout();
      messageLookup.Properties.BeginInit();
      messageTypeLookup.Properties.BeginInit();
      passagePanel.SuspendLayout();
      SuspendLayout();
      typeLookup.Location = new Point(3, 6);
      typeLookup.Name = "typeLookup";
      typeLookup.Properties.AllowNullInput = DefaultBoolean.False;
      typeLookup.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      typeLookup.Properties.DropDownRows = 4;
      typeLookup.Properties.NullText = "";
      typeLookup.Properties.ShowFooter = false;
      typeLookup.Properties.ShowHeader = false;
      typeLookup.Properties.ShowLines = false;
      typeLookup.Properties.ThrowExceptionOnInvalidLookUpEditValueType = true;
      typeLookup.Size = new Size(100, 20);
      typeLookup.TabIndex = 0;
      typeLookup.EditValueChanged += new EventHandler(typeLookup_EditValueChanged);
      songPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      songPanel.Controls.Add((Control)openButton);
      songPanel.Controls.Add((Control)label3);
      songPanel.Controls.Add((Control)label2);
      songPanel.Controls.Add((Control)label1);
      songPanel.Controls.Add((Control)verseCheckList);
      songPanel.Controls.Add((Control)songLookup);
      songPanel.Controls.Add((Control)bookLookup);
      songPanel.Location = new Point(105, 1);
      songPanel.Name = "songPanel";
      songPanel.Size = new Size(490, 48);
      songPanel.TabIndex = 2;
      openButton.Location = new Point((int)sbyte.MaxValue, 24);
      openButton.Name = "openButton";
      openButton.Size = new Size(46, 21);
      openButton.TabIndex = 11;
      openButton.Text = "Open";
      openButton.UseVisualStyleBackColor = true;
      openButton.Click += new EventHandler(openButton_Click);
      label3.AutoSize = true;
      label3.Location = new Point(7, 28);
      label3.Name = "label3";
      label3.Size = new Size(32, 13);
      label3.TabIndex = 10;
      label3.Text = "Song";
      label2.AutoSize = true;
      label2.Location = new Point(7, 6);
      label2.Name = "label2";
      label2.Size = new Size(32, 13);
      label2.TabIndex = 9;
      label2.Text = "Book";
      label1.AutoSize = true;
      label1.Location = new Point(179, 6);
      label1.Name = "label1";
      label1.Size = new Size(39, 13);
      label1.TabIndex = 8;
      label1.Text = "Verses";
      verseCheckList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      verseCheckList.CheckOnClick = true;
      verseCheckList.ColumnWidth = 35;
      verseCheckList.Location = new Point(224, 4);
      verseCheckList.MultiColumn = true;
      verseCheckList.Name = "verseCheckList";
      verseCheckList.SelectionMode = SelectionMode.None;
      verseCheckList.Size = new Size((int)byte.MaxValue, 41);
      verseCheckList.TabIndex = 7;
      verseCheckList.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(verseCheckList_ItemCheck);
      verseCheckList.KeyPress += new KeyPressEventHandler(verseCheckList_KeyPress);
      songLookup.Location = new Point(44, 25);
      songLookup.Name = "songLookup";
      songLookup.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      songLookup.Properties.NullText = "";
      songLookup.Size = new Size(68, 20);
      songLookup.TabIndex = 6;
      songLookup.EditValueChanged += new EventHandler(songLookup_EditValueChanged);
      bookLookup.Location = new Point(44, 3);
      bookLookup.Name = "bookLookup";
      bookLookup.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      bookLookup.Size = new Size(129, 20);
      bookLookup.TabIndex = 5;
      bookLookup.EditValueChanged += new EventHandler(bookLookup_EditValueChanged);
      messagePanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      messagePanel.Controls.Add((Control)label5);
      messagePanel.Controls.Add((Control)label4);
      messagePanel.Controls.Add((Control)messageLookup);
      messagePanel.Controls.Add((Control)messageTypeLookup);
      messagePanel.Location = new Point(105, 64);
      messagePanel.Name = "messagePanel";
      messagePanel.Size = new Size(490, 30);
      messagePanel.TabIndex = 3;
      label5.AutoSize = true;
      label5.Location = new Point(148, 8);
      label5.Name = "label5";
      label5.Size = new Size(50, 13);
      label5.TabIndex = 12;
      label5.Text = "Message";
      label4.AutoSize = true;
      label4.Location = new Point(7, 8);
      label4.Name = "label4";
      label4.Size = new Size(31, 13);
      label4.TabIndex = 11;
      label4.Text = "Type";
      messageLookup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      messageLookup.Location = new Point(204, 5);
      messageLookup.Name = "messageLookup";
      messageLookup.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      messageLookup.Properties.DropDownRows = 15;
      messageLookup.Properties.NullText = "";
      messageLookup.Properties.PopupWidth = 400;
      messageLookup.Size = new Size(275, 20);
      messageLookup.TabIndex = 10;
      messageTypeLookup.Location = new Point(44, 5);
      messageTypeLookup.Name = "messageTypeLookup";
      messageTypeLookup.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      messageTypeLookup.Size = new Size(98, 20);
      messageTypeLookup.TabIndex = 9;
      messageTypeLookup.EditValueChanged += new EventHandler(messageTypeLookup_EditValueChanged);
      passagePanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      passagePanel.Controls.Add((Control)passageTextBox);
      passagePanel.Controls.Add((Control)label7);
      passagePanel.Location = new Point(105, 99);
      passagePanel.Name = "passagePanel";
      passagePanel.Size = new Size(490, 30);
      passagePanel.TabIndex = 14;
      passageTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      passageTextBox.Location = new Point(57, 5);
      passageTextBox.Name = "passageTextBox";
      passageTextBox.Size = new Size(422, 20);
      passageTextBox.TabIndex = 12;
      label7.AutoSize = true;
      label7.Location = new Point(3, 8);
      label7.Name = "label7";
      label7.Size = new Size(48, 13);
      label7.TabIndex = 11;
      label7.Text = "Passage";
      panel1.BorderStyle = BorderStyle.FixedSingle;
      panel1.Dock = DockStyle.Bottom;
      panel1.Location = new Point(0, 143);
      panel1.Name = "panel1";
      panel1.Size = new Size(598, 1);
      panel1.TabIndex = 15;
      AutoScaleDimensions = new SizeF(6f, 13f);
      AutoScaleMode = AutoScaleMode.Font;
      BackColor = SystemColors.Control;
      Controls.Add((Control)panel1);
      Controls.Add((Control)passagePanel);
      Controls.Add((Control)messagePanel);
      Controls.Add((Control)songPanel);
      Controls.Add((Control)typeLookup);
      Name = "SlideControl";
      Size = new Size(598, 144);
      Click += new EventHandler(SlideControl_Click);
      typeLookup.Properties.EndInit();
      songPanel.ResumeLayout(false);
      songPanel.PerformLayout();
      //verseCheckList.EndInit();
      songLookup.Properties.EndInit();
      bookLookup.Properties.EndInit();
      messagePanel.ResumeLayout(false);
      messagePanel.PerformLayout();
      messageLookup.Properties.EndInit();
      messageTypeLookup.Properties.EndInit();
      passagePanel.ResumeLayout(false);
      passagePanel.PerformLayout();
      ResumeLayout(false);
    }

    public delegate void ChangedEventHandler(object sender, EventArgs e);
  }
}
