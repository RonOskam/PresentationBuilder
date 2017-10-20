// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.Win.Program
// Assembly: PresentationBuilder, Version=1.0.0.28120, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 295F5AD1-A97E-4830-A536-CA2F8525E5B1
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.exe

using System;
using System.Windows.Forms;

namespace PresentationBuilder.Win
{
  internal static class Program
  {
    [STAThread]
    private static void Main(string[] args)
    {
      Application.EnableVisualStyles();
      Application.SetCompatibleTextRenderingDefault(false);
      if (args.Length == 0)
        Application.Run((Form) new MainForm());
      else
        Application.Run((Form) new MainForm(args[0]));
    }
  }
}
