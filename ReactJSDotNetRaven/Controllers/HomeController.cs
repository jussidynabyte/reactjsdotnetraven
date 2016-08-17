using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using Raven.Client;
using Raven.Client.Document;
using ReactJSDotNetRaven.Models;

namespace ReactDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(new List<CommentModel>());
        }

        [HttpPost]
        public ActionResult AddComment(CommentModel comment)
        {
                using (IDocumentSession session = RavenContext.CreateSession()) 
                {
                    session.Store(comment); // stores employee in session, assigning it to a collection `Employees`
                    session.SaveChanges(); // sends all changes to server
                }
            
            return Content("Success :)");
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Comments()
        {

                using (IDocumentSession session = RavenContext.CreateSession()) 
                {
                    List<CommentModel> comments = session.Query<CommentModel>().Take(100).ToList();

                    // Session implements Unit of Work pattern,
                    // therefore employee instance would be the same and no server call will be made
                    return Json(comments, JsonRequestBehavior.AllowGet);
                }
           
        }
    }
}