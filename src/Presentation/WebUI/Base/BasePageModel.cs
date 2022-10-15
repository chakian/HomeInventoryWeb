﻿using HomeInv.Common.ServiceContracts;
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
            TempData.Add("Error", message);
        }
        protected void SetSuccessMessage(string message)
        {
            TempData.Add("Success", message);
        }
        protected void SetInfoMessage(string message)
        {
            TempData.Add("Info", message);
        }
        protected void SetWarningMessage(string message)
        {
            TempData.Add("Warning", message);
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
            base.OnPageHandlerExecuting(context);
        }
    }
}
