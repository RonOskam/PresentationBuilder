using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using System.Collections.Generic;

namespace PresentationBuilder.BLL.PowerPoint
{
  public class SlidePresentation
  {
    private List<SlideItem> _slides = new List<SlideItem>();

    public FontItem SongFont
    {
      get
      {
        return SongSlide.SongFont;
      }
      set
      {
        SongSlide.SongFont = value;
      }
    }

    public FontItem IndicatorFont
    {
      get
      {
        return SongSlide.IndicatorFont;
      }
      set
      {
        SongSlide.IndicatorFont = value;
      }
    }

    public FontItem MessageFont
    {
      get
      {
        return MessageSlide.MessageFont;
      }
      set
      {
        MessageSlide.MessageFont = value;
      }
    }

    public void AddSlide(SlideItem slide)
    {
      this._slides.Add(slide);
    }

    public void Generate()
    {
      var applicationClass = new ApplicationClass();
      Presentation presentation = applicationClass.Presentations.Add(MsoTriState.msoTrue);
      foreach (SlideItem slideItem in this._slides)
        slideItem.Generate(presentation);
      applicationClass.Visible = MsoTriState.msoTrue;
    }
  }
}
