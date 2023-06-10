using HomeInv.Common.ServiceContracts;
using HomeInv.Language;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace WebUI.Base
{
    public abstract class BasePageModel<T> : PageModelBase<T>
    {
        public BasePageModel(ILogger<T> logger, HomeInventoryDbContext dbContext) : base(logger, dbContext)
        {
        }

        public virtual IActionResult OnPost()
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                dbContext.Database.BeginTransaction();
                var result = OnModelPost();
                if (_serviceCallResponse.IsSuccessful)
                {
                    SetSuccessMessage(_serviceCallSuccessMessage);
                    dbContext.Database.CommitTransaction();
                }
                else
                {
                    result = Page();
                    SetErrorMessage(_serviceCallResponse.Result.ToString());
                    dbContext.Database.RollbackTransaction();
                }

                return result;
            }
            catch(Exception ex)
            {
                dbContext.Database.RollbackTransaction();

                var error = TempData.Peek("Error");
                if (error == null || string.IsNullOrEmpty(error.ToString())) SetErrorMessage("İşleminiz başarıyla gerçekleşemedi. Developer herhangi bir hata da belirtmemiş. Elimizde sadece bu var. | " + ex.Message + " | " + ex.StackTrace);

                return Page();
            }
        }

        private BaseResponse _serviceCallResponse { get; set; }
        private string _serviceCallSuccessMessage { get; set; }
        protected TResponse CallService<TRequest, TResponse>(Func<TRequest, TResponse> serviceMethod, TRequest serviceRequest, string successMessage = null)
            where TResponse : BaseResponse
        {
            if (string.IsNullOrEmpty(successMessage)) successMessage = Resources.Success_Generic;
            _serviceCallSuccessMessage = successMessage;

            _serviceCallResponse = serviceMethod.Invoke(serviceRequest);

            return (TResponse)_serviceCallResponse;
        }

        protected virtual IActionResult OnModelPost()
        {
            throw new NotImplementedException();
        }
    }
}
