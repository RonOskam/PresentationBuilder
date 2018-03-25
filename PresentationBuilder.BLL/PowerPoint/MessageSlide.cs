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
