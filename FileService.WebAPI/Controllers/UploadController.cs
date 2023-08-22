using Common;
using FileService.Domain;
using FileService.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace FileService.WebAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [UnitOfWork(typeof(FSDbContext))]
    [Authorize(Roles = "Admin")]
    public class UploadController
    {
        private readonly FSDbContext dbContext;
        private readonly FSDomainService domainService;
        private readonly IFSRepository repository;

        public UploadController(IFSRepository repository,FSDomainService domainService, FSDbContext dbContext)
        {
            this.domainService = domainService;
            this.dbContext = dbContext;
            this.repository = repository;
        }

        [HttpPost]
        public async Task<ActionResult<Uri>> Upload([FromForm] UploadRequest req, CancellationToken cancellationToken = default)
        {
            IFormFile file = req.File;
            string fileName = file.FileName;
            using Stream stream = file.OpenReadStream();
            var upItem = await domainService.UploadAsync(stream, fileName, cancellationToken);
            dbContext.Add(upItem);
            return upItem.RemoteUrl;
        }

    }
}
