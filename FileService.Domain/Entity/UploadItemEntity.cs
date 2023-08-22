using Common;
using DomainEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Domain.Entity
{
    public record UploadItemEntity : AggregateRootEntity
    {
        //文件名
        public string FileName{get; set;}

        //用于对比文件内容是否相同
        public string FileSHA256Hash { get; set; }

        //备份路径
        public Uri BackUpUrl { get; set; }

        //外界访问路径
        public Uri RemoteUrl { get; set; }


        //文件大小
        public long FileSize { get; set; }

        public UploadItemEntity(Guid id, string fileName, string fileSHA256Hash, Uri backUpUrl, Uri remoteUrl, long fileSize)
        {
            id = id;
            FileName = fileName;
            FileSHA256Hash = fileSHA256Hash;
            BackUpUrl = backUpUrl;
            RemoteUrl = remoteUrl;
            FileSize = fileSize;
        }
    }
}
