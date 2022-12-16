using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NLog;
using NLog.Fluent;
using NLog.Web;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;

namespace ExceptionHandling.Controllers
{
    public class ExceptionController : Controller
    {

        //declare Nlog logger object 
        private readonly Logger logger = NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

        private readonly IConfiguration _configuration;

        public ExceptionController(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IActionResult Index()
        {
            ViewData["counter"] = 0;
            return View(ViewData);
        }

        
        // method to increment the counter
        // generates custom exception if counter value greater than 10
        public IActionResult ExceptionCounter(int counter)
        {
            try
            {
                if (counter < 10)
                {
                    logger.Debug("Increasing counter value by 1");

                    ViewData["counter"] = ++counter;
                    return View("Index", ViewData);
                }
                else
                {
                    logger.Debug("Exceeded the count limit");  //logging in text file 

                    throw new InvalidDataException("You have exceeded the count limit");  //custom exception thrown
                    
                }
            }
            catch(Exception ex)
            {
                logger.Error(ex);   //logs error in log -file
                return View();
                throw;
            }
        }
    

        //method to connect to database
        [HttpGet]
        public IActionResult DatabaseConnect(bool flag)
        {
            //flag false initially to load get view
            if(!flag) 
            {
                ViewData["message"] = true;
                return View();
            }
            else
            {
                ViewData["message"] = false;
                

                //setting db connection
                SqlConnection cnn; 
                string connetionString = _configuration.GetConnectionString("DatabaseConnection");
                
                cnn = new SqlConnection(connetionString);

                try
                {
                    logger.Debug("Connection to database initiated");
                    
                    cnn.Open();
                    return View(); //return connected message
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    return View("ExceptionCounter");  //return exception page
                    throw;
                }
                finally 
                {
                    //closing database connection
                    logger.Debug("Closing database connection");
                    cnn.Close();
                }

            }
            
        }
    }
}
