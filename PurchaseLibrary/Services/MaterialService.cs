using Microsoft.EntityFrameworkCore;

using PurchaseLibrary.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseLibrary.Services
{
    public class MaterialService : IMaterialService
    {
        PurchaseDbContext dbContext;

        public MaterialService(PurchaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task UpsertAllMaterialTypesAsync(List<MaterialType> materialTypes)
        {
            var missingMaterialTypes = from matType in materialTypes
                                       join matType2 in dbContext.MaterialTypes.AsNoTracking() on matType.Id equals matType2.Id into matGroup
                                       from matType3 in matGroup.DefaultIfEmpty()
                                       where matType3 == null
                                       select matType;

            var matchingMaterialTypes = from matType in materialTypes
                                        join matType2 in dbContext.MaterialTypes.AsNoTracking() on matType.Id equals matType2.Id
                                        select matType;

            if (missingMaterialTypes.Any())
            {
                await dbContext.MaterialTypes.AddRangeAsync(missingMaterialTypes);
            }

            if (matchingMaterialTypes.Any())
                dbContext.MaterialTypes.UpdateRange(matchingMaterialTypes);

            await dbContext.SaveChangesAsync();
        }

        public async Task UpsertAllMaterialsAsync(List<Material> materials)
        {
            var missingMaterials = from matType in materials
                                   join matType2 in dbContext.Materials.AsNoTracking() on matType.Id equals matType2.Id into matGroup
                                   from matType3 in matGroup.DefaultIfEmpty()
                                   where matType3 == null
                                   select matType;

            var matchingMaterials = from matType in materials
                                    join matType2 in dbContext.Materials.AsNoTracking() on matType.Id equals matType2.Id
                                    select matType;

            if (missingMaterials.Any())
                await dbContext.Materials.AddRangeAsync(missingMaterials);

            if (matchingMaterials.Any())
                dbContext.Materials.UpdateRange(matchingMaterials);

            await dbContext.SaveChangesAsync();

        }

        public async Task UpsertMaterialAsync(Material material)
        {
            if (await dbContext.Materials.AsNoTracking().Where(mat => mat.Id == material.Id).CountAsync() > 0)
                dbContext.Materials.Update(material);
            else
                dbContext.Materials.Add(material);

            await dbContext.SaveChangesAsync();
        }

        public async Task UpsertMaterialTypeAsync(MaterialType materialType)
        {
            if (await dbContext.MaterialTypes.AsNoTracking().Where(mattype => mattype.Id == materialType.Id ).CountAsync() > 0)
                dbContext.MaterialTypes.Update(materialType);
            else
                dbContext.MaterialTypes.Add(materialType);

            await dbContext.SaveChangesAsync();
        }
    }
}
