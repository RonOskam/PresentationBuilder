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
