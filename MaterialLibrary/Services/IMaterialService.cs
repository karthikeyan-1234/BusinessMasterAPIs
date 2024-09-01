using MaterialLibrary.DTOs;
using MaterialLibrary.Models;

namespace MaterialLibrary.Services
{
    public interface IMaterialService
    {
        Task<Material> AddNewMaterialAsync(NewMaterialDTO newMaterialDto);
        Task<MaterialType> AddNewMaterialTypeAsync(NewMaterialTypeDTO newMatType);
        Task<bool> DeleteMaterialAsync(int materialId);
        Task<bool> DeleteMaterialTypeAsync(int materialTypeId);
        Task<IEnumerable<MaterialType>?> GetMaterialTypesAsync();
        Task<IEnumerable<MaterialDTO>?> GetallMaterialsAsync();
        Task SendAllMaterialsAsync();
        Task SendAllMaterialTypesAsync();
        Task<bool> UpdateMaterialAsync(Material updateMaterial);
        Task<bool> UpdateMaterialTypeAsync(MaterialType updateMaterialType);
        Task<MaterialType> UpdateMaterialTypeAsync(NewMaterialTypeDTO newMatType);
    }
}