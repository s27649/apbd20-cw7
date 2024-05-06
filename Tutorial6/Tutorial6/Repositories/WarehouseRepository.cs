using Microsoft.Data.SqlClient;
using Tutorial6.Models;

namespace Tutorial6.Repositories;

public class WarehouseRepository:IWarehouseRepository
{
    
    private readonly IConfiguration _configuration;
    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<bool> DoesProductExist(int id)
    {
        var query = "SELECT 1 FROM Product WHERE IdProduct=@IdProduct";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdProduct", id);
        await connection.OpenAsync();
        var res = await command.ExecuteScalarAsync();
        return res is not null;
        
    }
    
    public async Task<bool> DoesWarehouseExist(int id)
    {
        var query = "SELECT 1 FROM Warehouse WHERE IdWarehouse=@IdWarehouse";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdIdWarehouse", id);
        await connection.OpenAsync();
        var res = await command.ExecuteScalarAsync();
        return res is not null;
    }

    public async Task<bool> AmountMoreThenZero(int amount)
    {
        return amount > 0;
    }

    public async Task CheckOrderByIdAndAmount(int idProduct, int amount, DateTime createdAt)
    {
        var query = @"SELECT 1 FROM [Order] o 
                      JOIN  Product_Warehouse p ON o.IdOrder=p.IdOrder
                      WHERE o.Amount=@Amount AND o.IdProduct=@IdProduct AND o.FulfilledAt IS NULL AND o.CreatedAt<@CreatedAt";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdProduct", idProduct);
        command.Parameters.AddWithValue("@Amount", amount);
        command.Parameters.AddWithValue("@CreatedAt", createdAt);
        
        await connection.OpenAsync();
        
        await command.ExecuteScalarAsync();
        
    }

    public async Task<bool> DoesOrderNoTaskExist(int id)
    {
        var query = "SELECT 1 FROM Product_Warehouse WHERE IdOrder=@Id ";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@Id", id);
        await connection.OpenAsync();
        var res = await command.ExecuteScalarAsync();
        return res is null;
    }
    
    
    public async Task UpdateFullfilledAt(int idOrder, DateTime createdAt)
    {
        var query = "UPDATE [Order] SET FulfilledAt=@CreatedAt WHERE IdOrder=@IdOrder";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdOrder", idOrder);
        command.Parameters.AddWithValue("@CreatedAt", createdAt);
    
        await connection.OpenAsync();
        
        await command.ExecuteNonQueryAsync();
    }
    
    public async Task<int> InsertInProductWarehouse(int idOrder,Warehouse warehouse)
    {
        
        var query1 = "Select Price from Product_Warehouse Where IdOrder=@IdOrder ";
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        decimal price = (decimal)await command.ExecuteScalarAsync();
        command.Connection = connection;
        command.CommandText = query1;
        command.Parameters.AddWithValue("@IdOrder", idOrder);
        await connection.OpenAsync();
        
        var query = @" INSERT INTO Product_Warehouse(IdWarehouse,IdProduct, IdOrder, Amount, Price, CreatedAt) 
     OUTPUT INSERTED.IdProductWarehouse VALUES(@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Amount*@Price, @CreatedAt)";
        using SqlConnection con = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand com = new SqlCommand();
        com.Connection = con;
        com.CommandText = query;
        com.Parameters.AddWithValue("@IdOrder", idOrder);
        com.Parameters.AddWithValue("@IdWarehouse", warehouse.IdWarehouse);
        com.Parameters.AddWithValue("@CreatedAt", warehouse.CreatedAt);
        com.Parameters.AddWithValue("@Amount*@Price", price);
        com.Parameters.AddWithValue("@Amount", warehouse.Amount);
        com.Parameters.AddWithValue("@IdProduct", warehouse.IdProduct);
    
        con.OpenAsync();
        var res = await com.ExecuteScalarAsync();
        return Convert.ToInt32(res);
    }
    
    public async Task<bool> FullExistMetods(int idProduct, int amount, DateTime createAt,int idWarehouse )
    {
        CheckOrderByIdAndAmount(idProduct, amount, createAt);
        bool warehouseExists =await DoesWarehouseExist(idWarehouse);
        bool productExists = await DoesProductExist(idProduct);
        bool amountExist = await AmountMoreThenZero(amount);
    
        return warehouseExists && productExists && amountExist;
    }
    public async Task<int> AddWarehouseProduct(Warehouse warehouse)
    {
        using SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        using SqlCommand command = new SqlCommand();
        await UpdateFullfilledAt(warehouse.IdProduct, warehouse.CreatedAt);
        await connection.OpenAsync();
        var res=await InsertInProductWarehouse(warehouse.IdProduct, warehouse);
        return res;
        
    }
    
    
}