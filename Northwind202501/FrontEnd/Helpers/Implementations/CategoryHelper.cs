using FrontEnd.ApiModels;
using FrontEnd.Helpers.Interfaces;
using FrontEnd.Models;
using Newtonsoft.Json;

namespace FrontEnd.Helpers.Implementations
{
    public class CategoryHelper : ICategoryHelper
    {
        IServiceRepository _ServiceRepository;
        public string Token { get; set; }

        CategoryViewModel Convertir(CategoryAPI category)
        {
            CategoryViewModel categoryViewModel = new CategoryViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName
            };
            return categoryViewModel;
        }


        public CategoryHelper(IServiceRepository serviceRepository)
        {
            _ServiceRepository = serviceRepository;
        }

        public CategoryViewModel Add(CategoryViewModel category)
        {
            HttpResponseMessage response = _ServiceRepository.PostResponse("api/Category", category);
            if (response.IsSuccessStatusCode)
            {

                var content = response.Content.ReadAsStringAsync().Result;
            }
            return category;
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<CategoryViewModel> GetCategories()
        {


            _ServiceRepository.Client.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);

            HttpResponseMessage responseMessage = _ServiceRepository.GetResponse("api/Category");
            List<CategoryAPI> categories = new List<CategoryAPI>();
            if (responseMessage != null)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                categories = JsonConvert.DeserializeObject<List<CategoryAPI>>(content);
            }
            List<CategoryViewModel> lista = new List<CategoryViewModel>();
            foreach (var category in categories)
            {
                lista.Add(Convertir(category));
            }   
            return lista;
        }

        public CategoryViewModel GetCategory(int? id)
        {
            HttpResponseMessage responseMessage = _ServiceRepository.GetResponse("api/Category/" + id.ToString());
            CategoryAPI category = new CategoryAPI();
            if (responseMessage != null)
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                category = JsonConvert.DeserializeObject<CategoryAPI>(content);
            }

            CategoryViewModel resultado = Convertir(category);


            return resultado;
        }

        public CategoryViewModel Update(CategoryViewModel category)
        {
            throw new NotImplementedException();
        }
    }
}
