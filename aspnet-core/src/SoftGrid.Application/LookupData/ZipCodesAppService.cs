using SoftGrid.LookupData;

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
    [AbpAuthorize(AppPermissions.Pages_ZipCodes)]
    public class ZipCodesAppService : SoftGridAppServiceBase, IZipCodesAppService
    {
        private readonly IRepository<ZipCode, long> _zipCodeRepository;
        private readonly IZipCodesExcelExporter _zipCodesExcelExporter;
        private readonly IRepository<Country, long> _lookup_countryRepository;
        private readonly IRepository<State, long> _lookup_stateRepository;
        private readonly IRepository<City, long> _lookup_cityRepository;
        private readonly IRepository<County, long> _lookup_countyRepository;

        public ZipCodesAppService(IRepository<ZipCode, long> zipCodeRepository, IZipCodesExcelExporter zipCodesExcelExporter, IRepository<Country, long> lookup_countryRepository, IRepository<State, long> lookup_stateRepository, IRepository<City, long> lookup_cityRepository, IRepository<County, long> lookup_countyRepository)
        {
            _zipCodeRepository = zipCodeRepository;
            _zipCodesExcelExporter = zipCodesExcelExporter;
            _lookup_countryRepository = lookup_countryRepository;
            _lookup_stateRepository = lookup_stateRepository;
            _lookup_cityRepository = lookup_cityRepository;
            _lookup_countyRepository = lookup_countyRepository;

        }

        public async Task<PagedResultDto<GetZipCodeForViewDto>> GetAll(GetAllZipCodesInput input)
        {

            var filteredZipCodes = _zipCodeRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.CityFk)
                        .Include(e => e.CountyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.AreaCode.Contains(input.Filter) || e.AsianPopulation.Contains(input.Filter) || e.AverageHouseValue.Contains(input.Filter) || e.BlackPopulation.Contains(input.Filter) || e.CBSA.Contains(input.Filter) || e.CBSA_Div.Contains(input.Filter) || e.CBSA_Div_Name.Contains(input.Filter) || e.CBSA_Name.Contains(input.Filter) || e.CBSA_Type.Contains(input.Filter) || e.CSA.Contains(input.Filter) || e.CSAName.Contains(input.Filter) || e.CarrierRouteRateSortation.Contains(input.Filter) || e.City.Contains(input.Filter) || e.CityAliasCode.Contains(input.Filter) || e.CityAliasMixedCase.Contains(input.Filter) || e.CityAliasName.Contains(input.Filter) || e.CityDeliveryIndicator.Contains(input.Filter) || e.CityMixedCase.Contains(input.Filter) || e.CityStateKey.Contains(input.Filter) || e.CityType.Contains(input.Filter) || e.ClassificationCode.Contains(input.Filter) || e.County.Contains(input.Filter) || e.CountyANSI.Contains(input.Filter) || e.CountyFIPS.Contains(input.Filter) || e.CountyMixedCase.Contains(input.Filter) || e.DayLightSaving.Contains(input.Filter) || e.Division.Contains(input.Filter) || e.Elevation.Contains(input.Filter) || e.FacilityCode.Contains(input.Filter) || e.FemalePopulation.Contains(input.Filter) || e.FinanceNumber.Contains(input.Filter) || e.HawaiianPopulation.Contains(input.Filter) || e.HispanicPopulation.Contains(input.Filter) || e.HouseholdsPerZipCode.Contains(input.Filter) || e.IncomePerHousehold.Contains(input.Filter) || e.IndianPopulation.Contains(input.Filter) || e.Latitude.Contains(input.Filter) || e.Longitude.Contains(input.Filter) || e.MSA.Contains(input.Filter) || e.MSA_Name.Contains(input.Filter) || e.MailingName.Contains(input.Filter) || e.MalePopulation.Contains(input.Filter) || e.MultiCounty.Contains(input.Filter) || e.OtherPopulation.Contains(input.Filter) || e.PMSA.Contains(input.Filter) || e.PMSA_Name.Contains(input.Filter) || e.PersonsPerHousehold.Contains(input.Filter) || e.Population.Contains(input.Filter) || e.PreferredLastLineKey.Contains(input.Filter) || e.PrimaryRecord.Contains(input.Filter) || e.Region.Contains(input.Filter) || e.State.Contains(input.Filter) || e.StateANSI.Contains(input.Filter) || e.StateFIPS.Contains(input.Filter) || e.StateFullName.Contains(input.Filter) || e.TimeZone.Contains(input.Filter) || e.UniqueZIPName.Contains(input.Filter) || e.WhitePopulation.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AreaCodeFilter), e => e.AreaCode.Contains(input.AreaCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AsianPopulationFilter), e => e.AsianPopulation.Contains(input.AsianPopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AverageHouseValueFilter), e => e.AverageHouseValue.Contains(input.AverageHouseValueFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BlackPopulationFilter), e => e.BlackPopulation.Contains(input.BlackPopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CBSAFilter), e => e.CBSA.Contains(input.CBSAFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CBSA_DivFilter), e => e.CBSA_Div.Contains(input.CBSA_DivFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CBSA_Div_NameFilter), e => e.CBSA_Div_Name.Contains(input.CBSA_Div_NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CBSA_NameFilter), e => e.CBSA_Name.Contains(input.CBSA_NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CBSA_TypeFilter), e => e.CBSA_Type.Contains(input.CBSA_TypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CSAFilter), e => e.CSA.Contains(input.CSAFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CSANameFilter), e => e.CSAName.Contains(input.CSANameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CarrierRouteRateSortationFilter), e => e.CarrierRouteRateSortation.Contains(input.CarrierRouteRateSortationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityAliasCodeFilter), e => e.CityAliasCode.Contains(input.CityAliasCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityAliasMixedCaseFilter), e => e.CityAliasMixedCase.Contains(input.CityAliasMixedCaseFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityAliasNameFilter), e => e.CityAliasName.Contains(input.CityAliasNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityDeliveryIndicatorFilter), e => e.CityDeliveryIndicator.Contains(input.CityDeliveryIndicatorFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityMixedCaseFilter), e => e.CityMixedCase.Contains(input.CityMixedCaseFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityStateKeyFilter), e => e.CityStateKey.Contains(input.CityStateKeyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityTypeFilter), e => e.CityType.Contains(input.CityTypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ClassificationCodeFilter), e => e.ClassificationCode.Contains(input.ClassificationCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountyFilter), e => e.County.Contains(input.CountyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountyANSIFilter), e => e.CountyANSI.Contains(input.CountyANSIFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountyFIPSFilter), e => e.CountyFIPS.Contains(input.CountyFIPSFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountyMixedCaseFilter), e => e.CountyMixedCase.Contains(input.CountyMixedCaseFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DayLightSavingFilter), e => e.DayLightSaving.Contains(input.DayLightSavingFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DivisionFilter), e => e.Division.Contains(input.DivisionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ElevationFilter), e => e.Elevation.Contains(input.ElevationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FacilityCodeFilter), e => e.FacilityCode.Contains(input.FacilityCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FemalePopulationFilter), e => e.FemalePopulation.Contains(input.FemalePopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FinanceNumberFilter), e => e.FinanceNumber.Contains(input.FinanceNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HawaiianPopulationFilter), e => e.HawaiianPopulation.Contains(input.HawaiianPopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HispanicPopulationFilter), e => e.HispanicPopulation.Contains(input.HispanicPopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HouseholdsPerZipCodeFilter), e => e.HouseholdsPerZipCode.Contains(input.HouseholdsPerZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IncomePerHouseholdFilter), e => e.IncomePerHousehold.Contains(input.IncomePerHouseholdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IndianPopulationFilter), e => e.IndianPopulation.Contains(input.IndianPopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LatitudeFilter), e => e.Latitude.Contains(input.LatitudeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LongitudeFilter), e => e.Longitude.Contains(input.LongitudeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MSAFilter), e => e.MSA.Contains(input.MSAFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MSA_NameFilter), e => e.MSA_Name.Contains(input.MSA_NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MailingNameFilter), e => e.MailingName.Contains(input.MailingNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MalePopulationFilter), e => e.MalePopulation.Contains(input.MalePopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MultiCountyFilter), e => e.MultiCounty.Contains(input.MultiCountyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OtherPopulationFilter), e => e.OtherPopulation.Contains(input.OtherPopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PMSAFilter), e => e.PMSA.Contains(input.PMSAFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PMSA_NameFilter), e => e.PMSA_Name.Contains(input.PMSA_NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PersonsPerHouseholdFilter), e => e.PersonsPerHousehold.Contains(input.PersonsPerHouseholdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PopulationFilter), e => e.Population.Contains(input.PopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PreferredLastLineKeyFilter), e => e.PreferredLastLineKey.Contains(input.PreferredLastLineKeyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PrimaryRecordFilter), e => e.PrimaryRecord.Contains(input.PrimaryRecordFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegionFilter), e => e.Region.Contains(input.RegionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateFilter), e => e.State.Contains(input.StateFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateANSIFilter), e => e.StateANSI.Contains(input.StateANSIFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateFIPSFilter), e => e.StateFIPS.Contains(input.StateFIPSFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateFullNameFilter), e => e.StateFullName.Contains(input.StateFullNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TimeZoneFilter), e => e.TimeZone.Contains(input.TimeZoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UniqueZIPNameFilter), e => e.UniqueZIPName.Contains(input.UniqueZIPNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WhitePopulationFilter), e => e.WhitePopulation.Contains(input.WhitePopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityNameFilter), e => e.CityFk != null && e.CityFk.Name == input.CityNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountyNameFilter), e => e.CountyFk != null && e.CountyFk.Name == input.CountyNameFilter);

            var pagedAndFilteredZipCodes = filteredZipCodes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var zipCodes = from o in pagedAndFilteredZipCodes
                           join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                           from s1 in j1.DefaultIfEmpty()

                           join o2 in _lookup_stateRepository.GetAll() on o.StateId equals o2.Id into j2
                           from s2 in j2.DefaultIfEmpty()

                           join o3 in _lookup_cityRepository.GetAll() on o.CityId equals o3.Id into j3
                           from s3 in j3.DefaultIfEmpty()

                           join o4 in _lookup_countyRepository.GetAll() on o.CountyId equals o4.Id into j4
                           from s4 in j4.DefaultIfEmpty()

                           select new
                           {

                               o.Name,
                               o.AreaCode,
                               o.AsianPopulation,
                               o.AverageHouseValue,
                               o.BlackPopulation,
                               o.CBSA,
                               o.CBSA_Div,
                               o.CBSA_Div_Name,
                               o.CBSA_Name,
                               o.CBSA_Type,
                               o.CSA,
                               o.CSAName,
                               o.CarrierRouteRateSortation,
                               o.City,
                               o.CityAliasCode,
                               o.CityAliasMixedCase,
                               o.CityAliasName,
                               o.CityDeliveryIndicator,
                               o.CityMixedCase,
                               o.CityStateKey,
                               o.CityType,
                               o.ClassificationCode,
                               o.County,
                               o.CountyANSI,
                               o.CountyFIPS,
                               o.CountyMixedCase,
                               o.DayLightSaving,
                               o.Division,
                               o.Elevation,
                               o.FacilityCode,
                               o.FemalePopulation,
                               o.FinanceNumber,
                               o.HawaiianPopulation,
                               o.HispanicPopulation,
                               o.HouseholdsPerZipCode,
                               o.IncomePerHousehold,
                               o.IndianPopulation,
                               o.Latitude,
                               o.Longitude,
                               o.MSA,
                               o.MSA_Name,
                               o.MailingName,
                               o.MalePopulation,
                               o.MultiCounty,
                               o.OtherPopulation,
                               o.PMSA,
                               o.PMSA_Name,
                               o.PersonsPerHousehold,
                               o.Population,
                               o.PreferredLastLineKey,
                               o.PrimaryRecord,
                               o.Region,
                               o.State,
                               o.StateANSI,
                               o.StateFIPS,
                               o.StateFullName,
                               o.TimeZone,
                               o.UniqueZIPName,
                               o.WhitePopulation,
                               Id = o.Id,
                               CountryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                               StateName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                               CityName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                               CountyName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                           };

            var totalCount = await filteredZipCodes.CountAsync();

            var dbList = await zipCodes.ToListAsync();
            var results = new List<GetZipCodeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetZipCodeForViewDto()
                {
                    ZipCode = new ZipCodeDto
                    {

                        Name = o.Name,
                        AreaCode = o.AreaCode,
                        AsianPopulation = o.AsianPopulation,
                        AverageHouseValue = o.AverageHouseValue,
                        BlackPopulation = o.BlackPopulation,
                        CBSA = o.CBSA,
                        CBSA_Div = o.CBSA_Div,
                        CBSA_Div_Name = o.CBSA_Div_Name,
                        CBSA_Name = o.CBSA_Name,
                        CBSA_Type = o.CBSA_Type,
                        CSA = o.CSA,
                        CSAName = o.CSAName,
                        CarrierRouteRateSortation = o.CarrierRouteRateSortation,
                        City = o.City,
                        CityAliasCode = o.CityAliasCode,
                        CityAliasMixedCase = o.CityAliasMixedCase,
                        CityAliasName = o.CityAliasName,
                        CityDeliveryIndicator = o.CityDeliveryIndicator,
                        CityMixedCase = o.CityMixedCase,
                        CityStateKey = o.CityStateKey,
                        CityType = o.CityType,
                        ClassificationCode = o.ClassificationCode,
                        County = o.County,
                        CountyANSI = o.CountyANSI,
                        CountyFIPS = o.CountyFIPS,
                        CountyMixedCase = o.CountyMixedCase,
                        DayLightSaving = o.DayLightSaving,
                        Division = o.Division,
                        Elevation = o.Elevation,
                        FacilityCode = o.FacilityCode,
                        FemalePopulation = o.FemalePopulation,
                        FinanceNumber = o.FinanceNumber,
                        HawaiianPopulation = o.HawaiianPopulation,
                        HispanicPopulation = o.HispanicPopulation,
                        HouseholdsPerZipCode = o.HouseholdsPerZipCode,
                        IncomePerHousehold = o.IncomePerHousehold,
                        IndianPopulation = o.IndianPopulation,
                        Latitude = o.Latitude,
                        Longitude = o.Longitude,
                        MSA = o.MSA,
                        MSA_Name = o.MSA_Name,
                        MailingName = o.MailingName,
                        MalePopulation = o.MalePopulation,
                        MultiCounty = o.MultiCounty,
                        OtherPopulation = o.OtherPopulation,
                        PMSA = o.PMSA,
                        PMSA_Name = o.PMSA_Name,
                        PersonsPerHousehold = o.PersonsPerHousehold,
                        Population = o.Population,
                        PreferredLastLineKey = o.PreferredLastLineKey,
                        PrimaryRecord = o.PrimaryRecord,
                        Region = o.Region,
                        State = o.State,
                        StateANSI = o.StateANSI,
                        StateFIPS = o.StateFIPS,
                        StateFullName = o.StateFullName,
                        TimeZone = o.TimeZone,
                        UniqueZIPName = o.UniqueZIPName,
                        WhitePopulation = o.WhitePopulation,
                        Id = o.Id,
                    },
                    CountryName = o.CountryName,
                    StateName = o.StateName,
                    CityName = o.CityName,
                    CountyName = o.CountyName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetZipCodeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetZipCodeForViewDto> GetZipCodeForView(long id)
        {
            var zipCode = await _zipCodeRepository.GetAsync(id);

            var output = new GetZipCodeForViewDto { ZipCode = ObjectMapper.Map<ZipCodeDto>(zipCode) };

            if (output.ZipCode.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.ZipCode.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.ZipCode.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.ZipCode.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.ZipCode.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((long)output.ZipCode.CityId);
                output.CityName = _lookupCity?.Name?.ToString();
            }

            if (output.ZipCode.CountyId != null)
            {
                var _lookupCounty = await _lookup_countyRepository.FirstOrDefaultAsync((long)output.ZipCode.CountyId);
                output.CountyName = _lookupCounty?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ZipCodes_Edit)]
        public async Task<GetZipCodeForEditOutput> GetZipCodeForEdit(EntityDto<long> input)
        {
            var zipCode = await _zipCodeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetZipCodeForEditOutput { ZipCode = ObjectMapper.Map<CreateOrEditZipCodeDto>(zipCode) };

            if (output.ZipCode.CountryId != null)
            {
                var _lookupCountry = await _lookup_countryRepository.FirstOrDefaultAsync((long)output.ZipCode.CountryId);
                output.CountryName = _lookupCountry?.Name?.ToString();
            }

            if (output.ZipCode.StateId != null)
            {
                var _lookupState = await _lookup_stateRepository.FirstOrDefaultAsync((long)output.ZipCode.StateId);
                output.StateName = _lookupState?.Name?.ToString();
            }

            if (output.ZipCode.CityId != null)
            {
                var _lookupCity = await _lookup_cityRepository.FirstOrDefaultAsync((long)output.ZipCode.CityId);
                output.CityName = _lookupCity?.Name?.ToString();
            }

            if (output.ZipCode.CountyId != null)
            {
                var _lookupCounty = await _lookup_countyRepository.FirstOrDefaultAsync((long)output.ZipCode.CountyId);
                output.CountyName = _lookupCounty?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditZipCodeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_ZipCodes_Create)]
        protected virtual async Task Create(CreateOrEditZipCodeDto input)
        {
            var zipCode = ObjectMapper.Map<ZipCode>(input);

            if (AbpSession.TenantId != null)
            {
                zipCode.TenantId = (int?)AbpSession.TenantId;
            }

            await _zipCodeRepository.InsertAsync(zipCode);

        }

        [AbpAuthorize(AppPermissions.Pages_ZipCodes_Edit)]
        protected virtual async Task Update(CreateOrEditZipCodeDto input)
        {
            var zipCode = await _zipCodeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, zipCode);

        }

        [AbpAuthorize(AppPermissions.Pages_ZipCodes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _zipCodeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetZipCodesToExcel(GetAllZipCodesForExcelInput input)
        {

            var filteredZipCodes = _zipCodeRepository.GetAll()
                        .Include(e => e.CountryFk)
                        .Include(e => e.StateFk)
                        .Include(e => e.CityFk)
                        .Include(e => e.CountyFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.AreaCode.Contains(input.Filter) || e.AsianPopulation.Contains(input.Filter) || e.AverageHouseValue.Contains(input.Filter) || e.BlackPopulation.Contains(input.Filter) || e.CBSA.Contains(input.Filter) || e.CBSA_Div.Contains(input.Filter) || e.CBSA_Div_Name.Contains(input.Filter) || e.CBSA_Name.Contains(input.Filter) || e.CBSA_Type.Contains(input.Filter) || e.CSA.Contains(input.Filter) || e.CSAName.Contains(input.Filter) || e.CarrierRouteRateSortation.Contains(input.Filter) || e.City.Contains(input.Filter) || e.CityAliasCode.Contains(input.Filter) || e.CityAliasMixedCase.Contains(input.Filter) || e.CityAliasName.Contains(input.Filter) || e.CityDeliveryIndicator.Contains(input.Filter) || e.CityMixedCase.Contains(input.Filter) || e.CityStateKey.Contains(input.Filter) || e.CityType.Contains(input.Filter) || e.ClassificationCode.Contains(input.Filter) || e.County.Contains(input.Filter) || e.CountyANSI.Contains(input.Filter) || e.CountyFIPS.Contains(input.Filter) || e.CountyMixedCase.Contains(input.Filter) || e.DayLightSaving.Contains(input.Filter) || e.Division.Contains(input.Filter) || e.Elevation.Contains(input.Filter) || e.FacilityCode.Contains(input.Filter) || e.FemalePopulation.Contains(input.Filter) || e.FinanceNumber.Contains(input.Filter) || e.HawaiianPopulation.Contains(input.Filter) || e.HispanicPopulation.Contains(input.Filter) || e.HouseholdsPerZipCode.Contains(input.Filter) || e.IncomePerHousehold.Contains(input.Filter) || e.IndianPopulation.Contains(input.Filter) || e.Latitude.Contains(input.Filter) || e.Longitude.Contains(input.Filter) || e.MSA.Contains(input.Filter) || e.MSA_Name.Contains(input.Filter) || e.MailingName.Contains(input.Filter) || e.MalePopulation.Contains(input.Filter) || e.MultiCounty.Contains(input.Filter) || e.OtherPopulation.Contains(input.Filter) || e.PMSA.Contains(input.Filter) || e.PMSA_Name.Contains(input.Filter) || e.PersonsPerHousehold.Contains(input.Filter) || e.Population.Contains(input.Filter) || e.PreferredLastLineKey.Contains(input.Filter) || e.PrimaryRecord.Contains(input.Filter) || e.Region.Contains(input.Filter) || e.State.Contains(input.Filter) || e.StateANSI.Contains(input.Filter) || e.StateFIPS.Contains(input.Filter) || e.StateFullName.Contains(input.Filter) || e.TimeZone.Contains(input.Filter) || e.UniqueZIPName.Contains(input.Filter) || e.WhitePopulation.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AreaCodeFilter), e => e.AreaCode.Contains(input.AreaCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AsianPopulationFilter), e => e.AsianPopulation.Contains(input.AsianPopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.AverageHouseValueFilter), e => e.AverageHouseValue.Contains(input.AverageHouseValueFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.BlackPopulationFilter), e => e.BlackPopulation.Contains(input.BlackPopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CBSAFilter), e => e.CBSA.Contains(input.CBSAFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CBSA_DivFilter), e => e.CBSA_Div.Contains(input.CBSA_DivFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CBSA_Div_NameFilter), e => e.CBSA_Div_Name.Contains(input.CBSA_Div_NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CBSA_NameFilter), e => e.CBSA_Name.Contains(input.CBSA_NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CBSA_TypeFilter), e => e.CBSA_Type.Contains(input.CBSA_TypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CSAFilter), e => e.CSA.Contains(input.CSAFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CSANameFilter), e => e.CSAName.Contains(input.CSANameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CarrierRouteRateSortationFilter), e => e.CarrierRouteRateSortation.Contains(input.CarrierRouteRateSortationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityFilter), e => e.City.Contains(input.CityFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityAliasCodeFilter), e => e.CityAliasCode.Contains(input.CityAliasCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityAliasMixedCaseFilter), e => e.CityAliasMixedCase.Contains(input.CityAliasMixedCaseFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityAliasNameFilter), e => e.CityAliasName.Contains(input.CityAliasNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityDeliveryIndicatorFilter), e => e.CityDeliveryIndicator.Contains(input.CityDeliveryIndicatorFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityMixedCaseFilter), e => e.CityMixedCase.Contains(input.CityMixedCaseFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityStateKeyFilter), e => e.CityStateKey.Contains(input.CityStateKeyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityTypeFilter), e => e.CityType.Contains(input.CityTypeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ClassificationCodeFilter), e => e.ClassificationCode.Contains(input.ClassificationCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountyFilter), e => e.County.Contains(input.CountyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountyANSIFilter), e => e.CountyANSI.Contains(input.CountyANSIFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountyFIPSFilter), e => e.CountyFIPS.Contains(input.CountyFIPSFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountyMixedCaseFilter), e => e.CountyMixedCase.Contains(input.CountyMixedCaseFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DayLightSavingFilter), e => e.DayLightSaving.Contains(input.DayLightSavingFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DivisionFilter), e => e.Division.Contains(input.DivisionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ElevationFilter), e => e.Elevation.Contains(input.ElevationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FacilityCodeFilter), e => e.FacilityCode.Contains(input.FacilityCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FemalePopulationFilter), e => e.FemalePopulation.Contains(input.FemalePopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.FinanceNumberFilter), e => e.FinanceNumber.Contains(input.FinanceNumberFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HawaiianPopulationFilter), e => e.HawaiianPopulation.Contains(input.HawaiianPopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HispanicPopulationFilter), e => e.HispanicPopulation.Contains(input.HispanicPopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.HouseholdsPerZipCodeFilter), e => e.HouseholdsPerZipCode.Contains(input.HouseholdsPerZipCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IncomePerHouseholdFilter), e => e.IncomePerHousehold.Contains(input.IncomePerHouseholdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.IndianPopulationFilter), e => e.IndianPopulation.Contains(input.IndianPopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LatitudeFilter), e => e.Latitude.Contains(input.LatitudeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LongitudeFilter), e => e.Longitude.Contains(input.LongitudeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MSAFilter), e => e.MSA.Contains(input.MSAFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MSA_NameFilter), e => e.MSA_Name.Contains(input.MSA_NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MailingNameFilter), e => e.MailingName.Contains(input.MailingNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MalePopulationFilter), e => e.MalePopulation.Contains(input.MalePopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MultiCountyFilter), e => e.MultiCounty.Contains(input.MultiCountyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.OtherPopulationFilter), e => e.OtherPopulation.Contains(input.OtherPopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PMSAFilter), e => e.PMSA.Contains(input.PMSAFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PMSA_NameFilter), e => e.PMSA_Name.Contains(input.PMSA_NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PersonsPerHouseholdFilter), e => e.PersonsPerHousehold.Contains(input.PersonsPerHouseholdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PopulationFilter), e => e.Population.Contains(input.PopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PreferredLastLineKeyFilter), e => e.PreferredLastLineKey.Contains(input.PreferredLastLineKeyFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PrimaryRecordFilter), e => e.PrimaryRecord.Contains(input.PrimaryRecordFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RegionFilter), e => e.Region.Contains(input.RegionFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateFilter), e => e.State.Contains(input.StateFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateANSIFilter), e => e.StateANSI.Contains(input.StateANSIFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateFIPSFilter), e => e.StateFIPS.Contains(input.StateFIPSFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateFullNameFilter), e => e.StateFullName.Contains(input.StateFullNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.TimeZoneFilter), e => e.TimeZone.Contains(input.TimeZoneFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UniqueZIPNameFilter), e => e.UniqueZIPName.Contains(input.UniqueZIPNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.WhitePopulationFilter), e => e.WhitePopulation.Contains(input.WhitePopulationFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountryNameFilter), e => e.CountryFk != null && e.CountryFk.Name == input.CountryNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.StateNameFilter), e => e.StateFk != null && e.StateFk.Name == input.StateNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CityNameFilter), e => e.CityFk != null && e.CityFk.Name == input.CityNameFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.CountyNameFilter), e => e.CountyFk != null && e.CountyFk.Name == input.CountyNameFilter);

            var query = (from o in filteredZipCodes
                         join o1 in _lookup_countryRepository.GetAll() on o.CountryId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         join o2 in _lookup_stateRepository.GetAll() on o.StateId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()

                         join o3 in _lookup_cityRepository.GetAll() on o.CityId equals o3.Id into j3
                         from s3 in j3.DefaultIfEmpty()

                         join o4 in _lookup_countyRepository.GetAll() on o.CountyId equals o4.Id into j4
                         from s4 in j4.DefaultIfEmpty()

                         select new GetZipCodeForViewDto()
                         {
                             ZipCode = new ZipCodeDto
                             {
                                 Name = o.Name,
                                 AreaCode = o.AreaCode,
                                 AsianPopulation = o.AsianPopulation,
                                 AverageHouseValue = o.AverageHouseValue,
                                 BlackPopulation = o.BlackPopulation,
                                 CBSA = o.CBSA,
                                 CBSA_Div = o.CBSA_Div,
                                 CBSA_Div_Name = o.CBSA_Div_Name,
                                 CBSA_Name = o.CBSA_Name,
                                 CBSA_Type = o.CBSA_Type,
                                 CSA = o.CSA,
                                 CSAName = o.CSAName,
                                 CarrierRouteRateSortation = o.CarrierRouteRateSortation,
                                 City = o.City,
                                 CityAliasCode = o.CityAliasCode,
                                 CityAliasMixedCase = o.CityAliasMixedCase,
                                 CityAliasName = o.CityAliasName,
                                 CityDeliveryIndicator = o.CityDeliveryIndicator,
                                 CityMixedCase = o.CityMixedCase,
                                 CityStateKey = o.CityStateKey,
                                 CityType = o.CityType,
                                 ClassificationCode = o.ClassificationCode,
                                 County = o.County,
                                 CountyANSI = o.CountyANSI,
                                 CountyFIPS = o.CountyFIPS,
                                 CountyMixedCase = o.CountyMixedCase,
                                 DayLightSaving = o.DayLightSaving,
                                 Division = o.Division,
                                 Elevation = o.Elevation,
                                 FacilityCode = o.FacilityCode,
                                 FemalePopulation = o.FemalePopulation,
                                 FinanceNumber = o.FinanceNumber,
                                 HawaiianPopulation = o.HawaiianPopulation,
                                 HispanicPopulation = o.HispanicPopulation,
                                 HouseholdsPerZipCode = o.HouseholdsPerZipCode,
                                 IncomePerHousehold = o.IncomePerHousehold,
                                 IndianPopulation = o.IndianPopulation,
                                 Latitude = o.Latitude,
                                 Longitude = o.Longitude,
                                 MSA = o.MSA,
                                 MSA_Name = o.MSA_Name,
                                 MailingName = o.MailingName,
                                 MalePopulation = o.MalePopulation,
                                 MultiCounty = o.MultiCounty,
                                 OtherPopulation = o.OtherPopulation,
                                 PMSA = o.PMSA,
                                 PMSA_Name = o.PMSA_Name,
                                 PersonsPerHousehold = o.PersonsPerHousehold,
                                 Population = o.Population,
                                 PreferredLastLineKey = o.PreferredLastLineKey,
                                 PrimaryRecord = o.PrimaryRecord,
                                 Region = o.Region,
                                 State = o.State,
                                 StateANSI = o.StateANSI,
                                 StateFIPS = o.StateFIPS,
                                 StateFullName = o.StateFullName,
                                 TimeZone = o.TimeZone,
                                 UniqueZIPName = o.UniqueZIPName,
                                 WhitePopulation = o.WhitePopulation,
                                 Id = o.Id
                             },
                             CountryName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                             StateName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                             CityName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                             CountyName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                         });

            var zipCodeListDtos = await query.ToListAsync();

            return _zipCodesExcelExporter.ExportToFile(zipCodeListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_ZipCodes)]
        public async Task<List<ZipCodeCountryLookupTableDto>> GetAllCountryForTableDropdown()
        {
            return await _lookup_countryRepository.GetAll()
                .Select(country => new ZipCodeCountryLookupTableDto
                {
                    Id = country.Id,
                    DisplayName = country == null || country.Name == null ? "" : country.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_ZipCodes)]
        public async Task<List<ZipCodeStateLookupTableDto>> GetAllStateForTableDropdown()
        {
            return await _lookup_stateRepository.GetAll()
                .Select(state => new ZipCodeStateLookupTableDto
                {
                    Id = state.Id,
                    DisplayName = state == null || state.Name == null ? "" : state.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_ZipCodes)]
        public async Task<List<ZipCodeCityLookupTableDto>> GetAllCityForTableDropdown()
        {
            return await _lookup_cityRepository.GetAll()
                .Select(city => new ZipCodeCityLookupTableDto
                {
                    Id = city.Id,
                    DisplayName = city == null || city.Name == null ? "" : city.Name.ToString()
                }).ToListAsync();
        }

        [AbpAuthorize(AppPermissions.Pages_ZipCodes)]
        public async Task<List<ZipCodeCountyLookupTableDto>> GetAllCountyForTableDropdown()
        {
            return await _lookup_countyRepository.GetAll()
                .Select(county => new ZipCodeCountyLookupTableDto
                {
                    Id = county.Id,
                    DisplayName = county == null || county.Name == null ? "" : county.Name.ToString()
                }).ToListAsync();
        }

    }
}