using FluentValidation;

namespace FileService.WebAPI.Controllers
{
    public class UploadRequest
    {
        public IFormFile File { get; set; }
    }


    public class UploadRequestValidator: AbstractValidator<UploadRequest>
    {
        public UploadRequestValidator() {
            RuleFor(x => x.File).NotEmpty().WithMessage("文件不能为空").Must(f => f != null && f.Length > 0 && f.Length <= 1024 * 1024 * 50).WithMessage("文件大小应在0-50MB");
        }
    }



}
