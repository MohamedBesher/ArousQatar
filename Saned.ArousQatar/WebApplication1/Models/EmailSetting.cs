//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class EmailSetting
    {
        public int Id { get; set; }
        public string Host { get; set; }
        public string FromEmail { get; set; }
        public string Password { get; set; }
        public string SubjectAr { get; set; }
        public string SubjectEn { get; set; }
        public string MessageBodyAr { get; set; }
        public string MessageBodyEn { get; set; }
        public string EmailSettingType { get; set; }
    }
}