using Microsoft.VisualStudio.TestTools.UnitTesting;
using SAE2._01_Pilot.Models;
using System;
using Npgsql;
using TD3_BindingBDPension.Model;
using System.Collections.ObjectModel;

namespace SAE2._01_Pilot.Models.Tests
{
    [TestClass()]
    public class RevendeurTests
    {
        [TestInitialize()]
        public void TestInitialize()
        {
            try
            {
                DataAccess.Instance.SetupTestBDD();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void TestCreateRevendeur()
        {
            Adresse adresse = new Adresse("123 Rue Exemple", "75000", "Paris");
            Revendeur revendeur = new Revendeur("Revendeur Test", adresse);

            revendeur.Create();
            Assert.IsNotNull(revendeur.Id);

            revendeur.Read();

            Assert.AreEqual("Revendeur Test", revendeur.RaisonSociale);
            Assert.IsNotNull(revendeur.Adresse);
            Assert.AreEqual("123 Rue Exemple", revendeur.Adresse.Rue);
            Assert.AreEqual("75000", revendeur.Adresse.CodePostal);
            Assert.AreEqual("Paris", revendeur.Adresse.Ville);
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
        public void TestUpdateRevendeur()
        {
            Adresse adresse = new Adresse("123 Rue Exemple", "75000", "Paris");
            Revendeur revendeur = new Revendeur("Revendeur Test 1", adresse);

            revendeur.Create();

            try
            {
                revendeur.RaisonSociale = "Nouveau Revendeur Test";
                revendeur.Adresse.Rue = "456 Rue Modifiée";
                revendeur.Adresse.CodePostal = "75001";
                revendeur.Adresse.Ville = "Lyon";

                revendeur.Update();
                revendeur.Read();

                Assert.AreEqual("Nouveau Revendeur Test", revendeur.RaisonSociale, "La raison sociale n'a pas été mise à jour");
                Assert.AreEqual("456 Rue Modifiée", revendeur.Adresse.Rue, "L'adresse rue n'a pas été mise à jour");
                Assert.AreEqual("75001", revendeur.Adresse.CodePostal, "L'adresse code postal n'a pas été mise à jour");
                Assert.AreEqual("Lyon", revendeur.Adresse.Ville, "L'adresse ville n'a pas été mise à jour");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void TestGetAllRevendeurs()
        {
            try
            {
                Adresse adresse = new Adresse("125 Rue Exemple", "78000", "Paris");
                Revendeur revendeur = new Revendeur("Revendeur Test GETALL", adresse);

                revendeur.Create();

                ObservableCollection<Revendeur> revendeurs = Revendeur.GetAll();

                Assert.IsNotNull(revendeurs);
                Assert.IsTrue(revendeurs.Count > 0, "Les revendeurs sont pas récupérés correctement.");
            } catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
