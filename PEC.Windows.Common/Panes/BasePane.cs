// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.Panes.BasePane
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using PEC.Windows.Common;
using System.ComponentModel;
using System.Windows.Forms;

namespace PEC.Windows.Common.Panes
{
  public class BasePane : UserControl
  {
    private ToolStrip _toolStrip = (ToolStrip) null;
    private string _title = "";
    private bool _closed = false;
    private bool _keepOpen = false;
    private IContainer components = (IContainer) null;

    public CommonMainForm HostForm
    {
      get
      {
        return CommonMainForm.Instance;
      }
    }

    public ToolStrip ToolStrip
    {
      get
      {
        return this._toolStrip;
      }
      set
      {
        this._toolStrip = value;
      }
    }

    public string Title
    {
      get
      {
        return this._title;
      }
      set
      {
        this._title = value;
      }
    }

    public bool Closed
    {
      get
      {
        return this._closed;
      }
    }

    public bool KeepOpen
    {
      get
      {
        return this._keepOpen;
      }
      set
      {
        this._keepOpen = value;
      }
    }

    public virtual event BasePane.GetContentHandler GetContent;

    public override void Refresh()
    {
      if (!this.Visible || this.DesignMode)
        return;
      this.LoadPane();
    }

    public virtual void Open()
    {
      CommonMainForm.Instance.KeyPreview = true;
      CommonMainForm.Instance.KeyPress += new KeyPressEventHandler(this.Form_KeyPress);
      this.LoadPane();
    }

    public virtual void LoadPane()
    {
    }

    public virtual bool Close()
    {
      if (CommonMainForm.Instance != null)
      {
        CommonMainForm.Instance.KeyPreview = false;
        CommonMainForm.Instance.KeyPress -= new KeyPressEventHandler(this.Form_KeyPress);
      }
      this._closed = true;
      return true;
    }

    public virtual bool CanChange()
    {
      return true;
    }

    public virtual void Form_KeyPress(object sender, KeyPressEventArgs e)
    {
    }

    public void OnGetContent()
    {
      // ISSUE: reference to a compiler-generated field
      // ISSUE: reference to a compiler-generated field
      if (this.GetContent == null || this.GetContent == null)
        return;
      // ISSUE: reference to a compiler-generated field
      foreach (BasePane.GetContentHandler getContentHandler in this.GetContent.GetInvocationList())
        this.AddContent(getContentHandler());
    }

    protected virtual void AddContent(HtmlContentSectionCollection section)
    {
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
      this.AutoScaleMode = AutoScaleMode.Font;
    }

    public delegate HtmlContentSectionCollection GetContentHandler();
  }
}
