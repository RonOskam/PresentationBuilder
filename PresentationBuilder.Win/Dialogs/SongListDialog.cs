// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.Win.Dialogs.SongListDialog
// Assembly: PresentationBuilder, Version=1.0.0.28120, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 295F5AD1-A97E-4830-A536-CA2F8525E5B1
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.exe

using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
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
  public class SongListDialog : OkCancelDialog
  {
    private IContainer components = (IContainer) null;
    private LookUpEdit bookLookup;
    private Label label1;
    private GridContainer songGridContainer;
    private Button editButton;
    private GridControl songGridControl;
    private GridView songGridView;
    private BindingSource songBindingSource;
    private GridColumn colNumber;
    private GridColumn colName;
    private GridColumn colVerseCount;
    private Button searchButton;

    public SongListDialog()
    {
      this.InitializeComponent();
      this.songGridContainer.addButton.Click += new EventHandler(this.addButton_Click);
    }

    private void SongEditDialog_Load(object sender, EventArgs e)
    {
      LookupEdit.Initialize(this.bookLookup, "Title", (string) null, (IEnumerable) DataSource.GetBooks());
    }

    private void bookLookup_EditValueChanged(object sender, EventArgs e)
    {
      this.songBindingSource.DataSource = (object) DataSource.GetSongItems((Book) this.bookLookup.EditValue);
      this.editButton.Enabled = true;
    }

    private void addButton_Click(object sender, EventArgs e)
    {
      if (this.bookLookup.EditValue != null)
      {
        SongEditDialog songEditDialog = new SongEditDialog();
        Book book = (Book) this.bookLookup.EditValue;
        songEditDialog.AddSong(book.BookID);
        if (songEditDialog.ShowDialog() != DialogResult.OK)
          return;
        DataSource.ResetBook((Book) this.bookLookup.EditValue);
        this.bookLookup_EditValueChanged((object) null, EventArgs.Empty);
      }
      else
      {
        int num = (int) MessageBox.Show("Select a book first.", Application.ProductName);
      }
    }

    private void editButton_Click(object sender, EventArgs e)
    {
      if (this.songBindingSource.Current == null)
        return;
      SongItem songItem = (SongItem) this.songBindingSource.Current;
      SongEditDialog songEditDialog = new SongEditDialog();
      songEditDialog.EditSong(songItem.SongID);
      if (songEditDialog.ShowDialog() != DialogResult.Cancel)
      {
        DataSource.ResetBook((Book) this.bookLookup.EditValue);
        this.bookLookup_EditValueChanged((object) null, EventArgs.Empty);
      }
    }

    private void searchButton_Click(object sender, EventArgs e)
    {
      SongSearchDialog songSearchDialog = new SongSearchDialog();
      if (songSearchDialog.ShowDialog() != DialogResult.OK)
        return;
      Song selectedSong = songSearchDialog.SelectedSong;
      songSearchDialog.Close();
      SongEditDialog songEditDialog = new SongEditDialog();
      songEditDialog.MustSave = true;
      songEditDialog.EditSong(selectedSong);
      DialogResult dialogResult = songEditDialog.ShowDialog();
      if (dialogResult == DialogResult.OK || dialogResult == DialogResult.Retry)
      {
        DataSource.ResetBook(selectedSong.Book);
        if (this.bookLookup.EditValue != null && ((Book) this.bookLookup.EditValue).BookID == selectedSong.Book.BookID)
          this.bookLookup_EditValueChanged((object) null, EventArgs.Empty);
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
      this.components = (IContainer) new Container();
      this.bookLookup = new LookUpEdit();
      this.label1 = new Label();
      this.songGridContainer = new GridContainer();
      this.editButton = new Button();
      this.songGridControl = new GridControl();
      this.songBindingSource = new BindingSource(this.components);
      this.songGridView = new GridView();
      this.colNumber = new GridColumn();
      this.colName = new GridColumn();
      this.colVerseCount = new GridColumn();
      this.searchButton = new Button();
      this.bookLookup.Properties.BeginInit();
      this.songGridContainer.ButtonPanel.BeginInit();
      this.songGridContainer.ButtonPanel.SuspendLayout();
      this.songGridContainer.SuspendLayout();
      this.songGridControl.BeginInit();
      ((ISupportInitialize) this.songBindingSource).BeginInit();
      this.songGridView.BeginInit();
      this.SuspendLayout();
      this._okButton.Location = new Point(357, 394);
      this._okButton.Visible = false;
      this._cancelButton.Location = new Point(442, 400);
      this._cancelButton.Size = new Size(67, 23);
      this._cancelButton.Text = "Close";
      this.bookLookup.Location = new Point(82, 8);
      this.bookLookup.Name = "bookLookup";
      this.bookLookup.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      this.bookLookup.Properties.DropDownRows = 15;
      this.bookLookup.Size = new Size(119, 20);
      this.bookLookup.TabIndex = 6;
      this.bookLookup.EditValueChanged += new EventHandler(this.bookLookup_EditValueChanged);
      this.label1.AutoSize = true;
      this.label1.Location = new Point(2, 11);
      this.label1.Name = "label1";
      this.label1.Size = new Size(74, 13);
      this.label1.TabIndex = 7;
      this.label1.Text = "Select a Book";
      this.songGridContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.songGridContainer.ButtonPanel.Appearance.BackColor = Color.Transparent;
      this.songGridContainer.ButtonPanel.Appearance.Options.UseBackColor = true;
      this.songGridContainer.ButtonPanel.BorderStyle = BorderStyles.NoBorder;
      this.songGridContainer.ButtonPanel.Controls.Add((Control) this.editButton);
      this.songGridContainer.ButtonPanel.Dock = DockStyle.Right;
      this.songGridContainer.ButtonPanel.Location = new Point(431, 0);
      this.songGridContainer.ButtonPanel.Name = "ButtonPanel";
      this.songGridContainer.ButtonPanel.Size = new Size(77, 392);
      this.songGridContainer.ButtonPanel.TabIndex = 2;
      this.songGridContainer.Controls.Add((Control) this.songGridControl);
      this.songGridContainer.Grid = this.songGridControl;
      this.songGridContainer.GridType = GridType.SelectionGrid;
      this.songGridContainer.Location = new Point(5, 34);
      this.songGridContainer.Name = "songGridContainer";
      this.songGridContainer.SidePanelButtons = SidePanelButtonOptions.AddButton;
      this.songGridContainer.Size = new Size(508, 392);
      this.songGridContainer.TabIndex = 8;
      this.editButton.Enabled = false;
      this.editButton.Location = new Point(6, 36);
      this.editButton.Name = "editButton";
      this.editButton.Size = new Size(67, 23);
      this.editButton.TabIndex = 4;
      this.editButton.Text = "Edit";
      this.editButton.UseVisualStyleBackColor = true;
      this.editButton.Click += new EventHandler(this.editButton_Click);
      this.songGridControl.DataSource = (object) this.songBindingSource;
      this.songGridControl.Dock = DockStyle.Fill;
      this.songGridControl.Location = new Point(0, 0);
      this.songGridControl.MainView = (BaseView) this.songGridView;
      this.songGridControl.Name = "songGridControl";
      this.songGridControl.Size = new Size(431, 392);
      this.songGridControl.TabIndex = 3;
      this.songGridControl.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.songGridView
      });
      this.songBindingSource.DataSource = (object) typeof (SongItem);
      this.songGridView.Columns.AddRange(new GridColumn[3]
      {
        this.colNumber,
        this.colName,
        this.colVerseCount
      });
      this.songGridView.GridControl = this.songGridControl;
      this.songGridView.Name = "songGridView";
      this.colNumber.AppearanceCell.Options.UseTextOptions = true;
      this.colNumber.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
      this.colNumber.FieldName = "Number";
      this.colNumber.Name = "colNumber";
      this.colNumber.Visible = true;
      this.colNumber.VisibleIndex = 0;
      this.colNumber.Width = 64;
      this.colName.FieldName = "Name";
      this.colName.Name = "colName";
      this.colName.Visible = true;
      this.colName.VisibleIndex = 1;
      this.colName.Width = 265;
      this.colVerseCount.AppearanceCell.Options.UseTextOptions = true;
      this.colVerseCount.AppearanceCell.TextOptions.HAlignment = HorzAlignment.Near;
      this.colVerseCount.FieldName = "VerseCount";
      this.colVerseCount.Name = "colVerseCount";
      this.colVerseCount.Visible = true;
      this.colVerseCount.VisibleIndex = 2;
      this.colVerseCount.Width = 83;
      this.searchButton.Location = new Point(234, 6);
      this.searchButton.Name = "searchButton";
      this.searchButton.Size = new Size(100, 23);
      this.searchButton.TabIndex = 5;
      this.searchButton.Text = "Search Internet";
      this.searchButton.UseVisualStyleBackColor = true;
      this.searchButton.Click += new EventHandler(this.searchButton_Click);
      this.ClientSize = new Size(525, 429);
      this.Controls.Add((Control) this.songGridContainer);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.bookLookup);
      this.Controls.Add((Control) this.searchButton);
      this.Name = "SongListDialog";
      this.Text = "Songs";
      this.Load += new EventHandler(this.SongEditDialog_Load);
      this.Controls.SetChildIndex((Control) this.searchButton, 0);
      this.Controls.SetChildIndex((Control) this._okButton, 0);
      this.Controls.SetChildIndex((Control) this.bookLookup, 0);
      this.Controls.SetChildIndex((Control) this.label1, 0);
      this.Controls.SetChildIndex((Control) this.songGridContainer, 0);
      this.Controls.SetChildIndex((Control) this._cancelButton, 0);
      this.bookLookup.Properties.EndInit();
      this.songGridContainer.ButtonPanel.EndInit();
      this.songGridContainer.ButtonPanel.ResumeLayout(false);
      this.songGridContainer.ButtonPanel.PerformLayout();
      this.songGridContainer.ResumeLayout(false);
      this.songGridControl.EndInit();
      ((ISupportInitialize) this.songBindingSource).EndInit();
      this.songGridView.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
