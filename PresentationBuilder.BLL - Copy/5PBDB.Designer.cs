// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.BLL.Verse
// Assembly: PresentationBuilder.BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 3C38E67D-5DE8-463E-9D0A-ECF8F27A6106
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.BLL.dll

using System;
using System.ComponentModel;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace PresentationBuilder.BLL
{
  [EdmEntityType(Name = "Verse", NamespaceName = "PresentationBuilderModel")]
  [DataContract(IsReference = true)]
  [Serializable]
  public class Verse : EntityObject
  {
    private int _VerseID;
    private byte _VerseNumber;
    private string _Text;

    [EdmScalarProperty(EntityKeyProperty = true, IsNullable = false)]
    [DataMember]
    public int VerseID
    {
      get
      {
        return this._VerseID;
      }
      set
      {
        if (this._VerseID == value)
          return;
        this.ReportPropertyChanging("VerseID");
        this._VerseID = StructuralObject.SetValidValue(value, "VerseID");
        this.ReportPropertyChanged("VerseID");
      }
    }

    [EdmScalarProperty(EntityKeyProperty = false, IsNullable = false)]
    [DataMember]
    public byte VerseNumber
    {
      get
      {
        return this._VerseNumber;
      }
      set
      {
        this.ReportPropertyChanging("VerseNumber");
        this._VerseNumber = StructuralObject.SetValidValue(value, "VerseNumber");
        this.ReportPropertyChanged("VerseNumber");
      }
    }

    [EdmScalarProperty(EntityKeyProperty = false, IsNullable = false)]
    [DataMember]
    public string Text
    {
      get
      {
        return this._Text;
      }
      set
      {
        this.ReportPropertyChanging("Text");
        this._Text = StructuralObject.SetValidValue(value, false, "Text");
        this.ReportPropertyChanged("Text");
      }
    }

    [XmlIgnore]
    [SoapIgnore]
    [DataMember]
    [EdmRelationshipNavigationProperty("PresentationBuilderModel", "FK_Verse_Song", "Song")]
    public Song Song
    {
      get
      {
        return this.RelationshipManager.GetRelatedReference<Song>("PresentationBuilderModel.FK_Verse_Song", "Song").Value;
      }
      set
      {
        this.RelationshipManager.GetRelatedReference<Song>("PresentationBuilderModel.FK_Verse_Song", "Song").Value = value;
      }
    }

    [Browsable(false)]
    [DataMember]
    public EntityReference<Song> SongReference
    {
      get
      {
        return this.RelationshipManager.GetRelatedReference<Song>("PresentationBuilderModel.FK_Verse_Song", "Song");
      }
      set
      {
        if (value == null)
          return;
        this.RelationshipManager.InitializeRelatedReference<Song>("PresentationBuilderModel.FK_Verse_Song", "Song", value);
      }
    }

    public static Verse CreateVerse(int verseID, byte verseNumber, string text)
    {
      return new Verse()
      {
        VerseID = verseID,
        VerseNumber = verseNumber,
        Text = text
      };
    }
  }
}
