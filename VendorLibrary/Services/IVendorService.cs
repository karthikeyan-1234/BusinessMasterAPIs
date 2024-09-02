using VendorLibrary.Models;

namespace VendorLibrary.Services
{
    public interface IVendorService
    {
        Task<Vendor> AddVendorAsync(Vendor newVendor);
        Task<bool> DeleteVendor(int id);
        Task<IEnumerable<Vendor>> GetAllVendors();
        Task<bool> UpdateVendorAsync(Vendor updateVendor);
        Task SendAllVendorsAsync();
    }
}