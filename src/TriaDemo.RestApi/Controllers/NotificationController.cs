using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace TriaDemo.RestApi.Controllers;

[ApiController]
[Route("api/notifications")]
[Produces(MediaTypeNames.Application.Json)]
public class NotificationController : ControllerBase
{
    
}