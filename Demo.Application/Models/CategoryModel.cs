using Demo.Application.Models.Base;

namespace Demo.Application.Models
{
    public class CategoryModel : BaseModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
