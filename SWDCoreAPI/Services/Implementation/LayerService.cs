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
}
