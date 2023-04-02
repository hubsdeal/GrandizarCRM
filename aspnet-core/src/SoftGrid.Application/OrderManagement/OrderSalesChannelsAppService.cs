using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using SoftGrid.OrderManagement.Exporting;
using SoftGrid.OrderManagement.Dtos;
using SoftGrid.Dto;
using Abp.Application.Services.Dto;
using SoftGrid.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using SoftGrid.Storage;

namespace SoftGrid.OrderManagement
{
    [AbpAuthorize(AppPermissions.Pages_OrderSalesChannels)]
    public class OrderSalesChannelsAppService : SoftGridAppServiceBase, IOrderSalesChannelsAppService
    {
        private readonly IRepository<OrderSalesChannel, long> _orderSalesChannelRepository;
        private readonly IOrderSalesChannelsExcelExporter _orderSalesChannelsExcelExporter;

        public OrderSalesChannelsAppService(IRepository<OrderSalesChannel, long> orderSalesChannelRepository, IOrderSalesChannelsExcelExporter orderSalesChannelsExcelExporter)
        {
            _orderSalesChannelRepository = orderSalesChannelRepository;
            _orderSalesChannelsExcelExporter = orderSalesChannelsExcelExporter;

        }

        public async Task<PagedResultDto<GetOrderSalesChannelForViewDto>> GetAll(GetAllOrderSalesChannelsInput input)
        {

            var filteredOrderSalesChannels = _orderSalesChannelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.LinkName.Contains(input.Filter) || e.ApiLink.Contains(input.Filter) || e.UserId.Contains(input.Filter) || e.Password.Contains(input.Filter) || e.Notes.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LinkNameFilter), e => e.LinkName.Contains(input.LinkNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ApiLinkFilter), e => e.ApiLink.Contains(input.ApiLinkFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserIdFilter), e => e.UserId.Contains(input.UserIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PasswordFilter), e => e.Password.Contains(input.PasswordFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter));

            var pagedAndFilteredOrderSalesChannels = filteredOrderSalesChannels
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orderSalesChannels = from o in pagedAndFilteredOrderSalesChannels
                                     select new
                                     {

                                         o.Name,
                                         o.LinkName,
                                         o.ApiLink,
                                         o.UserId,
                                         o.Password,
                                         o.Notes,
                                         Id = o.Id
                                     };

            var totalCount = await filteredOrderSalesChannels.CountAsync();

            var dbList = await orderSalesChannels.ToListAsync();
            var results = new List<GetOrderSalesChannelForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderSalesChannelForViewDto()
                {
                    OrderSalesChannel = new OrderSalesChannelDto
                    {

                        Name = o.Name,
                        LinkName = o.LinkName,
                        ApiLink = o.ApiLink,
                        UserId = o.UserId,
                        Password = o.Password,
                        Notes = o.Notes,
                        Id = o.Id,
                    }
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderSalesChannelForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOrderSalesChannelForViewDto> GetOrderSalesChannelForView(long id)
        {
            var orderSalesChannel = await _orderSalesChannelRepository.GetAsync(id);

            var output = new GetOrderSalesChannelForViewDto { OrderSalesChannel = ObjectMapper.Map<OrderSalesChannelDto>(orderSalesChannel) };

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OrderSalesChannels_Edit)]
        public async Task<GetOrderSalesChannelForEditOutput> GetOrderSalesChannelForEdit(EntityDto<long> input)
        {
            var orderSalesChannel = await _orderSalesChannelRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderSalesChannelForEditOutput { OrderSalesChannel = ObjectMapper.Map<CreateOrEditOrderSalesChannelDto>(orderSalesChannel) };

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOrderSalesChannelDto input)
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

        [AbpAuthorize(AppPermissions.Pages_OrderSalesChannels_Create)]
        protected virtual async Task Create(CreateOrEditOrderSalesChannelDto input)
        {
            var orderSalesChannel = ObjectMapper.Map<OrderSalesChannel>(input);

            if (AbpSession.TenantId != null)
            {
                orderSalesChannel.TenantId = (int?)AbpSession.TenantId;
            }

            await _orderSalesChannelRepository.InsertAsync(orderSalesChannel);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderSalesChannels_Edit)]
        protected virtual async Task Update(CreateOrEditOrderSalesChannelDto input)
        {
            var orderSalesChannel = await _orderSalesChannelRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, orderSalesChannel);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderSalesChannels_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _orderSalesChannelRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOrderSalesChannelsToExcel(GetAllOrderSalesChannelsForExcelInput input)
        {

            var filteredOrderSalesChannels = _orderSalesChannelRepository.GetAll()
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.LinkName.Contains(input.Filter) || e.ApiLink.Contains(input.Filter) || e.UserId.Contains(input.Filter) || e.Password.Contains(input.Filter) || e.Notes.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.LinkNameFilter), e => e.LinkName.Contains(input.LinkNameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ApiLinkFilter), e => e.ApiLink.Contains(input.ApiLinkFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserIdFilter), e => e.UserId.Contains(input.UserIdFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.PasswordFilter), e => e.Password.Contains(input.PasswordFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NotesFilter), e => e.Notes.Contains(input.NotesFilter));

            var query = (from o in filteredOrderSalesChannels
                         select new GetOrderSalesChannelForViewDto()
                         {
                             OrderSalesChannel = new OrderSalesChannelDto
                             {
                                 Name = o.Name,
                                 LinkName = o.LinkName,
                                 ApiLink = o.ApiLink,
                                 UserId = o.UserId,
                                 Password = o.Password,
                                 Notes = o.Notes,
                                 Id = o.Id
                             }
                         });

            var orderSalesChannelListDtos = await query.ToListAsync();

            return _orderSalesChannelsExcelExporter.ExportToFile(orderSalesChannelListDtos);
        }

    }
}