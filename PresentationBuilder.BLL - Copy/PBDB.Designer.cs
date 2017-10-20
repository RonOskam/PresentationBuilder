// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.BLL.PresentationBuilderEntities
// Assembly: PresentationBuilder.BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 3C38E67D-5DE8-463E-9D0A-ECF8F27A6106
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.BLL.dll

using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.SqlClient;

namespace PresentationBuilder.BLL
{
  public class PresentationBuilderEntities : ObjectContext
  {
    private static PresentationBuilderEntities _context = (PresentationBuilderEntities) null;
    private ObjectSet<Book> _Books;
    private ObjectSet<Message> _Messages;
    private ObjectSet<MessageType> _MessageTypes;
    private ObjectSet<Song> _Songs;
    private ObjectSet<Verse> _Verses;

    public ObjectSet<Book> Books
    {
      get
      {
        if (this._Books == null)
          this._Books = this.CreateObjectSet<Book>("Books");
        return this._Books;
      }
    }

    public ObjectSet<Message> Messages
    {
      get
      {
        if (this._Messages == null)
          this._Messages = this.CreateObjectSet<Message>("Messages");
        return this._Messages;
      }
    }

    public ObjectSet<MessageType> MessageTypes
    {
      get
      {
        if (this._MessageTypes == null)
          this._MessageTypes = this.CreateObjectSet<MessageType>("MessageTypes");
        return this._MessageTypes;
      }
    }

    public ObjectSet<Song> Songs
    {
      get
      {
        if (this._Songs == null)
          this._Songs = this.CreateObjectSet<Song>("Songs");
        return this._Songs;
      }
    }

    public ObjectSet<Verse> Verses
    {
      get
      {
        if (this._Verses == null)
          this._Verses = this.CreateObjectSet<Verse>("Verses");
        return this._Verses;
      }
    }

    public static PresentationBuilderEntities Context
    {
      get
      {
        if (PresentationBuilderEntities._context == null)
          PresentationBuilderEntities._context = new PresentationBuilderEntities(PresentationBuilderEntities.GetConnectionString());
        return PresentationBuilderEntities._context;
      }
    }

    public PresentationBuilderEntities()
      : base("name=PresentationBuilderEntities", "PresentationBuilderEntities")
    {
    }

    public PresentationBuilderEntities(string connectionString)
      : base(connectionString, "PresentationBuilderEntities")
    {
    }

    public PresentationBuilderEntities(EntityConnection connection)
      : base(connection, "PresentationBuilderEntities")
    {
    }

    public void AddToBooks(Book book)
    {
      this.AddObject("Books", (object) book);
    }

    public void AddToMessages(Message message)
    {
      this.AddObject("Messages", (object) message);
    }

    public void AddToMessageTypes(MessageType messageType)
    {
      this.AddObject("MessageTypes", (object) messageType);
    }

    public void AddToSongs(Song song)
    {
      this.AddObject("Songs", (object) song);
    }

    public void AddToVerses(Verse verse)
    {
      this.AddObject("Verses", (object) verse);
    }

    public static PresentationBuilderEntities GetNewContext()
    {
      return new PresentationBuilderEntities(PresentationBuilderEntities.GetConnectionString());
    }

    public static string GetConnectionString()
    {
      string str1 = "System.Data.SqlClient";
      string str2 = "PresentationBuilder";
      SqlConnectionStringBuilder connectionStringBuilder = new SqlConnectionStringBuilder();
      string str3 = "extranet.pectechnologies.com,52000";
      connectionStringBuilder.UserID = "BuilderUser";
      connectionStringBuilder.Password = "pbdb";
      connectionStringBuilder.DataSource = str3;
      connectionStringBuilder.InitialCatalog = str2;
      string str4 = connectionStringBuilder.ToString();
      return new EntityConnectionStringBuilder()
      {
        Provider = str1,
        ProviderConnectionString = str4,
        Metadata = "res://*/PBDB.csdl|res://*/PBDB.ssdl|res://*/PBDB.msl"
      }.ToString();
    }
  }
}
