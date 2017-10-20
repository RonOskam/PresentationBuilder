// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.Dialogs.DateRangeDialog
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PEC.Windows.Common.Dialogs
{
  public class DateRangeDialog : OkCancelDialog
  {
    private IContainer components = (IContainer) null;
    private DateEdit startDateEdit;
    private DateEdit endDateEdit;
    private Label label1;
    private Label messageLabel;

    public DateTime StartDate
    {
      get
      {
        return this.startDateEdit.DateTime;
      }
      set
      {
        this.startDateEdit.DateTime = value;
      }
    }

    public DateTime EndDate
    {
      get
      {
        return this.endDateEdit.DateTime;
      }
      set
      {
        this.endDateEdit.DateTime = value;
      }
    }

    public DateRangeDialog(string labelText, string caption)
    {
      this.InitializeComponent();
      this.Text = caption;
      this.messageLabel.Text = labelText;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.startDateEdit = new DateEdit();
      this.endDateEdit = new DateEdit();
      this.label1 = new Label();
      this.messageLabel = new Label();
      this.startDateEdit.Properties.VistaTimeProperties.BeginInit();
      this.startDateEdit.Properties.BeginInit();
      this.endDateEdit.Properties.VistaTimeProperties.BeginInit();
      this.endDateEdit.Properties.BeginInit();
      this.SuspendLayout();
      this._okButton.Location = new Point(95, 105);
      this._cancelButton.Location = new Point(176, 105);
      this.startDateEdit.EditValue = (object) null;
      this.startDateEdit.Location = new Point(14, 59);
      this.startDateEdit.Name = "startDateEdit";
      this.startDateEdit.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      this.startDateEdit.Properties.VistaTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.startDateEdit.Size = new Size(100, 20);
      this.startDateEdit.TabIndex = 2;
      this.endDateEdit.EditValue = (object) null;
      this.endDateEdit.Location = new Point(142, 59);
      this.endDateEdit.Name = "endDateEdit";
      this.endDateEdit.Properties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton(ButtonPredefines.Combo)
      });
      this.endDateEdit.Properties.VistaTimeProperties.Buttons.AddRange(new EditorButton[1]
      {
        new EditorButton()
      });
      this.endDateEdit.Size = new Size(100, 20);
      this.endDateEdit.TabIndex = 3;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(120, 62);
      this.label1.Name = "label1";
      this.label1.Size = new Size(16, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "to";
      this.messageLabel.Location = new Point(12, 9);
      this.messageLabel.Name = "messageLabel";
      this.messageLabel.Size = new Size(239, 34);
      this.messageLabel.TabIndex = 5;
      this.ClientSize = new Size(263, 140);
      this.Controls.Add((Control) this.messageLabel);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.endDateEdit);
      this.Controls.Add((Control) this.startDateEdit);
      this.Name = "DateRangeDialog";
      this.Text = "Enter a Date Range";
      this.Controls.SetChildIndex((Control) this.startDateEdit, 0);
      this.Controls.SetChildIndex((Control) this._okButton, 0);
      this.Controls.SetChildIndex((Control) this._cancelButton, 0);
      this.Controls.SetChildIndex((Control) this.endDateEdit, 0);
      this.Controls.SetChildIndex((Control) this.label1, 0);
      this.Controls.SetChildIndex((Control) this.messageLabel, 0);
      this.startDateEdit.Properties.VistaTimeProperties.EndInit();
      this.startDateEdit.Properties.EndInit();
      this.endDateEdit.Properties.VistaTimeProperties.EndInit();
      this.endDateEdit.Properties.EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
