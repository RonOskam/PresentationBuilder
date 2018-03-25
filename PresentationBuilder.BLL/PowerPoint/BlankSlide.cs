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
