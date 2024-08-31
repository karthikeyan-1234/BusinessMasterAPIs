using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PurchaseLibrary.DTOs
{
    public class NewPurchaseWithDetailDTO
    {
        public NewPurchaseDTO? purchase { get; set; }
        public IEnumerable<NewPurchaseItemDTO>? Items { get; set; }
    }
}
