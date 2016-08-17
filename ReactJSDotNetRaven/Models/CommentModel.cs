using System.Web;
using Raven.Client;
using Raven.Client.Document;

namespace ReactJSDotNetRaven.Models
{
    public static class RavenContext
    {
        public static readonly IDocumentStore Db = new DocumentStore
        {
            ConnectionStringName = "RavenDb"
        }.Initialize();

        public static IDocumentSession CreateSession()
        {
            return Db.OpenSession();   
        }

        
    }

    public class CommentModel
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public string Text { get; set; }
    }
}