//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PresentationBuilder.BLL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Song
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Song()
        {
            this.Verses = new HashSet<Verse>();
        }
    
        public int SongID { get; set; }
        public int BookID { get; set; }
        public short Number { get; set; }
        public string Name { get; set; }
        public string Refrain { get; set; }
        public string Comments { get; set; }
        public bool IsRefrainFirst { get; set; }
        public string EnteredBy { get; set; }
    
        public virtual Book Book { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Verse> Verses { get; set; }
    }
}
