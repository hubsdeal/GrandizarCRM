using SoftGrid.Authorization.Roles;

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
    [AbpAuthorize(AppPermissions.Pages_OrderStatuses)]
    public class OrderStatusesAppService : SoftGridAppServiceBase, IOrderStatusesAppService
    {
        private readonly IRepository<OrderStatus, long> _orderStatusRepository;
        private readonly IOrderStatusesExcelExporter _orderStatusesExcelExporter;
        private readonly IRepository<Role, int> _lookup_roleRepository;

        public OrderStatusesAppService(IRepository<OrderStatus, long> orderStatusRepository, IOrderStatusesExcelExporter orderStatusesExcelExporter, IRepository<Role, int> lookup_roleRepository)
        {
            _orderStatusRepository = orderStatusRepository;
            _orderStatusesExcelExporter = orderStatusesExcelExporter;
            _lookup_roleRepository = lookup_roleRepository;

        }

        public async Task<PagedResultDto<GetOrderStatusForViewDto>> GetAll(GetAllOrderStatusesInput input)
        {

            var filteredOrderStatuses = _orderStatusRepository.GetAll()
                        .Include(e => e.RoleFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.ColorCode.Contains(input.Filter) || e.Message.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.MinSequenceNoFilter != null, e => e.SequenceNo >= input.MinSequenceNoFilter)
                        .WhereIf(input.MaxSequenceNoFilter != null, e => e.SequenceNo <= input.MaxSequenceNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ColorCodeFilter), e => e.ColorCode.Contains(input.ColorCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MessageFilter), e => e.Message.Contains(input.MessageFilter))
                        .WhereIf(input.DeliveryOrPickupFilter.HasValue && input.DeliveryOrPickupFilter > -1, e => (input.DeliveryOrPickupFilter == 1 && e.DeliveryOrPickup) || (input.DeliveryOrPickupFilter == 0 && !e.DeliveryOrPickup))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RoleNameFilter), e => e.RoleFk != null && e.RoleFk.Name == input.RoleNameFilter);

            var pagedAndFilteredOrderStatuses = filteredOrderStatuses
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var orderStatuses = from o in pagedAndFilteredOrderStatuses
                                join o1 in _lookup_roleRepository.GetAll() on o.RoleId equals o1.Id into j1
                                from s1 in j1.DefaultIfEmpty()

                                select new
                                {

                                    o.Name,
                                    o.Description,
                                    o.SequenceNo,
                                    o.ColorCode,
                                    o.Message,
                                    o.DeliveryOrPickup,
                                    Id = o.Id,
                                    RoleName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                                };

            var totalCount = await filteredOrderStatuses.CountAsync();

            var dbList = await orderStatuses.ToListAsync();
            var results = new List<GetOrderStatusForViewDto>();

            foreach (var o in dbList)
            {
                var res = new GetOrderStatusForViewDto()
                {
                    OrderStatus = new OrderStatusDto
                    {

                        Name = o.Name,
                        Description = o.Description,
                        SequenceNo = o.SequenceNo,
                        ColorCode = o.ColorCode,
                        Message = o.Message,
                        DeliveryOrPickup = o.DeliveryOrPickup,
                        Id = o.Id,
                    },
                    RoleName = o.RoleName
                };

                results.Add(res);
            }

            return new PagedResultDto<GetOrderStatusForViewDto>(
                totalCount,
                results
            );

        }

        public async Task<GetOrderStatusForViewDto> GetOrderStatusForView(long id)
        {
            var orderStatus = await _orderStatusRepository.GetAsync(id);

            var output = new GetOrderStatusForViewDto { OrderStatus = ObjectMapper.Map<OrderStatusDto>(orderStatus) };

            if (output.OrderStatus.RoleId != null)
            {
                var _lookupRole = await _lookup_roleRepository.FirstOrDefaultAsync((int)output.OrderStatus.RoleId);
                output.RoleName = _lookupRole?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_OrderStatuses_Edit)]
        public async Task<GetOrderStatusForEditOutput> GetOrderStatusForEdit(EntityDto<long> input)
        {
            var orderStatus = await _orderStatusRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetOrderStatusForEditOutput { OrderStatus = ObjectMapper.Map<CreateOrEditOrderStatusDto>(orderStatus) };

            if (output.OrderStatus.RoleId != null)
            {
                var _lookupRole = await _lookup_roleRepository.FirstOrDefaultAsync((int)output.OrderStatus.RoleId);
                output.RoleName = _lookupRole?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditOrderStatusDto input)
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

        [AbpAuthorize(AppPermissions.Pages_OrderStatuses_Create)]
        protected virtual async Task Create(CreateOrEditOrderStatusDto input)
        {
            var orderStatus = ObjectMapper.Map<OrderStatus>(input);

            if (AbpSession.TenantId != null)
            {
                orderStatus.TenantId = (int?)AbpSession.TenantId;
            }

            await _orderStatusRepository.InsertAsync(orderStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderStatuses_Edit)]
        protected virtual async Task Update(CreateOrEditOrderStatusDto input)
        {
            var orderStatus = await _orderStatusRepository.FirstOrDefaultAsync((long)input.Id);
            ObjectMapper.Map(input, orderStatus);

        }

        [AbpAuthorize(AppPermissions.Pages_OrderStatuses_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _orderStatusRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetOrderStatusesToExcel(GetAllOrderStatusesForExcelInput input)
        {

            var filteredOrderStatuses = _orderStatusRepository.GetAll()
                        .Include(e => e.RoleFk)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false || e.Name.Contains(input.Filter) || e.Description.Contains(input.Filter) || e.ColorCode.Contains(input.Filter) || e.Message.Contains(input.Filter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name.Contains(input.NameFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter), e => e.Description.Contains(input.DescriptionFilter))
                        .WhereIf(input.MinSequenceNoFilter != null, e => e.SequenceNo >= input.MinSequenceNoFilter)
                        .WhereIf(input.MaxSequenceNoFilter != null, e => e.SequenceNo <= input.MaxSequenceNoFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.ColorCodeFilter), e => e.ColorCode.Contains(input.ColorCodeFilter))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.MessageFilter), e => e.Message.Contains(input.MessageFilter))
                        .WhereIf(input.DeliveryOrPickupFilter.HasValue && input.DeliveryOrPickupFilter > -1, e => (input.DeliveryOrPickupFilter == 1 && e.DeliveryOrPickup) || (input.DeliveryOrPickupFilter == 0 && !e.DeliveryOrPickup))
                        .WhereIf(!string.IsNullOrWhiteSpace(input.RoleNameFilter), e => e.RoleFk != null && e.RoleFk.Name == input.RoleNameFilter);

            var query = (from o in filteredOrderStatuses
                         join o1 in _lookup_roleRepository.GetAll() on o.RoleId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()

                         select new GetOrderStatusForViewDto()
                         {
                             OrderStatus = new OrderStatusDto
                             {
                                 Name = o.Name,
                                 Description = o.Description,
                                 SequenceNo = o.SequenceNo,
                                 ColorCode = o.ColorCode,
                                 Message = o.Message,
                                 DeliveryOrPickup = o.DeliveryOrPickup,
                                 Id = o.Id
                             },
                             RoleName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                         });

            var orderStatusListDtos = await query.ToListAsync();

            return _orderStatusesExcelExporter.ExportToFile(orderStatusListDtos);
        }

        [AbpAuthorize(AppPermissions.Pages_OrderStatuses)]
        public async Task<PagedResultDto<OrderStatusRoleLookupTableDto>> GetAllRoleForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_roleRepository.GetAll().WhereIf(
                   !string.IsNullOrWhiteSpace(input.Filter),
                  e => e.Name != null && e.Name.Contains(input.Filter)
               );

            var totalCount = await query.CountAsync();

            var roleList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<OrderStatusRoleLookupTableDto>();
            foreach (var role in roleList)
            {
                lookupTableDtoList.Add(new OrderStatusRoleLookupTableDto
                {
                    Id = role.Id,
                    DisplayName = role.Name?.ToString()
                });
            }

            return new PagedResultDto<OrderStatusRoleLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

    }
}