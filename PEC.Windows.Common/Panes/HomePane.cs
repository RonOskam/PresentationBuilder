// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.HomePane
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using PEC.Windows.Common.Panes;
using PEC.Windows.Common.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PEC.Windows.Common
{
  public class HomePane : BasePane
  {
    private IContainer components = (IContainer) null;
    private string _html = "";
    protected WebBrowser toDoHTML;
    protected Label VersionLabel;
    private PictureBox pecPicture;
    private Label label1;

    protected virtual string HTMLStyle
    {
      get
      {
        return "\r\n        <style>\r\n          body {font-family:arial; font-size:9pt; color:WindowText; }\r\n          h1 {font-size:10pt; font-weight:bold; margin-bottom:5px}\r\n          ul {font-size:9pt; margin-top:0px}\r\n          a, a:visited {font-weight:normal; color:Blue; text-decoration:none; }\r\n          a:hover {color:Blue; text-decoration:underline; }\r\n          th {font-size:10pt; font-weight:normal; text-align:left; margin-bottom:0px}\r\n          td {font-size:9pt; margin-top:0px}\r\n        </style>\r\n        ";
      }
    }

    public HomePane()
    {
      this.InitializeComponent();
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    protected override void AddContent(HtmlContentSectionCollection content)
    {
      foreach (HtmlContentSection htmlContentSection in (List<HtmlContentSection>) content)
      {
        this._html = this._html + "<span><h1>" + htmlContentSection.Title + ":</h1></span>";
        this._html = this._html + "<span>" + htmlContentSection.Content + "</span>";
      }
    }

    public override void LoadPane()
    {
      this.VersionLabel.Text = "Version:  " + Application.ProductVersion;
      this._html = "<html>" + this.HTMLStyle + "<body scroll=\"auto\">";
      this.OnGetContent();
      this._html = this._html + "</body></html>";
      this.Visible = true;
      this.toDoHTML.DocumentText = this._html;
      this.toDoHTML.ObjectForScripting = (object) this.HostForm;
    }

    private void InitializeComponent()
    {
      this.toDoHTML = new WebBrowser();
      this.VersionLabel = new Label();
      this.pecPicture = new PictureBox();
      this.label1 = new Label();
      ((ISupportInitialize) this.pecPicture).BeginInit();
      this.SuspendLayout();
      this.toDoHTML.AllowWebBrowserDrop = false;
      this.toDoHTML.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
      this.toDoHTML.IsWebBrowserContextMenuEnabled = false;
      this.toDoHTML.Location = new Point(156, 104);
      this.toDoHTML.Name = "toDoHTML";
      this.toDoHTML.ScriptErrorsSuppressed = true;
      this.toDoHTML.Size = new Size(335, 178);
      this.toDoHTML.TabIndex = 0;
      this.toDoHTML.WebBrowserShortcutsEnabled = false;
      this.VersionLabel.Location = new Point(329, 84);
      this.VersionLabel.Name = "VersionLabel";
      this.VersionLabel.Size = new Size(155, 13);
      this.VersionLabel.TabIndex = 7;
      this.VersionLabel.Text = "version";
      this.VersionLabel.TextAlign = ContentAlignment.TopRight;
      this.pecPicture.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.pecPicture.BackgroundImageLayout = ImageLayout.Center;
      this.pecPicture.Cursor = Cursors.Hand;
      this.pecPicture.Image = (Image) Resources.PECLogoSmWhiteBG;
      this.pecPicture.InitialImage = (Image) null;
      this.pecPicture.Location = new Point(404, 307);
      this.pecPicture.Name = "pecPicture";
      this.pecPicture.Size = new Size(87, 44);
      this.pecPicture.SizeMode = PictureBoxSizeMode.StretchImage;
      this.pecPicture.TabIndex = 11;
      this.pecPicture.TabStop = false;
      this.pecPicture.Click += new EventHandler(this.pecPicture_Click);
      this.label1.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
      this.label1.Location = new Point(404, 292);
      this.label1.Name = "label1";
      this.label1.Size = new Size(87, 13);
      this.label1.TabIndex = 12;
      this.label1.Text = "developed by";
      this.label1.TextAlign = ContentAlignment.BottomCenter;
      this.BackColor = SystemColors.Window;
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.pecPicture);
      this.Controls.Add((Control) this.toDoHTML);
      this.Controls.Add((Control) this.VersionLabel);
      this.Name = "HomePane";
      this.Size = new Size(526, 364);
      this.Title = "Home Page";
      ((ISupportInitialize) this.pecPicture).EndInit();
      this.ResumeLayout(false);
    }

    private void pecPicture_Click(object sender, EventArgs e)
    {
      Process.Start("http://www.pectechnologies.com");
    }
  }
}
