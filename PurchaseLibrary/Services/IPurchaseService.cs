using PurchaseLibrary.DTOs;
using PurchaseLibrary.Models;

namespace PurchaseLibrary.Services
{
    public interface IPurchaseService
    {
        Task<IEnumerable<PurchaseResult>> GetAllPurchasesAsync();
        Task<IEnumerable<PurchaseItemResult>> GetPurchaseItemsForPurchase(int purchaseId);
        Task<Purchase> AddNewPurchaseWithDetails(NewPurchaseWithDetailDTO newPurchase);
        Task<Purchase> AddNewPurchaseAsync(NewPurchaseDTO newPurchase);
        Task AddNewPurchaseItemAsync(NewPurchaseItemDTO newPurchaseItem, int PurchaseId);        Task<bool> RemovePurchaseAsync(int purchaseId);
        Task<bool> RemovePurchaseItemAsync(int purchaseItemId);
    }
}