using BeeKeeperRegister.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeeKeeperRegister.Services.Interfaces
{
    public interface IBeeFarmProfileAttachmentService
    {
        Task<bool> AddBeeFarmProfileAttachmentAsync(string locationId, string attachmentCode,
            string attachmentName, FileResult fileResult);
    }
}
