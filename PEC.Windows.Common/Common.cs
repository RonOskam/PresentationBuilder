// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.Common
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using System.ComponentModel;
using System.Drawing;
using System.Resources;
using System.Threading;

namespace PEC.Windows.Common
{
  public class Common
  {
    public static ResourceManager AppResourceManager = (ResourceManager) null;

    public static Image GetResourceImage(string imageName)
    {
      if (Common.AppResourceManager == null)
        return (Image) null;
      object @object = Common.AppResourceManager.GetObject(imageName);
      if (@object != null)
        return (Image) @object;
      return (Image) null;
    }

    public static Image GetResourceImage(string imageName, int size)
    {
      if (Common.AppResourceManager == null)
        return (Image) null;
      object @object = Common.AppResourceManager.GetObject(imageName + size.ToString());
      if (@object != null)
        return (Image) @object;
      return (Image) null;
    }

    public static string ConvertToFriendlyName(string name)
    {
      if (name.EndsWith("ID"))
        name = name.Substring(0, name.Length - 2);
      string str = "";
      foreach (char c in name)
      {
        if (str.Length != 0 && (char.IsNumber(c) || char.IsUpper(c)))
          str += " ";
        str += c.ToString();
      }
      return str;
    }

    public static void DelayCall(Common.delayCallDelegate method, int delay)
    {
      BackgroundWorker backgroundWorker = new BackgroundWorker();
      backgroundWorker.DoWork += (DoWorkEventHandler) ((sender, e) => Thread.Sleep(delay));
      backgroundWorker.RunWorkerCompleted += (RunWorkerCompletedEventHandler) ((sender, e) => method());
      backgroundWorker.RunWorkerAsync();
    }

    public delegate void delayCallDelegate();

    private struct ListItem
    {
      private int? _ID;
      private string _description;

      public int? ID
      {
        get
        {
          return this._ID;
        }
        set
        {
          this._ID = value;
        }
      }

      public string Description
      {
        get
        {
          return this._description;
        }
        set
        {
          this._description = value;
        }
      }
    }
  }
}
