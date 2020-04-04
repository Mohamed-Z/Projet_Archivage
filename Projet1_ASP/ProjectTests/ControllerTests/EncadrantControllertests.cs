using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Projet1_ASP;
using Projet1_ASP.Controllers;

namespace ProjectTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void inscription()
        {
            //Arrange
            EncadrantController controller = new EncadrantController();


            //Act
            ViewResult result = controller.Inscription() as ViewResult;


            //Assert
            Assert.IsNotNull(result);
        }
    }
}
