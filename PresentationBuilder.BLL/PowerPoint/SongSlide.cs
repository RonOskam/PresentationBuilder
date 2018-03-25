using Microsoft.Office.Interop.PowerPoint;
using PresentationBuilder.BLL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PresentationBuilder.BLL.PowerPoint
{
  public class SongSlide : SlideItem
  {
    private Song _song;
    private List<int> _verses;
    private static FontItem _songFont;
    private static FontItem _indicatorFont;

    public static FontItem SongFont
    {
      get
      {
        return SongSlide._songFont;
      }
      set
      {
        SongSlide._songFont = value;
      }
    }

    public static FontItem IndicatorFont
    {
      get
      {
        return SongSlide._indicatorFont;
      }
      set
      {
        SongSlide._indicatorFont = value;
      }
    }

    public SongSlide(Song song, List<int> verses)
    {
      this._song = song;
      this._verses = verses;
    }

    internal override void Generate(Presentation presentation)
    {
      bool? nullable = new bool?();
      bool flag = !string.IsNullOrEmpty(this._song.Refrain) && (uint) this._song.Refrain.Trim().Length > 0U;
      if (this._song.IsRefrainFirst & flag)
      {
        Slide slide = this.GenerateSlide(presentation);
        this.AddMainTextbox(slide, this._song.Refrain, SongSlide._songFont);
        this.AddDescriptorTextbox(slide, "Refrain", SongSlide._indicatorFont);
      }
      foreach (int num in this._verses)
      {
        int verse = num;
        string text1 = Enumerable.FirstOrDefault<string>(Enumerable.Select<Verse, string>(Enumerable.Where<Verse>((IEnumerable<Verse>) this._song.Verses, (Func<Verse, bool>) (v => (int) v.VerseNumber == verse)), (Func<Verse, string>) (v => v.Text))).Trim();
        if (!nullable.HasValue)
          nullable = new bool?(flag && !this._song.IsRefrainFirst && !this.DoesTextFit(text1 + "\r\n" + this._song.Refrain, SongSlide._songFont));
        Slide slide1 = this.GenerateSlide(presentation);
        if (flag && !nullable.Value && !this._song.IsRefrainFirst)
          text1 = text1 + "\r\n" + this._song.Refrain.Trim();
        this.AddMainTextbox(slide1, text1, SongSlide._songFont);
        string text2;
        if (this._song.Book != null && this._song.Book.InPew)
          text2 = this._song.Book.Title + " " + this._song.Number.ToString() + ": " + verse.ToString();
        else
          text2 = "Verse " + verse.ToString();
        this.AddDescriptorTextbox(slide1, text2, SongSlide._indicatorFont);
        if (nullable.Value || this._song.IsRefrainFirst)
        {
          Slide slide2 = this.GenerateSlide(presentation);
          this.AddMainTextbox(slide2, this._song.Refrain.Trim(), SongSlide._songFont);
          this.AddDescriptorTextbox(slide2, "Refrain", SongSlide._indicatorFont);
        }
      }
    }

    public override string Validate()
    {
      if (this._song == null)
        return "A song must be selected.";
      if (this._verses.Count == 0)
        return "One or more verses must be selected.";
      return base.Validate();
    }
  }

  public enum SlideType
  {
    BlankSlide,
    Song,
    Message,
    BiblePassage,
  }
}
