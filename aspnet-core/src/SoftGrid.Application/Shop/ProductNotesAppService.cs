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
    [AbpAuthorize(AppPermissions.Pages_ProductNotes)]
    public class ProductNotesAppService : SoftGridAppServiceBase, IProductNotesAppService
    {
        private readonly IRepository<ProductNote, long> _productNoteRepository;
        private readonly IProductNotesExcelExporter _productNotesExcelExporter;
        private readonly IRepository<Product, long> _lookup_productRepository;

        public ProductNotesAppService(IRepository<ProductNote, long> productNoteRepository, IProductNotesExcelExporter productNotesExcelExporter, IRepository<Product, long> lookup_productRepository)
        {
            _productNoteRepository = productNoteRepository;
            _productNotesExcelExporter = productNotesExcelExporter;
            _lookup_productRepository = lookup_productRepository;

        }

        public async Task<PagedResultDto<GetProductNoteForViewDto>> GetAll(GetAllProductNotesInput input)
        {

            var filteredProductNotes = _productNoteRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Notes.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var pagedAndFilteredProductNotes = filteredProductNotes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var productNotes = from o in pagedAndFilteredProductNotes
                               join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                               from s1 in j1.DefaultIfEmpty()

                               select new
                               {

                                   o.Notes,
                                   Id = o.Id,
                                   ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                               };

            var totalCount = await filteredProductNotes.CountAsync();

            var dbList = await productNotes.ToListAsync();
            var results = new List<GetProductNoteForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetProductNoteForViewDto()
                {
                    ProductNote = new ProductNoteDto
                    {

                        Notes = o.Notes,
                        Id = o.Id,
                    },
                    ProductName = o.ProductName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetProductNoteForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetProductNoteForViewDto> GetProductNoteForView(long id)
        {
            var productNote = await _productNoteRepository.GetAsync(id);

            var output = new GetProductNoteForViewDto { ProductNote = ObjectMapper.Map<ProductNoteDto>(productNote) };

            if (output.ProductNote.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductNote.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ProductNotes_Edit)]
        public async Task<GetProductNoteForEditOutput> GetProductNoteForEdit(EntityDto<long> input)
        {
            var productNote = await _productNoteRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetProductNoteForEditOutput { ProductNote = ObjectMapper.Map<CreateOrEditProductNoteDto>(productNote) };

            if (output.ProductNote.ProductId != null)
            {
                var _lookupProduct = await _lookup_productRepository.FirstOrDefaultAsync((long)output.ProductNote.ProductId);
                output.ProductName = _lookupProduct?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditProductNoteDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ProductNotes_Create)]
        protected virtual async Task Create(CreateOrEditProductNoteDto input)
        {
            var productNote = ObjectMapper.Map<ProductNote>(input);

            if (AbpSession.TenantId != null)
            {
                productNote.TenantId = (int?)AbpSession.TenantId;
            }

            await _productNoteRepository.InsertAsync(productNote);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductNotes_Edit)]
        protected virtual async Task Update(CreateOrEditProductNoteDto input)
        {
            var productNote = await _productNoteRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, productNote);

        }

        [AbpAuthorize(AppPermissions.Pages_ProductNotes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _productNoteRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetProductNotesToExcel(GetAllProductNotesForExcelInput input)
        {

            var filteredProductNotes = _productNoteRepository.GetAll()
                        .Include(e => e.ProductFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Notes.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ProductNameFilter), e => e.ProductFk != null && e.ProductFk.Name == input.ProductNameFilter);

            var query = (from o in filteredProductNotes
                         join o1 in _lookup_productRepository.GetAll() on o.ProductId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetProductNoteForViewDto()
                         {
                             ProductNote = new ProductNoteDto
                             {
                                 Notes = o.Notes,
                                 Id = o.Id
                             },
                             ProductName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var productNoteListDtos = await query.ToListAsync();

            return _productNotesExcelExporter.ExportToFile(productNoteListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ProductNotes)]
        public async Task<PagedResultDto<ProductNoteProductLookupTableDto>> GetAllProductForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_productRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var productList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ProductNoteProductLookupTableDto>();
            foreach (var product in productList)
            {
                lookupTableDtoList.Add(new ProductNoteProductLookupTableDto
                {
                    Id = product.Id,
                    DisplayName = product.Name?.ToString()
                });
            }

            return new PagedResultDto<ProductNoteProductLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}