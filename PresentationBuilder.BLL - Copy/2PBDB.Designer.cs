// Decompiled with JetBrains decompiler
// Type: PresentationBuilder.BLL.Message
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
  [EdmEntityType(Name = "Message", NamespaceName = "PresentationBuilderModel")]
  [DataContract(IsReference = true)]
  [Serializable]
  public class Message : EntityObject
  {
    private int _MessageID;
    private string _Code;
    private string _Text;

    [EdmScalarProperty(EntityKeyProperty = true, IsNullable = false)]
    [DataMember]
    public int MessageID
    {
      get
      {
        return this._MessageID;
      }
      set
      {
        if (this._MessageID == value)
          return;
        this.ReportPropertyChanging("MessageID");
        this._MessageID = StructuralObject.SetValidValue(value, "MessageID");
        this.ReportPropertyChanged("MessageID");
      }
    }

    [EdmScalarProperty(EntityKeyProperty = false, IsNullable = false)]
    [DataMember]
    public string Code
    {
      get
      {
        return this._Code;
      }
      set
      {
        this.ReportPropertyChanging("Code");
        this._Code = StructuralObject.SetValidValue(value, false, "Code");
        this.ReportPropertyChanged("Code");
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
    [EdmRelationshipNavigationProperty("PresentationBuilderModel", "FK_Message_MessageType", "MessageType")]
    public MessageType MessageType
    {
      get
      {
        return this.RelationshipManager.GetRelatedReference<MessageType>("PresentationBuilderModel.FK_Message_MessageType", "MessageType").Value;
      }
      set
      {
        this.RelationshipManager.GetRelatedReference<MessageType>("PresentationBuilderModel.FK_Message_MessageType", "MessageType").Value = value;
      }
    }

    [Browsable(false)]
    [DataMember]
    public EntityReference<MessageType> MessageTypeReference
    {
      get
      {
        return this.RelationshipManager.GetRelatedReference<MessageType>("PresentationBuilderModel.FK_Message_MessageType", "MessageType");
      }
      set
      {
        if (value == null)
          return;
        this.RelationshipManager.InitializeRelatedReference<MessageType>("PresentationBuilderModel.FK_Message_MessageType", "MessageType", value);
      }
    }

    public static Message CreateMessage(int messageID, string code, string text)
    {
      return new Message()
      {
        MessageID = messageID,
        Code = code,
        Text = text
      };
    }
  }
}
