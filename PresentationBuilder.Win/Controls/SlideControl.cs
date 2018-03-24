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
    private Song _internetSong = (Song) null;
    private IContainer components = (IContainer) null;
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
      this.InitializeComponent();
      LookupEdit.Initialize(this.typeLookup, typeof (SlideType));
      this.songLookup.Properties.DropDownRows = 15;
      this.bookLookup.Properties.DropDownRows = 15;
      this.messagePanel.Top = this.songPanel.Top;
      this.passagePanel.Top = this.songPanel.Top;
      this.typeLookup.Properties.ForceInitialize();
      this.typeLookup.ItemIndex = 0;
      this.typeLookup_EditValueChanged((object) null, EventArgs.Empty);
      this.SetClickEvent(this.Controls);
    }

    private void SetClickEvent(Control.ControlCollection controls)
    {
      foreach (Control control in (ArrangedElementCollection) controls)
      {
        control.Click += new EventHandler(this.SlideControl_Click);
        this.SetClickEvent(control.Controls);
      }
    }

    public void GotFocus()
    {
      this.BackColor = Color.LightCyan;
    }

    public void LostFocus()
    {
      this.BackColor = Color.FromName("Control");
    }

    private void typeLookup_EditValueChanged(object sender, EventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.Changed != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.Changed((object) this, EventArgs.Empty);
      }
      if (this.typeLookup.EditValue == null)
        return;
      SlideType slideType = (SlideType) this.typeLookup.EditValue;
      this.songPanel.Visible = slideType == SlideType.Song;
      this.messagePanel.Visible = slideType == SlideType.Message;
      this.passagePanel.Visible = slideType == SlideType.BiblePassage;
      if (slideType == SlideType.Song)
      {
        List<Book> books = DataSource.GetBooks();
        books.Add(new Book()
        {
          Title = "(Search Internet)",
          BookID = -1
        });
        LookupEdit.Initialize(this.bookLookup, "Title", (string) null, (IEnumerable) books);
        this.Height = 50;
      }
      else
        this.Height = 33;
      if (slideType != SlideType.Message)
        return;
      LookupEdit.Initialize(this.messageTypeLookup, "Description", (string) null, (IEnumerable) DataSource.GetMessageTypes());
    }

    private void bookLookup_EditValueChanged(object sender, EventArgs e)
    {
      if (this.bookLookup.EditValue == null || this.bookLookup.Tag != null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (this.Changed != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.Changed((object) this, EventArgs.Empty);
      }
      Book book = (Book) this.bookLookup.EditValue;
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
              this.bookLookup.EditValue = (object) selectedSong.Book;
              foreach (SongItem song in (List<SongItem>) this.songLookup.Properties.DataSource)
              {
                if (song.SongID == selectedSongId.Value)
                {
                  this.songLookup.EditValue = (object) song;
                  this.ConfigureVerses(song);
                  break;
                }
              }
            }
            else
            {
              this._internetSong = selectedSong;
              this.ConfigureVerses(this._internetSong);
              this.songLookup.EditValue = (object) null;
            }
          }
          else
            this.bookLookup.EditValue = (object) null;
        }
        else
          this.bookLookup.EditValue = (object) null;
      }
      else
        this.LoadSongLookup(book);
    }

    private void LoadSongLookup(Book book)
    {
      this.songLookup.Properties.Columns.Clear();
      LookupEdit.Initialize(this.songLookup, "Number", (string) null, (IEnumerable) DataSource.GetSongItems(book));
      this.songLookup.Properties.Columns.Add(new LookUpColumnInfo("Name", 150));
    }

    public void UpdateSongList()
    {
      if ((SlideType) this.typeLookup.EditValue != SlideType.Song || this.bookLookup.EditValue == null)
        return;
      Book book = (Book) this.bookLookup.EditValue;
      if (book.BookID != -1)
      {
        object editValue = this.songLookup.EditValue;
        this.LoadSongLookup(book);
        if (editValue != null)
        {
          this.songLookup.Text = ((SongItem) editValue).Number.ToString();
          this.songLookup.ClosePopup();
        }
      }
    }

    private void messageTypeLookup_EditValueChanged(object sender, EventArgs e)
    {
      if (this.messageTypeLookup.EditValue == null)
        return;
      // ISSUE: reference to a compiler-generated field
      if (this.Changed != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.Changed((object) this, EventArgs.Empty);
      }
      MessageType messageType = (MessageType) this.messageTypeLookup.EditValue;
      this.messageLookup.Properties.Columns.Clear();
      LookupEdit.Initialize(this.messageLookup, "Text", "MessageID", (IEnumerable) messageType.Messages);
      this.messageLookup.Properties.Columns.Add(new LookUpColumnInfo("Code", 5));
    }

    private void songLookup_EditValueChanged(object sender, EventArgs e)
    {
      if (this.songLookup.EditValue == null)
        return;
      this.ConfigureVerses(this.songLookup.EditValue as SongItem);
      // ISSUE: reference to a compiler-generated field
      if (this.Changed != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.Changed((object) this, EventArgs.Empty);
      }
    }

    private void ConfigureVerses(SongItem song)
    {
      this.verseCheckList.Items.Clear();
      for (int index = 1; index <= (int) song.VerseCount; ++index)
        this.verseCheckList.Items.Add((object) index, index == 1);
    }

    private void ConfigureVerses(Song song)
    {
      this.verseCheckList.Items.Clear();
      foreach (Verse verse in song.Verses)
        this.verseCheckList.Items.Add((object) Convert.ToInt32(verse.VerseNumber), (int) verse.VerseNumber == 1);
    }

    private void verseCheckList_KeyPress(object sender, KeyPressEventArgs e)
    {
      if (char.IsDigit(e.KeyChar) && (int) e.KeyChar != 48)
      {
        int num = Convert.ToInt32(e.KeyChar.ToString());
        if (num > this.verseCheckList.Items.Count)
          return;
        CheckedListBoxItem checkedListBoxItem = this.verseCheckList.Items[num - 1];
        checkedListBoxItem.CheckState = checkedListBoxItem.CheckState != CheckState.Unchecked ? CheckState.Unchecked : CheckState.Checked;
      }
      else
      {
        if ((int) e.KeyChar != 97)
          return;
        foreach (CheckedListBoxItem checkedListBoxItem in (CollectionBase) this.verseCheckList.Items)
          checkedListBoxItem.CheckState = CheckState.Checked;
      }
    }

    private void SlideControl_Click(object sender, EventArgs e)
    {
      this.ParentBox.SetFocusControl(this);
    }

    public SlideItem CreateSlide()
    {
      switch ((SlideType) this.typeLookup.EditValue)
      {
        case SlideType.Song:
          Song song = (Song) null;
          if (this._internetSong != null)
            song = this._internetSong;
          else if (this.songLookup.EditValue != null)
            song = DataSource.GetSong(((SongItem) this.songLookup.EditValue).SongID);
          List<int> verses = new List<int>();
          foreach (CheckedListBoxItem checkedListBoxItem in (CollectionBase) this.verseCheckList.Items)
          {
            if (checkedListBoxItem.CheckState == CheckState.Checked)
              verses.Add((int) checkedListBoxItem.Value);
          }
          return (SlideItem) new SongSlide(song, verses);
        case SlideType.Message:
          return (SlideItem) new MessageSlide(this.messageLookup.Text);
        case SlideType.BiblePassage:
          return (SlideItem) new PassageSlide(this.passageTextBox.Text);
        default:
          return (SlideItem) new BlankSlide();
      }
    }

    private void openButton_Click(object sender, EventArgs e)
    {
      SongEditDialog songEditDialog = new SongEditDialog();
      if (this.songLookup.EditValue != null)
      {
        SongItem songItem = (SongItem) this.songLookup.EditValue;
        songEditDialog.EditSong(songItem.SongID);
      }
      else if (this._internetSong != null)
        songEditDialog.EditSong(this._internetSong);
      DialogResult dialogResult = songEditDialog.ShowDialog();
      int num;
      switch (dialogResult)
      {
        case DialogResult.Abort:
          this.bookLookup_EditValueChanged((object) null, EventArgs.Empty);
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
      if (this.Changed != null)
      {
        // ISSUE: reference to a compiler-generated field
        this.Changed((object) this, EventArgs.Empty);
      }
      int? selectedSongId = songEditDialog.SelectedSongID;
      if (selectedSongId.HasValue)
      {
        this.bookLookup.EditValue = (object) DataSource.GetBookBySong(selectedSongId.Value);
        this.UpdateSongList();
        foreach (SongItem song in (List<SongItem>) this.songLookup.Properties.DataSource)
        {
          if (song.SongID == selectedSongId.Value)
          {
            this.songLookup.EditValue = (object) song;
            this.songLookup.ClosePopup();
            this.ConfigureVerses(song);
            this._internetSong = (Song) null;
            break;
          }
        }
      }
      else
        this.ConfigureVerses(this._internetSong);
    }

    public void WriteToXML(XmlWriter writer)
    {
      writer.WriteStartElement("Slide");
      writer.WriteAttributeString("Type", this.typeLookup.Text);
      switch ((SlideType) this.typeLookup.EditValue)
      {
        case SlideType.Song:
          if (this._internetSong != null)
          {
            this._internetSong.WriteToXML(writer);
          }
          else
          {
            if (this.bookLookup.EditValue != null)
              writer.WriteAttributeString("Book", ((Book) this.bookLookup.EditValue).BookID.ToString());
            if (this.songLookup.EditValue != null)
              writer.WriteAttributeString("Song", ((SongItem) this.songLookup.EditValue).SongID.ToString());
          }
          writer.WriteStartElement("SelectedVerses");
          foreach (CheckedListBoxItem checkedListBoxItem in (CollectionBase) this.verseCheckList.Items)
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
          if (this.messageTypeLookup.EditValue != null)
            writer.WriteAttributeString("MessageType", ((MessageType) this.messageTypeLookup.EditValue).MessageTypeID.ToString());
          if (this.messageLookup.EditValue != null)
          {
            writer.WriteAttributeString("Message", Convert.ToString(this.messageLookup.EditValue));
            break;
          }
          break;
        case SlideType.BiblePassage:
          writer.WriteAttributeString("Passage", this.passageTextBox.Text);
          break;
      }
      writer.WriteEndElement();
    }

    public void ReadFromXML(XmlReader reader)
    {
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "Type")
          this.typeLookup.Text = reader.Value;
        else if (reader.Name == "Book")
          this.bookLookup.EditValue = (object) Enumerable.FirstOrDefault<Book>((IEnumerable<Book>) this.bookLookup.Properties.DataSource, (Func<Book, bool>) (t => t.BookID == Convert.ToInt32(reader.Value)));
        else if (reader.Name == "Song")
        {
          SongItem song = Enumerable.FirstOrDefault<SongItem>((IEnumerable<SongItem>) this.songLookup.Properties.DataSource, (Func<SongItem, bool>) (t => t.SongID == Convert.ToInt32(reader.Value)));
          this.songLookup.EditValue = (object) song;
          reader.Read();
          if (song != null)
            this.ConfigureVerses(song);
          this.ReadSelectedVerses(reader);
        }
        else if (reader.Name == "InternetSong")
        {
          this._internetSong = new Song();
          this.bookLookup.Tag = (object) "X";
          this.bookLookup.EditValue = this.bookLookup.Properties.GetDataSourceRowByDisplayValue((object) "(Search Internet)");
          this.bookLookup.Tag = (object) null;
        }
        else if (reader.Name == "Name")
          this._internetSong.Name = reader.Value;
        else if (reader.Name == "Refrain")
          this._internetSong.Refrain = reader.Value;
        else if (reader.Name == "RefrainFirst")
        {
          this._internetSong.IsRefrainFirst = Convert.ToBoolean(reader.Value);
          reader.Read();
          if (reader.Name == "Verses")
          {
            while (true)
            {
              reader.Read();
              if (!(reader.Name != "Verse"))
              {
                Verse entity = new Verse();
                this._internetSong.Verses.Add(entity);
                if (reader.MoveToAttribute("Number"))
                  entity.VerseNumber = Convert.ToByte(reader.Value);
                if (reader.MoveToAttribute("Text"))
                  entity.Text = reader.Value;
              }
              else
                break;
            }
            this.ConfigureVerses(this._internetSong);
          }
          this.ReadSelectedVerses(reader);
        }
        else if (reader.Name == "Passage")
          this.passageTextBox.Text = reader.Value;
        else if (reader.Name == "MessageType")
          this.messageTypeLookup.EditValue = (object) Enumerable.FirstOrDefault<MessageType>((IEnumerable<MessageType>) this.messageTypeLookup.Properties.DataSource, (Func<MessageType, bool>) (t => t.MessageTypeID == Convert.ToInt32(reader.Value)));
        else if (reader.Name == "Message")
          this.messageLookup.EditValue = (object) Convert.ToInt32(reader.Value);
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
          if (num <= this.verseCheckList.Items.Count)
            this.verseCheckList.Items[num - 1].CheckState = CheckState.Checked;
        }
        reader.Read();
      }
    }

    private void verseCheckList_ItemCheck(object sender, DevExpress.XtraEditors.Controls.ItemCheckEventArgs e)
    {
      // ISSUE: reference to a compiler-generated field
      if (this.Changed == null)
        return;
      // ISSUE: reference to a compiler-generated field
      this.Changed((object) this, EventArgs.Empty);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.typeLookup = new LookUpEdit();
      this.songPanel = new Panel();
      this.openButton = new Button();
      this.label3 = new Label();
      this.label2 = new Label();
      this.label1 = new Label();
      this.verseCheckList = new CheckedListBoxControl();
      this.songLookup = new LookUpEdit();
      this.bookLookup = new LookUpEdit();
      this.messagePanel = new Panel();
      this.label5 = new Label();
      this.label4 = new Label();
      this.messageLookup = new LookUpEdit();
      this.messageTypeLookup = new LookUpEdit();
      this.passagePanel = new Panel();
      this.passageTextBox = new TextBox();
      this.label7 = new Label();
      this.panel1 = new Panel();
      this.typeLookup.Properties.BeginInit();
      this.songPanel.SuspendLayout();
      //this.verseCheckList.BeginInit();
      this.songLookup.Properties.BeginInit();
      this.bookLookup.Properties.BeginInit();
      this.messagePanel.SuspendLayout();
      this.messageLookup.Properties.BeginInit();
      this.messageTypeLookup.Properties.BeginInit();
      this.passagePanel.SuspendLayout();
      this.SuspendLayout();
      this.typeLookup.Location = new Point(3, 6);
      this.typeLookup.Name = "typeLookup";
      this.typeLookup.Properties.AllowNullInput = DefaultBoolean.False;
      this.typeLookup.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      this.typeLookup.Properties.DropDownRows = 4;
      this.typeLookup.Properties.NullText = "";
      this.typeLookup.Properties.ShowFooter = false;
      this.typeLookup.Properties.ShowHeader = false;
      this.typeLookup.Properties.ShowLines = false;
      this.typeLookup.Properties.ThrowExceptionOnInvalidLookUpEditValueType = true;
      this.typeLookup.Size = new Size(100, 20);
      this.typeLookup.TabIndex = 0;
      this.typeLookup.EditValueChanged += new EventHandler(this.typeLookup_EditValueChanged);
      this.songPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.songPanel.Controls.Add((Control) this.openButton);
      this.songPanel.Controls.Add((Control) this.label3);
      this.songPanel.Controls.Add((Control) this.label2);
      this.songPanel.Controls.Add((Control) this.label1);
      this.songPanel.Controls.Add((Control) this.verseCheckList);
      this.songPanel.Controls.Add((Control) this.songLookup);
      this.songPanel.Controls.Add((Control) this.bookLookup);
      this.songPanel.Location = new Point(105, 1);
      this.songPanel.Name = "songPanel";
      this.songPanel.Size = new Size(490, 48);
      this.songPanel.TabIndex = 2;
      this.openButton.Location = new Point((int) sbyte.MaxValue, 24);
      this.openButton.Name = "openButton";
      this.openButton.Size = new Size(46, 21);
      this.openButton.TabIndex = 11;
      this.openButton.Text = "Open";
      this.openButton.UseVisualStyleBackColor = true;
      this.openButton.Click += new EventHandler(this.openButton_Click);
      this.label3.AutoSize = true;
      this.label3.Location = new Point(7, 28);
      this.label3.Name = "label3";
      this.label3.Size = new Size(32, 13);
      this.label3.TabIndex = 10;
      this.label3.Text = "Song";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(7, 6);
      this.label2.Name = "label2";
      this.label2.Size = new Size(32, 13);
      this.label2.TabIndex = 9;
      this.label2.Text = "Book";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(179, 6);
      this.label1.Name = "label1";
      this.label1.Size = new Size(39, 13);
      this.label1.TabIndex = 8;
      this.label1.Text = "Verses";
      this.verseCheckList.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.verseCheckList.CheckOnClick = true;
      this.verseCheckList.ColumnWidth = 35;
      this.verseCheckList.Location = new Point(224, 4);
      this.verseCheckList.MultiColumn = true;
      this.verseCheckList.Name = "verseCheckList";
      this.verseCheckList.SelectionMode = SelectionMode.None;
      this.verseCheckList.Size = new Size((int) byte.MaxValue, 41);
      this.verseCheckList.TabIndex = 7;
      this.verseCheckList.ItemCheck += new DevExpress.XtraEditors.Controls.ItemCheckEventHandler(this.verseCheckList_ItemCheck);
      this.verseCheckList.KeyPress += new KeyPressEventHandler(this.verseCheckList_KeyPress);
      this.songLookup.Location = new Point(44, 25);
      this.songLookup.Name = "songLookup";
      this.songLookup.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      this.songLookup.Properties.NullText = "";
      this.songLookup.Size = new Size(68, 20);
      this.songLookup.TabIndex = 6;
      this.songLookup.EditValueChanged += new EventHandler(this.songLookup_EditValueChanged);
      this.bookLookup.Location = new Point(44, 3);
      this.bookLookup.Name = "bookLookup";
      this.bookLookup.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      this.bookLookup.Size = new Size(129, 20);
      this.bookLookup.TabIndex = 5;
      this.bookLookup.EditValueChanged += new EventHandler(this.bookLookup_EditValueChanged);
      this.messagePanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.messagePanel.Controls.Add((Control) this.label5);
      this.messagePanel.Controls.Add((Control) this.label4);
      this.messagePanel.Controls.Add((Control) this.messageLookup);
      this.messagePanel.Controls.Add((Control) this.messageTypeLookup);
      this.messagePanel.Location = new Point(105, 64);
      this.messagePanel.Name = "messagePanel";
      this.messagePanel.Size = new Size(490, 30);
      this.messagePanel.TabIndex = 3;
      this.label5.AutoSize = true;
      this.label5.Location = new Point(148, 8);
      this.label5.Name = "label5";
      this.label5.Size = new Size(50, 13);
      this.label5.TabIndex = 12;
      this.label5.Text = "Message";
      this.label4.AutoSize = true;
      this.label4.Location = new Point(7, 8);
      this.label4.Name = "label4";
      this.label4.Size = new Size(31, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "Type";
      this.messageLookup.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.messageLookup.Location = new Point(204, 5);
      this.messageLookup.Name = "messageLookup";
      this.messageLookup.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      this.messageLookup.Properties.DropDownRows = 15;
      this.messageLookup.Properties.NullText = "";
      this.messageLookup.Properties.PopupWidth = 400;
      this.messageLookup.Size = new Size(275, 20);
      this.messageLookup.TabIndex = 10;
      this.messageTypeLookup.Location = new Point(44, 5);
      this.messageTypeLookup.Name = "messageTypeLookup";
      this.messageTypeLookup.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      this.messageTypeLookup.Size = new Size(98, 20);
      this.messageTypeLookup.TabIndex = 9;
      this.messageTypeLookup.EditValueChanged += new EventHandler(this.messageTypeLookup_EditValueChanged);
      this.passagePanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.passagePanel.Controls.Add((Control) this.passageTextBox);
      this.passagePanel.Controls.Add((Control) this.label7);
      this.passagePanel.Location = new Point(105, 99);
      this.passagePanel.Name = "passagePanel";
      this.passagePanel.Size = new Size(490, 30);
      this.passagePanel.TabIndex = 14;
      this.passageTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this.passageTextBox.Location = new Point(57, 5);
      this.passageTextBox.Name = "passageTextBox";
      this.passageTextBox.Size = new Size(422, 20);
      this.passageTextBox.TabIndex = 12;
      this.label7.AutoSize = true;
      this.label7.Location = new Point(3, 8);
      this.label7.Name = "label7";
      this.label7.Size = new Size(48, 13);
      this.label7.TabIndex = 11;
      this.label7.Text = "Passage";
      this.panel1.BorderStyle = BorderStyle.FixedSingle;
      this.panel1.Dock = DockStyle.Bottom;
      this.panel1.Location = new Point(0, 143);
      this.panel1.Name = "panel1";
      this.panel1.Size = new Size(598, 1);
      this.panel1.TabIndex = 15;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackColor = SystemColors.Control;
      this.Controls.Add((Control) this.panel1);
      this.Controls.Add((Control) this.passagePanel);
      this.Controls.Add((Control) this.messagePanel);
      this.Controls.Add((Control) this.songPanel);
      this.Controls.Add((Control) this.typeLookup);
      this.Name = "SlideControl";
      this.Size = new Size(598, 144);
      this.Click += new EventHandler(this.SlideControl_Click);
      this.typeLookup.Properties.EndInit();
      this.songPanel.ResumeLayout(false);
      this.songPanel.PerformLayout();
      //this.verseCheckList.EndInit();
      this.songLookup.Properties.EndInit();
      this.bookLookup.Properties.EndInit();
      this.messagePanel.ResumeLayout(false);
      this.messagePanel.PerformLayout();
      this.messageLookup.Properties.EndInit();
      this.messageTypeLookup.Properties.EndInit();
      this.passagePanel.ResumeLayout(false);
      this.passagePanel.PerformLayout();
      this.ResumeLayout(false);
    }

    public delegate void ChangedEventHandler(object sender, EventArgs e);
  }
}
