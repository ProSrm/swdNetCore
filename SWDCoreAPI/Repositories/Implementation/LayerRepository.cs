using System.Data;
using System.Data.SqlClient;
using SWDCoreAPI.Models;


public class LayerRepository : ILayerRepository
{
    private readonly string _connectionString;

    public LayerRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("DefaultConnection");
    }

    public async Task<string> GetLayerType(string layerName)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand("spLayerMaster_GetLayerGeomType", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LayerName", layerName);

                var result = await cmd.ExecuteScalarAsync();
                return result?.ToString();
            }
        }
    }

    public async Task<int> AddOrUpdateLayer(LayerFormData data)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();
            using (SqlCommand cmd = new SqlCommand("spAddUpdateLayerFormData", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // cmd.Parameters.AddWithValue("@LayerName", data.LayerName);
                cmd.Parameters.AddWithValue("@Longitude", data.Longitude);
                cmd.Parameters.AddWithValue("@Latitude", data.Latitude);
                cmd.Parameters.AddWithValue("@ImagePath", data.ImagePath ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@CreatedBy", data.CreatedBy);
                cmd.Parameters.AddWithValue("@ModifiedBy", data.ModifiedBy);
                cmd.Parameters.AddWithValue("@QgsFid", data.QgsFid ?? (object)DBNull.Value);

                return await cmd.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task<int> LoginToPortal(Login data)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            using (SqlCommand cmd = new SqlCommand("spLoginToPortal", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserName", data.UserName);
                cmd.Parameters.AddWithValue("@Password", data.Password); 

             
                var result = await cmd.ExecuteScalarAsync();

                if (result != null && result!= DBNull.Value)
                    return 1;
                else
                    return 0;
                
            }
        }
    }

}
