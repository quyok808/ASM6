using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace ASM6.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string? Name { get; set; }
		[ValidateNever]
		public virtual ICollection<Book> Books { get; set; }
    }
}
