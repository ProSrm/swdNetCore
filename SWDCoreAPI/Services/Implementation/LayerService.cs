using SWDCoreAPI.Models;
using System.Threading.Tasks;

public class LayerService : ILayerService
{
    private readonly ILayerRepository _layerRepository;

    public LayerService(ILayerRepository layerRepository)
    {
        _layerRepository = layerRepository;
    }

    public async Task<string> GetLayerType(string layerName)
    {
        return await _layerRepository.GetLayerType(layerName);
    }
    public async Task<int> AddOrUpdateLayer(LayerFormData data)
    {
        return await _layerRepository.AddOrUpdateLayer(data);
    }
    public async Task<int> LoginToPortal(Login data)
    {
        return await _layerRepository.LoginToPortal(data);
    }
    public async Task<int> UpdateUser(UserModel data)
    {
        return await _layerRepository.UpdateUser(data);
    }

    public async Task<List<UserModel>> GetAllUsers()
    {
        return await _layerRepository.GetAllUsers();
    }

    public Task<int> SignupUser(SignupModel data)
    {
        throw new NotImplementedException();
    }
}
