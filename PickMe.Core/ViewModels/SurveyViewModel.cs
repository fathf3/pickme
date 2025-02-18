using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PickMe.Core.ViewModels
{
    public class SurveyViewModel
    {
        [Required(ErrorMessage = "Başlık alanı zorunludur.")]
        [StringLength(100, ErrorMessage = "Başlık en fazla 100 karakter olabilir.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Açıklama alanı zorunludur.")]
        [StringLength(500, ErrorMessage = "Açıklama en fazla 500 karakter olabilir.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "1. resim zorunludur.")]
        public IFormFile Image1File { get; set; }

        [Required(ErrorMessage = "2. resim zorunludur.")]
        public IFormFile Image2File { get; set; }
        [Required(ErrorMessage = "Lütfen bir kategori seçiniz")]
        public int CategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
