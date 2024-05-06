using Microsoft.AspNetCore.Mvc;
using Tutorial6.Models;
using Tutorial6.Services;

namespace Tutorial6.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WarehouseController : ControllerBase
{
    public readonly WarehouseService _WarehouseService;

    public WarehouseController(WarehouseService warehouseService)
    {
        _WarehouseService = warehouseService;
    }

    [HttpPost]
    public async Task<IActionResult> AddWarehouse(Warehouse warehouse)
    {
        var res = await _WarehouseService.AddWarehouseProduct(warehouse);
        return StatusCode(StatusCodes.Status201Created);
    }

}