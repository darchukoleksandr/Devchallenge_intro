using System.ComponentModel.DataAnnotations;
using System.Web;

namespace WebAPI.ViewModels
{
    public class DocumentViewModel
    {
        [Required, FileExtensions(Extensions = "pdf", ErrorMessage = "Please, specify a PDF file.")]
        [Display(Name = "PDF file")]
        public HttpPostedFileBase File { get; set; }
    }
}