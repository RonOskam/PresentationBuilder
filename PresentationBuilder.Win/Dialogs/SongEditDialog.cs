// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.Win.Dialogs.SongEditDialog
// Assembly: PresentationBuilder, Version=1.0.0.28120, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 295F5AD1-A97E-4830-A536-CA2F8525E5B1
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.exe

using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using PEC.Windows.Common.Controls;
using PEC.Windows.Common.Dialogs;
using PresentationBuilder.BLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Windows.Forms;

namespace PresentationBuilder.Win.Dialogs
{
  public class SongEditDialog : OkCancelDialog
  {
    private PresentationBuilderEntities _context = new PresentationBuilderEntities();
    private bool _saved = false;
    private bool _isNew = false;
    private IContainer components = (IContainer) null;
    private Song _song;
    private int? _songID;
    private TextBox numberTextBox;
    private Label numberLabel;
    private Label label2;
    private TextBox textBox2;
    private GridContainer verseGridContainer;
    private GridControl verseGridControl;
    private GridView verseGridView;
    private BindingSource verseBindingSource;
    private GridColumn colVerseNumber;
    private GridColumn colText;
    private RepositoryItemMemoEdit repositoryItemMemoEdit1;
    private CheckBox checkBox1;
    private BindingSource songBindingSource;
    private MemoEdit refrainMemo;
    private Label label3;
    private Label bookLabel;
    private TextBox textBox3;
    private Button saveAsButton;
    private Button deleteSongButton;

    public bool MustSave { get; set; }

    public int? SelectedSongID
    {
      get
      {
        return this._songID;
      }
    }

    public SongEditDialog()
    {
      this.InitializeComponent();
      this.verseGridContainer.addButton.Click += new EventHandler(this.addButton_Click);
      this.verseGridContainer.deleteButton.Click += new EventHandler(this.deleteButton_Click);
      this.verseGridContainer.addButton.ToolTip = "Add a Verse";
      this.verseGridContainer.deleteButton.ToolTip = "Delete the selected Verse.";
      this.verseGridView.SortInfo.Add(this.colVerseNumber, ColumnSortOrder.Ascending);
    }

