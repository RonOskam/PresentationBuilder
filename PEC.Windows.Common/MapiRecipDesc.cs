// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.MapiRecipDesc
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using System;
using System.Runtime.InteropServices;

namespace PEC.Windows.Common
{
  [StructLayout(LayoutKind.Sequential)]
  public class MapiRecipDesc
  {
    public int reserved;
    public int recipClass;
    public string name;
    public string address;
    public int eIDSize;
    public IntPtr entryID;
  }
}
