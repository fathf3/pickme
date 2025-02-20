using System.ComponentModel.DataAnnotations;

namespace PickMe.Core.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Kategori adı zorunludur.")]
        [StringLength(100, ErrorMessage = "Kategori adı en fazla 100 karakter olabilir.")]
        
        public string Name { get; set; } = string.Empty;

        // Kategoriye ait anketler
        public virtual ICollection<Survey> Surveys { get; set; } = new List<Survey>();
    }
}
