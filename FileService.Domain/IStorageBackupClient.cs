using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain
{
    public interface IStorageBackupClient
    {
        Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken = default);
    }
}
