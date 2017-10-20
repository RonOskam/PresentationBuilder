// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.NavigationItem
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using System.ComponentModel;
using System.Drawing;

namespace PEC.Windows.Common
{
  public class NavigationItem
  {
    private string _name = (string) null;
    private Image _image = (Image) null;
    private string _imageName = (string) null;
    private CancelEventHandler _eventHandler = (CancelEventHandler) null;
    private bool _allowTabTo = true;
    private string _linkCode = (string) null;

    public string Name
    {
      get
      {
        return this._name;
      }
      set
      {
        this._name = value;
      }
    }

    public Image Image
    {
      get
      {
        return this._image;
      }
      set
      {
        this._image = value;
      }
    }

    public string ImageName
    {
      get
      {
        return this._imageName;
      }
      set
      {
        this._imageName = value;
      }
    }

    public CancelEventHandler EventHandler
    {
      get
      {
        return this._eventHandler;
      }
      set
      {
        this._eventHandler = value;
      }
    }

    public bool AllowTabTo
    {
      get
      {
        return this._allowTabTo;
      }
      set
      {
        this._allowTabTo = value;
      }
    }

    public string LinkCode
    {
      get
      {
        return this._linkCode;
      }
      set
      {
        this._linkCode = value;
      }
    }

    public NavigationItem()
    {
    }

    public NavigationItem(string name, string imageName)
    {
      this._name = name;
      this._imageName = imageName;
    }
  }
}
