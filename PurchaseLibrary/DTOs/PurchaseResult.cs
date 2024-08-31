using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseLibrary.DTOs
{
    public class PurchaseResult
    {
        public int Id { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public bool IsActive { get; set; }
    }
}
