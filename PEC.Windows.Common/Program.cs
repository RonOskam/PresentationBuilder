// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.Program
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using DevExpress.LookAndFeel;
using System;
using System.Windows.Forms;

namespace PEC.Windows.Common
{
  internal static class Program
  {
    [STAThread]
    private static void Main()
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      UserLookAndFeel.Default.UseWindowsXPTheme = true;
      Application.Run((Form) new CommonMainForm());
    }
  }
}
