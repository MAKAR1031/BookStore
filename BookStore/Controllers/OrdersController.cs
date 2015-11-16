using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using BookStore.Models;
using WebMatrix.WebData;
using BookStore.Filters;

namespace BookStore.Controllers {
    [InitializeSimpleMembership]
    public class OrdersController : Controller {
        private BookStoreContext db = new BookStoreContext();

        [Authorize(Roles = "OrdersManager, Client")]
        public ActionResult Index() {
            int userID = WebSecurity.CurrentUserId;
            List<Order> orders;
            if (User.IsInRole("OrdersManager")) {
                orders = db.Orders.ToList();
            } else {
                orders = db.Orders.Where(order => order.ClientID == userID).ToList();
            }
            return View(orders);
        }

        [Authorize(Roles = "OrdersManager, Client")]
        public ActionResult Details(int id = 0) {
            Order order = db.Orders.Find(id);
            if (order == null) {
                return HttpNotFound();
            }
            if (User.IsInRole("Client") && WebSecurity.CurrentUserId != order.ClientID) {
                return View("Error");
            }
            return View(order);
        }

        [Authorize(Roles = "Client")]
        public ActionResult Create() {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null || cart.IsEmpty()) {
                return View("EmptyCartError");
            }
            int cost = 0;
            cart.Content.Keys.ToList().ForEach(key => cost += db.Books.Find(key).Price * cart.Content[key]);
            ViewBag.cost = cost;
            return View();
        }

        [HttpPost]
        public ActionResult Create(FormCollection formCollection) {
            try {
                Order order = new Order();
                order.ClientID = WebSecurity.CurrentUserId;
                order.Date = DateTime.Now;
                order.ConditionID = db.Conditions.Where(condition => condition.Name.Equals("Оформлен")).FirstOrDefault().ID;
                order.Address = formCollection["address"];
                db.Orders.Add(order);
                db.SaveChanges();
                Cart cart = (Cart)Session["Cart"];
                List<OrderItem> orderItems = new List<OrderItem>();
                foreach (var item in cart.Content) {
                    OrderItem orderItem = new OrderItem();
                    orderItem.BookID = item.Key;
                    orderItem.Count = item.Value;
                    orderItem.OrderID = order.ID;
                    orderItems.Add(orderItem);
                }
                order.OrderItems = orderItems;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                ((Cart)Session["Cart"]).Clear();
            } catch {

            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "OrdersManager, Client")]
        public ActionResult Delete(int id = 0) {
            Order order = db.Orders.Find(id);
            if (order == null) {
                return HttpNotFound();
            }
            if (User.IsInRole("Client") && WebSecurity.CurrentUserId != order.ClientID) {
                return View("Error");
            }
            return View(order);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id) {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Client")]
        public ActionResult Checkout(int id = 0) {
            Order order = db.Orders.Find(id);
            if (order == null) {
                return HttpNotFound();
            }
            if (order.ClientID != WebSecurity.CurrentUserId) {
                return View("Error");
            }
            return View(order);
        }

        [HttpPost]
        public ActionResult Checkout(FormCollection form, int id = 0) {
            Order order = db.Orders.Find(id);
            order.Condition = db.Conditions.Where(condition => condition.Name.Equals("Оплачен")).FirstOrDefault();
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Client")]
        public ActionResult PutOff(int id = 0) {
            Order order = db.Orders.Find(id);
            if (order == null) {
                return HttpNotFound();
            }
            if (order.ClientID != WebSecurity.CurrentUserId) {
                return View("Error");
            }
            return View(order);
        }

        [HttpPost, ActionName("PutOff")]
        public ActionResult PutOffConfirmed(int id = 0) {
            Order order = db.Orders.Find(id);
            if (order.Condition.Name.Equals("Оформлен")) {
                order.Condition = db.Conditions.Where(condition => condition.Name.Equals("Отложен")).FirstOrDefault();
            } else {
                order.Condition = db.Conditions.Where(condition => condition.Name.Equals("Оформлен")).FirstOrDefault();
            }
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "OrdersManager")]
        public ActionResult Submit(int id = 0) {
            Order order = db.Orders.Find(id);
            if (order == null) {
                return HttpNotFound();
            }
            return View(order);
        }

        [HttpPost, ActionName("Submit")]
        public ActionResult SubmitConfirmed(int id = 0) {
            Order order = db.Orders.Find(id);
            order.ConditionID = db.Conditions.Where(condition => condition.Name.Equals("Подтвержден")).FirstOrDefault().ID;
            db.Entry(order).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing) {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}