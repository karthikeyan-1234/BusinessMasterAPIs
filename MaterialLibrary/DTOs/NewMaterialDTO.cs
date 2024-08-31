namespace MaterialLibrary.DTOs
{
    public class NewMaterialDTO
    {
        public string? Name { get; set; }

        public int? MaterialType { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public bool? IsActive { get; set; }
    }
}
