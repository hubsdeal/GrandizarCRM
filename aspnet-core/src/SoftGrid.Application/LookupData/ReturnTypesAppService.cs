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
    [AbpAuthorize(AppPermissions.Pages_ReturnTypes)]
    public class ReturnTypesAppService : SoftGridAppServiceBase, IReturnTypesAppService
    {
        private readonly IRepository<ReturnType, long> _returnTypeRepository;
        private readonly IReturnTypesExcelExporter _returnTypesExcelExporter;

        public ReturnTypesAppService(IRepository<ReturnType, long> returnTypeRepository, IReturnTypesExcelExporter returnTypesExcelExporter)
        {
            _returnTypeRepository = returnTypeRepository;
            _returnTypesExcelExporter = returnTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetReturnTypeForViewDto>> GetAll(GetAllReturnTypesInput input)
        {

            var filteredReturnTypes = _returnTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredReturnTypes = filteredReturnTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var returnTypes = from o in pagedAndFilteredReturnTypes
                              select new
                              {

                                  o.Name,
                                  Id = o.Id
                              };

            var totalCount = await filteredReturnTypes.CountAsync();

            var dbList = await returnTypes.ToListAsync();
            var results = new List<GetReturnTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetReturnTypeForViewDto()
                {
                    ReturnType = new ReturnTypeDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetReturnTypeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetReturnTypeForViewDto> GetReturnTypeForView(long id)
        {
            var returnType = await _returnTypeRepository.GetAsync(id);

            var output = new GetReturnTypeForViewDto { ReturnType = ObjectMapper.Map<ReturnTypeDto>(returnType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ReturnTypes_Edit)]
        public async Task<GetReturnTypeForEditOutput> GetReturnTypeForEdit(EntityDto<long> input)
        {
            var returnType = await _returnTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetReturnTypeForEditOutput { ReturnType = ObjectMapper.Map<CreateOrEditReturnTypeDto>(returnType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditReturnTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ReturnTypes_Create)]
        protected virtual async Task Create(CreateOrEditReturnTypeDto input)
        {
            var returnType = ObjectMapper.Map<ReturnType>(input);

            if (AbpSession.TenantId != null)
            {
                returnType.TenantId = (int?)AbpSession.TenantId;
            }

            await _returnTypeRepository.InsertAsync(returnType);

        }

        [AbpAuthorize(AppPermissions.Pages_ReturnTypes_Edit)]
        protected virtual async Task Update(CreateOrEditReturnTypeDto input)
        {
            var returnType = await _returnTypeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, returnType);

        }

        [AbpAuthorize(AppPermissions.Pages_ReturnTypes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _returnTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetReturnTypesToExcel(GetAllReturnTypesForExcelInput input)
        {

            var filteredReturnTypes = _returnTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredReturnTypes
                         select new GetReturnTypeForViewDto()
                         {
                             ReturnType = new ReturnTypeDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var returnTypeListDtos = await query.ToListAsync();

            return _returnTypesExcelExporter.ExportToFile(returnTypeListDtos);
        }

    }
}