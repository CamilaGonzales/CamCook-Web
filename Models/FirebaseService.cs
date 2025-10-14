using Firebase.Database;
using Firebase.Database.Query;

namespace CamCook.Models
{
    public class FirebaseService
    {
        private readonly FirebaseClient _firebase;

        public FirebaseService(FirebaseClient firebase)
        {
            _firebase = firebase;
        }

        public async Task<List<RecipeViewModel>> GetAllAsync()
        {
            var result = await _firebase
                .Child("recetas")
                .OnceAsync<RecipeViewModel>();

            return result.Select(r =>
            {
                var recipe = r.Object;
                recipe.Id = r.Key ?? "";
                return recipe;
            }).ToList();
        }

        public async Task<string> CreateAsync(RecipeViewModel recipe)
        {
            var result = await _firebase
                .Child("recetas")
                .PostAsync(recipe);

            return result.Key ?? "";
        }
    }
}
