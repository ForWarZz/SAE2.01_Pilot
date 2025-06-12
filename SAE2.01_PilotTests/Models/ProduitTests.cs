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
            try
            {
                DataAccess.Instance.SetupTestBDD();
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }

            typeProduits = TypeProduit.GetAll(CategorieProduit.GetAll());
            typePointes = TypePointe.GetAll();
            couleurProduits = new ObservableCollection<CouleurProduit>(CouleurProduit.GetAll());
        }

        [TestMethod()]
        public void TestCreateProduit()
        {
            TypePointe typePointe = typePointes.First();
            TypeProduit typeProduit = typeProduits.First();

            Produit produit = new Produit(typePointe, typeProduit, "PR124", "Produit Test", 100, 50, couleurProduits, true);
            produit.Create();

            Assert.IsNotNull(produit.Id);

            produit.Read(typePointes, typeProduits, couleurProduits.ToList());

            Assert.AreEqual("Produit Test", produit.Nom, "Le nom du produit n'est pas correct.");
            Assert.AreEqual(100, produit.PrixVente, "Le prix de vente du produit n'est pas correct.");
            Assert.AreEqual(50, produit.QuantiteStock, "La quantité en stock du produit n'est pas correcte.");
            Assert.IsTrue(produit.Disponible, "Le produit devrait être marqué comme disponible.");
            Assert.IsNotNull(produit.TypePointe, "Le type de pointe ne devrait pas être nul.");
            Assert.IsNotNull(produit.Type, "Le type de produit ne devrait pas être nul.");
            Assert.AreEqual(typePointe.Id, produit.TypePointe.Id, "L'ID du type de pointe ne correspond pas.");
            Assert.AreEqual(typeProduit.Id, produit.Type.Id, "L'ID du type de produit ne correspond pas.");
            Assert.IsNotNull(produit.Couleurs, "La liste des couleurs du produit ne devrait pas être nulle.");

            Assert.IsTrue(produit.Couleurs.All(c => couleurProduits.Any(cp => cp.Id == c.Id)), "Toutes les couleurs du produit doivent exister dans la liste des couleurs.");
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
        public void TestUpdateProduit()
        {
            TypePointe typePointe = typePointes.First();
            TypeProduit typeProduit = typeProduits.First();

            Produit produit = new Produit(typePointe, typeProduit, "PR125", "Produit Test", 100, 50, couleurProduits, true);
            produit.Create();

            try
            {
                produit.Nom = "Produit Test Mis à jour";
                produit.PrixVente = 120;
                produit.QuantiteStock = 60;
                produit.Disponible = false;
                produit.Couleurs.Add(couleurProduits[1]);
                produit.TypePointe = typePointes[1];
                produit.Type = typeProduits[1];

                produit.Update();
                produit.Read(typePointes, typeProduits, couleurProduits.ToList());

                Assert.AreEqual("Produit Test Mis à jour", produit.Nom, "Le nom du produit n'a pas été mis à jour correctement.");
                Assert.AreEqual(120, produit.PrixVente, "Le prix de vente du produit n'a pas été mis à jour correctement.");
                Assert.AreEqual(60, produit.QuantiteStock, "La quantité en stock du produit n'a pas été mise à jour correctement.");
                Assert.IsFalse(produit.Disponible, "Le produit devrait être marqué comme indisponible.");
                Assert.IsTrue(produit.Couleurs.Contains(couleurProduits[1]), "La couleur ajoutée n'est pas présente dans la liste des couleurs du produit.");
                Assert.AreEqual(typePointes[1].Id, produit.TypePointe.Id, "Le type de pointe du produit n'a pas été mis à jour correctement.");
                Assert.AreEqual(typeProduits[1].Id, produit.Type.Id, "Le type de produit du produit n'a pas été mis à jour correctement.");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }

        [TestMethod()]
        public void TestGetAllProduit()
        {
            try
            {
                TypePointe typePointe = typePointes.First();
                TypeProduit typeProduit = typeProduits.First();

                Produit produit = new Produit(typePointe, typeProduit, "TH845", "Produit Test", 100, 50, couleurProduits, true);
                produit.Create();

                ObservableCollection<Produit> produits = Produit.GetAll(typePointes, typeProduits, couleurProduits.ToList());

                Assert.IsNotNull(produits);
                Assert.IsTrue(produits.Count > 0, "Les produits ne sont pas récupérés correctement.");
            }
            catch (Exception ex)
            {
                Assert.Fail(ex.Message);
            }
        }
    }
}
