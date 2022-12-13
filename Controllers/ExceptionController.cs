using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Fluent;
using NLog.Web;

namespace ExceptionHandling.Controllers
{
    public class ExceptionController : Controller
    {
        private readonly Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();


        public IActionResult Index()
        {
            ViewData["counter"] = 0;
            return View(ViewData);
        }

        
        public IActionResult ExceptionCounter(int counter)
        {
            try
            {
                if (counter < 10)
                {
                    ViewData["counter"] = ++counter;
                    return View("Index", ViewData);
                }
                else
                {
                    logger.Error("Exceeded the count limit");

                    throw new InvalidDataException("You have exceeded the count limit");
                    
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex);
                return View();
                throw;
            }

         
        }
    }
}