    private void deleteButton_Click(object sender, EventArgs e)
    {
      if (this.verseBindingSource.Current == null || MessageBox.Show("Are you sure you want to delete the selected item?", Application.ProductName, MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      Verse entity = (Verse) this.verseBindingSource.Current;
      if (this._songID.HasValue)
        this._context.Verses.Remove(entity);
      else
        this._song.Verses.Remove(entity);
    }

    private void addButton_Click(object sender, EventArgs e)
    {
      Song song = (Song) this.songBindingSource.Current;
      Verse entity = new Verse();
      if (song.Verses.Count == 0)
      {
        entity.VerseNumber = 1;
      }
      else
      {
        entity.VerseNumber = Enumerable.Max<Verse, byte>((IEnumerable<Verse>) song.Verses, (Func<Verse, byte>) (v => v.VerseNumber));
        ++entity.VerseNumber;
      }
      song.Verses.Add(entity);
    }

    public void EditSong(int songId)
    {
      this._song = DataSource.GetSong(this._context, songId);
      this.songBindingSource.DataSource = (object) this._song;
      this._songID = new int?(songId);
      this.Text = "Edit Song";
    }

    public void EditSong(Song song)
    {
      this._song = song;
      this.songBindingSource.DataSource = (object) this._song;
      if (song.Book == null)
      {
        this._songID = new int?();
        this.saveAsButton.Visible = true;
        this.deleteSongButton.Visible = false;
        this.bookLabel.Enabled = false;
        this.numberLabel.Enabled = false;
        this.numberTextBox.Enabled = false;
      }
      else
        this._songID = new int?(song.SongID);
      this.Text = "Edit Song";
    }

    public void AddSong(int bookId)
    {
      this._song = new Song();
      this._song.Book = Queryable.FirstOrDefault<Book>((IQueryable<Book>) this._context.Books, (Expression<Func<Book, bool>>) (b => b.BookID == bookId));
      this._song.EnteredBy = WindowsIdentity.GetCurrent().Name;
      this._context.Songs.Add(_song);
      this.songBindingSource.DataSource = (object) this._song;
      this._isNew = true;
      this.Text = "Add Song";
    }

    private void _okButton_Click(object sender, EventArgs e)
    {
      string text = (string) null;
      if (!this._songID.HasValue && this.MustSave)
        text = "Click on Save As to save the song first.";
      else if (this._song.Verses.Count == 0)
        text = "You must enter at least one verse.";
      else if (this._song.Verses.Count != (int) Enumerable.Max<Verse, byte>((IEnumerable<Verse>) this._song.Verses, (Func<Verse, byte>) (v => v.VerseNumber)))
        text = "Please enter all the verses for the song.";
      else if (string.IsNullOrEmpty(this._song.Name))
        text = "Name is required.";
      else if (this._song.IsRefrainFirst && string.IsNullOrEmpty(this._song.Refrain))
      {
        text = "You must enter a refrain if Refrain First is checked.";
      }
      else
      {
        int num;
        if (this._isNew)
          num = (uint) Queryable.Count<Song>((IQueryable<Song>) PresentationBuilderEntities.Context.Songs, (Expression<Func<Song, bool>>) (s => (int) s.Number == (int) this._song.Number && s.Book.BookID == this._song.Book.BookID)) > 0U ? 1 : 0;
        else
          num = 0;
        if (num != 0)
          text = "There is already a song with that number in this book.";
        else if ((int) this._song.Number == 0)
          text = "0 is not a valid song number.";
      }
      if (text != null)
      {
        int num = (int) MessageBox.Show(text, Application.ProductName);
        this.DialogResult = DialogResult.None;
      }
      else
      {
        DataSource.ResetBook(this._song.Book);
        this._context.SaveChanges();
        if (this._saved)
          this.DialogResult = DialogResult.Retry;
      }
    }

    private void saveAsButton_Click(object sender, EventArgs e)
    {
      if (new SongSaveAsDialog(this._song, !this.MustSave).ShowDialog() != DialogResult.OK)
        return;
      this.EditSong(this._song.SongID);
      this._saved = true;
      this.saveAsButton.Visible = false;
      this.bookLabel.Enabled = true;
      this.numberLabel.Enabled = true;
      this.numberTextBox.Enabled = true;
      this.deleteSongButton.Visible = true;
    }

    private void deleteSongButton_Click(object sender, EventArgs e)
    {
      if (!this._songID.HasValue || MessageBox.Show("Are you sure you want to delete this song?", Application.ProductName, MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      DataSource.DeleteSong(this._songID.Value);
      this._songID = new int?();
      this.DialogResult = DialogResult.Abort;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.components = (IContainer) new Container();
      this.numberTextBox = new TextBox();
      this.songBindingSource = new BindingSource(this.components);
      this.numberLabel = new Label();
      this.label2 = new Label();
      this.textBox2 = new TextBox();
      this.verseGridContainer = new GridContainer();
      this.verseGridControl = new GridControl();
      this.verseBindingSource = new BindingSource(this.components);
      this.verseGridView = new GridView();
      this.colVerseNumber = new GridColumn();
      this.colText = new GridColumn();
      this.repositoryItemMemoEdit1 = new RepositoryItemMemoEdit();
      this.checkBox1 = new CheckBox();
      this.refrainMemo = new MemoEdit();
      this.label3 = new Label();
      this.bookLabel = new Label();
      this.textBox3 = new TextBox();
      this.saveAsButton = new Button();
      this.deleteSongButton = new Button();
      ((ISupportInitialize) this.songBindingSource).BeginInit();
      this.verseGridContainer.ButtonPanel.BeginInit();
      this.verseGridContainer.SuspendLayout();
      this.verseGridControl.BeginInit();
      ((ISupportInitialize) this.verseBindingSource).BeginInit();
      this.verseGridView.BeginInit();
      this.repositoryItemMemoEdit1.BeginInit();
      this.refrainMemo.Properties.BeginInit();
      this.SuspendLayout();
      this._okButton.Location = new Point(542, 461);
      this._okButton.Click += new EventHandler(this._okButton_Click);
      this._cancelButton.Location = new Point(623, 461);
      this.numberTextBox.DataBindings.Add(new Binding("Text", (object) this.songBindingSource, "Number", true));
      this.numberTextBox.Location = new Point(231, 5);
      this.numberTextBox.Name = "numberTextBox";
      this.numberTextBox.Size = new Size(56, 20);
      this.numberTextBox.TabIndex = 2;
      this.songBindingSource.DataSource = (object) typeof (Song);
      this.numberLabel.AutoSize = true;
      this.numberLabel.Location = new Point(181, 8);
      this.numberLabel.Name = "numberLabel";
      this.numberLabel.Size = new Size(44, 13);
      this.numberLabel.TabIndex = 3;
      this.numberLabel.Text = "Number";
      this.label2.AutoSize = true;
      this.label2.Location = new Point(293, 8);
      this.label2.Name = "label2";
      this.label2.Size = new Size(35, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Name";
      this.textBox2.DataBindings.Add(new Binding("Text", (object) this.songBindingSource, "Name", true));
      this.textBox2.Location = new Point(334, 5);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new Size(262, 20);
      this.textBox2.TabIndex = 4;
      this.verseGridContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.verseGridContainer.AutoDelete = false;
      this.verseGridContainer.ButtonPanel.Appearance.BackColor = Color.Transparent;
      this.verseGridContainer.ButtonPanel.Appearance.Options.UseBackColor = true;
      this.verseGridContainer.ButtonPanel.BorderStyle = BorderStyles.NoBorder;
      this.verseGridContainer.ButtonPanel.Dock = DockStyle.Right;
      this.verseGridContainer.ButtonPanel.Location = new Point(408, 0);
      this.verseGridContainer.ButtonPanel.Name = "ButtonPanel";
      this.verseGridContainer.ButtonPanel.Size = new Size(77, 452);
      this.verseGridContainer.ButtonPanel.TabIndex = 2;
      this.verseGridContainer.Controls.Add((Control) this.verseGridControl);
      this.verseGridContainer.Grid = this.verseGridControl;
      this.verseGridContainer.Location = new Point(12, 38);
      this.verseGridContainer.Name = "verseGridContainer";
      this.verseGridContainer.Size = new Size(485, 452);
      this.verseGridContainer.TabIndex = 6;
      this.verseGridControl.DataSource = (object) this.verseBindingSource;
      this.verseGridControl.Dock = DockStyle.Fill;
      this.verseGridControl.Location = new Point(0, 0);
      this.verseGridControl.MainView = (BaseView) this.verseGridView;
      this.verseGridControl.Name = "verseGridControl";
      this.verseGridControl.RepositoryItems.AddRange(new RepositoryItem[1]
      {
        (RepositoryItem) this.repositoryItemMemoEdit1
      });
      this.verseGridControl.Size = new Size(408, 452);
      this.verseGridControl.TabIndex = 4;
      this.verseGridControl.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.verseGridView
      });
      this.verseBindingSource.DataMember = "Verses";
      this.verseBindingSource.DataSource = (object) this.songBindingSource;
      this.verseGridView.Columns.AddRange(new GridColumn[2]
      {
        this.colVerseNumber,
        this.colText
      });
      this.verseGridView.GridControl = this.verseGridControl;
      this.verseGridView.Name = "verseGridView";
      this.verseGridView.RowHeight = 80;
      this.colVerseNumber.AppearanceCell.Options.UseTextOptions = true;
      this.colVerseNumber.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
      this.colVerseNumber.AppearanceCell.TextOptions.VAlignment = VertAlignment.Top;
      this.colVerseNumber.Caption = "Number";
      this.colVerseNumber.FieldName = "VerseNumber";
      this.colVerseNumber.Name = "colVerseNumber";
      this.colVerseNumber.Visible = true;
      this.colVerseNumber.VisibleIndex = 0;
      this.colVerseNumber.Width = 56;
      this.colText.ColumnEdit = (RepositoryItem) this.repositoryItemMemoEdit1;
      this.colText.FieldName = "Text";
      this.colText.Name = "colText";
      this.colText.Visible = true;
      this.colText.VisibleIndex = 1;
      this.colText.Width = 333;
      this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
      this.checkBox1.AutoSize = true;
      this.checkBox1.DataBindings.Add(new Binding("Checked", (object) this.songBindingSource, "IsRefrainFirst", true));
      this.checkBox1.Location = new Point(506, 38);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new Size(82, 17);
      this.checkBox1.TabIndex = 7;
      this.checkBox1.Text = "Refrain First";
      this.checkBox1.UseVisualStyleBackColor = true;
      this.refrainMemo.DataBindings.Add(new Binding("Text", (object) this.songBindingSource, "Refrain", true));
      this.refrainMemo.Location = new Point(503, 76);
      this.refrainMemo.Name = "refrainMemo";
      this.refrainMemo.Size = new Size(194, 199);
      this.refrainMemo.TabIndex = 8;
      this.label3.AutoSize = true;
      this.label3.Location = new Point(503, 60);
      this.label3.Name = "label3";
      this.label3.Size = new Size(41, 13);
      this.label3.TabIndex = 9;
      this.label3.Text = "Refrain";
      this.bookLabel.AutoSize = true;
      this.bookLabel.Location = new Point(12, 8);
      this.bookLabel.Name = "bookLabel";
      this.bookLabel.Size = new Size(32, 13);
      this.bookLabel.TabIndex = 11;
      this.bookLabel.Text = "Book";
      this.textBox3.DataBindings.Add(new Binding("Text", (object) this.songBindingSource, "Book.Title", true));
      this.textBox3.Location = new Point(44, 6);
      this.textBox3.Name = "textBox3";
      this.textBox3.ReadOnly = true;
      this.textBox3.Size = new Size(131, 20);
      this.textBox3.TabIndex = 10;
      this.saveAsButton.Location = new Point(623, 3);
      this.saveAsButton.Name = "saveAsButton";
      this.saveAsButton.Size = new Size(75, 23);
      this.saveAsButton.TabIndex = 12;
      this.saveAsButton.Text = "Save As";
      this.saveAsButton.UseVisualStyleBackColor = true;
      this.saveAsButton.Visible = false;
      this.saveAsButton.Click += new EventHandler(this.saveAsButton_Click);
      this.deleteSongButton.Location = new Point(426, 461);
      this.deleteSongButton.Name = "deleteSongButton";
      this.deleteSongButton.Size = new Size(75, 23);
      this.deleteSongButton.TabIndex = 14;
      this.deleteSongButton.Text = "Delete Song";
      this.deleteSongButton.UseVisualStyleBackColor = true;
      this.deleteSongButton.Click += new EventHandler(this.deleteSongButton_Click);
      this.ClientSize = new Size(710, 496);
      this.Controls.Add((Control) this.deleteSongButton);
      this.Controls.Add((Control) this.saveAsButton);
      this.Controls.Add((Control) this.bookLabel);
      this.Controls.Add((Control) this.textBox3);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.refrainMemo);
      this.Controls.Add((Control) this.checkBox1);
      this.Controls.Add((Control) this.verseGridContainer);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.textBox2);
      this.Controls.Add((Control) this.numberLabel);
      this.Controls.Add((Control) this.numberTextBox);
      this.Name = "SongEditDialog";
      this.Text = "Song Edit";
      this.Controls.SetChildIndex((Control) this.numberTextBox, 0);
      this.Controls.SetChildIndex((Control) this.numberLabel, 0);
      this.Controls.SetChildIndex((Control) this._okButton, 0);
      this.Controls.SetChildIndex((Control) this._cancelButton, 0);
      this.Controls.SetChildIndex((Control) this.textBox2, 0);
      this.Controls.SetChildIndex((Control) this.label2, 0);
      this.Controls.SetChildIndex((Control) this.verseGridContainer, 0);
      this.Controls.SetChildIndex((Control) this.checkBox1, 0);
      this.Controls.SetChildIndex((Control) this.refrainMemo, 0);
      this.Controls.SetChildIndex((Control) this.label3, 0);
      this.Controls.SetChildIndex((Control) this.textBox3, 0);
      this.Controls.SetChildIndex((Control) this.bookLabel, 0);
      this.Controls.SetChildIndex((Control) this.saveAsButton, 0);
      this.Controls.SetChildIndex((Control) this.deleteSongButton, 0);
      ((ISupportInitialize) this.songBindingSource).EndInit();
      this.verseGridContainer.ButtonPanel.EndInit();
      this.verseGridContainer.ResumeLayout(false);
      this.verseGridControl.EndInit();
      ((ISupportInitialize) this.verseBindingSource).EndInit();
      this.verseGridView.EndInit();
      this.repositoryItemMemoEdit1.EndInit();
      this.refrainMemo.Properties.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
