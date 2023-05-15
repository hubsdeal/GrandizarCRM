using SoftGrid.CRM;
using SoftGrid.Shop;

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.Shop.Exporting;
using SoftGrid.Shop.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.Shop
{
    [AbpAuthorize(AppPermissions.Pages_ProductTeams)]
    public class ProductTeamsAppService : SoftGridAppServiceBase, IProductTeamsAppService
    {
        private readonly IRepository<ProductTeam, long> _productTeamRepository;
        private readonly IProductTeamsExcelExporter _productTeamsExcelExporter;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;

        public ProductTeamsAppService(IRepository<ProductTeam, long> productTeamRepository, IProductTeamsExcelExporter productTeamsExcelExporter, IRepository<Employee, long> lookup_employeeRepository, IRepository<Product, long> lookup_productRepository)
        {
            _productTeamRepository = productTeamRepository;
            _productTeamsExcelExporter = productTeamsExcelExporter;
            _lookup_employeeRepository = lookup_employeeRepository;
            _lookup_productRepository = lookup_productRepository;

        }

        public async Task<PagedResultDto<GetProductTeamForViewDto>> GetAll(GetAllProductTeamsInput input)
        {

            var filteredProductTeams = _productTeamRepository.GetAll()
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PrimaryFilter.HasValue && input.PrimaryFilter > -1, e => (input.PrimaryFilter == 1 && e.Primary) || (input.PrimaryFilter == 0 && !e.Primary))
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var pagedAndFilteredProductTeams = filteredProductTeams
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productTeams = from o in pagedAndFilteredProductTeams
                               join o1 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                               from s2 in j2.DefaultIfEmpty()

                               select new
                               {

                                   o.Primary,
                                   o.Active,
                                   Id = o.Id,
                                   EmployeeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                   ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                               };

            var totalCount = await filteredProductTeams.CountAsync();

            var dbList = await productTeams.ToListAsync();
            var results = new List<GetProductTeamForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductTeamForViewDto()
                {
                    ProductTeam = new ProductTeamDto
                    {

                        Primary = o.Primary,
                        Active = o.Active,
                        Id = o.Id,
                    },
                    EmployeeName = o.EmployeeName,
                    ProductName = o.ProductName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductTeamForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductTeamForViewDto> GetProductTeamForView(long id)
        {
            var productTeam = await _productTeamRepository.GetAsync(id);

            var output = new GetProductTeamForViewDto { ProductTeam = ObjectMapper.Map<ProductTeamDto>(productTeam) };

            if (output.ProductTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.ProductTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            if (output.ProductTeam.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductTeam.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductTeams_Edit)]
        public async Task<GetProductTeamForEditOutput> GetProductTeamForEdit(EntityDto<long> input)
        {
            var productTeam = await _productTeamRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductTeamForEditOutput { ProductTeam = ObjectMapper.Map<CreateOrEditProductTeamDto>(productTeam) };

            if (output.ProductTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.ProductTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            if (output.ProductTeam.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductTeam.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductTeamDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductTeams_Create)]
        protected virtual async Task Create(CreateOrEditProductTeamDto input)
        {
            var productTeam = ObjectMapper.Map<ProductTeam>(input);

            if (AbpSession.TenantId != null)
            {
                productTeam.TenantId = (int?)AbpSession.TenantId;
            }

            await _productTeamRepository.InsertAsync(productTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductTeams_Edit)]
        protected virtual async Task Update(CreateOrEditProductTeamDto input)
        {
            var productTeam = await _productTeamRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductTeams_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productTeamRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductTeamsToExcel(GetAllProductTeamsForExcelInput input)
        {

            var filteredProductTeams = _productTeamRepository.GetAll()
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PrimaryFilter.HasValue && input.PrimaryFilter > -1, e => (input.PrimaryFilter == 1 && e.Primary) || (input.PrimaryFilter == 0 && !e.Primary))
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var query = (from o in filteredProductTeams
                         join o1 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProductTeamForViewDto()
                         {
                             ProductTeam = new ProductTeamDto
                             {
                                 Primary = o.Primary,
                                 Active = o.Active,
                                 Id = o.Id
                             },
                             EmployeeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var productTeamListDtos = await query.ToListAsync();

            return _productTeamsExcelExporter.ExportToFile(productTeamListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductTeams)]
        public async Task<PagedResultDto<ProductTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductTeamEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new ProductTeamEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductTeamEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductTeams)]
        public async Task<PagedResultDto<ProductTeamProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductTeamProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductTeamProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductTeamProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}