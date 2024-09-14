using PurchaseLibrary.DTOs;
using PurchaseLibrary.Models;

namespace PurchaseLibrary.Services
{
    public interface IPurchaseService
    {
        Task<IEnumerable<Material>> GetAllMaterialsAsync();
        Task<IEnumerable<PurchaseResult>> GetAllPurchasesAsync();
        Task<IEnumerable<PurchaseItemResult>> GetPurchaseItemsForPurchase(int purchaseId);
        Task<bool> UpdatePurchaseAsync(Purchase updatePurchase);
        Task<Purchase> AddNewPurchaseWithDetails(NewPurchaseWithDetailDTO newPurchase);
        Task<Purchase> AddNewPurchaseAsync(NewPurchaseDTO newPurchase);
        Task<PurchaseItemResult> AddNewPurchaseItemAsync(NewPurchaseItemDTO newPurchaseItem);        
        Task<bool> RemovePurchaseAsync(int purchaseId);
        Task<bool> RemovePurchaseItemAsync(int purchaseItemId);
        Task UpdatePurchaseItemAsync(PurchaseItem purchaseItem);
    }
}