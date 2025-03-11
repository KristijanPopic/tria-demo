using Microsoft.AspNetCore.Authorization;

namespace TriaDemo.RestApi.Authorization;

public sealed class AuthorizeAdminAttribute() : AuthorizeAttribute("IsAdmin");