using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace TriaDemo.RestApi.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
public class AuthenticationController : ControllerBase
{
    
}