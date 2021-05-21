using ISpaniInnerweb.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Interfaces.Services
{
    public interface IAttachmentService
    {
        void Create(Attachment attachment);
    }
}
