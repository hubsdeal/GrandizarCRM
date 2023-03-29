using SoftGrid.Shop;
using SoftGrid.CRM;

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
    [AbpAuthorize(AppPermissions.Pages_ProductCategoryTeams)]
    public class ProductCategoryTeamsAppService : SoftGridAppServiceBase, IProductCategoryTeamsAppService
    {
        private readonly IRepository<ProductCategoryTeam, long> _productCategoryTeamRepository;
        private readonly IProductCategoryTeamsExcelExporter _productCategoryTeamsExcelExporter;
        private readonly IRepository<ProductCategory, long> _lookup_productCategoryRepository;
        private readonly IRepository<Employee, long> _lookup_employeeRepository;

        public ProductCategoryTeamsAppService(IRepository<ProductCategoryTeam, long> productCategoryTeamRepository, IProductCategoryTeamsExcelExporter productCategoryTeamsExcelExporter, IRepository<ProductCategory, long> lookup_productCategoryRepository, IRepository<Employee, long> lookup_employeeRepository)
        {
            _productCategoryTeamRepository = productCategoryTeamRepository;
            _productCategoryTeamsExcelExporter = productCategoryTeamsExcelExporter;
            _lookup_productCategoryRepository = lookup_productCategoryRepository;
            _lookup_employeeRepository = lookup_employeeRepository;

        }

        public async Task<PagedResultDto<GetProductCategoryTeamForViewDto>> GetAll(GetAllProductCategoryTeamsInput input)
        {

            var filteredProductCategoryTeams = _productCategoryTeamRepository.GetAll()
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PrimaryFilter.HasValue && input.PrimaryFilter > -1, e => (input.PrimaryFilter == 1 && e.Primary) || (input.PrimaryFilter == 0 && !e.Primary))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var pagedAndFilteredProductCategoryTeams = filteredProductCategoryTeams
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productCategoryTeams = from o in pagedAndFilteredProductCategoryTeams
                                       join o1 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o1.Id into j1
                                       from s1 in j1.DefaultIfEmpty()

                                       join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                                       from s2 in j2.DefaultIfEmpty()

                                       select new
                                       {

                                           o.Primary,
                                           Id = o.Id,
                                           ProductCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                           EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                                       };

            var totalCount = await filteredProductCategoryTeams.CountAsync();

            var dbList = await productCategoryTeams.ToListAsync();
            var results = new List<GetProductCategoryTeamForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductCategoryTeamForViewDto()
                {
                    ProductCategoryTeam = new ProductCategoryTeamDto
                    {

                        Primary = o.Primary,
                        Id = o.Id,
                    },
                    ProductCategoryName = o.ProductCategoryName,
                    EmployeeName = o.EmployeeName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductCategoryTeamForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductCategoryTeamForViewDto> GetProductCategoryTeamForView(long id)
        {
            var productCategoryTeam = await _productCategoryTeamRepository.GetAsync(id);

            var output = new GetProductCategoryTeamForViewDto { ProductCategoryTeam = ObjectMapper.Map<ProductCategoryTeamDto>(productCategoryTeam) };

            if (output.ProductCategoryTeam.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.ProductCategoryTeam.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.ProductCategoryTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.ProductCategoryTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryTeams_Edit)]
        public async Task<GetProductCategoryTeamForEditOutput> GetProductCategoryTeamForEdit(EntityDto<long> input)
        {
            var productCategoryTeam = await _productCategoryTeamRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductCategoryTeamForEditOutput { ProductCategoryTeam = ObjectMapper.Map<CreateOrEditProductCategoryTeamDto>(productCategoryTeam) };

            if (output.ProductCategoryTeam.ProductCategoryId != null)
            {
                var _lookupProductCategory = await _lookup_productCategoryRepository.FirstOrDefaultAsync((long)output.ProductCategoryTeam.ProductCategoryId);
                output.ProductCategoryName = _lookupProductCategory?.Name?.ToString();
            }

            if (output.ProductCategoryTeam.EmployeeId != null)
            {
                var _lookupEmployee = await _lookup_employeeRepository.FirstOrDefaultAsync((long)output.ProductCategoryTeam.EmployeeId);
                output.EmployeeName = _lookupEmployee?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductCategoryTeamDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryTeams_Create)]
        protected virtual async Task Create(CreateOrEditProductCategoryTeamDto input)
        {
            var productCategoryTeam = ObjectMapper.Map<ProductCategoryTeam>(input);

            if (AbpSession.TenantId != null)
            {
                productCategoryTeam.TenantId = (int?)AbpSession.TenantId;
            }

            await _productCategoryTeamRepository.InsertAsync(productCategoryTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryTeams_Edit)]
        protected virtual async Task Update(CreateOrEditProductCategoryTeamDto input)
        {
            var productCategoryTeam = await _productCategoryTeamRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productCategoryTeam);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryTeams_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productCategoryTeamRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductCategoryTeamsToExcel(GetAllProductCategoryTeamsForExcelInput input)
        {

            var filteredProductCategoryTeams = _productCategoryTeamRepository.GetAll()
                        .Include(e => e.ProductCategoryFk)
                        .Include(e => e.EmployeeFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.PrimaryFilter.HasValue && input.PrimaryFilter > -1, e => (input.PrimaryFilter == 1 && e.Primary) || (input.PrimaryFilter == 0 && !e.Primary))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductCategoryNameFilter), e => e.ProductCategoryFk != null && e.ProductCategoryFk.Name == input.ProductCategoryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.EmployeeNameFilter), e => e.EmployeeFk != null && e.EmployeeFk.Name == input.EmployeeNameFilter);

            var query = (from o in filteredProductCategoryTeams
                         join o1 in _lookup_productCategoryRepository.GetAll() on o.ProductCategoryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_employeeRepository.GetAll() on o.EmployeeId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetProductCategoryTeamForViewDto()
                         {
                             ProductCategoryTeam = new ProductCategoryTeamDto
                             {
                                 Primary = o.Primary,
                                 Id = o.Id
                             },
                             ProductCategoryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             EmployeeName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var productCategoryTeamListDtos = await query.ToListAsync();

            return _productCategoryTeamsExcelExporter.ExportToFile(productCategoryTeamListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryTeams)]
        public async Task<PagedResultDto<ProductCategoryTeamProductCategoryLookupTableDto>> GetAllProductCategoryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productCategoryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productCategoryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCategoryTeamProductCategoryLookupTableDto>();
            foreach (var productCategory in productCategoryList)
            {
                lookupTableDtoList.Add(new ProductCategoryTeamProductCategoryLookupTableDto
                {
                    Id = productCategory.Id,
                    DisplayName = productCategory.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductCategoryTeamProductCategoryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_ProductCategoryTeams)]
        public async Task<PagedResultDto<ProductCategoryTeamEmployeeLookupTableDto>> GetAllEmployeeForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_employeeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var employeeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductCategoryTeamEmployeeLookupTableDto>();
            foreach (var employee in employeeList)
            {
                lookupTableDtoList.Add(new ProductCategoryTeamEmployeeLookupTableDto
                {
                    Id = employee.Id,
                    DisplayName = employee.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductCategoryTeamEmployeeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}