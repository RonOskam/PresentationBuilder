using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PresentationBuilder.BLL
{
    public partial class Song
    {
    public bool ValidNumber()
    {
      return Queryable.FirstOrDefault<Song>(Queryable.Where<Song>(DataSource.GetSongs(), (Expression<Func<Song, bool>>)(s => s.SongID != this.SongID && s.Book.BookID == this.Book.BookID && (int)s.Number == (int)this.Number))) == null;
    }

    public void WriteToXML(XmlWriter writer)
    {
      writer.WriteAttributeString("InternetSong", "Y");
      writer.WriteAttributeString("Name", this.Name);
      writer.WriteAttributeString("Refrain", this.Refrain);
      writer.WriteAttributeString("RefrainFirst", Convert.ToString(this.IsRefrainFirst));
      writer.WriteStartElement("Verses");
      foreach (Verse verse in this.Verses)
      {
        writer.WriteStartElement("Verse");
        writer.WriteAttributeString("Number", Convert.ToString(verse.VerseNumber));
        writer.WriteAttributeString("Text", verse.Text);
        writer.WriteEndElement();
      }
    }
  }
}
