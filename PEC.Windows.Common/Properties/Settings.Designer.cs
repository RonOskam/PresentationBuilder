// Decompiled with JetBrains decompiler
// Type: PEC.Windows.Common.Properties.Settings
// Assembly: PEC.Windows.Common, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: CDBD3A57-B7D2-4B12-ADDA-3F67C481AC77
// Assembly location: C:\oaisd_app\_Misc\Presentation Builder\EXE\PEC.Windows.Common.dll

using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace PEC.Windows.Common.Properties
{
  [CompilerGenerated]
  [GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
  internal sealed class Settings : ApplicationSettingsBase
  {
    private static Settings defaultInstance = (Settings) SettingsBase.Synchronized((SettingsBase) new Settings());

    public static Settings Default
    {
      get
      {
        Settings settings = Settings.defaultInstance;
        return settings;
      }
    }
  }
}
