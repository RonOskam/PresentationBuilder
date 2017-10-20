// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.ExtensionMethods
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

namespace PEC.Windows.Common
{
  public static class ExtensionMethods
  {
    public static bool IsEmpty(this string input)
    {
      return string.IsNullOrEmpty(input);
    }

    public static string TrimTo(this string input, int len)
    {
      if (ExtensionMethods.IsEmpty(input) || input.Length <= len)
        return input;
      return input.Substring(0, len);
    }
  }
}
