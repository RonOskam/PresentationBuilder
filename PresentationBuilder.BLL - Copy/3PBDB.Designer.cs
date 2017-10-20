// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.BLL.MessageType
// Assembly: PresentationBuilder.BLL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=ed425d74cb6df699
// MVID: 3C38E67D-5DE8-463E-9D0A-ECF8F27A6106
// Assembly location: C:\oaisd_app\_Misc\Presentation Builer\EXE\PresentationBuilder.BLL.dll

using System;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace PresentationBuilder.BLL
{
  [EdmEntityType(Name = "MessageType", NamespaceName = "PresentationBuilderModel")]
  [DataContract(IsReference = true)]
  [Serializable]
  public class MessageType : EntityObject
  {
    private int _MessageTypeID;
    private string _Description;

    [EdmScalarProperty(EntityKeyProperty = true, IsNullable = false)]
    [DataMember]
    public int MessageTypeID
    {
      get
      {
        return this._MessageTypeID;
      }
      set
      {
        if (this._MessageTypeID == value)
          return;
        this.ReportPropertyChanging("MessageTypeID");
        this._MessageTypeID = StructuralObject.SetValidValue(value, "MessageTypeID");
        this.ReportPropertyChanged("MessageTypeID");
      }
    }

    [EdmScalarProperty(EntityKeyProperty = false, IsNullable = false)]
    [DataMember]
    public string Description
    {
      get
      {
        return this._Description;
      }
      set
      {
        this.ReportPropertyChanging("Description");
        this._Description = StructuralObject.SetValidValue(value, false, "Description");
        this.ReportPropertyChanged("Description");
      }
    }

    [XmlIgnore]
    [SoapIgnore]
    [DataMember]
    [EdmRelationshipNavigationProperty("PresentationBuilderModel", "FK_Message_MessageType", "Message")]
    public EntityCollection<Message> Messages
    {
      get
      {
        return this.RelationshipManager.GetRelatedCollection<Message>("PresentationBuilderModel.FK_Message_MessageType", "Message");
      }
      set
      {
        if (value == null)
          return;
        this.RelationshipManager.InitializeRelatedCollection<Message>("PresentationBuilderModel.FK_Message_MessageType", "Message", value);
      }
    }

    public static MessageType CreateMessageType(int messageTypeID, string description)
    {
      return new MessageType()
      {
        MessageTypeID = messageTypeID,
        Description = description
      };
    }
  }
}
