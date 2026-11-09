using System.Web.Mvc;
using EMS.MVC.Frontend.Models;

namespace EMS.MVC.Frontend.Controllers
{
    public class ErrorController : Controller
    {
        //[XAuthorize(FunctionalTitle = "")]
        //[AuthorizeController(FunctionalTitle = "")]
        public ActionResult PageNotFound()
        {
            return PartialView();
        }
        public ActionResult InternalServerError()
        {
            return PartialView();
        }
        public ActionResult NotPermission()
        {
            return PartialView();
        }
        public ActionResult StatusCode203(string Message)
        {
            /*“Non-Authoritative Information.”
            This status code may appear when a proxy is in use. 
            It means that the proxy server received a 200 “Everything is OK” status code from the origin server, but has modified the response before passing it on to your browser.*/
            StatusCode203ViewModel model = new StatusCode203ViewModel() 
            {
                Message = Message
            };
            return PartialView(model);
        }
    }
}