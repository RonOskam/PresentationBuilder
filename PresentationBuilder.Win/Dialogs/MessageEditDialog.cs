// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.Win.Dialogs.MessageEditDialog
// Assembly: PresentationBuilder, Version=1.0.0.28120, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 295F5AD1-A97E-4830-A536-CA2F8525E5B1
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.exe

using DevExpress.Data;
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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PresentationBuilder.Win.Dialogs
{
  public class MessageEditDialog : OkCancelDialog
  {
    private PresentationBuilderEntities _context = new PresentationBuilderEntities();
    private IContainer components = (IContainer) null;
    private GridContainer messageGridContainer;
    private GridControl messageGridControl;
    private BindingSource messageBindingSource;
    private GridView messageGridView;
    private GridColumn colCode;
    private GridColumn colText;
    private Label label1;
    private LookUpEdit typeLookup;
    private RepositoryItemMemoEdit repositoryItemMemoEdit1;

    public MessageEditDialog()
    {
      this.InitializeComponent();
      LookupEdit.Initialize(this.typeLookup, "Description", (string) null, (IEnumerable) this._context.MessageTypes.Include("Messages"));
      this.messageGridView.SortInfo.Add(this.colCode, ColumnSortOrder.Ascending);
      this.messageGridContainer.addButton.Click += new EventHandler(this.addButton_Click);
      this.messageGridContainer.deleteButton.Click += new EventHandler(this.deleteButton_Click);
    }

    private void deleteButton_Click(object sender, EventArgs e)
    {
      if (this.messageBindingSource.Current == null || MessageBox.Show("Are you sure you want to delete the selected item?", Application.ProductName, MessageBoxButtons.YesNo) != DialogResult.Yes)
        return;
      this._context.Messages.Remove((BLL.Message)messageBindingSource.Current);
    }

    private void addButton_Click(object sender, EventArgs e)
    {
      if (this.typeLookup.EditValue == null)
        return;
      ((MessageType) this.typeLookup.EditValue).Messages.Add(new PresentationBuilder.BLL.Message());
    }

    private void typeLookup_EditValueChanged(object sender, EventArgs e)
    {
      this.messageBindingSource.DataSource = (object) ((MessageType) this.typeLookup.EditValue).Messages;
    }

    private void _okButton_Click(object sender, EventArgs e)
    {
      this._context.SaveChanges();
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
      this.messageGridContainer = new GridContainer();
      this.messageGridControl = new GridControl();
      this.messageBindingSource = new BindingSource(this.components);
      this.messageGridView = new GridView();
      this.colCode = new GridColumn();
      this.colText = new GridColumn();
      this.repositoryItemMemoEdit1 = new RepositoryItemMemoEdit();
      this.label1 = new Label();
      this.typeLookup = new LookUpEdit();
      this.messageGridContainer.ButtonPanel.BeginInit();
      this.messageGridContainer.SuspendLayout();
      this.messageGridControl.BeginInit();
      ((ISupportInitialize) this.messageBindingSource).BeginInit();
      this.messageGridView.BeginInit();
      this.repositoryItemMemoEdit1.BeginInit();
      this.typeLookup.Properties.BeginInit();
      this.SuspendLayout();
      this._okButton.Location = new Point(433, 406);
      this._okButton.Click += new EventHandler(this._okButton_Click);
      this._cancelButton.Location = new Point(514, 406);
      this.messageGridContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.messageGridContainer.AutoDelete = false;
      this.messageGridContainer.ButtonPanel.Appearance.BackColor = Color.Transparent;
      this.messageGridContainer.ButtonPanel.Appearance.Options.UseBackColor = true;
      this.messageGridContainer.ButtonPanel.BorderStyle = BorderStyles.NoBorder;
      this.messageGridContainer.ButtonPanel.Dock = DockStyle.Right;
      this.messageGridContainer.ButtonPanel.Location = new Point(515, 0);
      this.messageGridContainer.ButtonPanel.Name = "ButtonPanel";
      this.messageGridContainer.ButtonPanel.Size = new Size(77, 368);
      this.messageGridContainer.ButtonPanel.TabIndex = 2;
      this.messageGridContainer.Controls.Add((Control) this.messageGridControl);
      this.messageGridContainer.Grid = this.messageGridControl;
      this.messageGridContainer.Location = new Point(2, 32);
      this.messageGridContainer.Name = "messageGridContainer";
      this.messageGridContainer.Size = new Size(592, 368);
      this.messageGridContainer.TabIndex = 11;
      this.messageGridControl.DataSource = (object) this.messageBindingSource;
      this.messageGridControl.Dock = DockStyle.Fill;
      this.messageGridControl.Location = new Point(0, 0);
      this.messageGridControl.MainView = (BaseView) this.messageGridView;
      this.messageGridControl.Name = "messageGridControl";
      this.messageGridControl.RepositoryItems.AddRange(new RepositoryItem[1]
      {
        (RepositoryItem) this.repositoryItemMemoEdit1
      });
      this.messageGridControl.Size = new Size(515, 368);
      this.messageGridControl.TabIndex = 3;
      this.messageGridControl.ViewCollection.AddRange(new BaseView[1]
      {
        (BaseView) this.messageGridView
      });
      this.messageBindingSource.DataSource = (object) typeof (PresentationBuilder.BLL.Message);
      this.messageGridView.Columns.AddRange(new GridColumn[2]
      {
        this.colCode,
        this.colText
      });
      this.messageGridView.GridControl = this.messageGridControl;
      this.messageGridView.Name = "messageGridView";
      this.messageGridView.OptionsView.RowAutoHeight = true;
      this.colCode.FieldName = "Code";
      this.colCode.Name = "colCode";
      this.colCode.Visible = true;
      this.colCode.VisibleIndex = 0;
      this.colCode.Width = 60;
      this.colText.ColumnEdit = (RepositoryItem) this.repositoryItemMemoEdit1;
      this.colText.FieldName = "Text";
      this.colText.Name = "colText";
      this.colText.Visible = true;
      this.colText.VisibleIndex = 1;
      this.colText.Width = 436;
      this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new Size(77, 13);
      this.label1.TabIndex = 10;
      this.label1.Text = "Message Type";
      this.typeLookup.Location = new Point(95, 6);
      this.typeLookup.Name = "typeLookup";
      this.typeLookup.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      this.typeLookup.Size = new Size(119, 20);
      this.typeLookup.TabIndex = 9;
      this.typeLookup.EditValueChanged += new EventHandler(this.typeLookup_EditValueChanged);
      this.ClientSize = new Size(601, 441);
      this.Controls.Add((Control) this.messageGridContainer);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.typeLookup);
      this.Name = "MessageEditDialog";
      this.Text = "Edit Messages";
      this.Controls.SetChildIndex((Control) this._okButton, 0);
      this.Controls.SetChildIndex((Control) this._cancelButton, 0);
      this.Controls.SetChildIndex((Control) this.typeLookup, 0);
      this.Controls.SetChildIndex((Control) this.label1, 0);
      this.Controls.SetChildIndex((Control) this.messageGridContainer, 0);
      this.messageGridContainer.ButtonPanel.EndInit();
      this.messageGridContainer.ResumeLayout(false);
      this.messageGridControl.EndInit();
      ((ISupportInitialize) this.messageBindingSource).EndInit();
      this.messageGridView.EndInit();
      this.repositoryItemMemoEdit1.EndInit();
      this.typeLookup.Properties.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
