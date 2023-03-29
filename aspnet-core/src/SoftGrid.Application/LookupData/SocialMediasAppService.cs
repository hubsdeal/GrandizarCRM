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
    [AbpAuthorize(AppPermissions.Pages_SocialMedias)]
    public class SocialMediasAppService : SoftGridAppServiceBase, ISocialMediasAppService
    {
        private readonly IRepository<SocialMedia, long> _socialMediaRepository;
        private readonly ISocialMediasExcelExporter _socialMediasExcelExporter;

        public SocialMediasAppService(IRepository<SocialMedia, long> socialMediaRepository, ISocialMediasExcelExporter socialMediasExcelExporter)
        {
            _socialMediaRepository = socialMediaRepository;
            _socialMediasExcelExporter = socialMediasExcelExporter;

        }

        public async Task<PagedResultDto<GetSocialMediaForViewDto>> GetAll(GetAllSocialMediasInput input)
        {

            var filteredSocialMedias = _socialMediaRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredSocialMedias = filteredSocialMedias
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var socialMedias = from o in pagedAndFilteredSocialMedias
                               select new
                               {

                                   o.Name,
                                   Id = o.Id
                               };

            var totalCount = await filteredSocialMedias.CountAsync();

            var dbList = await socialMedias.ToListAsync();
            var results = new List<GetSocialMediaForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetSocialMediaForViewDto()
                {
                    SocialMedia = new SocialMediaDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetSocialMediaForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetSocialMediaForViewDto> GetSocialMediaForView(long id)
        {
            var socialMedia = await _socialMediaRepository.GetAsync(id);

            var output = new GetSocialMediaForViewDto { SocialMedia = ObjectMapper.Map<SocialMediaDto>(socialMedia) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_SocialMedias_Edit)]
        public async Task<GetSocialMediaForEditOutput> GetSocialMediaForEdit(EntityDto<long> input)
        {
            var socialMedia = await _socialMediaRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetSocialMediaForEditOutput { SocialMedia = ObjectMapper.Map<CreateOrEditSocialMediaDto>(socialMedia) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditSocialMediaDto input)
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

        [AbpAuthorize(AppPermissions.Pages_SocialMedias_Create)]
        protected virtual async Task Create(CreateOrEditSocialMediaDto input)
        {
            var socialMedia = ObjectMapper.Map<SocialMedia>(input);

            if (AbpSession.TenantId != null)
            {
                socialMedia.TenantId = (int?)AbpSession.TenantId;
            }

            await _socialMediaRepository.InsertAsync(socialMedia);

        }

        [AbpAuthorize(AppPermissions.Pages_SocialMedias_Edit)]
        protected virtual async Task Update(CreateOrEditSocialMediaDto input)
        {
            var socialMedia = await _socialMediaRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, socialMedia);

        }

        [AbpAuthorize(AppPermissions.Pages_SocialMedias_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _socialMediaRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetSocialMediasToExcel(GetAllSocialMediasForExcelInput input)
        {

            var filteredSocialMedias = _socialMediaRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredSocialMedias
                         select new GetSocialMediaForViewDto()
                         {
                             SocialMedia = new SocialMediaDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var socialMediaListDtos = await query.ToListAsync();

            return _socialMediasExcelExporter.ExportToFile(socialMediaListDtos);
        }

    }
}