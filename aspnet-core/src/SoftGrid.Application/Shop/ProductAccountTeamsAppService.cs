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
    [AbpAuthorize(AppPermissions.Pages_ProductAccountTeams)]
    public class ProductAccountTeamsAppService : SoftGridAppServiceBase, IProductAccountTeamsAppService
    {
        private readonly IRepository<ProductAccountTeam, long> _productAccountTeamRepository;
        private readonly IProductAccountTeamsExcelExporter _productAccountTeamsExcelExporter;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;
        private readonly IRepository<Product, long> _lookup_productRepository;

        public ProductAccountTeamsAppService(IRepository<ProductAccountTeam, long> productAccountTeamRepository, IProductAccountTeamsExcelExporter productAccountTeamsExcelExporter, IRepository<Employee, long> lookup_employeeRepository, IRepository<Product, long> lookup_productRepository)
        {
            _productAccountTeamRepository = productAccountTeamRepository;
            _productAccountTeamsExcelExporter = productAccountTeamsExcelExporter;
            _lookup_employeeRepository = lookup_employeeRepository;
            _lookup_productRepository = lookup_productRepository;

        }

        public async Task<PagedResultDto<GetProductAccountTeamForViewDto>> GetAll(GetAllProductAccountTeamsInput input)
        {

            var filteredProductAccountTeams = _productAccountTeamRepository.GetAll()
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PrimaryFilter.HasValue && input.PrimaryFilter > -1, e => (input.PrimaryFilter == 1 && e.Primary) || (input.PrimaryFilter == 0 && !e.Primary))
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(input.MinRemoveDateFilter != null, e => e.RemoveDate >= input.MinRemoveDateFilter)
                        .WhereIf(input.MaxRemoveDateFilter != null, e => e.RemoveDate <= input.MaxRemoveDateFilter)
                        .WhereIf(input.MinAssignDateFilter != null, e => e.AssignDate >= input.MinAssignDateFilter)
                        .WhereIf(input.MaxAssignDateFilter != null, e => e.AssignDate <= input.MaxAssignDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var pagedAndFilteredProductAccountTeams = filteredProductAccountTeams
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productAccountTeams = from o in pagedAndFilteredProductAccountTeams
                                      join o1 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o1.Id into j1
                                      from s1 in j1.DefaultIfEmpty()

                                      join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                                      from s2 in j2.DefaultIfEmpty()

                                      select new
                                      {

                                          o.Primary,
                                          o.Active,
                                          o.RemoveDate,
                                          o.AssignDate,
                                          Id = o.Id,
                                          EmployeeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                          ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                      };

            var totalCount = await filteredProductAccountTeams.CountAsync();

            var dbList = await productAccountTeams.ToListAsync();
            var results = new List<GetProductAccountTeamForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductAccountTeamForViewDto()
                {
                    ProductAccountTeam = new ProductAccountTeamDto
                    {

                        Primary = o.Primary,
                        Active = o.Active,
                        RemoveDate = o.RemoveDate,
                        AssignDate = o.AssignDate,
                        Id = o.Id,
                    },
                    EmployeeName = o.EmployeeName,
                    ProductName = o.ProductName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductAccountTeamForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductAccountTeamForViewDto> GetProductAccountTeamForView(long id)
        {
            var productAccountTeam = await _productAccountTeamRepository.GetAsync(id);

            var output = new GetProductAccountTeamForViewDto { ProductAccountTeam = ObjectMapper.Map<ProductAccountTeamDto>(productAccountTeam) };

            if (output.ProductAccountTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.ProductAccountTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            if (output.ProductAccountTeam.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductAccountTeam.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductAccountTeams_Edit)]
        public async Task<GetProductAccountTeamForEditOutput> GetProductAccountTeamForEdit(EntityDto<long> input)
        {
            var productAccountTeam = await _productAccountTeamRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductAccountTeamForEditOutput { ProductAccountTeam = ObjectMapper.Map<CreateOrEditProductAccountTeamDto>(productAccountTeam) };

            if (output.ProductAccountTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.ProductAccountTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            if (output.ProductAccountTeam.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductAccountTeam.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductAccountTeamDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductAccountTeams_Create)]
        protected virtual async Task Create(CreateOrEditProductAccountTeamDto input)
        {
            var productAccountTeam = ObjectMapper.Map<ProductAccountTeam>(input);

            if (AbpSession.TenantId != null)
            {
                productAccountTeam.TenantId = (int?)AbpSession.TenantId;
            }

            await _productAccountTeamRepository.InsertAsync(productAccountTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductAccountTeams_Edit)]
        protected virtual async Task Update(CreateOrEditProductAccountTeamDto input)
        {
            var productAccountTeam = await _productAccountTeamRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productAccountTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductAccountTeams_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productAccountTeamRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductAccountTeamsToExcel(GetAllProductAccountTeamsForExcelInput input)
        {

            var filteredProductAccountTeams = _productAccountTeamRepository.GetAll()
                        .Include(e => e.EmployeeFk)
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PrimaryFilter.HasValue && input.PrimaryFilter > -1, e => (input.PrimaryFilter == 1 && e.Primary) || (input.PrimaryFilter == 0 && !e.Primary))
                        .WhereIf(input.ActiveFilter.HasValue && input.ActiveFilter > -1, e => (input.ActiveFilter == 1 && e.Active) || (input.ActiveFilter == 0 && !e.Active))
                        .WhereIf(input.MinRemoveDateFilter != null, e => e.RemoveDate >= input.MinRemoveDateFilter)
                        .WhereIf(input.MaxRemoveDateFilter != null, e => e.RemoveDate <= input.MaxRemoveDateFilter)
                        .WhereIf(input.MinAssignDateFilter != null, e => e.AssignDate >= input.MinAssignDateFilter)
                        .WhereIf(input.MaxAssignDateFilter != null, e => e.AssignDate <= input.MaxAssignDateFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var query = (from o in filteredProductAccountTeams
                         join o1 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_productRepository.GetAll() on o.ProductId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProductAccountTeamForViewDto()
                         {
                             ProductAccountTeam = new ProductAccountTeamDto
                             {
                                 Primary = o.Primary,
                                 Active = o.Active,
                                 RemoveDate = o.RemoveDate,
                                 AssignDate = o.AssignDate,
                                 Id = o.Id
                             },
                             EmployeeName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             ProductName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var productAccountTeamListDtos = await query.ToListAsync();

            return _productAccountTeamsExcelExporter.ExportToFile(productAccountTeamListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductAccountTeams)]
        public async Task<PagedResultDto<ProductAccountTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductAccountTeamEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new ProductAccountTeamEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductAccountTeamEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductAccountTeams)]
        public async Task<PagedResultDto<ProductAccountTeamProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductAccountTeamProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductAccountTeamProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductAccountTeamProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}