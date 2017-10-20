// Decompiled with JetBrains decompiler
// Type: PowerPointBuilder.Common
// Assembly: PresentationBuilder, Version=1.0.0.28120, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 295F5AD1-A97E-4830-A536-CA2F8525E5B1
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.exe

using PresentationBuilder.BLL;

namespace PowerPointBuilder
{
  public class Common
  {
    private static PresentationBuilderEntities _dataSource = (PresentationBuilderEntities) null;

    public static PresentationBuilderEntities DataSource
    {
      get
      {
        if (Common._dataSource == null)
          Common._dataSource = new PresentationBuilderEntities();
        return Common._dataSource;
      }
    }
  }
}
