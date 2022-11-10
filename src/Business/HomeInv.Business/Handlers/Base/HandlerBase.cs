using HomeInv.Common.Interfaces.Handlers;
using HomeInv.Common.ServiceContracts;
using HomeInv.Persistence;

namespace HomeInv.Business.Handlers
{
    public abstract class HandlerBase<T, R> : HandlerBase
        where T : BaseRequest
        where R : BaseResponse, new()
    {
        protected HandlerBase(HomeInventoryDbContext context) : base(context)
        {
        }

        public R Execute(T request)
        {
            var response = new R();

            try
            {
                if ((response = ValidateRequest(request, response)).IsSuccessful)
                {
                    _context.Database.BeginTransaction();
                    response = ExecuteInternal(request, response);
                    if (response.IsSuccessful)
                    {
                        _context.Database.CommitTransaction();
                    }
                    else
                    {
                        _context.Database.RollbackTransaction();
                    }
                }
            }
            catch
            {
                _context.Database.RollbackTransaction();
            }

            return response;
        }

        protected abstract R ValidateRequest(T request, R response);
        protected abstract R ExecuteInternal(T request, R response);
    }

    public abstract class HandlerBase : IHandlerBase
    {
        protected readonly HomeInventoryDbContext _context;

        public HandlerBase(HomeInventoryDbContext context)
        {
            _context = context;
        }
    }
}
