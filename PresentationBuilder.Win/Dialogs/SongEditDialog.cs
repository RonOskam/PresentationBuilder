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
    private IContainer components = (IContainer)null;
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
        return _songID;
      }
    }

    public SongEditDialog()
    {
      InitializeComponent();
      verseGridContainer.addButton.Click += new EventHandler(addButton_Click);
      verseGridContainer.deleteButton.Click += new EventHandler(deleteButton_Click);
      verseGridContainer.addButton.ToolTip = "Add a Verse";
      verseGridContainer.deleteButton.ToolTip = "Delete the selected Verse.";
      verseGridView.SortInfo.Add(colVerseNumber, ColumnSortOrder.Ascending);
    }

    private void deleteButton_Click(object sender, EventArgs e)
    {
      if (verseBindingSource.Current == null || MessageBox.Show("Are you sure you want to delete the selected item?", Application.ProductName, MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      Verse entity = (Verse)verseBindingSource.Current;
      if (_songID.HasValue)
        _context.Verses.Remove(entity);
      else
        _song.Verses.Remove(entity);
    }

    private void addButton_Click(object sender, EventArgs e)
    {
      var song = (Song)songBindingSource.Current;
      var entity = new Verse();
      if (song.Verses.Count == 0)
        entity.VerseNumber = 1;
      else
        entity.VerseNumber = Convert.ToByte(song.Verses.Max(v => v.VerseNumber) + 1);

      song.Verses.Add(entity);

      songBindingSource.ResetCurrentItem();     
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
      _song = song;
      songBindingSource.DataSource = _song;
      if (song.Book == null)
      {
        _songID = new int?();
        saveAsButton.Visible = true;
        deleteSongButton.Visible = false;
        bookLabel.Enabled = false;
        numberLabel.Enabled = false;
        numberTextBox.Enabled = false;
      }
      else
        _songID = new int?(song.SongID);
      Text = "Edit Song";
    }

    public void AddSong(int bookId)
    {
      _song = new Song();
      _song.Book = _context.Books.FirstOrDefault(b => b.BookID == bookId);
      _song.EnteredBy = WindowsIdentity.GetCurrent().Name;
      _context.Songs.Add(_song);
      songBindingSource.DataSource = _song;
      _isNew = true;
      Text = "Add Song";
    }

    private void _okButton_Click(object sender, EventArgs e)
    {
      string text = (string)null;
      if (!_songID.HasValue && MustSave)
        text = "Click on Save As to save the song first.";
      else if (_song.Verses.Count == 0)
        text = "You must enter at least one verse.";
      else if (_song.Verses.Count != (int)Enumerable.Max<Verse, byte>((IEnumerable<Verse>)_song.Verses, (Func<Verse, byte>)(v => v.VerseNumber)))
        text = "Please enter all the verses for the song.";
      else if (string.IsNullOrEmpty(_song.Name))
        text = "Name is required.";
      else if (_song.IsRefrainFirst && string.IsNullOrEmpty(_song.Refrain))
      {
        text = "You must enter a refrain if Refrain First is checked.";
      }
      else
      {
        int num;
        if (_isNew)
          num = (uint)Queryable.Count<Song>((IQueryable<Song>)PresentationBuilderEntities.Context.Songs, (Expression<Func<Song, bool>>)(s => (int)s.Number == (int)_song.Number && s.Book.BookID == _song.Book.BookID)) > 0U ? 1 : 0;
        else
          num = 0;
        if (num != 0)
          text = "There is already a song with that number in this book.";
        else if ((int)_song.Number == 0)
          text = "0 is not a valid song number.";
      }
      if (text != null)
      {
        int num = (int)MessageBox.Show(text, Application.ProductName);
        DialogResult = DialogResult.None;
      }
      else
      {
        DataSource.ResetBook(_song.Book);
        _context.SaveChanges();
        if (_saved)
          DialogResult = DialogResult.Retry;
      }
    }

    private void saveAsButton_Click(object sender, EventArgs e)
    {
      if (new SongSaveAsDialog(_song, !MustSave).ShowDialog() != DialogResult.OK)
        return;
      EditSong(_song.SongID);
      _saved = true;
      saveAsButton.Visible = false;
      bookLabel.Enabled = true;
      numberLabel.Enabled = true;
      numberTextBox.Enabled = true;
      deleteSongButton.Visible = true;
    }

    private void deleteSongButton_Click(object sender, EventArgs e)
    {
      if (!_songID.HasValue || MessageBox.Show("Are you sure you want to delete this song?", Application.ProductName, MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      DataSource.DeleteSong(_songID.Value);
      _songID = new int?();
      DialogResult = DialogResult.Abort;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && components != null)
        components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      components = new System.ComponentModel.Container();
      numberTextBox = new System.Windows.Forms.TextBox();
      songBindingSource = new System.Windows.Forms.BindingSource(components);
      numberLabel = new System.Windows.Forms.Label();
      label2 = new System.Windows.Forms.Label();
      textBox2 = new System.Windows.Forms.TextBox();
      verseGridContainer = new PEC.Windows.Common.Controls.GridContainer();
      verseGridControl = new DevExpress.XtraGrid.GridControl();
      verseBindingSource = new System.Windows.Forms.BindingSource(components);
      verseGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
      colVerseNumber = new DevExpress.XtraGrid.Columns.GridColumn();
      colText = new DevExpress.XtraGrid.Columns.GridColumn();
      repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
      checkBox1 = new System.Windows.Forms.CheckBox();
      refrainMemo = new DevExpress.XtraEditors.MemoEdit();
      label3 = new System.Windows.Forms.Label();
      bookLabel = new System.Windows.Forms.Label();
      textBox3 = new System.Windows.Forms.TextBox();
      saveAsButton = new System.Windows.Forms.Button();
      deleteSongButton = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(songBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(verseGridContainer.ButtonPanel)).BeginInit();
      verseGridContainer.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(verseGridControl)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(verseBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(verseGridView)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(repositoryItemMemoEdit1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(refrainMemo.Properties)).BeginInit();
      SuspendLayout();
      // 
      // _okButton
      // 
      _okButton.Location = new System.Drawing.Point(542, 461);
      _okButton.Click += new System.EventHandler(_okButton_Click);
      // 
      // _cancelButton
      // 
      _cancelButton.Location = new System.Drawing.Point(623, 461);
      // 
      // numberTextBox
      // 
      numberTextBox.DataBindings.Add(new System.Windows.Forms.Binding("Text", songBindingSource, "Number", true));
      numberTextBox.Location = new System.Drawing.Point(231, 5);
      numberTextBox.Name = "numberTextBox";
      numberTextBox.Size = new System.Drawing.Size(56, 20);
      numberTextBox.TabIndex = 2;
      // 
      // songBindingSource
      // 
      songBindingSource.DataSource = typeof(PresentationBuilder.BLL.Song);
      // 
      // numberLabel
      // 
      numberLabel.AutoSize = true;
      numberLabel.Location = new System.Drawing.Point(181, 8);
      numberLabel.Name = "numberLabel";
      numberLabel.Size = new System.Drawing.Size(44, 13);
      numberLabel.TabIndex = 3;
      numberLabel.Text = "Number";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point(293, 8);
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size(35, 13);
      label2.TabIndex = 5;
      label2.Text = "Name";
      // 
      // textBox2
      // 
      textBox2.DataBindings.Add(new System.Windows.Forms.Binding("Text", songBindingSource, "Name", true));
      textBox2.Location = new System.Drawing.Point(334, 5);
      textBox2.Name = "textBox2";
      textBox2.Size = new System.Drawing.Size(262, 20);
      textBox2.TabIndex = 4;
      // 
      // verseGridContainer
      // 
      verseGridContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
      verseGridContainer.AutoDelete = false;
      // 
      // verseGridContainer.ButtonPanel
      // 
      verseGridContainer.ButtonPanel.Appearance.BackColor = System.Drawing.Color.Transparent;
      verseGridContainer.ButtonPanel.Appearance.Options.UseBackColor = true;
      verseGridContainer.ButtonPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
      verseGridContainer.ButtonPanel.Dock = System.Windows.Forms.DockStyle.Right;
      verseGridContainer.ButtonPanel.Location = new System.Drawing.Point(408, 0);
      verseGridContainer.ButtonPanel.Name = "ButtonPanel";
      verseGridContainer.ButtonPanel.Size = new System.Drawing.Size(77, 452);
      verseGridContainer.ButtonPanel.TabIndex = 2;
      verseGridContainer.Controls.Add(verseGridControl);
      verseGridContainer.Grid = verseGridControl;
      verseGridContainer.Location = new System.Drawing.Point(12, 38);
      verseGridContainer.Name = "verseGridContainer";
      verseGridContainer.Size = new System.Drawing.Size(485, 452);
      verseGridContainer.TabIndex = 6;
      // 
      // verseGridControl
      // 
      verseGridControl.DataSource = verseBindingSource;
      verseGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
      verseGridControl.Location = new System.Drawing.Point(0, 0);
      verseGridControl.MainView = verseGridView;
      verseGridControl.Name = "verseGridControl";
      verseGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            repositoryItemMemoEdit1});
      verseGridControl.Size = new System.Drawing.Size(408, 452);
      verseGridControl.TabIndex = 4;
      verseGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            verseGridView});
      // 
      // verseBindingSource
      // 
      verseBindingSource.DataMember = "VerseList";
      verseBindingSource.DataSource = songBindingSource;
      // 
      // verseGridView
      // 
      verseGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            colVerseNumber,
            colText});
      verseGridView.GridControl = verseGridControl;
      verseGridView.Name = "verseGridView";
      verseGridView.RowHeight = 80;
      // 
      // colVerseNumber
      // 
      colVerseNumber.AppearanceCell.Options.UseTextOptions = true;
      colVerseNumber.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
      colVerseNumber.AppearanceCell.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Top;
      colVerseNumber.Caption = "Number";
      colVerseNumber.FieldName = "VerseNumber";
      colVerseNumber.Name = "colVerseNumber";
      colVerseNumber.Visible = true;
      colVerseNumber.VisibleIndex = 0;
      colVerseNumber.Width = 56;
      // 
      // colText
      // 
      colText.ColumnEdit = repositoryItemMemoEdit1;
      colText.FieldName = "Text";
      colText.Name = "colText";
      colText.Visible = true;
      colText.VisibleIndex = 1;
      colText.Width = 333;
      // 
      // repositoryItemMemoEdit1
      // 
      repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
      // 
      // checkBox1
      // 
      checkBox1.AutoSize = true;
      checkBox1.DataBindings.Add(new System.Windows.Forms.Binding("Checked", songBindingSource, "IsRefrainFirst", true));
      checkBox1.Location = new System.Drawing.Point(506, 38);
      checkBox1.Name = "checkBox1";
      checkBox1.Size = new System.Drawing.Size(82, 17);
      checkBox1.TabIndex = 7;
      checkBox1.Text = "Refrain First";
      checkBox1.UseVisualStyleBackColor = true;
      // 
      // refrainMemo
      // 
      refrainMemo.DataBindings.Add(new System.Windows.Forms.Binding("Text", songBindingSource, "Refrain", true));
      refrainMemo.Location = new System.Drawing.Point(503, 76);
      refrainMemo.Name = "refrainMemo";
      refrainMemo.Size = new System.Drawing.Size(194, 199);
      refrainMemo.TabIndex = 8;
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point(503, 60);
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size(41, 13);
      label3.TabIndex = 9;
      label3.Text = "Refrain";
      // 
      // bookLabel
      // 
      bookLabel.AutoSize = true;
      bookLabel.Location = new System.Drawing.Point(12, 8);
      bookLabel.Name = "bookLabel";
      bookLabel.Size = new System.Drawing.Size(32, 13);
      bookLabel.TabIndex = 11;
      bookLabel.Text = "Book";
      // 
      // textBox3
      // 
      textBox3.DataBindings.Add(new System.Windows.Forms.Binding("Text", songBindingSource, "Book.Title", true));
      textBox3.Location = new System.Drawing.Point(44, 6);
      textBox3.Name = "textBox3";
      textBox3.ReadOnly = true;
      textBox3.Size = new System.Drawing.Size(131, 20);
      textBox3.TabIndex = 10;
      // 
      // saveAsButton
      // 
      saveAsButton.Location = new System.Drawing.Point(623, 3);
      saveAsButton.Name = "saveAsButton";
      saveAsButton.Size = new System.Drawing.Size(75, 23);
      saveAsButton.TabIndex = 12;
      saveAsButton.Text = "Save As";
      saveAsButton.UseVisualStyleBackColor = true;
      saveAsButton.Visible = false;
      saveAsButton.Click += new System.EventHandler(saveAsButton_Click);
      // 
      // deleteSongButton
      // 
      deleteSongButton.Location = new System.Drawing.Point(426, 461);
      deleteSongButton.Name = "deleteSongButton";
      deleteSongButton.Size = new System.Drawing.Size(75, 23);
      deleteSongButton.TabIndex = 14;
      deleteSongButton.Text = "Delete Song";
      deleteSongButton.UseVisualStyleBackColor = true;
      deleteSongButton.Click += new System.EventHandler(deleteSongButton_Click);
      // 
      // SongEditDialog
      // 
      ClientSize = new System.Drawing.Size(710, 496);
      Controls.Add(deleteSongButton);
      Controls.Add(saveAsButton);
      Controls.Add(bookLabel);
      Controls.Add(textBox3);
      Controls.Add(label3);
      Controls.Add(refrainMemo);
      Controls.Add(checkBox1);
      Controls.Add(verseGridContainer);
      Controls.Add(label2);
      Controls.Add(textBox2);
      Controls.Add(numberLabel);
      Controls.Add(numberTextBox);
      Name = "SongEditDialog";
      Text = "Song Edit";
      Controls.SetChildIndex(numberTextBox, 0);
      Controls.SetChildIndex(numberLabel, 0);
      Controls.SetChildIndex(_okButton, 0);
      Controls.SetChildIndex(_cancelButton, 0);
      Controls.SetChildIndex(textBox2, 0);
      Controls.SetChildIndex(label2, 0);
      Controls.SetChildIndex(verseGridContainer, 0);
      Controls.SetChildIndex(checkBox1, 0);
      Controls.SetChildIndex(refrainMemo, 0);
      Controls.SetChildIndex(label3, 0);
      Controls.SetChildIndex(textBox3, 0);
      Controls.SetChildIndex(bookLabel, 0);
      Controls.SetChildIndex(saveAsButton, 0);
      Controls.SetChildIndex(deleteSongButton, 0);
      ((System.ComponentModel.ISupportInitialize)(songBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(verseGridContainer.ButtonPanel)).EndInit();
      verseGridContainer.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(verseGridControl)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(verseBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(verseGridView)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(repositoryItemMemoEdit1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(refrainMemo.Properties)).EndInit();
      ResumeLayout(false);
      PerformLayout();

    }
  }
}
