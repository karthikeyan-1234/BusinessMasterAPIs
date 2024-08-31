using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.DTOs
{
    public class BroadCastMessage
    {
        public string? Message { get; set; }
        public NotificationType Type { get; set; }
    }
}
