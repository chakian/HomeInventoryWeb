using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeInv.Common.Interfaces.Services
{
    public interface ICategoryService<D> : ICategoryService, IServiceBase<D, Entities.CategoryEntity>
        where D : class
    {
    }

    public interface ICategoryService : IServiceBase
    {
    }
}
