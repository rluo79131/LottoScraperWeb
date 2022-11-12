using LottoScraperWeb.BLL;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace LottoScraperWeb.Controllers
{
    public class LottoController : Controller
    {
        private readonly LottoService lottoService = new LottoService();

        public ActionResult Index()
        {
            var informationList = lottoService.GetInformationList();
            return View(informationList);
        }

        public async Task<ActionResult> Scraper()
        {
            await lottoService.Scraper();            
            return RedirectToAction("Index");
        }
    }
}