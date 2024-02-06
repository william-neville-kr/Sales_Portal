using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PhysicianPortal.Web.Models
{
    public class PatientPictureModel
    {
        public int PictureId { get; set; }
        public int PatientId { get; set; }
        public string PicturePath { get; set; }
        public string PictureName { get; set; }

    }
}