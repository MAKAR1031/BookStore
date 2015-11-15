using System.Data;
using System.Linq;
using System.Web.Mvc;
using BookStore.Models;
using System.Web.Security;
using BookStore.Filters;

namespace BookStore.Controllers {
    [InitializeSimpleMembership]
    public class BooksController : Controller {
        private BookStoreContext db = new BookStoreContext();

        [AllowAnonymous]
        public ActionResult Index() {
            return View(db.Books.ToList());
        }

        [Authorize(Roles = "Client,BooksManager")]
        public ActionResult Details(int id = 0) {
            Book book = db.Books.Find(id);
            if (book == null) {
                return HttpNotFound();
            }
            return View(book);
        }

        [Authorize(Roles = "BooksManager")]
        public ActionResult Create() {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Book book) {
            if (ModelState.IsValid) {
                db.Books.Add(book);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(book);
        }

        [Authorize(Roles = "BooksManager")]
        public ActionResult Edit(int id = 0) {
            Book book = db.Books.Find(id);
            if (book == null) {
                return HttpNotFound();
            }
            return View(book);
        }

        [HttpPost]
        public ActionResult Edit(Book book) {
            if (ModelState.IsValid) {
                db.Entry(book).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(book);
        }

        [Authorize(Roles = "BooksManager")]
        public ActionResult Delete(int id = 0) {
            Book book = db.Books.Find(id);
            if (book == null) {
                return HttpNotFound();
            }
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id) {
            db.Books.Remove(db.Books.Find(id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        public ActionResult Search(string bookName) {
            var books = db.Books.Where(book => book.Name.Contains(bookName));
            return PartialView(books);
        }

        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}