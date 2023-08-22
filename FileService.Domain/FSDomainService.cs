using Common;
using FileService.Domain.Entity;
using Microsoft.Extensions.Configuration.UserSecrets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain
{
    public class FSDomainService
    {
        private readonly IFSRepository repository;
        private readonly IStoragePublicClient storagePublicClient;
        private readonly IStorageBackupClient storageBackupClient;

        public FSDomainService(IFSRepository repository, IStoragePublicClient storagePublicClient, IStorageBackupClient storageBackupClient)
        {
            this.repository = repository;
            this.storagePublicClient = storagePublicClient;
            this.storageBackupClient = storageBackupClient;
        }

        
        public async Task<UploadItemEntity> UploadAsync(Stream stream, string fileName,
            CancellationToken cancellationToken)
        {
            string hash = HashHelper.ComputeSha256Hash(stream);
            long fileSize = stream.Length;
            DateTime today = DateTime.Today;
     
            string key = $"{today.Year}/{today.Month}/{today.Day}/{hash}/{fileName}";

           
            var oldUploadItem = await repository.FindFileAsync(hash);
            if (oldUploadItem != null)
            {
                return oldUploadItem;
            }
            stream.Position = 0;  
            Uri backupUrl = await storageBackupClient.SaveAsync(key, stream, cancellationToken);//保存到本地备份
            stream.Position = 0;
            Uri remoteUrl = await storagePublicClient.SaveAsync(key, stream, cancellationToken);//保存到生产的存储系统
         
            
            Guid id = Guid.NewGuid();
  
            return new UploadItemEntity(id, fileName, hash, backupUrl, remoteUrl,fileSize);
        }




    }
}
