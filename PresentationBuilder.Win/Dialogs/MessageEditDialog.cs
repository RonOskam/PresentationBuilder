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
using System.Linq;

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

      this.messageGridView.SortInfo.Add(colCode, ColumnSortOrder.Ascending);
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
      this.components = new System.ComponentModel.Container();
      this.messageGridContainer = new PEC.Windows.Common.Controls.GridContainer();
      this.messageGridControl = new DevExpress.XtraGrid.GridControl();
      this.messageBindingSource = new System.Windows.Forms.BindingSource(this.components);
      this.messageGridView = new DevExpress.XtraGrid.Views.Grid.GridView();
      this.colCode = new DevExpress.XtraGrid.Columns.GridColumn();
      this.colText = new DevExpress.XtraGrid.Columns.GridColumn();
      this.repositoryItemMemoEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
      this.label1 = new System.Windows.Forms.Label();
      this.typeLookup = new DevExpress.XtraEditors.LookUpEdit();
      ((System.ComponentModel.ISupportInitialize)(this.messageGridContainer.ButtonPanel)).BeginInit();
      this.messageGridContainer.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.messageGridControl)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.messageBindingSource)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.messageGridView)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.typeLookup.Properties)).BeginInit();
      this.SuspendLayout();
      // 
      // _okButton
      // 
      this._okButton.Location = new System.Drawing.Point(433, 406);
      this._okButton.Click += new System.EventHandler(this._okButton_Click);
      // 
      // _cancelButton
      // 
      this._cancelButton.Location = new System.Drawing.Point(514, 406);
      // 
      // messageGridContainer
      // 
      this.messageGridContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.messageGridContainer.AutoDelete = false;
      // 
      // messageGridContainer.ButtonPanel
      // 
      this.messageGridContainer.ButtonPanel.Appearance.BackColor = System.Drawing.Color.Transparent;
      this.messageGridContainer.ButtonPanel.Appearance.Options.UseBackColor = true;
      this.messageGridContainer.ButtonPanel.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
      this.messageGridContainer.ButtonPanel.Dock = System.Windows.Forms.DockStyle.Right;
      this.messageGridContainer.ButtonPanel.Location = new System.Drawing.Point(515, 0);
      this.messageGridContainer.ButtonPanel.Name = "ButtonPanel";
      this.messageGridContainer.ButtonPanel.Size = new System.Drawing.Size(77, 368);
      this.messageGridContainer.ButtonPanel.TabIndex = 2;
      this.messageGridContainer.Controls.Add(this.messageGridControl);
      this.messageGridContainer.Grid = this.messageGridControl;
      this.messageGridContainer.Location = new System.Drawing.Point(2, 32);
      this.messageGridContainer.Name = "messageGridContainer";
      this.messageGridContainer.Size = new System.Drawing.Size(592, 368);
      this.messageGridContainer.TabIndex = 11;
      // 
      // messageGridControl
      // 
      this.messageGridControl.DataSource = this.messageBindingSource;
      this.messageGridControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.messageGridControl.Location = new System.Drawing.Point(0, 0);
      this.messageGridControl.MainView = this.messageGridView;
      this.messageGridControl.Name = "messageGridControl";
      this.messageGridControl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemMemoEdit1});
      this.messageGridControl.Size = new System.Drawing.Size(515, 368);
      this.messageGridControl.TabIndex = 3;
      this.messageGridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.messageGridView});
      // 
      // messageBindingSource
      // 
      this.messageBindingSource.DataSource = typeof(PresentationBuilder.BLL.Message);
      // 
      // messageGridView
      // 
      this.messageGridView.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCode,
            this.colText});
      this.messageGridView.GridControl = this.messageGridControl;
      this.messageGridView.Name = "messageGridView";
      this.messageGridView.OptionsView.RowAutoHeight = true;
      // 
      // colCode
      // 
      this.colCode.FieldName = "Code";
      this.colCode.Name = "colCode";
      this.colCode.Visible = true;
      this.colCode.VisibleIndex = 0;
      this.colCode.Width = 60;
      // 
      // colText
      // 
      this.colText.ColumnEdit = this.repositoryItemMemoEdit1;
      this.colText.FieldName = "Text";
      this.colText.Name = "colText";
      this.colText.Visible = true;
      this.colText.VisibleIndex = 1;
      this.colText.Width = 436;
      // 
      // repositoryItemMemoEdit1
      // 
      this.repositoryItemMemoEdit1.Name = "repositoryItemMemoEdit1";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(12, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(77, 13);
      this.label1.TabIndex = 10;
      this.label1.Text = "Message Type";
      // 
      // typeLookup
      // 
      this.typeLookup.Location = new System.Drawing.Point(95, 6);
      this.typeLookup.Name = "typeLookup";
      this.typeLookup.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.typeLookup.Size = new System.Drawing.Size(119, 20);
      this.typeLookup.TabIndex = 9;
      this.typeLookup.EditValueChanged += new System.EventHandler(this.typeLookup_EditValueChanged);
      // 
      // MessageEditDialog
      // 
      this.ClientSize = new System.Drawing.Size(601, 441);
      this.Controls.Add(this.messageGridContainer);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.typeLookup);
      this.Name = "MessageEditDialog";
      this.Text = "Edit Messages";
      this.Load += new System.EventHandler(this.MessageEditDialog_Load);
      this.Controls.SetChildIndex(this._okButton, 0);
      this.Controls.SetChildIndex(this._cancelButton, 0);
      this.Controls.SetChildIndex(this.typeLookup, 0);
      this.Controls.SetChildIndex(this.label1, 0);
      this.Controls.SetChildIndex(this.messageGridContainer, 0);
      ((System.ComponentModel.ISupportInitialize)(this.messageGridContainer.ButtonPanel)).EndInit();
      this.messageGridContainer.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.messageGridControl)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.messageBindingSource)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.messageGridView)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.repositoryItemMemoEdit1)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.typeLookup.Properties)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    private void MessageEditDialog_Load(object sender, EventArgs e)
    {
      var msgTypes = _context.MessageTypes.Include("Messages").ToList();
      LookupEdit.Initialize(typeLookup, "Description", null, msgTypes);
    }
  }
}
