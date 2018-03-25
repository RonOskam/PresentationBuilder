using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using PEC.Windows.Common.Dialogs;
using PresentationBuilder.BLL.PowerPoint;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace PresentationBuilder.Win.Dialogs
{
  public class FontSelectDialog : OkCancelDialog
  {
    private IContainer components = (IContainer) null;
    private Label fontLabel;
    private NumericUpDown upDownSize;
    private Label label1;
    private ColorEdit colorEdit;
    private Label label2;
    private ListBox styleListBox;
    private Label label3;
    private Label label4;
    private CheckBox underlineCheck;
    private CheckBox shadowCheck;
    private FontEdit fontDropDown;
    private NumericUpDown spacingUpDown;
    private Label label5;

    public FontItem SelectedFont
    {
      get
      {
        Font font = this.GetFont();
        return new FontItem()
        {
          Bold = font.Bold,
          FontColor = this.colorEdit.Color,
          Italic = font.Italic,
          FontName = font.Name,
          Shadow = this.shadowCheck.Checked,
          Size = font.Size,
          Underline = font.Underline,
          LineSpacing = Convert.ToSingle(this.spacingUpDown.Value)
        };
      }
      set
      {
        if (value == null)
          return;
        this.fontDropDown.SelectedItem = (object) value.FontName;
        this.upDownSize.Value = Convert.ToDecimal(value.Size);
        this.underlineCheck.Checked = value.Underline;
        this.shadowCheck.Checked = value.Shadow;
        this.colorEdit.Color = value.FontColor;
        if ((double) value.LineSpacing != 0.0)
          this.spacingUpDown.Value = Convert.ToDecimal(value.LineSpacing);
        this.styleListBox.SelectedItem = !value.Bold || !value.Italic ? (!value.Bold ? (!value.Italic ? (object) "Regular" : (object) "Italic") : (object) "Bold") : (object) "Bold Italic";
      }
    }

    public FontSelectDialog()
    {
      this.InitializeComponent();
    }

    private void fontDropDown_SelectedIndexChanged(object sender, EventArgs e)
    {
      FontFamily fontFamily = Enumerable.FirstOrDefault<FontFamily>((IEnumerable<FontFamily>) FontFamily.Families, (Func<FontFamily, bool>) (f => f.Name == Convert.ToString(this.fontDropDown.SelectedItem)));
      this.styleListBox.Items.Clear();
      if (fontFamily.IsStyleAvailable(FontStyle.Regular))
        this.styleListBox.Items.Add((object) "Regular");
      if (fontFamily.IsStyleAvailable(FontStyle.Bold))
        this.styleListBox.Items.Add((object) "Bold");
      if (fontFamily.IsStyleAvailable(FontStyle.Italic))
        this.styleListBox.Items.Add((object) "Italic");
      if (fontFamily.IsStyleAvailable(FontStyle.Bold) && fontFamily.IsStyleAvailable(FontStyle.Italic))
        this.styleListBox.Items.Add((object) "Bold Italic");
      this.styleListBox.SelectedIndex = 0;
      this.SetFont();
    }

    private void SetFont()
    {
      this.fontLabel.Font = this.GetFont();
      this.fontLabel.ForeColor = this.colorEdit.Color;
    }

    private Font GetFont()
    {
      FontFamily family = Enumerable.FirstOrDefault<FontFamily>((IEnumerable<FontFamily>) FontFamily.Families, (Func<FontFamily, bool>) (f => f.Name == Convert.ToString(this.fontDropDown.SelectedItem)));
      string str = this.styleListBox.SelectedItem.ToString();
      FontStyle style = !(str == "Regular") ? (!(str == "Bold") ? (!(str == "Italic") ? FontStyle.Bold | FontStyle.Italic : FontStyle.Italic) : FontStyle.Bold) : FontStyle.Regular;
      if (this.underlineCheck.Checked)
        style |= FontStyle.Underline;
      return new Font(family, Convert.ToSingle(this.upDownSize.Value), style);
    }

    private void styleListBox_SelectedIndexChanged(object sender, EventArgs e)
    {
      this.SetFont();
    }

    private void upDownSize_ValueChanged(object sender, EventArgs e)
    {
      this.SetFont();
    }

    private void colorEdit_EditValueChanged(object sender, EventArgs e)
    {
      this.SetFont();
    }

    private void underlineCheck_CheckedChanged(object sender, EventArgs e)
    {
      this.SetFont();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.fontLabel = new System.Windows.Forms.Label();
      this.upDownSize = new System.Windows.Forms.NumericUpDown();
      this.label1 = new System.Windows.Forms.Label();
      this.colorEdit = new DevExpress.XtraEditors.ColorEdit();
      this.label2 = new System.Windows.Forms.Label();
      this.styleListBox = new System.Windows.Forms.ListBox();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.underlineCheck = new System.Windows.Forms.CheckBox();
      this.shadowCheck = new System.Windows.Forms.CheckBox();
      this.fontDropDown = new DevExpress.XtraEditors.FontEdit();
      this.spacingUpDown = new System.Windows.Forms.NumericUpDown();
      this.label5 = new System.Windows.Forms.Label();
      ((System.ComponentModel.ISupportInitialize)(this.upDownSize)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.colorEdit.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.fontDropDown.Properties)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.spacingUpDown)).BeginInit();
      this.SuspendLayout();
      // 
      // _okButton
      // 
      this._okButton.Location = new System.Drawing.Point(215, 203);
      this._okButton.TabIndex = 7;
      // 
      // _cancelButton
      // 
      this._cancelButton.Location = new System.Drawing.Point(296, 203);
      this._cancelButton.TabIndex = 8;
      // 
      // fontLabel
      // 
      this.fontLabel.BackColor = System.Drawing.Color.LightGray;
      this.fontLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.fontLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 32F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.fontLabel.Location = new System.Drawing.Point(10, 106);
      this.fontLabel.Name = "fontLabel";
      this.fontLabel.Size = new System.Drawing.Size(361, 90);
      this.fontLabel.TabIndex = 3;
      this.fontLabel.Text = "AaBbCc";
      this.fontLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // upDownSize
      // 
      this.upDownSize.Increment = new decimal(new int[] {
            4,
            0,
            0,
            0});
      this.upDownSize.Location = new System.Drawing.Point(247, 25);
      this.upDownSize.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
      this.upDownSize.Name = "upDownSize";
      this.upDownSize.Size = new System.Drawing.Size(52, 20);
      this.upDownSize.TabIndex = 4;
      this.upDownSize.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
      this.upDownSize.ValueChanged += new System.EventHandler(this.upDownSize_ValueChanged);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(244, 9);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(51, 13);
      this.label1.TabIndex = 6;
      this.label1.Text = "Font Size";
      // 
      // colorEdit
      // 
      this.colorEdit.EditValue = System.Drawing.Color.Black;
      this.colorEdit.Location = new System.Drawing.Point(247, 74);
      this.colorEdit.Name = "colorEdit";
      this.colorEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.colorEdit.Properties.ShowCustomColors = false;
      this.colorEdit.Properties.ShowSystemColors = false;
      this.colorEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
      this.colorEdit.Size = new System.Drawing.Size(124, 20);
      this.colorEdit.TabIndex = 6;
      this.colorEdit.EditValueChanged += new System.EventHandler(this.colorEdit_EditValueChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(244, 58);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(55, 13);
      this.label2.TabIndex = 8;
      this.label2.Text = "Font Color";
      // 
      // styleListBox
      // 
      this.styleListBox.FormattingEnabled = true;
      this.styleListBox.Location = new System.Drawing.Point(159, 25);
      this.styleListBox.Name = "styleListBox";
      this.styleListBox.Size = new System.Drawing.Size(82, 69);
      this.styleListBox.TabIndex = 3;
      this.styleListBox.SelectedIndexChanged += new System.EventHandler(this.styleListBox_SelectedIndexChanged);
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(156, 9);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(54, 13);
      this.label3.TabIndex = 10;
      this.label3.Text = "Font Style";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(7, 9);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(28, 13);
      this.label4.TabIndex = 11;
      this.label4.Text = "Font";
      // 
      // underlineCheck
      // 
      this.underlineCheck.AutoSize = true;
      this.underlineCheck.Location = new System.Drawing.Point(12, 54);
      this.underlineCheck.Name = "underlineCheck";
      this.underlineCheck.Size = new System.Drawing.Size(71, 17);
      this.underlineCheck.TabIndex = 1;
      this.underlineCheck.Text = "Underline";
      this.underlineCheck.UseVisualStyleBackColor = true;
      this.underlineCheck.CheckedChanged += new System.EventHandler(this.underlineCheck_CheckedChanged);
      // 
      // shadowCheck
      // 
      this.shadowCheck.AutoSize = true;
      this.shadowCheck.Location = new System.Drawing.Point(12, 77);
      this.shadowCheck.Name = "shadowCheck";
      this.shadowCheck.Size = new System.Drawing.Size(65, 17);
      this.shadowCheck.TabIndex = 2;
      this.shadowCheck.Text = "Shadow";
      this.shadowCheck.UseVisualStyleBackColor = true;
      this.shadowCheck.CheckedChanged += new System.EventHandler(this.underlineCheck_CheckedChanged);
      // 
      // fontDropDown
      // 
      this.fontDropDown.Location = new System.Drawing.Point(10, 25);
      this.fontDropDown.Name = "fontDropDown";
      this.fontDropDown.Properties.AppearanceDropDown.Font = new System.Drawing.Font("Tahoma", 10F);
      this.fontDropDown.Properties.AppearanceDropDown.Options.UseFont = true;
      this.fontDropDown.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
      this.fontDropDown.Properties.DropDownRows = 12;
      this.fontDropDown.Properties.PopupSizeable = true;
      this.fontDropDown.Properties.RecentlyUsedItemCount = 0;
      this.fontDropDown.Properties.ShowOnlyRegularStyleFonts = false;
      this.fontDropDown.Properties.Sorted = true;
      this.fontDropDown.Size = new System.Drawing.Size(134, 20);
      this.fontDropDown.TabIndex = 0;
      this.fontDropDown.SelectedIndexChanged += new System.EventHandler(this.fontDropDown_SelectedIndexChanged);
      // 
      // spacingUpDown
      // 
      this.spacingUpDown.DecimalPlaces = 1;
      this.spacingUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
      this.spacingUpDown.Location = new System.Drawing.Point(319, 25);
      this.spacingUpDown.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
      this.spacingUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.spacingUpDown.Name = "spacingUpDown";
      this.spacingUpDown.Size = new System.Drawing.Size(52, 20);
      this.spacingUpDown.TabIndex = 5;
      this.spacingUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(316, 9);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(69, 13);
      this.label5.TabIndex = 16;
      this.label5.Text = "Line Spacing";
      // 
      // FontSelectDialog
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(387, 234);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.spacingUpDown);
      this.Controls.Add(this.fontDropDown);
      this.Controls.Add(this.shadowCheck);
      this.Controls.Add(this.underlineCheck);
      this.Controls.Add(this.label4);
      this.Controls.Add(this.label3);
      this.Controls.Add(this.styleListBox);
      this.Controls.Add(this.label2);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.colorEdit);
      this.Controls.Add(this.fontLabel);
      this.Controls.Add(this.upDownSize);
      this.Name = "FontSelectDialog";
      this.Text = "Select Font";
      this.Controls.SetChildIndex(this.upDownSize, 0);
      this.Controls.SetChildIndex(this.fontLabel, 0);
      this.Controls.SetChildIndex(this.colorEdit, 0);
      this.Controls.SetChildIndex(this.label1, 0);
      this.Controls.SetChildIndex(this._okButton, 0);
      this.Controls.SetChildIndex(this._cancelButton, 0);
      this.Controls.SetChildIndex(this.label2, 0);
      this.Controls.SetChildIndex(this.styleListBox, 0);
      this.Controls.SetChildIndex(this.label3, 0);
      this.Controls.SetChildIndex(this.label4, 0);
      this.Controls.SetChildIndex(this.underlineCheck, 0);
      this.Controls.SetChildIndex(this.shadowCheck, 0);
      this.Controls.SetChildIndex(this.fontDropDown, 0);
      this.Controls.SetChildIndex(this.spacingUpDown, 0);
      this.Controls.SetChildIndex(this.label5, 0);
      ((System.ComponentModel.ISupportInitialize)(this.upDownSize)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.colorEdit.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.fontDropDown.Properties)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.spacingUpDown)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }
  }
}
