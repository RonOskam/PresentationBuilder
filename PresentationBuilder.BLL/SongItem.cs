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
