﻿using HomeInv.Common.ServiceContracts.Item;

namespace HomeInv.Common.Interfaces.Services
{
    public interface IItemService<D> : IItemService, IServiceBase<D, Entities.ItemEntity>
        where D : class
    {
    }

    public interface IItemService : IServiceBase
    {
        //GetAllItemsInHomeResponse GetAllItemsInHome(GetAllItemsInHomeRequest request, bool includeInactive);

        //GetItemResponse GetItem(GetItemRequest);

        CreateItemResponse CreateItem(CreateItemRequest request);
    }
}
