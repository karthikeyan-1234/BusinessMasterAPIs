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
            => await dbContext.PurchaseItems.Where(p => p.PurchaseId == purchaseId).Select(item => new PurchaseItemResult()
            {
                Id = item.Id,
                PurchaseId = purchaseId,
                ItemId = item.ItemId,
                CreatedAt = item.CreatedAt,
                UpdatedAt = item.UpdatedAt,
                IsActive = item.IsActive,
                Qty = (decimal)item.Qty,
                Rate = (decimal)item.Rate,
            }).ToListAsync();

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

        public async Task AddNewPurchaseItemAsync(NewPurchaseItemDTO newPurchaseItem, int PurchaseId)
        {
            await dbContext.PurchaseItems.AddAsync(new PurchaseItem()
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                IsActive = newPurchaseItem.IsActive,
                PurchaseId = PurchaseId,
                ItemId = newPurchaseItem.ItemId
            });

            await dbContext.SaveChangesAsync();
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
