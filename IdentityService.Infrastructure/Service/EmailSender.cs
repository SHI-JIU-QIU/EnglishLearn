using IdentityService.Domain;
using IdentityService.Infrastructure.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FluentEmail.Core;
using System.Net.Mail;
using FluentEmail.Smtp;

namespace IdentityService.Infrastructure.Service
{
    public class EmailSender:IEmailSender
    {
        private readonly ILogger<EmailSender> logger;
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IOptionsSnapshot<EmailSenderSettings> emailSettings;

        public EmailSender(ILogger<EmailSender> logger,
            IHttpClientFactory httpClientFactory,
            IOptionsSnapshot<EmailSenderSettings> emailSettings)
        {
            this.logger = logger;
            this.httpClientFactory = httpClientFactory;
            this.emailSettings = emailSettings;
        }
        public async Task SendAsync(string toEmail, string subject, string body)
        {
            //如果使用smtp服务发送邮件必须要设置smtp服务信息
            SmtpClient smtp = new SmtpClient
            {
                //smtp服务器地址(我这里以126邮箱为例，可以依据具体你使用的邮箱设置)
                Host = emailSettings.Value.Host,
                UseDefaultCredentials = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                //这里输入你在发送smtp服务器的用户名和密码
                Credentials = new NetworkCredential(emailSettings.Value.FromEmail, emailSettings.Value.Password)
            };
            //设置默认发送信息
            Email.DefaultSender = new SmtpSender(smtp);
            var email = Email
                //发送人
                .From(emailSettings.Value.FromEmail)
                //收件人
                .To(toEmail)
                //邮件标题
                .Subject(subject)
                //邮件内容
                .Body(body);
            //依据发送结果判断是否发送成功
            var result = email.Send();
            //或使用异步的方式发送
            //await email.SendAsync();
            if (result.Successful)
            {
                //发送成功逻辑
            }
            else
            {
                //发送失败可以通过result.ErrorMessages查看失败原因
            }
        }

     
    }
}

