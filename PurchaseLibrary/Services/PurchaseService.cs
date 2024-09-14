using Microsoft.EntityFrameworkCore;

using PurchaseLibrary.DTOs;
using PurchaseLibrary.Models;

namespace PurchaseLibrary.Services
{
    public class PurchaseService : IPurchaseService
    {
        PurchaseDbContext dbContext;

        public PurchaseService(PurchaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Material>> GetAllMaterialsAsync()
        => await dbContext.Materials.ToListAsync();

        public async Task<IEnumerable<PurchaseResult>> GetAllPurchasesAsync()
            => await dbContext.Purchases.Select(purchase => new PurchaseResult()
            {
                Id = purchase.Id,
                UpdatedAt = DateOnly.FromDateTime(purchase.UpdatedAt.DateTime),
                CreatedAt = DateOnly.FromDateTime(purchase.CreatedAt.DateTime),
                IsActive = purchase.IsActive,

            }).ToListAsync();

        public async Task<bool> UpdatePurchaseAsync(Purchase updatePurchase)
        {
            try
            {
                dbContext.Entry(updatePurchase).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<IEnumerable<PurchaseItemResult>> GetPurchaseItemsForPurchase(int purchaseId)
            => await dbContext.PurchaseItems
            .Join(dbContext.Materials,p => p.ItemId, m => m.Id, (p,m) => new PurchaseItemResult{
                Id =p.Id!,
                PurchaseId = p.PurchaseId,
                ItemId = p.ItemId,
                ItemName = m.Name,
                CreatedAt = p.CreatedAt,
                UpdatedAt = p.UpdatedAt,
                IsActive = p.IsActive,
                Qty = p.Qty??0,
                Rate = p.Rate??0,
            }).Where(x => x.PurchaseId == purchaseId).ToListAsync();

        public async Task<Purchase> AddNewPurchaseWithDetails(NewPurchaseWithDetailDTO newPurchase)
        {
            await dbContext.Database.BeginTransactionAsync();

            var newAddedPurchase = (await dbContext.Purchases.AddAsync(new Purchase()
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = newPurchase!.purchase!.IsActive
            })).Entity;

            await dbContext.SaveChangesAsync();

            foreach (var item in newPurchase.Items!)
            {
                var newPurchaseItem = new PurchaseItem
                {
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    IsActive = true,
                    ItemId = item.ItemId,
                    PurchaseId = newAddedPurchase.Id
                };
                dbContext.PurchaseItems.Add(newPurchaseItem);
            }

            await dbContext.SaveChangesAsync();

            await dbContext.Database.CommitTransactionAsync();

            return newAddedPurchase;
        }

        public async Task<Purchase> AddNewPurchaseAsync(NewPurchaseDTO newPurchase)
        {
            var newAddedPurchase = (await dbContext.Purchases.AddAsync(new Purchase()
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = newPurchase.IsActive
            })).Entity;

            await dbContext.SaveChangesAsync();
            return newAddedPurchase;

        }

        public async Task<bool> RemovePurchaseAsync(int purchaseId)
        {
            var purchase = await dbContext.Purchases.FindAsync(purchaseId);

            if (purchase != null)
            {
                var purchaseItems = await dbContext.PurchaseItems.Where(item => item.PurchaseId == purchaseId).ToListAsync();

                if (purchaseItems.Any())
                {
                    foreach (var item in purchaseItems)
                        dbContext.PurchaseItems.Remove(item);
                }

                dbContext.Entry(purchase).State = EntityState.Deleted;

                await dbContext.SaveChangesAsync();

                return true;
            }

            return false;
        }

        public async Task<PurchaseItemResult> AddNewPurchaseItemAsync(NewPurchaseItemDTO newPurchaseItem)
        {
            var result = (await dbContext.PurchaseItems.AddAsync(new PurchaseItem()
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = newPurchaseItem.IsActive,
                PurchaseId = newPurchaseItem.PurchaseId,
                ItemId = newPurchaseItem.ItemId,
                Rate = newPurchaseItem.Rate,
                Qty = newPurchaseItem.Qty
            })).Entity;

            await dbContext.SaveChangesAsync();

            return new PurchaseItemResult()
            {
                Id = result.Id,
                PurchaseId = result.PurchaseId,
                ItemId = result.ItemId,
                IsActive = result.IsActive,
                CreatedAt = result.CreatedAt,
                UpdatedAt = result.UpdatedAt,
                Rate = result.Rate,
                Qty = result.Qty
            };
        }

        public async Task UpdatePurchaseItemAsync(PurchaseItem purchaseItem)
        {
            try
            {
                purchaseItem.Purchase = dbContext.Purchases.Where(p => p.Id == purchaseItem.PurchaseId).FirstOrDefault();
                dbContext.PurchaseItems.Entry(purchaseItem).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }   

        public async Task<bool> RemovePurchaseItemAsync(int purchaseItemId)
        {
            var purchaseItem = await dbContext.PurchaseItems.FindAsync(purchaseItemId);

            if (purchaseItem != null)
            {
                dbContext.PurchaseItems.Remove(purchaseItem);
                await dbContext.SaveChangesAsync();

                return true;
            }

            return false;

        }
    }
}
