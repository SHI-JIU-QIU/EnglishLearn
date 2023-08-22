using FileService.Domain;
using FileService.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Infrastructure
{
    public class FSRepository : IFSRepository
    {
        private readonly FSDbContext db;

        public FSRepository(FSDbContext db)
        {
            this.db = db;
        }

        public Task<UploadItemEntity?> FindFileAsync(string sha256Hash)
        {
            return db.UploadItemEntities.FirstOrDefaultAsync(e=>e.FileSHA256Hash == sha256Hash);
        }
    }
}
