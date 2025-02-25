using SWDCoreAPI.Models;
using System.Threading.Tasks;

public interface ILayerService
{
    Task<string> GetLayerType(string layerName);
    Task<int> AddOrUpdateLayer(LayerFormData data);
    Task<int> LoginToPortal(Login data);
}
