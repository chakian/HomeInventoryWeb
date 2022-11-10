using HomeInv.Common.ServiceContracts;
using HomeInv.Common.ServiceContracts.Home;
using HomeInv.Language;
using HomeInv.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;

namespace WebUI.Base
{
    public abstract class BasePageModel<T> : PageModel
    {
        private readonly HomeInventoryDbContext dbContext;
        protected readonly ILogger<T> logger;
        private const string DEFAULT_LANDING_PAGE_FOR_LOGGEDIN = "/ItemDefinition/List";

        public BasePageModel(ILogger<T> logger, HomeInventoryDbContext dbContext)
        {
            this.logger = logger;
            this.dbContext = dbContext;
        }

        protected string UserId
        {
            get
            {
                string id = "";
                if (User != null && User.Identity.IsAuthenticated)
                {
                    var claim = User.FindFirst(ClaimTypes.NameIdentifier);
                    if (claim != null)
                    {
                        id = claim.Value;
                    }
                }
                return id;
            }
        }

        protected void SetErrorMessage(string message)
        {
            var key = "Error";
            AddOrUpdateTempData(key, message);
        }
        protected void SetSuccessMessage(string message)
        {
            var key = "Success";
            AddOrUpdateTempData(key, message);
        }
        protected void SetInfoMessage(string message)
        {
            var key = "Info";
            AddOrUpdateTempData(key, message);
        }
        protected void SetWarningMessage(string message)
        {
            var key = "Warning";
            AddOrUpdateTempData(key, message);
        }
        private void AddOrUpdateTempData(string key, string message)
        {
            var _tempData = TempData.Peek(key);
            string currentText = (_tempData != null) ? _tempData.ToString() : "";
            if (string.IsNullOrEmpty(currentText.ToString()))
            {
                TempData.Add(key, message);
            }
            else if(!currentText.Contains(message))
            {
                TempData.Remove(key);
                currentText += " " + message;
                TempData.Add(key, currentText);
            }
        }

        public IActionResult OnPost()
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
            catch
            {
                dbContext.Database.RollbackTransaction();

                var error = TempData.Peek("Error");
                if (error == null || string.IsNullOrEmpty(error.ToString())) SetErrorMessage("İşleminiz başarıyla gerçekleşemedi. Developer herhangi bir hata da belirtmemiş. Elimizde sadece bu var.");

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

        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            if (User != null && User.Identity.IsAuthenticated && (context.ActionDescriptor.ViewEnginePath == "/" || context.ActionDescriptor.ViewEnginePath == "/Index"))
            {
                context.Result = RedirectToPage(DEFAULT_LANDING_PAGE_FOR_LOGGEDIN);
            }
            base.OnPageHandlerExecuting(context);
        }
    }
}
