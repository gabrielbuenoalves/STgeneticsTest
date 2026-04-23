using GoodHamburger.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.Api.Controllers;

[ApiController]
[Route("api/menu")]
public sealed class MenuController(IMenuService menuService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var items = await menuService.ListAsync(cancellationToken);
        return Ok(items);
    }
}
