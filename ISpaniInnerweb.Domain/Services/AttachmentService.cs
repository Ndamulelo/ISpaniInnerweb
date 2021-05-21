using ISpaniInnerweb.Domain.Entities;
using ISpaniInnerweb.Domain.Interfaces.Repositories;
using ISpaniInnerweb.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ISpaniInnerweb.Domain.Services
{
    public class AttachmentService :IAttachmentService
    {
        private readonly IRepository<Attachment> attachmentRepository;
        public AttachmentService(IRepository<Attachment> attachmentRepository)
        {
            this.attachmentRepository = attachmentRepository;
        }
        public void Create(Attachment attachment)
        {
            var attachmentToCheck = attachmentRepository.
                        FindByConditionAsNoTracking(a => a.JobSeekerId.Equals(attachment.JobSeekerId) &&
                        a.AttachmentName.Equals(attachment.AttachmentType)).FirstOrDefault();

            if (attachmentToCheck != null)
            {
                //Update instead
                attachmentRepository.Update(attachmentToCheck);
            }
            else
            {
                attachmentRepository.Insert(attachment);
            }
        }
    }
}