using PurchaseLibrary.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseLibrary.Services
{
    public class VendorService : IVendorService
    {
        PurchaseDbContext dbContext;

        public VendorService(PurchaseDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task UpsertAllVendorsAsync(List<Vendor> Vendors)
        {
            var missingVendors = from vendor in Vendors
                                 join vendor2 in dbContext.Vendors on vendor.Id equals vendor2.Id into vendorGroup
                                 from vendor3 in vendorGroup.DefaultIfEmpty()
                                 where vendor3 == null
                                 select vendor;

            var matchingVendors = from vendor in Vendors
                                  join vendor2 in dbContext.Vendors on vendor.Id equals vendor2.Id
                                  select vendor;

            if (missingVendors.Any())
                await dbContext.Vendors.AddRangeAsync(missingVendors);

            if (matchingVendors.Any())
                dbContext.Vendors.UpdateRange(matchingVendors);

            await dbContext.SaveChangesAsync();

        }

        public async Task UpsertVendorAsync(Vendor Vendor)
        {
            if (await dbContext.Vendors.FindAsync(Vendor.Id) != null)
                dbContext.Vendors.Update(Vendor);
            else
                dbContext.Vendors.Add(Vendor);

            await dbContext.SaveChangesAsync();
        }
    }
}
