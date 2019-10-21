using Domain.Abstract;
using Domain.Concrete;
//using Domain.Concrete.Domain.Concrete;
using Domain.Entities;
using Moq;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;

        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }

        private void AddBindings()
        {
            //Mock<IBookRepository> mock = new Mock<IBookRepository>();
            //mock.Setup(m => m.Books).Returns(new List<Book>
            //{
            //    new Book {Name="Язык программирования C# 5.0", Author="Трофимов",Price = 1179 },
            //    new Book {Name="Язык программирования C++", Author="Бритни",Price = 1155 },
            //    new Book {Name="Язык программирования C", Author="Катни",Price = 359 }
            //});
            //kernel.Bind<IBookRepository>().ToConstant(mock.Object);

            kernel.Bind<IBookRepository>().To<EFBookRepository>();
        }

        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
    }
}