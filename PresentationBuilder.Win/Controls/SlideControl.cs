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
    public Panel messagePanel;
    private Label label5;
    private Label label4;
    private LookUpEdit messageLookup;
    private LookUpEdit messageTypeLookup;
    private Panel passagePanel;
    private TextBox passageTextBox;
    private Label label7;
    private Panel panel1;
    private Button openButton;
    private Panel filePanel;
    private Button browseButton;
    private TextBox fileTextBox;
    private Label label6;
    public LookUpEdit typeLookup;

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
      filePanel.Top = songPanel.Top;
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
      filePanel.Visible = (slideType == SlideType.File);
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

      if (slideType == SlideType.Message)
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
        case SlideType.File:
          return new FileSlide(fileTextBox.Tag.ToString());
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
      if (Changed != null)
        Changed(this, EventArgs.Empty);
    }

    private void browseButton_Click(object sender, EventArgs e)
    {
      OpenFileDialog openFileDialog = new OpenFileDialog();
      openFileDialog.Filter = "Powerpoint Files (*.pptx) | *.pptx";
      if (openFileDialog.ShowDialog() == DialogResult.OK)
      {
        fileTextBox.Text = System.IO.Path.GetFileName(openFileDialog.FileName);
        fileTextBox.Tag = openFileDialog.FileName;
      }
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.typeLookup = new DevExpress.XtraEditors.LookUpEdit();
      this.songPanel = new System.Windows.Forms.Panel();
      this.openButton = new System.Windows.Forms.Button();
      this.label3 = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.label1 = new System.Windows.Forms.Label();
      this.verseCheckList = new DevExpress.XtraEditors.CheckedListBoxControl();
      this.songLookup = new DevExpress.XtraEditors.LookUpEdit();
      this.bookLookup = new DevExpress.XtraEditors.LookUpEdit();
      this.messagePanel = new System.Windows.Forms.Panel();
      this.label5 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.messageLookup = new DevExpress.XtraEditors.LookUpEdit();
      this.messageTypeLookup = new DevExpress.XtraEditors.LookUpEdit();
      this.passagePanel = new System.Windows.Forms.Panel();
      this.passageTextBox = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.panel1 = new System.Windows.Forms.Panel();
      this.filePanel = new System.Windows.Forms.Panel();
      this.browseButton = new System.Windows.Forms.Button();
      this.fileTextBox = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.typeLookup.Properties)).BeginInit();
      this.songPanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.verseCheckList)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.songLookup.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.bookLookup.Properties)).BeginInit();
      this.messagePanel.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.messageLookup.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.messageTypeLookup.Properties)).BeginInit();
      this.passagePanel.SuspendLayout();
      this.filePanel.SuspendLayout();
      this.SuspendLayout();
      // 
      // typeLookup
      // 
      this.typeLookup.Location = new System.Drawing.Point(3, 6);
      this.typeLookup.Name = "typeLookup";
      // 
      // 
      // 
      this.typeLookup.Properties.AllowNullInput = DevExpress.Utils.DefaultBoolean.False;
      this.typeLookup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.typeLookup.Properties.DropDownRows = 5;
      this.typeLookup.Properties.NullText = "";
      this.typeLookup.Properties.ShowFooter = false;
      this.typeLookup.Properties.ShowHeader = false;
      this.typeLookup.Properties.ShowLines = false;
      this.typeLookup.Properties.ThrowExceptionOnInvalidLookUpEditValueType = true;
      this.typeLookup.TabIndex = 0;
      this.typeLookup.EditValueChanged += new System.EventHandler(this.typeLookup_EditValueChanged);
      // 
      // songPanel
      // 
      this.songPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.songPanel.Controls.Add(this.openButton);
      this.songPanel.Controls.Add(this.label3);
      this.songPanel.Controls.Add(this.label2);
      this.songPanel.Controls.Add(this.label1);
      this.songPanel.Controls.Add(this.verseCheckList);
      this.songPanel.Controls.Add(this.songLookup);
      this.songPanel.Controls.Add(this.bookLookup);
      this.songPanel.Location = new System.Drawing.Point(105, 1);
      this.songPanel.Name = "songPanel";
      this.songPanel.Size = new System.Drawing.Size(490, 48);
      this.songPanel.TabIndex = 2;
      // 
      // openButton
      // 
      this.openButton.Location = new System.Drawing.Point(127, 24);
      this.openButton.Name = "openButton";
      this.openButton.Size = new System.Drawing.Size(46, 21);
      this.openButton.TabIndex = 11;
      this.openButton.Text = "Open";
      this.openButton.UseVisualStyleBackColor = true;
      this.openButton.Click += new System.EventHandler(this.openButton_Click);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(7, 28);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(32, 13);
      this.label3.TabIndex = 10;
      this.label3.Text = "Song";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(7, 6);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(32, 13);
      this.label2.TabIndex = 9;
      this.label2.Text = "Book";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(179, 6);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(39, 13);
      this.label1.TabIndex = 8;
      this.label1.Text = "Verses";
      // 
      // verseCheckList
      // 
      this.verseCheckList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.verseCheckList.CheckOnClick = true;
      this.verseCheckList.ColumnWidth = 35;
      this.verseCheckList.Location = new System.Drawing.Point(224, 4);
      this.verseCheckList.MultiColumn = true;
      this.verseCheckList.Name = "verseCheckList";
      this.verseCheckList.SelectionMode = System.Windows.Forms.SelectionMode.None;
      this.verseCheckList.Size = new System.Drawing.Size(255, 41);
      this.verseCheckList.TabIndex = 7;
      this.verseCheckList.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.verseCheckList_ItemCheck);
      this.verseCheckList.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.verseCheckList_KeyPress);
      // 
      // songLookup
      // 
      this.songLookup.Location = new System.Drawing.Point(44, 25);
      this.songLookup.Name = "songLookup";
      // 
      // 
      // 
      this.songLookup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.songLookup.Properties.NullText = "";
      this.songLookup.Size = new System.Drawing.Size(68, 20);
      this.songLookup.TabIndex = 6;
      this.songLookup.EditValueChanged += new System.EventHandler(this.songLookup_EditValueChanged);
      // 
      // bookLookup
      // 
      this.bookLookup.Location = new System.Drawing.Point(44, 3);
      this.bookLookup.Name = "bookLookup";
      // 
      // 
      // 
      this.bookLookup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.bookLookup.Size = new System.Drawing.Size(129, 20);
      this.bookLookup.TabIndex = 5;
      this.bookLookup.EditValueChanged += new System.EventHandler(this.bookLookup_EditValueChanged);
      // 
      // messagePanel
      // 
      this.messagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.messagePanel.Controls.Add(this.label5);
      this.messagePanel.Controls.Add(this.label4);
      this.messagePanel.Controls.Add(this.messageLookup);
      this.messagePanel.Controls.Add(this.messageTypeLookup);
      this.messagePanel.Location = new System.Drawing.Point(105, 55);
      this.messagePanel.Name = "messagePanel";
      this.messagePanel.Size = new System.Drawing.Size(490, 30);
      this.messagePanel.TabIndex = 3;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(148, 8);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(50, 13);
      this.label5.TabIndex = 12;
      this.label5.Text = "Message";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(7, 8);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(31, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "Type";
      // 
      // messageLookup
      // 
      this.messageLookup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.messageLookup.Location = new System.Drawing.Point(204, 5);
      this.messageLookup.Name = "messageLookup";
      // 
      // 
      // 
      this.messageLookup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.messageLookup.Properties.DropDownRows = 15;
      this.messageLookup.Properties.NullText = "";
      this.messageLookup.Properties.PopupWidth = 400;
      this.messageLookup.Size = new System.Drawing.Size(275, 20);
      this.messageLookup.TabIndex = 10;
      // 
      // messageTypeLookup
      // 
      this.messageTypeLookup.Location = new System.Drawing.Point(44, 5);
      this.messageTypeLookup.Name = "messageTypeLookup";
      // 
      // 
      // 
      this.messageTypeLookup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.messageTypeLookup.Size = new System.Drawing.Size(98, 20);
      this.messageTypeLookup.TabIndex = 9;
      this.messageTypeLookup.EditValueChanged += new System.EventHandler(this.messageTypeLookup_EditValueChanged);
      // 
      // passagePanel
      // 
      this.passagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.passagePanel.Controls.Add(this.passageTextBox);
      this.passagePanel.Controls.Add(this.label7);
      this.passagePanel.Location = new System.Drawing.Point(105, 91);
      this.passagePanel.Name = "passagePanel";
      this.passagePanel.Size = new System.Drawing.Size(490, 30);
      this.passagePanel.TabIndex = 14;
      // 
      // passageTextBox
      // 
      this.passageTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.passageTextBox.Location = new System.Drawing.Point(57, 5);
      this.passageTextBox.Name = "passageTextBox";
      this.passageTextBox.Size = new System.Drawing.Size(422, 20);
      this.passageTextBox.TabIndex = 12;
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(3, 8);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(48, 13);
      this.label7.TabIndex = 11;
      this.label7.Text = "Passage";
      // 
      // panel1
      // 
      this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel1.Location = new System.Drawing.Point(0, 156);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(598, 1);
      this.panel1.TabIndex = 15;
      // 
      // filePanel
      // 
      this.filePanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.filePanel.Controls.Add(this.browseButton);
      this.filePanel.Controls.Add(this.fileTextBox);
      this.filePanel.Controls.Add(this.label6);
      this.filePanel.Location = new System.Drawing.Point(105, 123);
      this.filePanel.Name = "filePanel";
      this.filePanel.Size = new System.Drawing.Size(490, 30);
      this.filePanel.TabIndex = 16;
      // 
      // browseButton
      // 
      this.browseButton.Location = new System.Drawing.Point(323, 5);
      this.browseButton.Name = "browseButton";
      this.browseButton.Size = new System.Drawing.Size(60, 21);
      this.browseButton.TabIndex = 13;
      this.browseButton.Text = "Browse";
      this.browseButton.UseVisualStyleBackColor = true;
      this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
      // 
      // fileTextBox
      // 
      this.fileTextBox.Location = new System.Drawing.Point(57, 5);
      this.fileTextBox.Name = "fileTextBox";
      this.fileTextBox.ReadOnly = true;
      this.fileTextBox.Size = new System.Drawing.Size(260, 20);
      this.fileTextBox.TabIndex = 12;
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(3, 8);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(54, 13);
      this.label6.TabIndex = 11;
      this.label6.Text = "File Name";
      // 
      // SlideControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BackColor = System.Drawing.SystemColors.Control;
      this.Controls.Add(this.filePanel);
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.passagePanel);
      this.Controls.Add(this.messagePanel);
      this.Controls.Add(this.songPanel);
      this.Controls.Add(this.typeLookup);
      this.Name = "SlideControl";
      this.Size = new System.Drawing.Size(598, 157);
      this.Click += new System.EventHandler(this.SlideControl_Click);
      ((System.ComponentModel.ISupportInitialize)(this.typeLookup.Properties)).EndInit();
      this.songPanel.ResumeLayout(false);
      this.songPanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.verseCheckList)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.songLookup.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.bookLookup.Properties)).EndInit();
      this.messagePanel.ResumeLayout(false);
      this.messagePanel.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.messageLookup.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.messageTypeLookup.Properties)).EndInit();
      this.passagePanel.ResumeLayout(false);
      this.passagePanel.PerformLayout();
      this.filePanel.ResumeLayout(false);
      this.filePanel.PerformLayout();
      this.ResumeLayout(false);

    }

    public delegate void ChangedEventHandler(object sender, EventArgs e);



  }
}
