namespace GoodHamburger.Web.Services;

public sealed record ApiOperationResult(bool IsSuccess, string? ErrorMessage = null);
