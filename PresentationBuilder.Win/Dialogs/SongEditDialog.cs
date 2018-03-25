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
      var song = (Song)songBindingSource.Current;
      var entity = new Verse();
      if (song.Verses.Count == 0)      
        entity.VerseNumber = 1;      
      else
      {
        entity.VerseNumber = song.Verses.Max(v => v.VerseNumber);
        ++entity.VerseNumber;
      }
      song.Verses.Add(entity);
    }

    public void EditSong(int songId)
    {
      _song = DataSource.GetSong(_context, songId);
      songBindingSource.DataSource = _song;
      _songID = new int?(songId);
      Text = "Edit Song";
    }

    public void EditSong(Song song)
    {
      this._song = song;
      this.songBindingSource.DataSource = this._song;
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
      this.components = new System.ComponentModel.Container();
      this.numberTextBox = new System.Windows.Forms.TextBox();
      this.songBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.numberLabel = new System.Windows.Forms.Label();
      this.label2 = new System.Windows.Forms.Label();
      this.textBox2 = new System.Windows.Forms.TextBox();
      this.verseGridContainer = new PEC.Windows.Common.Controls.GridContainer();
      this.verseGridControl = new DevExpress.XtraGrid.GridControl();
      this.verseBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.verseGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
      this.colVerseNumber = new DevExpress.XtraGrid.Columns.GridColumn();
      this.colText = new DevExpress.XtraGrid.Columns.GridColumn();
      this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.refrainMemo = new DevExpress.XtraEditors.MemoEdit();
      this.label3 = new System.Windows.Forms.Label();
      this.bookLabel = new System.Windows.Forms.Label();
      this.textBox3 = new System.Windows.Forms.TextBox();
      this.saveAsButton = new System.Windows.Forms.Button();
      this.deleteSongButton = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.songBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.verseGridContainer.ButtonPanel)).BeginInit();
      this.verseGridContainer.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.verseGridControl)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.verseBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.verseGridView)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.refrainMemo.Properties)).BeginInit();
      this.SuspendLayout();
      // 
      // _okButton
      // 
      this._okButton.Location = new System.Drawing.Point(542, 461);
      this._okButton.Click += new System.EventHandler(this._okButton_Click);
      // 
      // _cancelButton
      // 
      this._cancelButton.Location = new System.Drawing.Point(623, 461);
      // 
      // numberTextBox
      // 
      this.numberTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.songBindingSource, "Number", true));
      this.numberTextBox.Location = new System.Drawing.Point(231, 5);
      this.numberTextBox.Name = "numberTextBox";
      this.numberTextBox.Size = new System.Drawing.Size(56, 20);
      this.numberTextBox.TabIndex = 2;
      // 
      // songBindingSource
      // 
      this.songBindingSource.DataSource = typeof(PresentationBuilder.BLL.Song);
      // 
      // numberLabel
      // 
      this.numberLabel.AutoSize = true;
      this.numberLabel.Location = new System.Drawing.Point(181, 8);
      this.numberLabel.Name = "numberLabel";
      this.numberLabel.Size = new System.Drawing.Size(44, 13);
      this.numberLabel.TabIndex = 3;
      this.numberLabel.Text = "Number";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(293, 8);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(35, 13);
      this.label2.TabIndex = 5;
      this.label2.Text = "Name";
      // 
      // textBox2
      // 
      this.textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.songBindingSource, "Name", true));
      this.textBox2.Location = new System.Drawing.Point(334, 5);
      this.textBox2.Name = "textBox2";
      this.textBox2.Size = new System.Drawing.Size(262, 20);
      this.textBox2.TabIndex = 4;
      // 
      // verseGridContainer
      // 
      this.verseGridContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.verseGridContainer.AutoDelete = false;
      // 
      // verseGridContainer.ButtonPanel
      // 
      this.verseGridContainer.ButtonPanel.Appearance.BackColor = System.Drawing.Color.Transparent;
      this.verseGridContainer.ButtonPanel.Appearance.Options.UseBackColor = true;
      this.verseGridContainer.ButtonPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
      this.verseGridContainer.ButtonPanel.Dock = System.Windows.Forms.DockStyle.Right;
      this.verseGridContainer.ButtonPanel.Location = new System.Drawing.Point(408, 0);
      this.verseGridContainer.ButtonPanel.Name = "ButtonPanel";
      this.verseGridContainer.ButtonPanel.Size = new System.Drawing.Size(77, 452);
      this.verseGridContainer.ButtonPanel.TabIndex = 2;
      this.verseGridContainer.Controls.Add(this.verseGridControl);
      this.verseGridContainer.Grid = this.verseGridControl;
      this.verseGridContainer.Location = new System.Drawing.Point(12, 38);
      this.verseGridContainer.Name = "verseGridContainer";
      this.verseGridContainer.Size = new System.Drawing.Size(485, 452);
      this.verseGridContainer.TabIndex = 6;
      // 
      // verseGridControl
      // 
      this.verseGridControl.DataSource = this.verseBindingSource;
      this.verseGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.verseGridControl.Location = new System.Drawing.Point(0, 0);
      this.verseGridControl.MainView = this.verseGridView;
      this.verseGridControl.Name = "verseGridControl";
      this.verseGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1});
      this.verseGridControl.Size = new System.Drawing.Size(408, 452);
      this.verseGridControl.TabIndex = 4;
      this.verseGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.verseGridView});
      // 
      // verseBindingSource
      // 
      this.verseBindingSource.DataMember = "VerseList";
      this.verseBindingSource.DataSource = this.songBindingSource;
      // 
      // verseGridView
      // 
      this.verseGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colVerseNumber,
            this.colText});
      this.verseGridView.GridControl = this.verseGridControl;
      this.verseGridView.Name = "verseGridView";
      this.verseGridView.RowHeight = 80;
      // 
      // colVerseNumber
      // 
      this.colVerseNumber.AppearanceCell.Options.UseTextOptions = true;
      this.colVerseNumber.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
      this.colVerseNumber.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
      this.colVerseNumber.Caption = "Number";
      this.colVerseNumber.FieldName = "VerseNumber";
      this.colVerseNumber.Name = "colVerseNumber";
      this.colVerseNumber.Visible = true;
      this.colVerseNumber.VisibleIndex = 0;
      this.colVerseNumber.Width = 56;
      // 
      // colText
      // 
      this.colText.ColumnEdit = this.repositoryItemMemoEdit1;
      this.colText.FieldName = "Text";
      this.colText.Name = "colText";
      this.colText.Visible = true;
      this.colText.VisibleIndex = 1;
      this.colText.Width = 333;
      // 
      // repositoryItemMemoEdit1
      // 
      this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
      // 
      // checkBox1
      // 
      this.checkBox1.AutoSize = true;
      this.checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", this.songBindingSource, "IsRefrainFirst", true));
      this.checkBox1.Location = new System.Drawing.Point(506, 38);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Size = new System.Drawing.Size(82, 17);
      this.checkBox1.TabIndex = 7;
      this.checkBox1.Text = "Refrain First";
      this.checkBox1.UseVisualStyleBackColor = true;
      // 
      // refrainMemo
      // 
      this.refrainMemo.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.songBindingSource, "Refrain", true));
      this.refrainMemo.Location = new System.Drawing.Point(503, 76);
      this.refrainMemo.Name = "refrainMemo";
      this.refrainMemo.Size = new System.Drawing.Size(194, 199);
      this.refrainMemo.TabIndex = 8;
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(503, 60);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(41, 13);
      this.label3.TabIndex = 9;
      this.label3.Text = "Refrain";
      // 
      // bookLabel
      // 
      this.bookLabel.AutoSize = true;
      this.bookLabel.Location = new System.Drawing.Point(12, 8);
      this.bookLabel.Name = "bookLabel";
      this.bookLabel.Size = new System.Drawing.Size(32, 13);
      this.bookLabel.TabIndex = 11;
      this.bookLabel.Text = "Book";
      // 
      // textBox3
      // 
      this.textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", this.songBindingSource, "Book.Title", true));
      this.textBox3.Location = new System.Drawing.Point(44, 6);
      this.textBox3.Name = "textBox3";
      this.textBox3.ReadOnly = true;
      this.textBox3.Size = new System.Drawing.Size(131, 20);
      this.textBox3.TabIndex = 10;
      // 
      // saveAsButton
      // 
      this.saveAsButton.Location = new System.Drawing.Point(623, 3);
      this.saveAsButton.Name = "saveAsButton";
      this.saveAsButton.Size = new System.Drawing.Size(75, 23);
      this.saveAsButton.TabIndex = 12;
      this.saveAsButton.Text = "Save As";
      this.saveAsButton.UseVisualStyleBackColor = true;
      this.saveAsButton.Visible = false;
      this.saveAsButton.Click += new System.EventHandler(this.saveAsButton_Click);
      // 
      // deleteSongButton
      // 
      this.deleteSongButton.Location = new System.Drawing.Point(426, 461);
      this.deleteSongButton.Name = "deleteSongButton";
      this.deleteSongButton.Size = new System.Drawing.Size(75, 23);
      this.deleteSongButton.TabIndex = 14;
      this.deleteSongButton.Text = "Delete Song";
      this.deleteSongButton.UseVisualStyleBackColor = true;
      this.deleteSongButton.Click += new System.EventHandler(this.deleteSongButton_Click);
      // 
      // SongEditDialog
      // 
      this.ClientSize = new System.Drawing.Size(710, 496);
      this.Controls.Add(this.deleteSongButton);
      this.Controls.Add(this.saveAsButton);
      this.Controls.Add(this.bookLabel);
      this.Controls.Add(this.textBox3);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.refrainMemo);
      this.Controls.Add(this.checkBox1);
      this.Controls.Add(this.verseGridContainer);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.textBox2);
      this.Controls.Add(this.numberLabel);
      this.Controls.Add(this.numberTextBox);
      this.Name = "SongEditDialog";
      this.Text = "Song Edit";
      this.Controls.SetChildIndex(this.numberTextBox, 0);
      this.Controls.SetChildIndex(this.numberLabel, 0);
      this.Controls.SetChildIndex(this._okButton, 0);
      this.Controls.SetChildIndex(this._cancelButton, 0);
      this.Controls.SetChildIndex(this.textBox2, 0);
      this.Controls.SetChildIndex(this.label2, 0);
      this.Controls.SetChildIndex(this.verseGridContainer, 0);
      this.Controls.SetChildIndex(this.checkBox1, 0);
      this.Controls.SetChildIndex(this.refrainMemo, 0);
      this.Controls.SetChildIndex(this.label3, 0);
      this.Controls.SetChildIndex(this.textBox3, 0);
      this.Controls.SetChildIndex(this.bookLabel, 0);
      this.Controls.SetChildIndex(this.saveAsButton, 0);
      this.Controls.SetChildIndex(this.deleteSongButton, 0);
      ((System.ComponentModel.ISupportInitialize)(this.songBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.verseGridContainer.ButtonPanel)).EndInit();
      this.verseGridContainer.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.verseGridControl)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.verseBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.verseGridView)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.refrainMemo.Properties)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }
  }
}
