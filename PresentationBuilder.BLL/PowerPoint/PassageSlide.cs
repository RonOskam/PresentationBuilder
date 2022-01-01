using Microsoft.Office.Interop.PowerPoint;
using PresentationBuilder.BLL;

namespace PresentationBuilder.BLL.PowerPoint
{
  public class PassageSlide : SlideItem
  {
    private string _verses;

    public PassageSlide(string verses)
    {
      _verses = verses;
    }

    internal override void Generate(Presentation presentation)
    {
      string passage = new PassageReader().GetBiblePassage(_verses);

      passage += $"\r\n\r\n{ _verses }";
      AddMainTextbox(GenerateSlide(presentation), passage, MessageSlide.MessageFont);
    }

    public override string Validate()
    {
      var passageReader = new PassageReader();
      if (string.IsNullOrEmpty(_verses))
        return "A passage must be entered.";
      else if (!passageReader.IsPassageValid(_verses))
        return _verses + " is not a valid passage";
      else
        return base.Validate();
    }
  }
}
