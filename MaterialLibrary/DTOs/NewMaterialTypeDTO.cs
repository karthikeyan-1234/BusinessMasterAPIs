namespace MaterialLibrary.DTOs
{
    public class NewMaterialTypeDTO
    {
        public string? Name { get; set; }

        public DateTimeOffset? CreatedAt { get; set; }

        public DateTimeOffset? UpdatedAt { get; set; }

        public bool? IsActive { get; set; }
    }
}
