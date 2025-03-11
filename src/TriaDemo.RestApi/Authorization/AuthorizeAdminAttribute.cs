using Microsoft.AspNetCore.Authorization;

namespace TriaDemo.RestApi.Authorization;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
public sealed class AuthorizeAdminAttribute() : AuthorizeAttribute("IsAdmin");