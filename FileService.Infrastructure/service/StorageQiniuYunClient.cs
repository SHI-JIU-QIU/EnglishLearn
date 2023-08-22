using FileService.Domain;
using Microsoft.Extensions.Options;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Infrastructure.service
{
    public class StorageQiniuYunClient:IStoragePublicClient
    {
        private readonly IOptionsSnapshot<StorageQiniuYunClientOptions> options;

        public StorageQiniuYunClient(IOptionsSnapshot<StorageQiniuYunClientOptions> options)
        {
            this.options = options;
        }

        static string ConcatUrl(params string[] segments)
        {
            for (int i = 0; i < segments.Length; i++)
            {
                string s = segments[i];
                if (s.Contains(".."))
                {
                    throw new ArgumentException("路径中不能含有..");
                }
                segments[i] = s.Trim('/');//把开头结尾的/去掉
            }
            return string.Join('/', segments);
        }
       

        public async Task<Uri> SaveAsync(string key, Stream content, CancellationToken cancellationToken = default)
        {

            if (key.StartsWith('/'))
            {
                throw new ArgumentException("key should not start with /", nameof(key));
            }

            Mac mac = new Mac(options.Value.AccessKey, options.Value.SecretKey);
            string url = ConcatUrl(options.Value.UrlRoot, options.Value.WorkingDir, key);//web访问的文件网址

            // 存储空间名
            string Bucket = options.Value.Bucket;
            // 设置上传策略，详见：https://developer.qiniu.com/kodo/manual/1206/put-policy
            PutPolicy putPolicy = new PutPolicy();
            putPolicy.Scope = Bucket;
            putPolicy.SetExpires(3600);
            putPolicy.DeleteAfterDays = 1;
            string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
            Config config = new Config();
            // 设置上传区域
            config.Zone = Zone.ZONE_CN_South;
            // 设置 http 或者 https 上传
            config.UseHttps = true;
            config.UseCdnDomains = true;
            // 表单上传
            FormUploader target = new FormUploader(config);
            HttpResult result = target.UploadStream(content,key,token,null);
            Console.WriteLine("form upload result: " + result.ToString());

            return new Uri(url);
           
        }
    }
}
