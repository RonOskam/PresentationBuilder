// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.Controls.HeaderBar
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace PEC.Windows.Common.Controls
{
  public class HeaderBar : Panel
  {
    private Label _label = new Label();
    private PictureBox _pictureBox = new PictureBox();
    private IContainer components = (IContainer) null;

    [Browsable(true)]
    public override string Text
    {
      get
      {
        return this._label.Text;
      }
      set
      {
        this._label.Text = value;
      }
    }

    public Image Image
    {
      get
      {
        return this._pictureBox.Image;
      }
      set
      {
        this._pictureBox.Image = value;
        this._pictureBox.Visible = this._pictureBox.Image != null;
        if (this._pictureBox.Image == null)
          return;
        this._pictureBox.Width = this._pictureBox.Image.Width;
      }
    }

    public HeaderBar()
    {
      this.InitializeComponent();
      this.Initialize();
    }

    public HeaderBar(IContainer container)
    {
      container.Add((IComponent) this);
      this.InitializeComponent();
      this.Initialize();
    }

    private void Initialize()
    {
      this.Paint += new PaintEventHandler(this.HeaderBar_Paint);
      this.FontChanged += new EventHandler(this.HeaderBar_FontChanged);
      this.ForeColorChanged += new EventHandler(this.HeaderBar_ForeColorChanged);
      this.SuspendLayout();
      this.Controls.Add((Control) this._label);
      this.Controls.Add((Control) this._pictureBox);
      this.Padding = new Padding(5, 0, 0, 0);
      this._pictureBox.Width = 32;
      this._pictureBox.Height = 32;
      this._pictureBox.BackColor = Color.Transparent;
      this._pictureBox.Dock = DockStyle.Left;
      this._pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
      this._pictureBox.Visible = false;
      this._label.Dock = DockStyle.Fill;
      this._label.Padding = new Padding(8, 0, 0, 0);
      this._label.TextAlign = ContentAlignment.MiddleLeft;
      this._label.BackColor = Color.Transparent;
      this._label.UseMnemonic = false;
      this._label.ForeColor = SystemColors.Window;
      this.Font = new Font("Arial", 12f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
      this.ResumeLayout(false);
      this.PerformLayout();
    }

    private void HeaderBar_ForeColorChanged(object sender, EventArgs e)
    {
      this._label.ForeColor = this.ForeColor;
    }

    private void HeaderBar_FontChanged(object sender, EventArgs e)
    {
      this._label.Font = this.Font;
    }

    private void HeaderBar_Paint(object sender, PaintEventArgs e)
    {
      Color controlDark1 = SystemColors.ControlDark;
      Color controlDark2 = SystemColors.ControlDark;
      Graphics graphics = e.Graphics;
      Rectangle clipRectangle = e.ClipRectangle;
      if (clipRectangle.Width <= 0 || clipRectangle.Height <= 0)
        return;
      using (Brush brush = (Brush) new LinearGradientBrush(clipRectangle, controlDark1, controlDark2, LinearGradientMode.Vertical))
        graphics.FillRectangle(brush, clipRectangle);
    }

    private void PaintPanelWithBorder(object sender, PaintEventArgs e)
    {
      this.OnPaint(e);
      int num = 1;
      Color controlDark = SystemColors.ControlDark;
      ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, controlDark, num, ButtonBorderStyle.Solid, controlDark, num, ButtonBorderStyle.Solid, controlDark, num, ButtonBorderStyle.Solid, controlDark, num, ButtonBorderStyle.Solid);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
    }
  }
}
