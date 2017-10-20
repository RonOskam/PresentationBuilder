// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.BLL.PowerPoint.SlideItem
// Assembly: PresentationBuilder.BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 3C38E67D-5DE8-463E-9D0A-ECF8F27A6106
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.BLL.dll

using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using System.Drawing;
using System.Drawing.Imaging;

namespace PresentationBuilder.BLL.PowerPoint
{
  public abstract class SlideItem
  {
    internal abstract void Generate(Presentation presentation);

    protected Slide GenerateSlide(Presentation presentation)
    {
      return presentation.Slides.Add(presentation.Slides.Count + 1, PpSlideLayout.ppLayoutBlank);
    }

    protected Microsoft.Office.Interop.PowerPoint.Shape AddMainTextbox(Slide slide, string text, FontItem font)
    {
      var shape = slide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 18f, 18f, 924f, 500f);
      shape.TextFrame.AutoSize = PpAutoSize.ppAutoSizeNone;
      shape.TextFrame.TextRange.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignCenter;
      shape.TextFrame.VerticalAnchor = MsoVerticalAnchor.msoAnchorMiddle;
      shape.TextFrame.TextRange.Text = text;
      if (font != null)
        font.SetOfficeFont(shape.TextFrame.TextRange);
      return shape;
    }

    protected Microsoft.Office.Interop.PowerPoint.Shape AddDescriptorTextbox(Slide slide, string text, FontItem font)
    {
      var shape = slide.Shapes.AddTextbox(MsoTextOrientation.msoTextOrientationHorizontal, 12f, 492f, 300f, 36f);
      shape.TextFrame.AutoSize = PpAutoSize.ppAutoSizeNone;
      shape.TextFrame.TextRange.ParagraphFormat.Alignment = PpParagraphAlignment.ppAlignLeft;
      shape.TextFrame.VerticalAnchor = MsoVerticalAnchor.msoAnchorTop;
      shape.TextFrame.TextRange.Text = text;
      if (font != null)
        font.SetOfficeFont(shape.TextFrame.TextRange);
      return shape;
    }

    protected bool DoesTextFit(string text, FontItem font)
    {
      SizeF layoutArea = new SizeF(684f, 900f);
      return (double) Graphics.FromImage((Image) new Bitmap(1, 1, PixelFormat.Format32bppArgb)).MeasureString(text, font.SetFont(), layoutArea).Height < 896.0;
    }

    public virtual string Validate()
    {
      return (string) null;
    }
  }
}
