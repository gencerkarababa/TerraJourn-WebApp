using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using TerraJourn.Data;
using TerraJourn.Models.Entities;
using TerraJourn.Services;

namespace TerraJourn.Controllers
{
    public class GuideController : Controller
    {
        private Context db = new Context();
        private GeminiService geminiService = new GeminiService();

        public ActionResult Index()
        {
            var questions = db.Questions
                              .Include(q => q.Options)
                              .OrderBy(q => q.OrderNumber)
                              .ToList();

            return View(questions);
        }

        [HttpPost]
        public async Task<ActionResult> SubmitChoices(int[] selectedOptions)
        {
            if (selectedOptions == null || selectedOptions.Length == 0)
            {
                TempData["Error"] = "Lütfen en az bir tercih yapın!";
                return RedirectToAction("Index");
            }

            // Seçilen Option ID'lerine karşılık gelen metinleri çeker
            var choices = db.Options
                            .Where(o => selectedOptions.Contains(o.OptionId))
                            .Select(o => o.OptionText)
                            .ToList();

            string userPreferences = string.Join(", ", choices);

            // Yapay zekaya gönderilen prompt bu
            string prompt = $"Sen profesyonel bir yapay zeka seyahat rehberisin. " +
                            $"Kullanıcının seyahat tercihleri şunlardır: {userPreferences}. " +
                            $"Bu tercihlere tam uygun, samimi, akıcı ve gün gün planlanmış harika bir seyahat rotası hazırla.";

            // Gemini servisi üzerinden yapay zekadan rotayı alıyoruz
            string aiRouteResult = await geminiService.GenerateRouteAsync(prompt);

            ViewBag.AiRoute = aiRouteResult;

            // Sonuç sayfamıza gidiyoruz
            return View("Result");
        }
    }
}