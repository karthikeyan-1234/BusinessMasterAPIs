using PurchaseLibrary.Models;

namespace PurchaseLibrary.Services
{
    public interface IMaterialService
    {
        Task UpsertAllMaterialsAsync(List<Material> materials);
        Task UpsertAllMaterialTypesAsync(List<MaterialType> materialTypes);
        Task UpsertMaterialAsync(Material material);
        Task UpsertMaterialTypeAsync(MaterialType materialType);
    }
}