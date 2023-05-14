using SoftGrid.Shop;
using SoftGrid.LookupData;

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
    [AbpAuthorize(AppPermissions.Pages_StoreMedias)]
    public class StoreMediasAppService : SoftGridAppServiceBase, IStoreMediasAppService
    {
        private readonly IRepository<StoreMedia, long> _storeMediaRepository;
        private readonly IStoreMediasExcelExporter _storeMediasExcelExporter;
        private readonly IRepository<Store, long> _lookup_storeRepository;
        private readonly IRepository<MediaLibrary, long> _lookup_mediaLibraryRepository;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public StoreMediasAppService(IRepository<StoreMedia, long> storeMediaRepository, IStoreMediasExcelExporter storeMediasExcelExporter, IRepository<Store, long> lookup_storeRepository, IRepository<MediaLibrary, long> lookup_mediaLibraryRepository, IBinaryObjectManager binaryObjectManager)
        {
            _storeMediaRepository = storeMediaRepository;
            _storeMediasExcelExporter = storeMediasExcelExporter;
            _lookup_storeRepository = lookup_storeRepository;
            _lookup_mediaLibraryRepository = lookup_mediaLibraryRepository;
            _binaryObjectManager = binaryObjectManager;
        }

        public async Task<PagedResultDto<GetStoreMediaForViewDto>> GetAll(GetAllStoreMediasInput input)
        {

            var filteredStoreMedias = _storeMediaRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.MediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.MediaLibraryFk != null && e.MediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var pagedAndFilteredStoreMedias = filteredStoreMedias
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var storeMedias = from o in pagedAndFilteredStoreMedias
                              join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              join o2 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o2.Id into j2
                              from s2 in j2.DefaultIfEmpty()

                              select new
                              {

                                  o.DisplaySequence,
                                  Id = o.Id,
                                  StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                  MediaLibraryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                              };

            var totalCount = await filteredStoreMedias.CountAsync();

            var dbList = await storeMedias.ToListAsync();
            var results = new List<GetStoreMediaForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetStoreMediaForViewDto()
                {
                    StoreMedia = new StoreMediaDto
                    {

                        DisplaySequence = o.DisplaySequence,
                        Id = o.Id,
                    },
                    StoreName = o.StoreName,
                    MediaLibraryName = o.MediaLibraryName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetStoreMediaForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetStoreMediaForViewDto> GetStoreMediaForView(long id)
        {
            var storeMedia = await _storeMediaRepository.GetAsync(id);

            var output = new GetStoreMediaForViewDto { StoreMedia = ObjectMapper.Map<StoreMediaDto>(storeMedia) };

            if (output.StoreMedia.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreMedia.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreMedia.MediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.StoreMedia.MediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_StoreMedias_Edit)]
        public async Task<GetStoreMediaForEditOutput> GetStoreMediaForEdit(EntityDto<long> input)
        {
            var storeMedia = await _storeMediaRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetStoreMediaForEditOutput { StoreMedia = ObjectMapper.Map<CreateOrEditStoreMediaDto>(storeMedia) };

            if (output.StoreMedia.StoreId != null)
            {
                var _lookupStore = await _lookup_storeRepository.FirstOrDefaultAsync((long)output.StoreMedia.StoreId);
                output.StoreName = _lookupStore?.Name?.ToString();
            }

            if (output.StoreMedia.MediaLibraryId != null)
            {
                var _lookupMediaLibrary = await _lookup_mediaLibraryRepository.FirstOrDefaultAsync((long)output.StoreMedia.MediaLibraryId);
                output.MediaLibraryName = _lookupMediaLibrary?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditStoreMediaDto input)
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

        [AbpAuthorize(AppPermissions.Pages_StoreMedias_Create)]
        protected virtual async Task Create(CreateOrEditStoreMediaDto input)
        {
            var storeMedia = ObjectMapper.Map<StoreMedia>(input);

            if (AbpSession.TenantId != null)
            {
                storeMedia.TenantId = (int?)AbpSession.TenantId;
            }

            await _storeMediaRepository.InsertAsync(storeMedia);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreMedias_Edit)]
        protected virtual async Task Update(CreateOrEditStoreMediaDto input)
        {
            var storeMedia = await _storeMediaRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, storeMedia);

        }

        [AbpAuthorize(AppPermissions.Pages_StoreMedias_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _storeMediaRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetStoreMediasToExcel(GetAllStoreMediasForExcelInput input)
        {

            var filteredStoreMedias = _storeMediaRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.MediaLibraryFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                        .WhereIf(input.MinDisplaySequenceFilter != null, e => e.DisplaySequence >= input.MinDisplaySequenceFilter)
                        .WhereIf(input.MaxDisplaySequenceFilter != null, e => e.DisplaySequence <= input.MaxDisplaySequenceFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StoreNameFilter), e => e.StoreFk != null && e.StoreFk.Name == input.StoreNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MediaLibraryNameFilter), e => e.MediaLibraryFk != null && e.MediaLibraryFk.Name == input.MediaLibraryNameFilter);

            var query = (from o in filteredStoreMedias
                         join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         select new GetStoreMediaForViewDto()
                         {
                             StoreMedia = new StoreMediaDto
                             {
                                 DisplaySequence = o.DisplaySequence,
                                 Id = o.Id
                             },
                             StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             MediaLibraryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                         });

            var storeMediaListDtos = await query.ToListAsync();

            return _storeMediasExcelExporter.ExportToFile(storeMediaListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_StoreMedias)]
        public async Task<PagedResultDto<StoreMediaStoreLookupTableDto>> GetAllStoreForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_storeRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var storeList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreMediaStoreLookupTableDto>();
            foreach (var store in storeList)
            {
                lookupTableDtoList.Add(new StoreMediaStoreLookupTableDto
                {
                    Id = store.Id,
                    DisplayName = store.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreMediaStoreLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_StoreMedias)]
        public async Task<PagedResultDto<StoreMediaMediaLibraryLookupTableDto>> GetAllMediaLibraryForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_mediaLibraryRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var mediaLibraryList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<StoreMediaMediaLibraryLookupTableDto>();
            foreach (var mediaLibrary in mediaLibraryList)
            {
                lookupTableDtoList.Add(new StoreMediaMediaLibraryLookupTableDto
                {
                    Id = mediaLibrary.Id,
                    DisplayName = mediaLibrary.Name?.ToString()
                });
            }

            return new PagedResultDto<StoreMediaMediaLibraryLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<PagedResultDto<GetStoreMediaForViewDto>> GetAllByStoreIdForStoreBuilder(long storeId)
        {

            var filteredStoreMedias = _storeMediaRepository.GetAll()
                        .Include(e => e.StoreFk)
                        .Include(e => e.MediaLibraryFk)
                        .Where(e => e.StoreId == storeId);

            var pagedAndFilteredStoreMedias = filteredStoreMedias
                .OrderBy("displaySequence asc");

            var storeMedias = from o in pagedAndFilteredStoreMedias
                              join o1 in _lookup_storeRepository.GetAll() on o.StoreId equals o1.Id into j1
                              from s1 in j1.DefaultIfEmpty()

                              join o2 in _lookup_mediaLibraryRepository.GetAll() on o.MediaLibraryId equals o2.Id into j2
                              from s2 in j2.DefaultIfEmpty()

                              select new GetStoreMediaForViewDto()
                              {
                                  StoreMedia = new StoreMediaDto
                                  {
                                      DisplaySequence = o.DisplaySequence,
                                      Id = o.Id,
                                      StoreId = o.StoreId,
                                      MediaLibraryId = o.MediaLibraryId
                                  },
                                  StoreName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                                  MediaLibraryName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                              };

            var result = await storeMedias.ToListAsync();

            foreach (var storeMedia in result)
            {
                if (storeMedia.StoreMedia.MediaLibraryId != null)
                {
                    var media = _lookup_mediaLibraryRepository.Get((long)storeMedia.StoreMedia.MediaLibraryId);
                    if (media.BinaryObjectId != null && media.BinaryObjectId != Guid.Empty)
                    {
                        //var binaryObject = await _binaryObjectManager.GetOrNullAsync(media.BinaryObjectId);
                        storeMedia.Picture = await _binaryObjectManager.GetStorePictureUrlAsync(media.BinaryObjectId, ".png");
                    }

                    if (media.VideoLink != null)
                    {
                        storeMedia.VideoUrl = media.VideoLink;
                    }
                }

            }

            var totalCount = await filteredStoreMedias.CountAsync();

            return new PagedResultDto<GetStoreMediaForViewDto>(
                totalCount,
                result
            );
        }

    }
}