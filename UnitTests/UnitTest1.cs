﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Domain.Abstract;
using System.Linq;
using System.Collections.Generic;
using Domain.Entities;
using WebUI.Controllers;
using System.Web.Mvc;
using WebUI.Models;
using WebUI.HtmlHelpers;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            // Организация (arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book{BookId = 1, Name = "Book1"},
                new Book{BookId = 2, Name = "Book2"},
                new Book{BookId = 3, Name = "Book3"},
                new Book{BookId = 4, Name = "Book4"},
                new Book{BookId = 5, Name = "Book5"}
            });

            BooksController controller = new BooksController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            BooksListViewModel result = (BooksListViewModel)controller.List(2).Model;

            // Утверждение (assert)
            List<Book> books = result.Books.ToList();
            Assert.IsTrue(books.Count == 2);
            Assert.AreEqual(books[0].Name, "Book4");
            Assert.AreEqual(books[1].Name, "Book5");
        }

        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            // Организация
            HtmlHelper myHelper = null;
            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage = 2,
                TotalItems = 28,
                ItemsPerPage = 10
            };
            Func<int, string> pageUrlDelegate = i => "Page" + i;

            // Действие
            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            // Утверждение
            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"
                + @"<a class=""btn btn-default btn-primary selected"" href=""Page2"">2</a>"
                + @"<a class=""btn btn-default"" href=""Page3"">3</a>",
                result.ToString());
        }

        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            // Организация (arrange)
            Mock<IBookRepository> mock = new Mock<IBookRepository>();
            mock.Setup(m => m.Books).Returns(new List<Book>
            {
                new Book{BookId = 1, Name = "Book1"},
                new Book{BookId = 2, Name = "Book2"},
                new Book{BookId = 3, Name = "Book3"},
                new Book{BookId = 4, Name = "Book4"},
                new Book{BookId = 5, Name = "Book5"}
            });

            BooksController controller = new BooksController(mock.Object);
            controller.pageSize = 3;

            // Действие (act)
            BooksListViewModel result = (BooksListViewModel)controller.List(2).Model;

            PagingInfo pagingInfo = result.PagingInfo;
            Assert.AreEqual(pagingInfo.CurrentPage, 2);
            Assert.AreEqual(pagingInfo.ItemsPerPage, 3);
            Assert.AreEqual(pagingInfo.TotalItems, 5);
            Assert.AreEqual(pagingInfo.TotalPages, 2);
        }
    }
}
