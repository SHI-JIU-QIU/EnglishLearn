using FileService.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain
{
    public interface IFSRepository
    {
        Task<UploadItemEntity?> FindFileAsync(string sha256Hash);
    }
}
