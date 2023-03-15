using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.LookupData.Exporting;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.LookupData
{
    [AbpAuthorize(AppPermissions.Pages_EmailTemplates)]
    public class EmailTemplatesAppService : SoftGridAppServiceBase, IEmailTemplatesAppService
    {
        private readonly IRepository<EmailTemplate, long> _emailTemplateRepository;
        private readonly IEmailTemplatesExcelExporter _emailTemplatesExcelExporter;

        public EmailTemplatesAppService(IRepository<EmailTemplate, long> emailTemplateRepository, IEmailTemplatesExcelExporter emailTemplatesExcelExporter)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _emailTemplatesExcelExporter = emailTemplatesExcelExporter;

        }

        public async Task<PagedResultDto<GetEmailTemplateForViewDto>> GetAll(GetAllEmailTemplatesInput input)
        {

            var filteredEmailTemplates = _emailTemplateRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Subject.Contains(input.Filter) || e.Content.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubjectFilter), e => e.Subject.Contains(input.SubjectFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContentFilter), e => e.Content.Contains(input.ContentFilter))
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published));

            var pagedAndFilteredEmailTemplates = filteredEmailTemplates
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var emailTemplates = from o in pagedAndFilteredEmailTemplates
                                 select new
                                 {

                                     o.Subject,
                                     o.Content,
                                     o.Published,
                                     Id = o.Id
                                 };

            var totalCount = await filteredEmailTemplates.CountAsync();

            var dbList = await emailTemplates.ToListAsync();
            var results = new List<GetEmailTemplateForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetEmailTemplateForViewDto()
                {
                    EmailTemplate = new EmailTemplateDto
                    {

                        Subject = o.Subject,
                        Content = o.Content,
                        Published = o.Published,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetEmailTemplateForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetEmailTemplateForViewDto> GetEmailTemplateForView(long id)
        {
            var emailTemplate = await _emailTemplateRepository.GetAsync(id);

            var output = new GetEmailTemplateForViewDto { EmailTemplate = ObjectMapper.Map<EmailTemplateDto>(emailTemplate) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_EmailTemplates_Edit)]
        public async Task<GetEmailTemplateForEditOutput> GetEmailTemplateForEdit(EntityDto<long> input)
        {
            var emailTemplate = await _emailTemplateRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetEmailTemplateForEditOutput { EmailTemplate = ObjectMapper.Map<CreateOrEditEmailTemplateDto>(emailTemplate) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditEmailTemplateDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_EmailTemplates_Create)]
        protected virtual async Task Create(CreateOrEditEmailTemplateDto input)
        {
            var emailTemplate = ObjectMapper.Map<EmailTemplate>(input);

            if (AbpSession.TenantId != null)
            {
                emailTemplate.TenantId = (int?)AbpSession.TenantId;
            }

            await _emailTemplateRepository.InsertAsync(emailTemplate);

        }

        [AbpAuthorize(AppPermissions.Pages_EmailTemplates_Edit)]
        protected virtual async Task Update(CreateOrEditEmailTemplateDto input)
        {
            var emailTemplate = await _emailTemplateRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, emailTemplate);

        }

        [AbpAuthorize(AppPermissions.Pages_EmailTemplates_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _emailTemplateRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetEmailTemplatesToExcel(GetAllEmailTemplatesForExcelInput input)
        {

            var filteredEmailTemplates = _emailTemplateRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Subject.Contains(input.Filter) || e.Content.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SubjectFilter), e => e.Subject.Contains(input.SubjectFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContentFilter), e => e.Content.Contains(input.ContentFilter))
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published));

            var query = (from o in filteredEmailTemplates
                         select new GetEmailTemplateForViewDto()
                         {
                             EmailTemplate = new EmailTemplateDto
                             {
                                 Subject = o.Subject,
                                 Content = o.Content,
                                 Published = o.Published,
                                 Id = o.Id
                             }
                         });

            var emailTemplateListDtos = await query.ToListAsync();

            return _emailTemplatesExcelExporter.ExportToFile(emailTemplateListDtos);
        }

    }
}