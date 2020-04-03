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
    public class EtudiantControllerTests
    {
        [TestMethod]
        public void inscription()
        {
            //Arrange
            EtudiantController controller = new EtudiantController();


            //Act
            ViewResult result = controller.inscription() as ViewResult;


            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void connexion()
        {
            //Arrange
            EtudiantController controller = new EtudiantController();


            //Act
            ViewResult result = controller.connexion() as ViewResult;


            //Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Archiver()
        {
            //Arrange
            EtudiantController controller = new EtudiantController();


            //Act
            ViewResult result = controller.Archiver() as ViewResult;


            //Assert
            Assert.IsNotNull(result);
        }
    }
}
