using System.ComponentModel.DataAnnotations;

namespace AspNetCoreWebApi.Models
{
    public class CreateNode
    {
        
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public int? ParentId { get; set; } = null;


    }
}
