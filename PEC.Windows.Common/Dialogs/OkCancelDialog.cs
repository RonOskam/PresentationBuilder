// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.Dialogs.OkCancelDialog
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PEC.Windows.Common.Dialogs
{
  public class OkCancelDialog : Form
  {
    private IContainer components = (IContainer) null;
    protected Button _okButton;
    protected Button _cancelButton;

    public OkCancelDialog()
    {
      this.InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this._okButton = new Button();
      this._cancelButton = new Button();
      this.SuspendLayout();
      this._okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this._okButton.DialogResult = DialogResult.OK;
      this._okButton.Location = new Point(157, 115);
      this._okButton.Name = "_okButton";
      this._okButton.Size = new Size(75, 23);
      this._okButton.TabIndex = 0;
      this._okButton.Text = "OK";
      this._okButton.UseVisualStyleBackColor = true;
      this._cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this._cancelButton.DialogResult = DialogResult.Cancel;
      this._cancelButton.Location = new Point(238, 115);
      this._cancelButton.Name = "_cancelButton";
      this._cancelButton.Size = new Size(75, 23);
      this._cancelButton.TabIndex = 1;
      this._cancelButton.Text = "Cancel";
      this._cancelButton.UseVisualStyleBackColor = true;
      this.AcceptButton = (IButtonControl) this._okButton;
      this.CancelButton = (IButtonControl) this._cancelButton;
      this.ClientSize = new Size(325, 150);
      this.Controls.Add((Control) this._cancelButton);
      this.Controls.Add((Control) this._okButton);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "OkCancelDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "OkCancelDialog";
      this.ResumeLayout(false);
    }
  }
}
