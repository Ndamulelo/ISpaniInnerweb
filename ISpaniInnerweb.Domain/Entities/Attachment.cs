using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ISpaniInnerweb.Domain.Entities
{
   // [Table("attachment")]
    public class Attachment : HustlersEntity
    {
        public string JobSeekerId { set; get; }
        public string AttachmentName { get; set; }
        public string AttachmentUrl { get; set; }
        public string AttachmentType { get; set; }
    }
}
