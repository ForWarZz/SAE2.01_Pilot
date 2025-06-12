using Microsoft.VisualStudio.TestTools.UnitTesting;
using SAE2._01_Pilot.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using TD3_BindingBDPension.Model;

namespace SAE2._01_Pilot.Models.Tests
{
    [TestClass()]
    public class CommandeTests
    {
        private List<ModeTransport> modeTransports;
        private ObservableCollection<Revendeur> revendeurs;
        private ObservableCollection<Produit> produits;
        private Employe employe;

        private Produit produitTest;

        [TestInitialize()]
        public void TestInitialize()
        {
            DataAccess.Instance.SetupTestBDD();

            List<TypeProduit> typeProduits = TypeProduit.GetAll(CategorieProduit.GetAll());
            List<TypePointe> typePointes = TypePointe.GetAll();
            ObservableCollection<CouleurProduit> couleurProduits = new ObservableCollection<CouleurProduit>(CouleurProduit.GetAll());

            modeTransports = ModeTransport.GetAll();
            revendeurs = Revendeur.GetAll();

            TypePointe typePointe = typePointes.First();
            TypeProduit typeProduit = typeProduits.First();

            produitTest = new Produit(typePointe, typeProduit, "PR124", "Produit Test", 100, 50, couleurProduits, true);
            produitTest.Create();

            produits = Produit.GetAll(typePointes, typeProduits, couleurProduits.ToList());

            employe = Employe.FindByIdentifiant(DataAccess.TEST_LOGIN);
        }

        [TestMethod()]
        public void TestCreateCommande()
        {
            Commande commande = new Commande(modeTransports.First(), revendeurs.First(), employe.Id, DateTime.Today, null);
            commande.LigneCommandes.Add(new LigneCommande(produitTest, 2));

            try
            {
                commande.Create();
                Assert.IsTrue(commande.Id > 0, "La commande n'a pas été créée correctement.");
            }
            catch (Exception ex)
            {
                Assert.Fail($"Erreur lors de la création : {ex.Message}");
            }
        }

        [TestMethod()]
        public void TestUpdateCommande()
        {
            Commande commande = new Commande(modeTransports.First(), revendeurs.First(), employe.Id, DateTime.Today, null);
            commande.LigneCommandes.Add(new LigneCommande(produitTest, 5));
            commande.Create();

            DateTime nouvelleDateLivraison = DateTime.Today.AddDays(3);
            commande.DateLivraison = nouvelleDateLivraison;

            commande.Update();
            commande.Read(modeTransports, revendeurs, produits, employe);

            Assert.AreEqual(nouvelleDateLivraison.Date, commande.DateLivraison?.Date, "La date de livraison n'a pas été mise à jour correctement.");
        }

        [TestMethod()]
        public void TestDeleteCommande()
        {
            Commande commande = new Commande(modeTransports.First(), revendeurs.First(), employe.Id, DateTime.Today, null);
            commande.LigneCommandes.Add(new LigneCommande(produitTest, 1));
            commande.Create();

            commande.Delete();

            Commande commandeLue = new Commande();
            commandeLue.Id = commande.Id;

            Assert.ThrowsException<Exception>(() =>
            {
                commandeLue.Read(modeTransports, revendeurs, produits, employe);
            }, "La commande aurait dû être supprimée.");
        }

        [TestMethod()]
        public void TestGetAllCommandesFromEmploye()
        {
            Commande commande = new Commande(modeTransports.First(), revendeurs.First(), employe.Id, DateTime.Today, null);
            commande.LigneCommandes.Add(new LigneCommande(produitTest, 3));
            commande.Create();

            ObservableCollection<Commande> commandes = Commande.GetFromEmploye(modeTransports, revendeurs, produits, employe);

            Assert.IsNotNull(commandes);
            Assert.IsTrue(commandes.Count > 0, "Les commandes ne sont pas récupérées correctement.");
            Assert.IsTrue(commandes.Any(c => c.Id == commande.Id), "La commande créée n'est pas présente dans la liste des commandes.");
        }

        [TestMethod()]
        public void TestCalculPrixTotalCommande()
        {
            Commande commande = new Commande(modeTransports.First(), revendeurs.First(), employe.Id, DateTime.Today, null);
            commande.LigneCommandes.Add(new LigneCommande(produitTest, 3));
            commande.Create();

            Assert.AreEqual(3 * produitTest.PrixVente, commande.PrixTotal, "Le prix total de la commande n'est pas calculé correctement.");
        }
    }
}
