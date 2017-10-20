// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.BLL.PowerPoint.BlankSlide
// Assembly: PresentationBuilder.BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 3C38E67D-5DE8-463E-9D0A-ECF8F27A6106
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.BLL.dll

using Microsoft.Office.Interop.PowerPoint;

namespace PresentationBuilder.BLL.PowerPoint
{
  public class BlankSlide : SlideItem
  {
    internal override void Generate(Presentation presentation)
    {
      this.GenerateSlide(presentation);
    }
  }
}
