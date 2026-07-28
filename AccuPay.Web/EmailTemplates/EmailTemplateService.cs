using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Account;
using AccuPay.Web.Core.Auth;
using AutoMapper;
using System;
using System.Threading.Tasks;

namespace AccuPay.Web.EmailTemplates
{
    public class EmailTemplateService
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        public EmailTemplateService(IEmailTemplateRepository emailTemplateRepository, IMapper mapper, ICurrentUser currentUser)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _mapper = mapper;
            _currentUser = currentUser;
        }
        public async Task<EmailTemplateDto> GetByCode(string code)
        {
            var emailTemplate =await _emailTemplateRepository.GetByCodeAsync(code, _currentUser.OrganizationId);
            return _mapper.Map<EmailTemplateDto>(emailTemplate);
        }

        public async Task<EmailTemplateDto> Update(int id, EmailTemplateDto dto)
        {
            var emailTemplate = await _emailTemplateRepository.GetByIdAsync(id);
            if (emailTemplate == null)
            {
                return null;
            }
            string formattedBody = dto.HtmlBody.Replace("\r\n", "<br />").Replace("\n", "<br />");
            var wrap = $"<div style='font-family:Segoe UI, Arial, sans-serif;'>{formattedBody}</div>";
            emailTemplate.Subject = dto.Subject;
            emailTemplate.HtmlBody = wrap;
            emailTemplate.TextBody = dto.TextBody;

            await _emailTemplateRepository.UpdateAsync(emailTemplate);

            return _mapper.Map<EmailTemplateDto>(emailTemplate);
        }
    }
}
