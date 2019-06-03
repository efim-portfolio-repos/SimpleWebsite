using Microsoft.AspNetCore.Mvc;

namespace SimpleWebsite.Controllers
{
    public class HomeController : Controller
    {
        public ViewResult Index() => View();
    }
}