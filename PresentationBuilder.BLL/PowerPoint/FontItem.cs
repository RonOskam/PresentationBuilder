using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using PEC.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Xml;

namespace PresentationBuilder.BLL.PowerPoint
{
  public class FontItem
  {
    public string FontName { get; set; }

    public float Size { get; set; }

    public bool Bold { get; set; }

    public bool Italic { get; set; }

    public bool Underline { get; set; }

    public bool Shadow { get; set; }

    public Color FontColor { get; set; }

    public float LineSpacing { get; set; }

    public void SetOfficeFont(TextRange fontRange)
    {
      fontRange.Font.Name = this.FontName;
      fontRange.Font.Size = this.Size;
      fontRange.Font.Bold = !this.Bold ? MsoTriState.msoFalse : MsoTriState.msoTrue;
      fontRange.Font.Italic = !this.Italic ? MsoTriState.msoFalse : MsoTriState.msoTrue;
      fontRange.Font.Underline = !this.Underline ? MsoTriState.msoFalse : MsoTriState.msoTrue;
      fontRange.Font.Shadow = !this.Shadow ? MsoTriState.msoFalse : MsoTriState.msoTrue;
      fontRange.Font.Color.RGB  = ColorTranslator.ToOle(FontColor);
      fontRange.ParagraphFormat.SpaceWithin = this.LineSpacing;
    }

    public System.Drawing.Font SetFont()
    {
      FontFamily family = Enumerable.FirstOrDefault<FontFamily>((IEnumerable<FontFamily>) FontFamily.Families, (Func<FontFamily, bool>) (f => f.Name == this.FontName));
      FontStyle style = !this.Bold || !this.Italic ? (!this.Bold ? (!this.Italic ? FontStyle.Regular : FontStyle.Italic) : FontStyle.Bold) : FontStyle.Bold | FontStyle.Italic;
      if (this.Underline)
        style |= FontStyle.Underline;
      return new System.Drawing.Font(family, this.Size, style);
    }

    public void SaveFont(string name)
    {
      ConfigurationManager.LocalSettingsProfile.SetValue(name, "FontName", FontName);
      ConfigurationManager.LocalSettingsProfile.SetValue(name, "Bold", Bold );
      ConfigurationManager.LocalSettingsProfile.SetValue(name, "Color", FontColor.Name);
      ConfigurationManager.LocalSettingsProfile.SetValue(name, "Italic", Italic );
      ConfigurationManager.LocalSettingsProfile.SetValue(name, "Shadow", Shadow);
      ConfigurationManager.LocalSettingsProfile.SetValue(name, "Size", Size);
      ConfigurationManager.LocalSettingsProfile.SetValue(name, "Underline", Underline );
      ConfigurationManager.LocalSettingsProfile.SetValue(name, "LineSpacing", LineSpacing);
    }

    public void LoadFont(string name)
    {
      this.FontName = ConfigurationManager.LocalSettingsProfile.GetValue(name, "FontName", "Arial");
      this.Bold = ConfigurationManager.LocalSettingsProfile.GetValue(name, "Bold", true);
      this.FontColor = Color.FromName(ConfigurationManager.LocalSettingsProfile.GetValue(name, "Color", "Black"));
      this.Italic = ConfigurationManager.LocalSettingsProfile.GetValue(name, "Italic", false);
      this.Shadow = ConfigurationManager.LocalSettingsProfile.GetValue(name, "Shadow", false);
      this.Size = (float) ConfigurationManager.LocalSettingsProfile.GetValue(name, "Size", 32);
      this.Underline = ConfigurationManager.LocalSettingsProfile.GetValue(name, "Underline", false);
      this.LineSpacing = Convert.ToSingle(ConfigurationManager.LocalSettingsProfile.GetValue(name, "LineSpacing", 1.0));
    }

    public void WriteToXML(XmlWriter writer, string name)
    {
      writer.WriteStartElement("Font");
      writer.WriteAttributeString("Name", name);
      writer.WriteAttributeString("FontName", this.FontName);
      writer.WriteAttributeString("Bold", Convert.ToString(this.Bold));
      writer.WriteAttributeString("Color", this.FontColor.Name);
      writer.WriteAttributeString("Italic", Convert.ToString(this.Italic));
      writer.WriteAttributeString("Shadow", Convert.ToString(this.Shadow));
      writer.WriteAttributeString("Underline", Convert.ToString(this.Underline));
      writer.WriteAttributeString("LineSpacing", Convert.ToString(this.LineSpacing));
      writer.WriteEndElement();
    }

    public void ReadFromXML(XmlReader reader)
    {
      while (reader.MoveToNextAttribute())
      {
        if (reader.Name == "FontName")
          this.FontName = reader.Value;
        else if (reader.Name == "Bold")
          this.Bold = Convert.ToBoolean(reader.Value);
        else if (reader.Name == "Color")
          this.FontColor = Color.FromName(reader.Value);
        else if (reader.Name == "Italic")
          this.Italic = Convert.ToBoolean(reader.Value);
        else if (reader.Name == "Shadow")
          this.Shadow = Convert.ToBoolean(reader.Value);
        else if (reader.Name == "Size")
          this.Size = Convert.ToSingle(reader.Value);
        else if (reader.Name == "Underline")
          this.Underline = Convert.ToBoolean(reader.Value);
        else if (reader.Name == "LineSpacing")
          this.LineSpacing = Convert.ToSingle(reader.Value);
      }
    }
  }
}
