// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.Dialogs.BaseDialog
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PEC.Windows.Common.Dialogs
{
  public class BaseDialog : Form
  {
    private IContainer components = (IContainer) null;

    public BaseDialog()
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
      this.SuspendLayout();
      this.ClientSize = new Size(325, 150);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "BaseDialog";
      this.ShowInTaskbar = false;
      this.StartPosition = FormStartPosition.CenterParent;
      this.Text = "BaseDialog";
      this.ResumeLayout(false);
    }
  }
}
