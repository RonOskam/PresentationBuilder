// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.Panes.HtmlContentSection
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

namespace PEC.Windows.Common.Panes
{
  public class HtmlContentSection
  {
    private string _title;
    private string _content;

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

    public string Content
    {
      get
      {
        return this._content;
      }
      set
      {
        this._content = value;
      }
    }

    public HtmlContentSection()
    {
    }

    public HtmlContentSection(string title, string content)
    {
      this._title = title;
      this._content = content;
    }
  }
}
