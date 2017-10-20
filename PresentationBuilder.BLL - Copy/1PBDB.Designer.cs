// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.BLL.Book
// Assembly: PresentationBuilder.BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 3C38E67D-5DE8-463E-9D0A-ECF8F27A6106
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.BLL.dll

using System;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace PresentationBuilder.BLL
{
  [EdmEntityType(Name = "Book", NamespaceName = "PresentationBuilderModel")]
  [DataContract(IsReference = true)]
  [Serializable]
  public class Book : EntityObject
  {
    private int _BookID;
    private string _Title;
    private bool _InPew;

    [EdmScalarProperty(EntityKeyProperty = true, IsNullable = false)]
    [DataMember]
    public int BookID
    {
      get
      {
        return this._BookID;
      }
      set
      {
        if (this._BookID == value)
          return;
        this.ReportPropertyChanging("BookID");
        this._BookID = StructuralObject.SetValidValue(value, "BookID");
        this.ReportPropertyChanged("BookID");
      }
    }

    [EdmScalarProperty(EntityKeyProperty = false, IsNullable = false)]
    [DataMember]
    public string Title
    {
      get
      {
        return this._Title;
      }
      set
      {
        this.ReportPropertyChanging("Title");
        this._Title = StructuralObject.SetValidValue(value, false, "Title");
        this.ReportPropertyChanged("Title");
      }
    }

    [EdmScalarProperty(EntityKeyProperty = false, IsNullable = false)]
    [DataMember]
    public bool InPew
    {
      get
      {
        return this._InPew;
      }
      set
      {
        this.ReportPropertyChanging("InPew");
        this._InPew = StructuralObject.SetValidValue(value, "InPew");
        this.ReportPropertyChanged("InPew");
      }
    }

    [XmlIgnore]
    [SoapIgnore]
    [DataMember]
    [EdmRelationshipNavigationProperty("PresentationBuilderModel", "FK_Song_Book", "Song")]
    public EntityCollection<Song> Songs
    {
      get
      {
        return this.RelationshipManager.GetRelatedCollection<Song>("PresentationBuilderModel.FK_Song_Book", "Song");
      }
      set
      {
        if (value == null)
          return;
        this.RelationshipManager.InitializeRelatedCollection<Song>("PresentationBuilderModel.FK_Song_Book", "Song", value);
      }
    }

    public static Book CreateBook(int bookID, string title, bool inPew)
    {
      return new Book()
      {
        BookID = bookID,
        Title = title,
        InPew = inPew
      };
    }
  }
}
