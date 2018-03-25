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
