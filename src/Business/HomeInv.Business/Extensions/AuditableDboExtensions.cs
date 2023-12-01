using HomeInv.Common.ServiceContracts;
using HomeInv.Persistence.Dbo;
using System;

namespace HomeInv.Business.Extensions;

public static class AuditableDboExtensions
{
    public static T SetCreateAuditValues<T>(this T auditableDbo, BaseRequest baseRequest, bool createAsActive = true)
        where T : BaseAuditableDbo
    {
        auditableDbo.InsertUserId = baseRequest.RequestUserId;
        auditableDbo.InsertTime = DateTime.UtcNow;
        auditableDbo.IsActive = createAsActive;
        return auditableDbo;
    }

    public static T SetUpdateAuditValues<T>(this T auditableDbo, BaseRequest baseRequest)
        where T : BaseAuditableDbo
    {
        auditableDbo.UpdateUserId = baseRequest.RequestUserId;
        auditableDbo.UpdateTime = DateTime.UtcNow;
        return auditableDbo;
    }
}
