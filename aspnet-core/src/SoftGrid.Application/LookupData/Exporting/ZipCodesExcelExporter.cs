using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using SoftGrid.DataExporting.Excel.NPOI;
using SoftGrid.LookupData.Dtos;
using SoftGrid.Dto;
using SoftGrid.Storage;

namespace SoftGrid.LookupData.Exporting
{
    public class ZipCodesExcelExporter : NpoiExcelExporterBase, IZipCodesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ZipCodesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            ITempFileCacheManager tempFileCacheManager) :
    base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetZipCodeForViewDto> zipCodes)
        {
            return CreateExcelPackage(
                "ZipCodes.xlsx",
                excelPackage =>
                {

                    var sheet = excelPackage.CreateSheet(L("ZipCodes"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("AreaCode"),
                        L("AsianPopulation"),
                        L("AverageHouseValue"),
                        L("BlackPopulation"),
                        L("CBSA"),
                        L("CBSA_Div"),
                        L("CBSA_Div_Name"),
                        L("CBSA_Name"),
                        L("CBSA_Type"),
                        L("CSA"),
                        L("CSAName"),
                        L("CarrierRouteRateSortation"),
                        L("City"),
                        L("CityAliasCode"),
                        L("CityAliasMixedCase"),
                        L("CityAliasName"),
                        L("CityDeliveryIndicator"),
                        L("CityMixedCase"),
                        L("CityStateKey"),
                        L("CityType"),
                        L("ClassificationCode"),
                        L("County"),
                        L("CountyANSI"),
                        L("CountyFIPS"),
                        L("CountyMixedCase"),
                        L("DayLightSaving"),
                        L("Division"),
                        L("Elevation"),
                        L("FacilityCode"),
                        L("FemalePopulation"),
                        L("FinanceNumber"),
                        L("HawaiianPopulation"),
                        L("HispanicPopulation"),
                        L("HouseholdsPerZipCode"),
                        L("IncomePerHousehold"),
                        L("IndianPopulation"),
                        L("Latitude"),
                        L("Longitude"),
                        L("MSA"),
                        L("MSA_Name"),
                        L("MailingName"),
                        L("MalePopulation"),
                        L("MultiCounty"),
                        L("OtherPopulation"),
                        L("PMSA"),
                        L("PMSA_Name"),
                        L("PersonsPerHousehold"),
                        L("Population"),
                        L("PreferredLastLineKey"),
                        L("PrimaryRecord"),
                        L("Region"),
                        L("State"),
                        L("StateANSI"),
                        L("StateFIPS"),
                        L("StateFullName"),
                        L("TimeZone"),
                        L("UniqueZIPName"),
                        L("WhitePopulation"),
                        (L("Country")) + L("Name"),
                        (L("State")) + L("Name"),
                        (L("City")) + L("Name"),
                        (L("County")) + L("Name")
                        );

                    AddObjects(
                        sheet, zipCodes,
                        _ => _.ZipCode.Name,
                        _ => _.ZipCode.AreaCode,
                        _ => _.ZipCode.AsianPopulation,
                        _ => _.ZipCode.AverageHouseValue,
                        _ => _.ZipCode.BlackPopulation,
                        _ => _.ZipCode.CBSA,
                        _ => _.ZipCode.CBSA_Div,
                        _ => _.ZipCode.CBSA_Div_Name,
                        _ => _.ZipCode.CBSA_Name,
                        _ => _.ZipCode.CBSA_Type,
                        _ => _.ZipCode.CSA,
                        _ => _.ZipCode.CSAName,
                        _ => _.ZipCode.CarrierRouteRateSortation,
                        _ => _.ZipCode.City,
                        _ => _.ZipCode.CityAliasCode,
                        _ => _.ZipCode.CityAliasMixedCase,
                        _ => _.ZipCode.CityAliasName,
                        _ => _.ZipCode.CityDeliveryIndicator,
                        _ => _.ZipCode.CityMixedCase,
                        _ => _.ZipCode.CityStateKey,
                        _ => _.ZipCode.CityType,
                        _ => _.ZipCode.ClassificationCode,
                        _ => _.ZipCode.County,
                        _ => _.ZipCode.CountyANSI,
                        _ => _.ZipCode.CountyFIPS,
                        _ => _.ZipCode.CountyMixedCase,
                        _ => _.ZipCode.DayLightSaving,
                        _ => _.ZipCode.Division,
                        _ => _.ZipCode.Elevation,
                        _ => _.ZipCode.FacilityCode,
                        _ => _.ZipCode.FemalePopulation,
                        _ => _.ZipCode.FinanceNumber,
                        _ => _.ZipCode.HawaiianPopulation,
                        _ => _.ZipCode.HispanicPopulation,
                        _ => _.ZipCode.HouseholdsPerZipCode,
                        _ => _.ZipCode.IncomePerHousehold,
                        _ => _.ZipCode.IndianPopulation,
                        _ => _.ZipCode.Latitude,
                        _ => _.ZipCode.Longitude,
                        _ => _.ZipCode.MSA,
                        _ => _.ZipCode.MSA_Name,
                        _ => _.ZipCode.MailingName,
                        _ => _.ZipCode.MalePopulation,
                        _ => _.ZipCode.MultiCounty,
                        _ => _.ZipCode.OtherPopulation,
                        _ => _.ZipCode.PMSA,
                        _ => _.ZipCode.PMSA_Name,
                        _ => _.ZipCode.PersonsPerHousehold,
                        _ => _.ZipCode.Population,
                        _ => _.ZipCode.PreferredLastLineKey,
                        _ => _.ZipCode.PrimaryRecord,
                        _ => _.ZipCode.Region,
                        _ => _.ZipCode.State,
                        _ => _.ZipCode.StateANSI,
                        _ => _.ZipCode.StateFIPS,
                        _ => _.ZipCode.StateFullName,
                        _ => _.ZipCode.TimeZone,
                        _ => _.ZipCode.UniqueZIPName,
                        _ => _.ZipCode.WhitePopulation,
                        _ => _.CountryName,
                        _ => _.StateName,
                        _ => _.CityName,
                        _ => _.CountyName
                        );

                });
        }
    }
}