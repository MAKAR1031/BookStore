using System;
using BookStore.Controllers;
using BookStore.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace BookStore.Tests.Controllers {
    [TestClass]
    public class BooksControllerTests {
        [TestMethod]
        public void Index() {
            BooksController bookController = new BooksController();
            ActionResult result = bookController.Index();
            Assert.IsNotNull(result);
        }
    }
}
