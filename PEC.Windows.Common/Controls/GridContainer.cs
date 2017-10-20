// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.Controls.GridContainer
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using PEC.Configuration;
using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PEC.Windows.Common.Controls
{
  [Designer(typeof (GridContainerDesigner))]
  public class GridContainer : XtraUserControl
  {
    private bool _showLineControl = false;
    private bool _autoDelete = true;
    private bool _autoAdd = false;
    private SidePanelButtonOptions _sidePanelButtons = SidePanelButtonOptions.AddDeleteButtons;
    private GridType _gridType = GridType.EditGrid;
    private bool _saveColumnWidths = true;
    private bool _disableButtons = false;
    private bool _validateData = false;
    private IContainer components = (IContainer) null;
    private GridControl _grid;
    private PanelControl buttonPanel;
    public SimpleButton addButton;
    public SimpleButton deleteButton;
    private Label linesLabel;
    private SpinEdit linesSpinEdit;

    private GridView CurrentGridView
    {
      get
      {
        return (GridView) this._grid.DefaultView;
      }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    public PanelControl ButtonPanel
    {
      get
      {
        return this.buttonPanel;
      }
    }

    [Browsable(true)]
    public GridControl Grid
    {
      get
      {
        return this._grid;
      }
      set
      {
        this._grid = value;
        if (this._grid == null)
          return;
        if (!this.Controls.Contains((Control) this._grid))
          this.Controls.Add((Control) this._grid);
        this._grid.Dock = DockStyle.Fill;
        this._grid.BringToFront();
        this._grid.Load += new EventHandler(this._grid_Load);
        this._grid.DoubleClick += (EventHandler) ((sender, e) =>
        {
          if (this.SelectionChoose == null)
            return;
          this.SelectionChoose(sender, e);
        });
        this._grid.KeyPress += (KeyPressEventHandler) ((sender, e) =>
        {
          if ((int) e.KeyChar != 13 || this.SelectionChoose == null)
            return;
          this.SelectionChoose(sender, new EventArgs());
          e.Handled = true;
        });
      }
    }

    [Browsable(true)]
    [DefaultValue(false)]
    public bool ShowLineControl
    {
      get
      {
        return this._showLineControl;
      }
      set
      {
        this._showLineControl = value;
        this.linesLabel.Visible = this._showLineControl;
        this.linesSpinEdit.Visible = this._showLineControl;
      }
    }

    [Browsable(true)]
    [DefaultValue(1)]
    public int DefaultLinesDisplayed
    {
      get
      {
        return Convert.ToInt32(this.linesSpinEdit.EditValue);
      }
      set
      {
        this.linesSpinEdit.EditValue = (object) value;
      }
    }

    [Browsable(true)]
    [DefaultValue(false)]
    public bool AutoAdd
    {
      get
      {
        return this._autoAdd;
      }
      set
      {
        this._autoAdd = value;
      }
    }

    [Browsable(true)]
    [DefaultValue(true)]
    public bool AutoDelete
    {
      get
      {
        return this._autoDelete;
      }
      set
      {
        this._autoDelete = value;
      }
    }

    [Browsable(true)]
    [DefaultValue(SidePanelButtonOptions.AddDeleteButtons)]
    public SidePanelButtonOptions SidePanelButtons
    {
      get
      {
        return this._sidePanelButtons;
      }
      set
      {
        this._sidePanelButtons = value;
        this.addButton.Visible = this._sidePanelButtons == SidePanelButtonOptions.AddButton || this._sidePanelButtons == SidePanelButtonOptions.AddDeleteButtons;
        this.deleteButton.Visible = this._sidePanelButtons == SidePanelButtonOptions.AddDeleteButtons;
        this.buttonPanel.Visible = this._sidePanelButtons != SidePanelButtonOptions.HidePanel;
      }
    }

    [Browsable(true)]
    [DefaultValue(GridType.EditGrid)]
    public GridType GridType
    {
      get
      {
        return this._gridType;
      }
      set
      {
        this._gridType = value;
        if (this.DesignMode)
          return;
        this.SetProperties();
      }
    }

    [Browsable(true)]
    [DefaultValue(true)]
    public bool SaveColumnWidths
    {
      get
      {
        return this._saveColumnWidths;
      }
      set
      {
        this._saveColumnWidths = value;
      }
    }

    [Browsable(true)]
    [DefaultValue(false)]
    public bool DisableButtons
    {
      get
      {
        return this._disableButtons;
      }
      set
      {
        this._disableButtons = value;
        this.addButton.Enabled = !this._disableButtons;
        if (this._disableButtons)
          this.deleteButton.Enabled = false;
        else
          this.GridView_RowCountChanged((object) null, (EventArgs) null);
      }
    }

    [Browsable(true)]
    [DefaultValue(false)]
    public bool ValidateData
    {
      get
      {
        return this._validateData;
      }
      set
      {
        this._validateData = value;
      }
    }

    private string GridName
    {
      get
      {
        string str = this._grid.Name;
        if (this._grid.DataSource is BindingSource && !(((BindingSource) this._grid.DataSource).DataSource is BindingSource) && ((BindingSource) this._grid.DataSource).DataSource != null)
          str = str + "_" + ((BindingSource) this._grid.DataSource).DataSource.ToString();
        return str;
      }
    }

    public event EventHandler SelectionChoose;

    public GridContainer()
    {
      this.InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
      base.OnLoad(e);
      this.buttonPanel.SendToBack();
    }

    private void SetGridLines()
    {
      this.linesSpinEdit.EditValue = (object) ConfigurationManager.LocalSettingsProfile.GetValue("GridLines", this.GridName, this.DefaultLinesDisplayed);
    }

    private void SetProperties()
    {
      if (this._grid == null || this._grid.DefaultView == null)
        return;
      GridViewMethods.SetTypeProperties((GridView) this._grid.DefaultView, this._gridType);
    }

    private void _grid_Load(object sender, EventArgs e)
    {
      if (!this.DesignMode)
        this.SetProperties();
      if (!this._validateData)
        ;
      if (this._showLineControl)
      {
        if (this._grid.DataSource is BindingSource && !(((BindingSource) this._grid.DataSource).DataSource is BindingSource))
          ((BindingSource) this._grid.DataSource).DataSourceChanged += new EventHandler(this.GridContainer_DataSourceChanged);
        else
          this.SetGridLines();
      }
      if (this._saveColumnWidths)
      {
        string str1 = ConfigurationManager.LocalSettingsProfile.GetValue("ColumnWidths", this.GridName, "");
        if (!string.IsNullOrEmpty(str1))
        {
          string[] strArray = str1.Split(',');
          int index = 0;
          foreach (string str2 in strArray)
          {
            if (index < this.CurrentGridView.Columns.Count)
            {
              this.CurrentGridView.Columns[index].Width = Convert.ToInt32(str2);
              ++index;
            }
            else
              break;
          }
        }
      }
      this.GridView_RowCountChanged((object) null, (EventArgs) null);
      this.CurrentGridView.RowCountChanged += new EventHandler(this.GridView_RowCountChanged);
      if (!this._saveColumnWidths)
        return;
      this.CurrentGridView.ColumnWidthChanged += new ColumnEventHandler(this.CurrentGridView_ColumnWidthChanged);
    }

    public T GetCurrentObject<T>()
    {
      T obj = default (T);
      if ((uint) this.CurrentGridView.GetSelectedRows().Length > 0U)
      {
        object row = this.CurrentGridView.GetRow(this.CurrentGridView.GetSelectedRows()[0]);
        if (row is T)
          obj = (T) row;
      }
      return obj;
    }

    public bool IsCurrentRowValid()
    {
      if (!this._validateData)
        return true;
      this.CurrentGridView.FocusedRowHandle = -1;
      if (this.CurrentGridView.GetSelectedRows().Length == 0 || this.CurrentGridView.GetSelectedRows()[0] < 0)
        return true;
      this.CurrentGridView.GetRow(this.CurrentGridView.GetSelectedRows()[0]);
      return true;
    }

    private void CurrentGridView_ColumnWidthChanged(object sender, ColumnEventArgs e)
    {
      if (!this._saveColumnWidths)
        return;
      string str = "";
      foreach (GridColumn gridColumn in (CollectionBase) this.CurrentGridView.Columns)
        str = str + gridColumn.Width.ToString() + ",";
      if ((uint) str.Length > 0U)
        str = str.TrimEnd(',');
      ConfigurationManager.LocalSettingsProfile.SetValue("ColumnWidths", this.GridName, (object) str);
    }

    private void GridView_RowCountChanged(object sender, EventArgs e)
    {
      if (this._disableButtons)
        return;
      this.deleteButton.Enabled = (uint) this.CurrentGridView.RowCount > 0U;
    }

    private void GridContainer_DataSourceChanged(object sender, EventArgs e)
    {
      this.SetGridLines();
    }

    private void linesSpinEdit_EditValueChanged(object sender, EventArgs e)
    {
      if (this._grid == null || !this._showLineControl)
        return;
      this.CurrentGridView.RowHeight = 4 + 13 * Convert.ToInt32(this.linesSpinEdit.EditValue);
      ConfigurationManager.LocalSettingsProfile.SetValue("GridLines", this.GridName, (object) Convert.ToInt32(this.linesSpinEdit.EditValue));
    }

    private void deleteButton_Click(object sender, EventArgs e)
    {
      if (!this._autoDelete || MessageBox.Show("Are you sure you want to delete the selected record?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        return;
      this.CurrentGridView.DeleteSelectedRows();
    }

    private void addButton_Click(object sender, EventArgs e)
    {
      if (!this._autoAdd || !this.IsCurrentRowValid())
        return;
      ((ColumnView) this._grid.DefaultView).AddNewRow();
      this.deleteButton.Enabled = true;
    }

    private void GridContainer_Load(object sender, EventArgs e)
    {
      if (this._grid == null || !(this._grid.DefaultView is GridView) || this.DesignMode)
        return;
      this.CurrentGridView.OptionsSelection.EnableAppearanceHideSelection = false;
      if (this._showLineControl)
        this.linesSpinEdit.EditValue = (object) Convert.ToInt32((this.CurrentGridView.RowHeight - 4) / 13);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.addButton = new SimpleButton();
      this.deleteButton = new SimpleButton();
      this.buttonPanel = new PanelControl();
      this.linesLabel = new Label();
      this.linesSpinEdit = new SpinEdit();
      this.buttonPanel.BeginInit();
      this.buttonPanel.SuspendLayout();
      this.linesSpinEdit.Properties.BeginInit();
      this.SuspendLayout();
      this.addButton.Location = new Point(5, 2);
      this.addButton.Name = "addButton";
      this.addButton.Size = new Size(68, 23);
      this.addButton.TabIndex = 0;
      this.addButton.Text = "Add";
      this.addButton.Click += new EventHandler(this.addButton_Click);
      this.deleteButton.CausesValidation = false;
      this.deleteButton.Location = new Point(5, 31);
      this.deleteButton.Name = "deleteButton";
      this.deleteButton.Size = new Size(68, 23);
      this.deleteButton.TabIndex = 1;
      this.deleteButton.Text = "Delete";
      this.deleteButton.Click += new EventHandler(this.deleteButton_Click);
      this.buttonPanel.Appearance.BackColor = Color.Transparent;
      this.buttonPanel.Appearance.Options.UseBackColor = true;
      this.buttonPanel.BorderStyle = BorderStyles.NoBorder;
      this.buttonPanel.Controls.Add((Control) this.linesLabel);
      this.buttonPanel.Controls.Add((Control) this.linesSpinEdit);
      this.buttonPanel.Controls.Add((Control) this.addButton);
      this.buttonPanel.Controls.Add((Control) this.deleteButton);
      this.buttonPanel.Dock = DockStyle.Right;
      this.buttonPanel.Location = new Point(369, 0);
      this.buttonPanel.Name = "buttonPanel";
      this.buttonPanel.Size = new Size(77, 259);
      this.buttonPanel.TabIndex = 2;
      this.linesLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.linesLabel.AutoSize = true;
      this.linesLabel.Location = new Point(5, 239);
      this.linesLabel.Name = "linesLabel";
      this.linesLabel.Size = new Size(31, 13);
      this.linesLabel.TabIndex = 3;
      this.linesLabel.Text = "Lines";
      this.linesLabel.Visible = false;
      this.linesSpinEdit.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      SpinEdit spinEdit = this.linesSpinEdit;
      int[] bits1 = new int[4];
      bits1[0] = 1;
      // ISSUE: variable of a boxed type
      var local = (ValueType) new Decimal(bits1);
      spinEdit.EditValue = (object) local;
      this.linesSpinEdit.Location = new Point(40, 236);
      this.linesSpinEdit.Name = "linesSpinEdit";
      this.linesSpinEdit.Properties.AllowNullInput = DefaultBoolean.False;
      this.linesSpinEdit.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.linesSpinEdit.Properties.IsFloatValue = false;
      this.linesSpinEdit.Properties.Mask.EditMask = "N00";
      RepositoryItemSpinEdit properties1 = this.linesSpinEdit.Properties;
      int[] bits2 = new int[4];
      bits2[0] = 5;
      Decimal num1 = new Decimal(bits2);
      properties1.MaxValue = num1;
      RepositoryItemSpinEdit properties2 = this.linesSpinEdit.Properties;
      int[] bits3 = new int[4];
      bits3[0] = 1;
      Decimal num2 = new Decimal(bits3);
      properties2.MinValue = num2;
      this.linesSpinEdit.Size = new Size(33, 20);
      this.linesSpinEdit.TabIndex = 2;
      this.linesSpinEdit.Visible = false;
      this.linesSpinEdit.EditValueChanged += new EventHandler(this.linesSpinEdit_EditValueChanged);
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.Controls.Add((Control) this.buttonPanel);
      this.Name = "GridContainer";
      this.Size = new Size(446, 259);
      this.Load += new EventHandler(this.GridContainer_Load);
      this.buttonPanel.EndInit();
      this.buttonPanel.ResumeLayout(false);
      this.buttonPanel.PerformLayout();
      this.linesSpinEdit.Properties.EndInit();
      this.ResumeLayout(false);
    }
  }
}
