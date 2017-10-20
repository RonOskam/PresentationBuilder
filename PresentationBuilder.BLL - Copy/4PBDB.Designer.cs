// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.BLL.Song
// Assembly: PresentationBuilder.BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 3C38E67D-5DE8-463E-9D0A-ECF8F27A6106
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.BLL.dll

using System;
using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Xml;
using System.Xml.Serialization;

namespace PresentationBuilder.BLL
{
  [EdmEntityType(Name = "Song", NamespaceName = "PresentationBuilderModel")]
  [DataContract(IsReference = true)]
  [Serializable]
  public class Song : EntityObject
  {
    private int _SongID;
    private short _Number;
    private string _Name;
    private string _Refrain;
    private string _Comments;
    private bool _IsRefrainFirst;
    private string _EnteredBy;

    [EdmScalarProperty(EntityKeyProperty = true, IsNullable = false)]
    [DataMember]
    public int SongID
    {
      get
      {
        return this._SongID;
      }
      set
      {
        if (this._SongID == value)
          return;
        this.ReportPropertyChanging("SongID");
        this._SongID = StructuralObject.SetValidValue(value, "SongID");
        this.ReportPropertyChanged("SongID");
      }
    }

    [EdmScalarProperty(EntityKeyProperty = false, IsNullable = false)]
    [DataMember]
    public short Number
    {
      get
      {
        return this._Number;
      }
      set
      {
        this.ReportPropertyChanging("Number");
        this._Number = StructuralObject.SetValidValue(value, "Number");
        this.ReportPropertyChanged("Number");
      }
    }

    [EdmScalarProperty(EntityKeyProperty = false, IsNullable = false)]
    [DataMember]
    public string Name
    {
      get
      {
        return this._Name;
      }
      set
      {
        this.ReportPropertyChanging("Name");
        this._Name = StructuralObject.SetValidValue(value, false, "Name");
        this.ReportPropertyChanged("Name");
      }
    }

    [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
    [DataMember]
    public string Refrain
    {
      get
      {
        return this._Refrain;
      }
      set
      {
        this.ReportPropertyChanging("Refrain");
        this._Refrain = StructuralObject.SetValidValue(value, true, "Refrain");
        this.ReportPropertyChanged("Refrain");
      }
    }

    [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
    [DataMember]
    public string Comments
    {
      get
      {
        return this._Comments;
      }
      set
      {
        this.ReportPropertyChanging("Comments");
        this._Comments = StructuralObject.SetValidValue(value, true, "Comments");
        this.ReportPropertyChanged("Comments");
      }
    }

    [EdmScalarProperty(EntityKeyProperty = false, IsNullable = false)]
    [DataMember]
    public bool IsRefrainFirst
    {
      get
      {
        return this._IsRefrainFirst;
      }
      set
      {
        this.ReportPropertyChanging("IsRefrainFirst");
        this._IsRefrainFirst = StructuralObject.SetValidValue(value, "IsRefrainFirst");
        this.ReportPropertyChanged("IsRefrainFirst");
      }
    }

    [EdmScalarProperty(EntityKeyProperty = false, IsNullable = true)]
    [DataMember]
    public string EnteredBy
    {
      get
      {
        return this._EnteredBy;
      }
      set
      {
        this.ReportPropertyChanging("EnteredBy");
        this._EnteredBy = StructuralObject.SetValidValue(value, true, "EnteredBy");
        this.ReportPropertyChanged("EnteredBy");
      }
    }

    [XmlIgnore]
    [SoapIgnore]
    [DataMember]
    [EdmRelationshipNavigationProperty("PresentationBuilderModel", "FK_Song_Book", "Book")]
    public Book Book
    {
      get
      {
        return this.RelationshipManager.GetRelatedReference<Book>("PresentationBuilderModel.FK_Song_Book", "Book").Value;
      }
      set
      {
        this.RelationshipManager.GetRelatedReference<Book>("PresentationBuilderModel.FK_Song_Book", "Book").Value = value;
      }
    }

    [Browsable(false)]
    [DataMember]
    public EntityReference<Book> BookReference
    {
      get
      {
        return this.RelationshipManager.GetRelatedReference<Book>("PresentationBuilderModel.FK_Song_Book", "Book");
      }
      set
      {
        if (value == null)
          return;
        this.RelationshipManager.InitializeRelatedReference<Book>("PresentationBuilderModel.FK_Song_Book", "Book", value);
      }
    }

    [XmlIgnore]
    [SoapIgnore]
    [DataMember]
    [EdmRelationshipNavigationProperty("PresentationBuilderModel", "FK_Verse_Song", "Verse")]
    public EntityCollection<Verse> Verses
    {
      get
      {
        return this.RelationshipManager.GetRelatedCollection<Verse>("PresentationBuilderModel.FK_Verse_Song", "Verse");
      }
      set
      {
        if (value == null)
          return;
        this.RelationshipManager.InitializeRelatedCollection<Verse>("PresentationBuilderModel.FK_Verse_Song", "Verse", value);
      }
    }

    public static Song CreateSong(int songID, short number, string name, bool isRefrainFirst)
    {
      return new Song()
      {
        SongID = songID,
        Number = number,
        Name = name,
        IsRefrainFirst = isRefrainFirst
      };
    }

    public bool ValidNumber()
    {
      return Queryable.FirstOrDefault<Song>(Queryable.Where<Song>(DataSource.GetSongs(), (Expression<Func<Song, bool>>) (s => s.SongID != this.SongID && s.Book.BookID == this.Book.BookID && (int) s.Number == (int) this.Number))) == null;
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
