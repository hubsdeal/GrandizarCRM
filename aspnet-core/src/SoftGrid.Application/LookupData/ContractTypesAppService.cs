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
    [AbpAuthorize(AppPermissions.Pages_ContractTypes)]
    public class ContractTypesAppService : SoftGridAppServiceBase, IContractTypesAppService
    {
        private readonly IRepository<ContractType, long> _contractTypeRepository;
        private readonly IContractTypesExcelExporter _contractTypesExcelExporter;

        public ContractTypesAppService(IRepository<ContractType, long> contractTypeRepository, IContractTypesExcelExporter contractTypesExcelExporter)
        {
            _contractTypeRepository = contractTypeRepository;
            _contractTypesExcelExporter = contractTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetContractTypeForViewDto>> GetAll(GetAllContractTypesInput input)
        {

            var filteredContractTypes = _contractTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredContractTypes = filteredContractTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var contractTypes = from o in pagedAndFilteredContractTypes
                                select new
                                {

                                    o.Name,
                                    Id = o.Id
                                };

            var totalCount = await filteredContractTypes.CountAsync();

            var dbList = await contractTypes.ToListAsync();
            var results = new List<GetContractTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetContractTypeForViewDto()
                {
                    ContractType = new ContractTypeDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetContractTypeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetContractTypeForViewDto> GetContractTypeForView(long id)
        {
            var contractType = await _contractTypeRepository.GetAsync(id);

            var output = new GetContractTypeForViewDto { ContractType = ObjectMapper.Map<ContractTypeDto>(contractType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ContractTypes_Edit)]
        public async Task<GetContractTypeForEditOutput> GetContractTypeForEdit(EntityDto<long> input)
        {
            var contractType = await _contractTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetContractTypeForEditOutput { ContractType = ObjectMapper.Map<CreateOrEditContractTypeDto>(contractType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditContractTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ContractTypes_Create)]
        protected virtual async Task Create(CreateOrEditContractTypeDto input)
        {
            var contractType = ObjectMapper.Map<ContractType>(input);

            if (AbpSession.TenantId != null)
            {
                contractType.TenantId = (int?)AbpSession.TenantId;
            }

            await _contractTypeRepository.InsertAsync(contractType);

        }

        [AbpAuthorize(AppPermissions.Pages_ContractTypes_Edit)]
        protected virtual async Task Update(CreateOrEditContractTypeDto input)
        {
            var contractType = await _contractTypeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, contractType);

        }

        [AbpAuthorize(AppPermissions.Pages_ContractTypes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _contractTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetContractTypesToExcel(GetAllContractTypesForExcelInput input)
        {

            var filteredContractTypes = _contractTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredContractTypes
                         select new GetContractTypeForViewDto()
                         {
                             ContractType = new ContractTypeDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var contractTypeListDtos = await query.ToListAsync();

            return _contractTypesExcelExporter.ExportToFile(contractTypeListDtos);
        }

    }
}