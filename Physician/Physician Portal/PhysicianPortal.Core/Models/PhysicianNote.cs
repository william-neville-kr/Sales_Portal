using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhysicianPortal.Core.Data
{
    //[MetadataType(typeof(PhysicianNoteMetada))]
    public partial class PhysicianNote
    {
        [Required]
        public string PatientIdEncrypted { get; set; }
        [Required]
        public string PhysicianIdEncrypted { get; set; }
    }

    //public class PhysicianNoteMetada
    //{
    //    [Required]
    //    public Nullable<int> PhysicianId { get; set; }
    //    [Required]
    //    public Nullable<int> PatientId { get; set; }
    //}
}
