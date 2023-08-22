using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Infrastructure.service
{
    public class StorageQiniuYunClientOptions
    {
        public string Bucket{ get; set; }

        public string AccessKey { get; set; }

        public string SecretKey { get; set; }

        public string UrlRoot { get; set; }

        public string WorkingDir { get; set; }

    }
}
