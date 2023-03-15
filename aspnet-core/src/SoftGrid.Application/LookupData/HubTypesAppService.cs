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
    [AbpAuthorize(AppPermissions.Pages_HubTypes)]
    public class HubTypesAppService : SoftGridAppServiceBase, IHubTypesAppService
    {
        private readonly IRepository<HubType, long> _hubTypeRepository;
        private readonly IHubTypesExcelExporter _hubTypesExcelExporter;

        public HubTypesAppService(IRepository<HubType, long> hubTypeRepository, IHubTypesExcelExporter hubTypesExcelExporter)
        {
            _hubTypeRepository = hubTypeRepository;
            _hubTypesExcelExporter = hubTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetHubTypeForViewDto>> GetAll(GetAllHubTypesInput input)
        {

            var filteredHubTypes = _hubTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredHubTypes = filteredHubTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var hubTypes = from o in pagedAndFilteredHubTypes
                           select new
                           {

                               o.Name,
                               Id = o.Id
                           };

            var totalCount = await filteredHubTypes.CountAsync();

            var dbList = await hubTypes.ToListAsync();
            var results = new List<GetHubTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetHubTypeForViewDto()
                {
                    HubType = new HubTypeDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetHubTypeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetHubTypeForViewDto> GetHubTypeForView(long id)
        {
            var hubType = await _hubTypeRepository.GetAsync(id);

            var output = new GetHubTypeForViewDto { HubType = ObjectMapper.Map<HubTypeDto>(hubType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_HubTypes_Edit)]
        public async Task<GetHubTypeForEditOutput> GetHubTypeForEdit(EntityDto<long> input)
        {
            var hubType = await _hubTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetHubTypeForEditOutput { HubType = ObjectMapper.Map<CreateOrEditHubTypeDto>(hubType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditHubTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_HubTypes_Create)]
        protected virtual async Task Create(CreateOrEditHubTypeDto input)
        {
            var hubType = ObjectMapper.Map<HubType>(input);

            if (AbpSession.TenantId != null)
            {
                hubType.TenantId = (int?)AbpSession.TenantId;
            }

            await _hubTypeRepository.InsertAsync(hubType);

        }

        [AbpAuthorize(AppPermissions.Pages_HubTypes_Edit)]
        protected virtual async Task Update(CreateOrEditHubTypeDto input)
        {
            var hubType = await _hubTypeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, hubType);

        }

        [AbpAuthorize(AppPermissions.Pages_HubTypes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _hubTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetHubTypesToExcel(GetAllHubTypesForExcelInput input)
        {

            var filteredHubTypes = _hubTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredHubTypes
                         select new GetHubTypeForViewDto()
                         {
                             HubType = new HubTypeDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var hubTypeListDtos = await query.ToListAsync();

            return _hubTypesExcelExporter.ExportToFile(hubTypeListDtos);
        }

    }
}