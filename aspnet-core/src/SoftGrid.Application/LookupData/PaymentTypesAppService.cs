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
    [AbpAuthorize(AppPermissions.Pages_PaymentTypes)]
    public class PaymentTypesAppService : SoftGridAppServiceBase, IPaymentTypesAppService
    {
        private readonly IRepository<PaymentType, long> _paymentTypeRepository;
        private readonly IPaymentTypesExcelExporter _paymentTypesExcelExporter;

        public PaymentTypesAppService(IRepository<PaymentType, long> paymentTypeRepository, IPaymentTypesExcelExporter paymentTypesExcelExporter)
        {
            _paymentTypeRepository = paymentTypeRepository;
            _paymentTypesExcelExporter = paymentTypesExcelExporter;

        }

        public async Task<PagedResultDto<GetPaymentTypeForViewDto>> GetAll(GetAllPaymentTypesInput input)
        {

            var filteredPaymentTypes = _paymentTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var pagedAndFilteredPaymentTypes = filteredPaymentTypes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var paymentTypes = from o in pagedAndFilteredPaymentTypes
                               select new
                               {

                                   o.Name,
                                   Id = o.Id
                               };

            var totalCount = await filteredPaymentTypes.CountAsync();

            var dbList = await paymentTypes.ToListAsync();
            var results = new List<GetPaymentTypeForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetPaymentTypeForViewDto()
                {
                    PaymentType = new PaymentTypeDto
                    {

                        Name = o.Name,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetPaymentTypeForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetPaymentTypeForViewDto> GetPaymentTypeForView(long id)
        {
            var paymentType = await _paymentTypeRepository.GetAsync(id);

            var output = new GetPaymentTypeForViewDto { PaymentType = ObjectMapper.Map<PaymentTypeDto>(paymentType) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_PaymentTypes_Edit)]
        public async Task<GetPaymentTypeForEditOutput> GetPaymentTypeForEdit(EntityDto<long> input)
        {
            var paymentType = await _paymentTypeRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetPaymentTypeForEditOutput { PaymentType = ObjectMapper.Map<CreateOrEditPaymentTypeDto>(paymentType) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditPaymentTypeDto input)
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

        [AbpAuthorize(AppPermissions.Pages_PaymentTypes_Create)]
        protected virtual async Task Create(CreateOrEditPaymentTypeDto input)
        {
            var paymentType = ObjectMapper.Map<PaymentType>(input);

            if (AbpSession.TenantId != null)
            {
                paymentType.TenantId = (int?)AbpSession.TenantId;
            }

            await _paymentTypeRepository.InsertAsync(paymentType);

        }

        [AbpAuthorize(AppPermissions.Pages_PaymentTypes_Edit)]
        protected virtual async Task Update(CreateOrEditPaymentTypeDto input)
        {
            var paymentType = await _paymentTypeRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, paymentType);

        }

        [AbpAuthorize(AppPermissions.Pages_PaymentTypes_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _paymentTypeRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetPaymentTypesToExcel(GetAllPaymentTypesForExcelInput input)
        {

            var filteredPaymentTypes = _paymentTypeRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter));

            var query = (from o in filteredPaymentTypes
                         select new GetPaymentTypeForViewDto()
                         {
                             PaymentType = new PaymentTypeDto
                             {
                                 Name = o.Name,
                                 Id = o.Id
                             }
                         });

            var paymentTypeListDtos = await query.ToListAsync();

            return _paymentTypesExcelExporter.ExportToFile(paymentTypeListDtos);
        }

    }
}