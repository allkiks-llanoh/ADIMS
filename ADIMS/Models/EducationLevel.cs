using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace ADIMS.Models
{
   
    public enum EducationLevel
    {
        [Description("Primary School")]
        Primary = 1,
        [Description("Secondary School")]
        Secondary = 2,
        [Description("Certificate")]
        Certificate = 3,
        [Description("Diploma")]
        Diploma = 4,
        [Description("Degree")]
        Degree = 5,
        [Description("None")]
        None = 5
    }
}