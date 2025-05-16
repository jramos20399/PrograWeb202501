using FrontEnd.Models;

namespace FrontEnd.Helpers.Interfaces
{
    public interface ICategoryHelper
    {
        string Token { get; set; }
        List<CategoryViewModel> GetCategories();

        CategoryViewModel GetCategory(int? id);
        CategoryViewModel Add(CategoryViewModel category);
        CategoryViewModel Update(CategoryViewModel category);
        void Delete(int id);
    }
}
