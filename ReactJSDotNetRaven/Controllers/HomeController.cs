using System.Collections.Generic;
using System.Web.Mvc;
using ReactDemo.Models;
using System.Web.UI;
using Raven.Client;
using Raven.Client.Document;

namespace ReactDemo.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View(_comments);
        }

        [HttpPost]
        public ActionResult AddComment(CommentModel comment)
        {
            using (IDocumentStore store = new DocumentStore
            {
                Url = "http://localhost:8080/", // server URL
                DefaultDatabase = "Comments"   // default database
            })
            {
                store.Initialize(); // initializes document store, by connecting to server and downloading various configurations

                using (IDocumentSession session = store.OpenSession()) // opens a session that will work in context of 'DefaultDatabase'
                {
                    session.Store(comment); // stores employee in session, assigning it to a collection `Employees`
                    int commentId = comment.Id; // Session.Store will assign Id to employee, if it is not set

                    session.SaveChanges(); // sends all changes to server
                }
            }
            return Content("Success :)");
        }

        [OutputCache(Location = OutputCacheLocation.None)]
        public ActionResult Comments()
        {
            using (IDocumentStore store = new DocumentStore
            {
                Url = "http://localhost:8080/", // server URL
                DefaultDatabase = "Comments"   // default database
            })
            {
                store.Initialize(); // initializes document store, by connecting to server and downloading various configurations

                using (IDocumentSession session = store.OpenSession()) // opens a session that will work in context of 'DefaultDatabase'
                {
                    CommentModel comment = new CommentModel
                    {
                        Author = "Calle",
                        Text = "Hello from Calle"
                    };

                    session.Store(comment); // stores employee in session, assigning it to a collection `Employees`
                    int commentId = comment.Id; // Session.Store will assign Id to employee, if it is not set

                    session.SaveChanges(); // sends all changes to server

                    // Session implements Unit of Work pattern,
                    // therefore employee instance would be the same and no server call will be made
                    CommentModel loadedComment = session.Load<CommentModel>(commentId);
                    return Json(new List<CommentModel> { comment }, JsonRequestBehavior.AllowGet);
                }
            }
           
        }
    }
}