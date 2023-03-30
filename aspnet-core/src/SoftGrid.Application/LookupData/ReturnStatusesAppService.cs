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
    [AbpAuthorize(AppPermissions.Pages_ReturnStatuses)]
    public class ReturnStatusesAppService : SoftGridAppServiceBase, IReturnStatusesAppService
    {
        private readonly IRepository<ReturnStatus, long> _returnStatusRepository;
        private readonly IReturnStatusesExcelExporter _returnStatusesExcelExporter;

        public ReturnStatusesAppService(IRepository<ReturnStatus, long> returnStatusRepository, IReturnStatusesExcelExporter returnStatusesExcelExporter)
        {
            _returnStatusRepository = returnStatusRepository;
            _returnStatusesExcelExporter = returnStatusesExcelExporter;

        }

        public async Task<PagedResultDto<GetReturnStatusForViewDto>> GetAll(GetAllReturnStatusesInput input)
        {

            var filteredReturnStatuses = _returnStatusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredReturnStatuses = filteredReturnStatuses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var returnStatuses = from o in pagedAndFilteredReturnStatuses
                                 select new
                                 {

                                     o.Name,
                                     Id = o.Id
                                 };

            var totalCount = await filteredReturnStatuses.CountAsync();

            var dbList = await returnStatuses.ToListAsync();
            var results = new List<GetReturnStatusForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetReturnStatusForViewDto()
                {
                    ReturnStatus = new ReturnStatusDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetReturnStatusForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetReturnStatusForViewDto> GetReturnStatusForView(long id)
        {
            var returnStatus = await _returnStatusRepository.GetAsync(id);

            var output = new GetReturnStatusForViewDto { ReturnStatus = ObjectMapper.Map<ReturnStatusDto>(returnStatus) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ReturnStatuses_Edit)]
        public async Task<GetReturnStatusForEditOutput> GetReturnStatusForEdit(EntityDto<long> input)
        {
            var returnStatus = await _returnStatusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetReturnStatusForEditOutput { ReturnStatus = ObjectMapper.Map<CreateOrEditReturnStatusDto>(returnStatus) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditReturnStatusDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ReturnStatuses_Create)]
        protected virtual async Task Create(CreateOrEditReturnStatusDto input)
        {
            var returnStatus = ObjectMapper.Map<ReturnStatus>(input);

            if (AbpSession.TenantId != null)
            {
                returnStatus.TenantId = (int?)AbpSession.TenantId;
            }

            await _returnStatusRepository.InsertAsync(returnStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_ReturnStatuses_Edit)]
        protected virtual async Task Update(CreateOrEditReturnStatusDto input)
        {
            var returnStatus = await _returnStatusRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, returnStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_ReturnStatuses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _returnStatusRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetReturnStatusesToExcel(GetAllReturnStatusesForExcelInput input)
        {

            var filteredReturnStatuses = _returnStatusRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredReturnStatuses
                         select new GetReturnStatusForViewDto()
                         {
                             ReturnStatus = new ReturnStatusDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var returnStatusListDtos = await query.ToListAsync();

            return _returnStatusesExcelExporter.ExportToFile(returnStatusListDtos);
        }

    }
}