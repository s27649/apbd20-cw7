using Tutorial6.Models;

namespace Tutorial6.Repositories;

public interface IWarehouseRepository
{
    Task<int> AddWarehouseProduct(Warehouse warehouse);

}