using PresentationBuilder.BLL;
using System.Data.Metadata.Edm;
using System.Data.Objects.DataClasses;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: EdmSchema]
[assembly: EdmRelationship("PresentationBuilderModel", "FK_Song_Book", "Book", RelationshipMultiplicity.One, typeof (Book), "Song", RelationshipMultiplicity.Many, typeof (Song))]
[assembly: EdmRelationship("PresentationBuilderModel", "FK_Message_MessageType", "MessageType", RelationshipMultiplicity.One, typeof (MessageType), "Message", RelationshipMultiplicity.Many, typeof (Message))]
[assembly: EdmRelationship("PresentationBuilderModel", "FK_Verse_Song", "Song", RelationshipMultiplicity.One, typeof (Song), "Verse", RelationshipMultiplicity.Many, typeof (Verse))]
[assembly: AssemblyTitle("PresentationBuilder.BLL")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("PresentationBuilder.BLL")]
[assembly: AssemblyCopyright("Copyright ©  2009")]
[assembly: AssemblyTrademark("")]
[assembly: ComVisible(false)]
[assembly: Guid("39903a0e-6ba1-468f-b58f-08dfdd6e915c")]
[assembly: AssemblyFileVersion("1.0.0.0")]
[assembly: AssemblyVersion("1.0.0.0")]
