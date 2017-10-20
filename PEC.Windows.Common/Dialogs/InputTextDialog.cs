// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.Dialogs.InputTextDialog
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace PEC.Windows.Common.Dialogs
{
  public class InputTextDialog : OkCancelDialog
  {
    private IContainer components = (IContainer) null;
    private Label _inputLabel;
    private TextBox _inputText;

    public string InputText
    {
      get
      {
        return this._inputText.Text;
      }
    }

    public InputTextDialog(string labelText, string caption)
      : this(labelText, caption, "")
    {
    }

    public InputTextDialog(string labelText, string caption, string text)
    {
      this.InitializeComponent();
      this.Text = caption;
      this._inputLabel.Text = labelText;
      this._inputText.Text = text;
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this._inputLabel = new Label();
      this._inputText = new TextBox();
      this.SuspendLayout();
      this._okButton.Location = new Point(148, 86);
      this._okButton.TabIndex = 1;
      this._cancelButton.Location = new Point(229, 86);
      this._cancelButton.TabIndex = 2;
      this._inputLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this._inputLabel.Location = new Point(12, 9);
      this._inputLabel.Name = "_inputLabel";
      this._inputLabel.Size = new Size(287, 46);
      this._inputLabel.TabIndex = 0;
      this._inputText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      this._inputText.Location = new Point(13, 58);
      this._inputText.Name = "_inputText";
      this._inputText.Size = new Size(286, 20);
      this._inputText.TabIndex = 0;
      this.AutoScaleDimensions = new SizeF(6f, 13f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(312, 121);
      this.Controls.Add((Control) this._inputText);
      this.Controls.Add((Control) this._inputLabel);
      this.Name = "InputTextDialog";
      this.Text = "Input";
      this.Controls.SetChildIndex((Control) this._okButton, 0);
      this.Controls.SetChildIndex((Control) this._cancelButton, 0);
      this.Controls.SetChildIndex((Control) this._inputLabel, 0);
      this.Controls.SetChildIndex((Control) this._inputText, 0);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
