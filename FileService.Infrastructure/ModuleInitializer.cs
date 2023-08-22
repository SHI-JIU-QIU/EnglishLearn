using FileService.Domain;
using FileService.Infrastructure.service;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileService.Infrastructure
{
    public class ModuleInitializer
    {
       
            public static void Initialize(IServiceCollection services)
            {
                services.AddHttpContextAccessor();
                services.AddScoped<IStorageBackupClient, StorageSMBClient>();
                services.AddScoped<IStoragePublicClient, StorageQiniuYunClient>();
                services.AddScoped<IFSRepository, FSRepository>();
                services.AddScoped<FSDomainService>();
                services.AddHttpClient();
            }
    
    }
}
