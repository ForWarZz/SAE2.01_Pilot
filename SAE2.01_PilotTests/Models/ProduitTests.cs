using Microsoft.VisualStudio.TestTools.UnitTesting;
using SAE2._01_Pilot.Models;
using System;
using Npgsql;
using TD3_BindingBDPension.Model;
using System.Collections.ObjectModel;

namespace SAE2._01_Pilot.Models.Tests
{
    [TestClass()]
    public class ProduitTests
    {
        private List<TypeProduit> typeProduits;
        private List<TypePointe> typePointes;
        private ObservableCollection<CouleurProduit> couleurProduits;

        [TestInitialize()]
        public void TestInitialize()
        {
            typeProduits = TypeProduit.GetAll(CategorieProduit.GetAll());
            typePointes = TypePointe.GetAll();
            couleurProduits = new ObservableCollection<CouleurProduit>();

            //DataAccess.Instance.StartTransaction();
        }

        [TestCleanup()]
        public void TestCleanup()
        {
            //DataAccess.Instance.RollbackTransaction();
        }

        [TestMethod()]
        public void TestCreateProduit()
        {
            TypePointe typePointe = typePointes.First();
            TypeProduit typeProduit = typeProduits.First();

            Produit produit = new Produit(typePointe, typeProduit, "PR123", "Produit Test", 100, 50, couleurProduits, true);
            produit.Create();

            Assert.IsNotNull(produit.Id);
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestCreateProduitDejaExistant()
        {
            TypePointe typePointe = typePointes.First();
            TypeProduit typeProduit = typeProduits.First();

            Produit produit1 = new Produit(typePointe, typeProduit, "PR123", "Produit Test", 100, 50, couleurProduits, true);
            produit1.Create();

            Produit produit2 = new Produit(typePointe, typeProduit, "PR123", "Produit Test 2", 150, 30, couleurProduits, true);
            produit2.Create();
        }

        [TestMethod()]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestCodeProduitValide()
        {
            TypePointe typePointe = typePointes.First();
            TypeProduit typeProduit = typeProduits.First();

            Produit produit = new Produit(typePointe, typeProduit, "PR123472", "Produit Test", 100, 50, couleurProduits, true);
        }

        [TestMethod()]
        public void TestUpdateProduit()
        {
            TypePointe typePointe = typePointes.First();
            TypeProduit typeProduit = typeProduits.First();

            Produit produit = new Produit(typePointe, typeProduit, "PR123", "Produit Test", 100, 50, couleurProduits, true);
            produit.Create();

            try
            {
                produit.Nom = "Produit Test Mis à jour";
                produit.Update();
            } catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
