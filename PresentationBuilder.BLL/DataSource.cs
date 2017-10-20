// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.BLL.DataSource
// Assembly: PresentationBuilder.BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 3C38E67D-5DE8-463E-9D0A-ECF8F27A6106
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.BLL.dll

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
      return Enumerable.ToList<Book>((IEnumerable<Book>) Queryable.OrderBy<Book, string>((IQueryable<Book>) PresentationBuilderEntities.Context.Books, (Expression<Func<Book, string>>) (b => b.Title)));
    }

    public static List<SongItem> GetSongItems(Book book)
    {
      if (!DataSource._bookSongs.ContainsKey(book.BookID))
      {
        //ParameterExpression parameterExpression;
        //IQueryable<SongItem> queryable = Queryable.Select<Song, SongItem>((IQueryable<Song>) Queryable.OrderBy<Song, short>(Queryable.Where<Song>((IQueryable<Song>) PresentationBuilderEntities.Context.Songs, (Expression<Func<Song, bool>>) (s => s.Book.BookID == book.BookID)), (Expression<Func<Song, short>>) (s => s.Number)), Expression.Lambda<Func<Song, SongItem>>((Expression) Expression.MemberInit(Expression.New(typeof (SongItem)), (MemberBinding) Expression.Bind((MethodInfo) MethodBase.GetMethodFromHandle(__methodref (SongItem.set_SongID)), ; //unable to render the statement
        //DataSource._bookSongs.Add(book.BookID, Enumerable.ToList<SongItem>((IEnumerable<SongItem>) queryable));

        var list = PresentationBuilderEntities.Context.Songs.Where(s => s.Book.BookID == book.BookID)
            .OrderBy(s => s.Number).ToList()
            .Select(s => SongItem.GetSongItem(s)).ToList();
        _bookSongs.Add(book.BookID, list);
      }
      return DataSource._bookSongs[book.BookID];
    }

    public static void ResetBook(Book book)
    {
      if (book == null || !DataSource._bookSongs.ContainsKey(book.BookID))
        return;
      DataSource._bookSongs.Remove(book.BookID);
    }

    public static List<MessageType> GetMessageTypes()
    {
      if (DataSource._messageTypes == null)
        DataSource._messageTypes = Enumerable.ToList<MessageType>((IEnumerable<MessageType>) Queryable.OrderBy<MessageType, string>((IQueryable<MessageType>) PresentationBuilderEntities.Context.MessageTypes.Include("Messages"), (Expression<Func<MessageType, string>>) (m => m.Description)));
      return DataSource._messageTypes;
    }

    public static Song GetSong(int songId)
    {
      return DataSource.GetSong(PresentationBuilderEntities.Context, songId);
    }

    public static IQueryable<Song> GetSongs()
    {
      return (IQueryable<Song>) PresentationBuilderEntities.Context.Songs;
    }

    public static Song GetSong(PresentationBuilderEntities context, int songId)
    {
      return Queryable.FirstOrDefault<Song>(Queryable.Where<Song>((IQueryable<Song>) context.Songs.Include("Verses").Include("Book"), (Expression<Func<Song, bool>>) (s => s.SongID == songId)));
    }

    public static Book GetBookBySong(int songId)
    {
      return Queryable.FirstOrDefault<Book>(Queryable.Select<Song, Book>(Queryable.Where<Song>((IQueryable<Song>) PresentationBuilderEntities.Context.Songs, (Expression<Func<Song, bool>>) (s => s.SongID == songId)), (Expression<Func<Song, Book>>) (s => s.Book)));
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
      Song song = Queryable.FirstOrDefault<Song>((IQueryable<Song>) PresentationBuilderEntities.Context.Songs, (Expression<Func<Song, bool>>) (s => s.SongID == songId));
      DataSource._bookSongs.Remove(song.Book.BookID);
      PresentationBuilderEntities.Context.Songs.Remove(song);

        foreach (var entity in song.Verses)
          PresentationBuilderEntities.Context.Verses.Remove(entity);
     
      PresentationBuilderEntities.Context.SaveChanges();
    }
  }
}
