// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.BLL.PowerPoint.MessageSlide
// Assembly: PresentationBuilder.BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 3C38E67D-5DE8-463E-9D0A-ECF8F27A6106
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.BLL.dll

using Microsoft.Office.Interop.PowerPoint;

namespace PresentationBuilder.BLL.PowerPoint
{
  public class MessageSlide : SlideItem
  {
    private string _message;
    private static FontItem _messageFont;

    public static FontItem MessageFont
    {
      get
      {
        return MessageSlide._messageFont;
      }
      set
      {
        MessageSlide._messageFont = value;
      }
    }

    public MessageSlide(string message)
    {
      this._message = message;
    }

    internal override void Generate(Presentation presentation)
    {
      this.AddMainTextbox(this.GenerateSlide(presentation), this._message, MessageSlide._messageFont);
    }

    public override string Validate()
    {
      if (string.IsNullOrEmpty(this._message))
        return "A message must be selected.";
      return base.Validate();
    }
  }
}
