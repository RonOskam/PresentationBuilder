// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.BLL.SongItem
// Assembly: PresentationBuilder.BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 3C38E67D-5DE8-463E-9D0A-ECF8F27A6106
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.BLL.dll

using System;

namespace PresentationBuilder.BLL
{
  public class SongItem
  {
    public int SongID { get; set; }

    public int Number { get; set; }

    public string Name { get; set; }

    public byte VerseCount { get; set; }

    public static SongItem GetSongItem(Song song)
    {
      return new SongItem()
      {
        SongID = song.SongID,
        Number = (int) song.Number,
        Name = song.Name,
        VerseCount = Convert.ToByte(song.Verses.Count)
      };
    }
  }
}
