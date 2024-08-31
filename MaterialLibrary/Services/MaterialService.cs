
//using CommonLibrary.Services;

using CommonLibrary;

using MaterialLibrary.DTOs;
using MaterialLibrary.Models;


using Microsoft.EntityFrameworkCore;

using System.Text.Json;

namespace MaterialLibrary.Services
{
    public class MaterialService : IMaterialService
    {
        MaterialDbContext dbContext;
        IMaterialChangeNotification notification;

        public MaterialService(MaterialDbContext dbContext, IMaterialChangeNotification notification)
        {
            this.dbContext = dbContext;
            this.notification = notification;
        }

        #region MaterialTypes

        public async Task SendAllMaterialTypesAsync()
        {
            var allMaterialTypes = await dbContext.MaterialTypes.ToListAsync();

            await notification.SendMaterialTypeChangeNotification(JsonSerializer.Serialize(allMaterialTypes), NotificationType.AllRecords);
        }

        public async Task<MaterialType> AddNewMaterialTypeAsync(NewMaterialTypeDTO newMatType)
        {
            MaterialType materialType = new MaterialType()
            {
                Name = newMatType.Name,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = newMatType.IsActive
            };

            var newMaterialType = (await dbContext.AddAsync(materialType)).Entity;
            await dbContext.SaveChangesAsync();

            await notification.SendMaterialChangeNotification(JsonSerializer.Serialize(newMaterialType), NotificationType.Added);

            return newMaterialType;
        }

        public async Task<MaterialType> UpdateMaterialTypeAsync(NewMaterialTypeDTO newMatType)
        {
            MaterialType materialType = new()
            {
                Name = newMatType.Name,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = newMatType.IsActive
            };

            dbContext.MaterialTypes.Update(materialType);
            await dbContext.SaveChangesAsync();

            await notification.SendMaterialChangeNotification(JsonSerializer.Serialize(materialType), NotificationType.Updated);

            return materialType;
        }

        public async Task<bool> DeleteMaterialTypeAsync(int materialTypeId)
        {
            try
            {
                var materialType = await dbContext.MaterialTypes.Where(type => type.Id == materialTypeId).FirstOrDefaultAsync();

                if (materialType != null)
                {
                    //dbContext.MaterialTypes.Remove(materialType);

                    materialType.IsActive = false;

                    await dbContext.SaveChangesAsync();

                    await notification.SendMaterialChangeNotification(JsonSerializer.Serialize(materialType), NotificationType.Updated);

                    return true;
                }
            }
            catch (Exception ex)
            {
                throw new NotFoundException("Material Type is not deleted..!! - " + ex.Message);
            }

            return false;
        }

        public async Task<IEnumerable<MaterialType>?> GetMaterialTypesAsync() => await dbContext.MaterialTypes.ToListAsync();

        public async Task<bool> UpdateMaterialTypeAsync(MaterialType updateMaterialType)
        {
            try
            {
                dbContext.MaterialTypes.Entry(updateMaterialType).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new NotFoundException("Material is not updated..!! - " + ex.Message);
            }

        }

        #endregion

        #region Materials

        //public async Task<IEnumerable<MaterialDTO>> GetallMaterialsAsync()
        //{
        //    var materials = await (from material in dbContext.Materials
        //                     join materialType in dbContext.MaterialTypes on material.MaterialType equals materialType.Id
        //                     select new MaterialDTO
        //                     {
        //                         Id = material.Id,
        //                         Name = material.Name,
        //                         MaterialType = materialType.Name,
        //                         MaterialTypeId = materialType.Id,
        //                         CreatedAt = material.CreatedAt,
        //                         UpdatedAt = material.UpdatedAt,
        //                         IsActive = material.IsActive
        //                     }).ToListAsync();

        //    return materials;
        //}

        public async Task<Material> AddNewMaterialAsync(NewMaterialDTO newMaterialDto)
        {
            Material material = new()
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = true,
                Name = newMaterialDto.Name,
                MaterialType = newMaterialDto.MaterialType
            };

            var newMaterial = (await dbContext.Materials.AddAsync(material)).Entity;
            await dbContext.SaveChangesAsync();
            return newMaterial;
        }

        public async Task<bool> UpdateMaterialAsync(Material updateMaterial)
        {
            try
            {
                dbContext.Materials.Entry(updateMaterial).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new NotFoundException("Material is not updated..!! - " + ex.Message);
            }

        }

        public async Task<bool> DeleteMaterialAsync(int materialId)
        {
            try
            {
                var material = await dbContext.Materials.Where(m => m.Id == materialId).FirstOrDefaultAsync();
                dbContext.Materials.Entry(material!).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new NotFoundException("Material is not deleted..!! - " + ex.Message);
            }
        }

        public async Task SendAllMaterialsAsync()
        {
            var allMaterials = await dbContext.Materials.ToListAsync();

            await notification.SendMaterialChangeNotification(JsonSerializer.Serialize(allMaterials), NotificationType.AllRecords);
        }

        #endregion
    }
}
