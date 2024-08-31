using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseLibrary.DTOs
{
    public class PurchaseItemResult
    {
        public int Id { get; set; }

        public int? PurchaseId { get; set; }

        public int ItemId { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public bool IsActive { get; set; }
    }
}
