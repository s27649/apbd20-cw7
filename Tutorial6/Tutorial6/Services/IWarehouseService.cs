using Tutorial6.Models;

namespace Tutorial6.Services;

public interface IWarehouseService
{
    public Task<int> AddWarehouseProduct(Warehouse warehouse);
}