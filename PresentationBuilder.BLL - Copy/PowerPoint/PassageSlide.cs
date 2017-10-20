// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.BLL.PowerPoint.PassageSlide
// Assembly: PresentationBuilder.BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 3C38E67D-5DE8-463E-9D0A-ECF8F27A6106
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.BLL.dll

using Microsoft.Office.Interop.PowerPoint;
using PresentationBuilder.BLL;

namespace PresentationBuilder.BLL.PowerPoint
{
  public class PassageSlide : SlideItem
  {
    private string _verses;

    public PassageSlide(string verses)
    {
      this._verses = verses;
    }

    internal override void Generate(Presentation presentation)
    {
      this.AddMainTextbox(this.GenerateSlide(presentation), new PassageReader().GetBiblePassage(this._verses), MessageSlide.MessageFont);
    }

    public override string Validate()
    {
      PassageReader passageReader = new PassageReader();
      if (string.IsNullOrEmpty(this._verses))
        return "A passage must be entered.";
      if (!passageReader.IsPassageValid(this._verses))
        return this._verses + " is not a valid passage";
      return base.Validate();
    }
  }
}
