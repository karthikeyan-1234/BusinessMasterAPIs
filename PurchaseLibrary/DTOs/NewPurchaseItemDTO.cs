using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseLibrary.DTOs
{
    public class NewPurchaseItemDTO
    {
        public int? PurchaseId { get; set; }

        public int ItemId { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public bool IsActive { get; set; }

        public decimal? Rate { get; set; }

        public decimal? Qty { get; set; }
    }
}
