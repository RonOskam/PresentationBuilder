// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.NavigationItemCollection
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using System.Collections.Generic;

namespace PEC.Windows.Common
{
  public class NavigationItemCollection : List<NavigationItem>
  {
    public NavigationItem FindByName(string name)
    {
      foreach (NavigationItem navigationItem in (List<NavigationItem>) this)
      {
        if (navigationItem.Name == name)
          return navigationItem;
      }
      return (NavigationItem) null;
    }
  }
}
