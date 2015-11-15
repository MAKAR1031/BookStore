using BookStore.Filters;
using BookStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookStore.Controllers {
    [Authorize(Roles="Client")]
    [InitializeSimpleMembership]
    public class CartController : Controller {

        private BookStoreContext db = new BookStoreContext();
        
        public ActionResult Index() {
            return View(getCart());
        }

        public ActionResult AddToCart(int id = 0) {
            Book book = db.Books.Find(id);
            if (book != null) {
                getCart().AddBook(id);
            } else {
                return HttpNotFound();
            }
            return PartialView(book);
        }

        public ActionResult Remove(int id = 0) {
            Book book = db.Books.Find(id);
            if (book != null) {
                getCart().RemoveBook(id);
            } else {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Clear() {
            return View();
        }

        [HttpPost, ActionName("Clear")]
        public ActionResult ClearConfirmed() {
            ((Cart)Session["Cart"]).Clear();
            return RedirectToAction("Index");
        }

        private Cart getCart() {
            Cart cart = (Cart) Session["Cart"];
            if (cart == null) {
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
    }
}
