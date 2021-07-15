﻿namespace HomeInv.Common.Interfaces.Services
{
    public interface IItemStockService<D> : IItemStockService, IServiceBase<D, Entities.ItemStockEntity>
        where D : class
    {
    }

    public interface IItemStockService : IServiceBase
    {
    }
}