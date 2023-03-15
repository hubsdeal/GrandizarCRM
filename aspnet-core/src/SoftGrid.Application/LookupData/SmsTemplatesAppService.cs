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
    [AbpAuthorize(AppPermissions.Pages_SmsTemplates)]
    public class SmsTemplatesAppService : SoftGridAppServiceBase, ISmsTemplatesAppService
    {
        private readonly IRepository<SmsTemplate, long> _smsTemplateRepository;
        private readonly ISmsTemplatesExcelExporter _smsTemplatesExcelExporter;

        public SmsTemplatesAppService(IRepository<SmsTemplate, long> smsTemplateRepository, ISmsTemplatesExcelExporter smsTemplatesExcelExporter)
        {
            _smsTemplateRepository = smsTemplateRepository;
            _smsTemplatesExcelExporter = smsTemplatesExcelExporter;

        }

        public async Task<PagedResultDto<GetSmsTemplateForViewDto>> GetAll(GetAllSmsTemplatesInput input)
        {

            var filteredSmsTemplates = _smsTemplateRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.Content.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title.Contains(input.TitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContentFilter), e => e.Content.Contains(input.ContentFilter))
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published));

            var pagedAndFilteredSmsTemplates = filteredSmsTemplates
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var smsTemplates = from o in pagedAndFilteredSmsTemplates
                               select new
                               {

                                   o.Title,
                                   o.Content,
                                   o.Published,
                                   Id = o.Id
                               };

            var totalCount = await filteredSmsTemplates.CountAsync();

            var dbList = await smsTemplates.ToListAsync();
            var results = new List<GetSmsTemplateForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSmsTemplateForViewDto()
                {
                    SmsTemplate = new SmsTemplateDto
                    {

                        Title = o.Title,
                        Content = o.Content,
                        Published = o.Published,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSmsTemplateForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSmsTemplateForViewDto> GetSmsTemplateForView(long id)
        {
            var smsTemplate = await _smsTemplateRepository.GetAsync(id);

            var output = new GetSmsTemplateForViewDto { SmsTemplate = ObjectMapper.Map<SmsTemplateDto>(smsTemplate) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SmsTemplates_Edit)]
        public async Task<GetSmsTemplateForEditOutput> GetSmsTemplateForEdit(EntityDto<long> input)
        {
            var smsTemplate = await _smsTemplateRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSmsTemplateForEditOutput { SmsTemplate = ObjectMapper.Map<CreateOrEditSmsTemplateDto>(smsTemplate) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSmsTemplateDto input)
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

        [AbpAuthorize(AppPermissions.Pages_SmsTemplates_Create)]
        protected virtual async Task Create(CreateOrEditSmsTemplateDto input)
        {
            var smsTemplate = ObjectMapper.Map<SmsTemplate>(input);

            if (AbpSession.TenantId != null)
            {
                smsTemplate.TenantId = (int?)AbpSession.TenantId;
            }

            await _smsTemplateRepository.InsertAsync(smsTemplate);

        }

        [AbpAuthorize(AppPermissions.Pages_SmsTemplates_Edit)]
        protected virtual async Task Update(CreateOrEditSmsTemplateDto input)
        {
            var smsTemplate = await _smsTemplateRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, smsTemplate);

        }

        [AbpAuthorize(AppPermissions.Pages_SmsTemplates_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _smsTemplateRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSmsTemplatesToExcel(GetAllSmsTemplatesForExcelInput input)
        {

            var filteredSmsTemplates = _smsTemplateRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Title.Contains(input.Filter) || e.Content.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TitleFilter), e => e.Title.Contains(input.TitleFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ContentFilter), e => e.Content.Contains(input.ContentFilter))
                        .WhereIf(input.PublishedFilter.HasValue && input.PublishedFilter > -1, e => (input.PublishedFilter == 1 && e.Published) || (input.PublishedFilter == 0 && !e.Published));

            var query = (from o in filteredSmsTemplates
                         select new GetSmsTemplateForViewDto()
                         {
                             SmsTemplate = new SmsTemplateDto
                             {
                                 Title = o.Title,
                                 Content = o.Content,
                                 Published = o.Published,
                                 Id = o.Id
                             }
                         });

            var smsTemplateListDtos = await query.ToListAsync();

            return _smsTemplatesExcelExporter.ExportToFile(smsTemplateListDtos);
        }

    }
}