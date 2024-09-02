using PurchaseLibrary.Models;

namespace PurchaseLibrary.Services
{
    public interface IVendorService
    {
        Task UpsertAllVendorsAsync(List<Vendor> Vendors);
        Task UpsertVendorAsync(Vendor Vendor);
    }
}