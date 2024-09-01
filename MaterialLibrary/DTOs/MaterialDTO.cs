using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialLibrary.DTOs
{
    public class MaterialDTO
    {
        public int Id { get; set; }

        public string? Name { get; set; }

        public int MaterialTypeId { get; set; }

        public string? MaterialType { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public bool? IsActive { get; set; }
    }
}
