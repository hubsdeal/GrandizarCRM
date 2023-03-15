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
    [AbpAuthorize(AppPermissions.Pages_MeasurementUnits)]
    public class MeasurementUnitsAppService : SoftGridAppServiceBase, IMeasurementUnitsAppService
    {
        private readonly IRepository<MeasurementUnit, long> _measurementUnitRepository;
        private readonly IMeasurementUnitsExcelExporter _measurementUnitsExcelExporter;

        public MeasurementUnitsAppService(IRepository<MeasurementUnit, long> measurementUnitRepository, IMeasurementUnitsExcelExporter measurementUnitsExcelExporter)
        {
            _measurementUnitRepository = measurementUnitRepository;
            _measurementUnitsExcelExporter = measurementUnitsExcelExporter;

        }

        public async Task<PagedResultDto<GetMeasurementUnitForViewDto>> GetAll(GetAllMeasurementUnitsInput input)
        {

            var filteredMeasurementUnits = _measurementUnitRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredMeasurementUnits = filteredMeasurementUnits
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var measurementUnits = from o in pagedAndFilteredMeasurementUnits
                                   select new
                                   {

                                       o.Name,
                                       Id = o.Id
                                   };

            var totalCount = await filteredMeasurementUnits.CountAsync();

            var dbList = await measurementUnits.ToListAsync();
            var results = new List<GetMeasurementUnitForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetMeasurementUnitForViewDto()
                {
                    MeasurementUnit = new MeasurementUnitDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetMeasurementUnitForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetMeasurementUnitForViewDto> GetMeasurementUnitForView(long id)
        {
            var measurementUnit = await _measurementUnitRepository.GetAsync(id);

            var output = new GetMeasurementUnitForViewDto { MeasurementUnit = ObjectMapper.Map<MeasurementUnitDto>(measurementUnit) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_MeasurementUnits_Edit)]
        public async Task<GetMeasurementUnitForEditOutput> GetMeasurementUnitForEdit(EntityDto<long> input)
        {
            var measurementUnit = await _measurementUnitRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetMeasurementUnitForEditOutput { MeasurementUnit = ObjectMapper.Map<CreateOrEditMeasurementUnitDto>(measurementUnit) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditMeasurementUnitDto input)
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

        [AbpAuthorize(AppPermissions.Pages_MeasurementUnits_Create)]
        protected virtual async Task Create(CreateOrEditMeasurementUnitDto input)
        {
            var measurementUnit = ObjectMapper.Map<MeasurementUnit>(input);

            if (AbpSession.TenantId != null)
            {
                measurementUnit.TenantId = (int?)AbpSession.TenantId;
            }

            await _measurementUnitRepository.InsertAsync(measurementUnit);

        }

        [AbpAuthorize(AppPermissions.Pages_MeasurementUnits_Edit)]
        protected virtual async Task Update(CreateOrEditMeasurementUnitDto input)
        {
            var measurementUnit = await _measurementUnitRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, measurementUnit);

        }

        [AbpAuthorize(AppPermissions.Pages_MeasurementUnits_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _measurementUnitRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetMeasurementUnitsToExcel(GetAllMeasurementUnitsForExcelInput input)
        {

            var filteredMeasurementUnits = _measurementUnitRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredMeasurementUnits
                         select new GetMeasurementUnitForViewDto()
                         {
                             MeasurementUnit = new MeasurementUnitDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var measurementUnitListDtos = await query.ToListAsync();

            return _measurementUnitsExcelExporter.ExportToFile(measurementUnitListDtos);
        }

    }
}