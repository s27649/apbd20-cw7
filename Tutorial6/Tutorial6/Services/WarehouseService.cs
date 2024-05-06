using Tutorial6.Models;
using Tutorial6.Repositories;

namespace Tutorial6.Services;

public class WarehouseService :IWarehouseService
{
    public readonly WarehouseRepository _WarehouseRepository;

    public WarehouseService(WarehouseRepository warehouseRepository)
    {
        _WarehouseRepository = warehouseRepository;
    }

    public async Task<int> AddWarehouseProduct(Warehouse warehouse)
    {
        // if (await _WarehouseRepository.FullExistMetods(warehouse.IdProduct, warehouse.Amount, warehouse.CreatedAt, warehouse.IdWarehouse))
        // {
        //     return -1;
        // }
        return await _WarehouseRepository.AddWarehouseProduct(warehouse);
    }
}