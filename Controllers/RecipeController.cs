using Microsoft.AspNetCore.Mvc;
using CamCook.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Linq; // <- importante
using System.Threading.Tasks;

namespace CamCook.Controllers
{
    public class RecipeController : Controller
    {
        private readonly string firestoreUrl =
            "https://firestore.googleapis.com/v1/projects/TU_PROYECTO_FIREBASE/databases/(default)/documents/recetas?key=TU_API_KEY";

        [HttpGet]
        public IActionResult Create()
        {
            // Render inicial
            return View(new RecipeViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(RecipeViewModel receta)
        {
            if (!ModelState.IsValid)
                return View(receta);

            // Evita nulls
            receta.Ingredientes ??= new();
            receta.Pasos ??= new();

            // --- Construir documento Firestore ---
            var payload = new
            {
                fields = new
                {
                    // Campos simples
                    Id = new { stringValue = receta.Id },
                    Titulo = new { stringValue = receta.Titulo },
                    Calorias = new { integerValue = receta.Calorias.ToString() },
                    Porciones = new { integerValue = receta.Porciones.ToString() },
                    TiempoPreparacion = new { stringValue = receta.TiempoPreparacion ?? string.Empty },
                    ImagenFinalUrl = new { stringValue = receta.ImagenFinalUrl ?? string.Empty },


                    // Ingredientes: array de mapValue
                    Ingredientes = new
                    {
                        arrayValue = new
                        {
                            values = receta.Ingredientes.Select(ing => new
                            {
                                mapValue = new
                                {
                                    fields = new
                                    {
                                        Nombre = new { stringValue = ing.Nombre ?? string.Empty },
                                        Cantidad = new { stringValue = ing.Cantidad ?? string.Empty },
                                        Unidad = new { stringValue = ing.Unidad ?? string.Empty }
                                    }
                                }
                            }).ToArray()
                        }
                    },

                    // Pasos: array de mapValue
                    Pasos = new
                    {
                        arrayValue = new
                        {
                            values = receta.Pasos.Select(p => new
                            {
                                mapValue = new
                                {
                                    fields = new
                                    {
                                        Orden = new { integerValue = p.Orden.ToString() },
                                        Descripcion = new { stringValue = p.Descripcion ?? string.Empty },
                                        ImagenUrl = new { stringValue = p.ImagenUrl ?? string.Empty }
                                    }
                                }
                            }).ToArray()
                        }
                    }
                }
            };

            var options = new JsonSerializerOptions { PropertyNamingPolicy = null };
            var json = JsonSerializer.Serialize(payload, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var http = new HttpClient();
            HttpResponseMessage resp;
            try
            {
                resp = await http.PostAsync(firestoreUrl, content);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "No se pudo conectar a Firestore: " + ex.Message);
                return View(receta);
            }

            if (resp.IsSuccessStatusCode)
            {
                TempData["Ok"] = true; // para la alerta verde en la vista
                // Si quieres limpiar el formulario tras guardar:
                // return RedirectToAction(nameof(Crear));
                return View(receta); // vuelve con alerta visible
            }
            else
            {
                var body = await resp.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Error Firestore ({(int)resp.StatusCode}): {resp.ReasonPhrase}. Detalle: {body}");
                return View(receta);
            }
        }

        // Si luego quieres listar, aquí harías un GET a Firestore y mapearías a RecipeViewModel
        public IActionResult Index() => View();
    }
}
