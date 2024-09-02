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

        public DateOnly UpdatedAt { get; set; }

        public DateOnly CreatedAt { get; set; }

        public bool IsActive { get; set; }

    }
}
