using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace ASM6.Models
{
    public class Book
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Không có cuốn sách nào mà không có tên cả !!!"), StringLength(50, ErrorMessage = "Tên dài quá, vui lòng đặt lại")]
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        [Range(10000.00, 1000000.00, ErrorMessage = "Giá trị từ 10.000 - 1.000.000")]
        public int? Price { get; set; }

        public int CategoryId { get; set; }
        [ValidateNever]
        public virtual required Category? Category { get; set; }
    }
}
