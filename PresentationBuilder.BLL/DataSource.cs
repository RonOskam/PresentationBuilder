using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Principal;

namespace PresentationBuilder.BLL
{

  public partial class PresentationBuilderEntities
  {
    private static PresentationBuilderEntities _context;

    public static PresentationBuilderEntities Context
    {
      get
      {
        if (_context == null)
          _context = new PresentationBuilderEntities();

        return _context;
      }
    }
  }
  public class DataSource
  {
    private static Dictionary<int, List<SongItem>> _bookSongs = new Dictionary<int, List<SongItem>>();
    private static List<MessageType> _messageTypes = null;

    public static List<Book> GetBooks()
    {
      return PresentationBuilderEntities.Context.Books.OrderBy(b => b.Title).ToList();
    }

    public static List<SongItem> GetSongItems(Book book)
    {
      if (!_bookSongs.ContainsKey(book.BookID))
      {
        var list = PresentationBuilderEntities.Context.Songs.Where(s => s.Book.BookID == book.BookID)
            .OrderBy(s => s.Number)
            .Select(s => new SongItem { SongID = s.SongID, Name = s.Name, Number = s.Number, VerseCount = s.Verses.Count() }).ToList();
        _bookSongs.Add(book.BookID, list);
      }
      return _bookSongs[book.BookID];
    }

    public static void ResetBook(Book book)
    {
      if (book == null || !_bookSongs.ContainsKey(book.BookID))
        return;
      _bookSongs.Remove(book.BookID);
    }

    public static List<MessageType> GetMessageTypes()
    {
      if (_messageTypes == null)
        _messageTypes = PresentationBuilderEntities.Context.MessageTypes.Include("Messages").OrderBy(m => m.Description).ToList();
      return _messageTypes;
    }

    public static Song GetSong(int songId)
    {
      return GetSong(PresentationBuilderEntities.Context, songId);
    }

    public static IQueryable<Song> GetSongs()
    {
      return PresentationBuilderEntities.Context.Songs;
    }

    public static Song GetSong(PresentationBuilderEntities context, int songId)
    {
      return context.Songs.Include("Verses").Include("Book").FirstOrDefault(s => s.SongID == songId);
    }

    public static Book GetBookBySong(int songId)
    {
      return PresentationBuilderEntities.Context.Songs.Where(s => s.SongID == songId).Select(s => s.Book).FirstOrDefault();
    }

    public static void AddSong(Song song)
    {
      DataSource._bookSongs.Remove(song.Book.BookID);
      song.EnteredBy = WindowsIdentity.GetCurrent().Name;
      PresentationBuilderEntities.Context.Songs.Add(song);
      PresentationBuilderEntities.Context.SaveChanges();
    }

    public static void DeleteSong(int songId)
    {
      var song = PresentationBuilderEntities.Context.Songs.FirstOrDefault(s => s.SongID == songId);
      _bookSongs.Remove(song.Book.BookID);
      PresentationBuilderEntities.Context.Songs.Remove(song);

      foreach (var entity in song.Verses)
        PresentationBuilderEntities.Context.Verses.Remove(entity);

      PresentationBuilderEntities.Context.SaveChanges();
    }
  }
}
