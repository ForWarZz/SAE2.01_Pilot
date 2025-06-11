using Microsoft.VisualStudio.TestTools.UnitTesting;
using SAE2._01_Pilot.Models;
using System;
using Npgsql;
using TD3_BindingBDPension.Model;

namespace SAE2._01_Pilot.Models.Tests
{
    [TestClass()]
    public class RevendeurTests
    {
        [TestInitialize()]
        public void TestInitialize()
        {

        }

        [TestMethod()]
        public void TestCreateRevendeur()
        {
            Adresse adresse = new Adresse("123 Rue Exemple", "75000", "Paris");
            Revendeur revendeur = new Revendeur("Revendeur Test", adresse);

            revendeur.Create();
            Assert.IsNotNull(revendeur.Id);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestCreateRevendeurDejaExistant()
        {
            Adresse adresse = new Adresse("123 Rue Exemple", "75000", "Paris");
            Revendeur revendeur1 = new Revendeur("Revendeur Test", adresse);
            revendeur1.Create();

            Revendeur revendeur2 = new Revendeur("Revendeur Test", adresse);
            revendeur2.Create();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestRaisonSocialeNonVide()
        {
            Adresse adresse = new Adresse("123 Rue Exemple", "75000", "Paris");
            Revendeur revendeur = new Revendeur("", adresse);
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestAdresseNonVide()
        {
            Revendeur revendeur = new Revendeur("Revendeur Test", null);
        }

        [TestMethod()]
        public void TestUpdateRevendeur()
        {
            Adresse adresse = new Adresse("123 Rue Exemple", "75000", "Paris");
            Revendeur revendeur = new Revendeur("Revendeur Test 1", adresse);

            revendeur.Create();

            try
            {
                revendeur.RaisonSociale = "Nouveau Revendeur Test";
                revendeur.Update();
            } catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
