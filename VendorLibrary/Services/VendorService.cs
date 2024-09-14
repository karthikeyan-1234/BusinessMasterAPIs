using CommonLibrary;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using VendorLibrary.Models;

namespace VendorLibrary.Services
{
    public class VendorService : IVendorService
    {
        VendorDbContext dbContext; //DB connection
        IVendorChangeNotification notification; //Notification service

        public VendorService(VendorDbContext dbContext, IVendorChangeNotification notification)
        {
            this.dbContext = dbContext;
            this.notification = notification;
        }

        public async Task<IEnumerable<Vendor>> GetAllVendors()
            => await dbContext.Vendors.ToListAsync();

        public async Task<Vendor> AddVendorAsync(Vendor newVendor)
        {
            var addedVendor = await dbContext.Vendors.AddAsync(newVendor);
            await dbContext.SaveChangesAsync();
            await notification.SendVendorChangeNotification(JsonSerializer.Serialize(addedVendor), NotificationType.Added);
            return addedVendor.Entity;
        }

        public async Task<bool> UpdateVendorAsync(Vendor updateVendor)
        {
            try
            {
                dbContext.Vendors.Entry(updateVendor).State = EntityState.Modified;
                await dbContext.SaveChangesAsync();
                await notification.SendVendorChangeNotification(JsonSerializer.Serialize(updateVendor), NotificationType.Updated);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> DeleteVendor(int id)
        {
            var vendor = await dbContext.Vendors.FindAsync(id);

            if (vendor != null)
            {
                dbContext.Entry(vendor).State = EntityState.Deleted;
                await dbContext.SaveChangesAsync();
                await notification.SendVendorChangeNotification(JsonSerializer.Serialize(vendor), NotificationType.Deleted);
                return true;
            }

            return false;
        }

        public async Task SendAllVendorsAsync()
        {
            var allVendors = await dbContext.Vendors.ToListAsync();

            await notification.SendVendorChangeNotification(JsonSerializer.Serialize(allVendors), NotificationType.AllRecords);
        }
    }
}
