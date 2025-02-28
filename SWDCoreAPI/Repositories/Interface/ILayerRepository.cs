using SWDCoreAPI.Models;
using System.Threading.Tasks;


public interface ILayerRepository
{
    Task<string> GetLayerType(string layerName);
    Task<int> AddOrUpdateLayer(LayerFormData data);
    Task<int> LoginToPortal(Login data);
    Task<int> SignupUser(SignupModel data);
    Task<int> UpdateUser(UserModel data);
    Task<int> DeleteUser(int id);

    Task<List<UserModel>> GetAllUsers();

}
