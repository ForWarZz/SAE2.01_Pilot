using SAE2._01_Pilot.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Pilot.Utils
{
    public class Core
    {
        private Employe employeConnecte;
        
        private List<ModeTransport> modeTransports;
        private List<TypePointe> typePointes;
        private List<TypeProduit> typeProduits;
        private List<CategorieProduit> categorieProduits;
        private List<CouleurProduit> couleurProduits;

        private ObservableCollection<Produit> produits;
        private ObservableCollection<Commande> commandes;
        private ObservableCollection<Revendeur> revendeurs;

        public static Core Instance { get; private set; }

        public Employe EmployeConnecte { get => employeConnecte; set => employeConnecte = value; }
        public List<ModeTransport> ModeTransports { get => modeTransports; set => modeTransports = value; }
        public List<TypePointe> TypePointes { get => typePointes; set => typePointes = value; }
        public List<TypeProduit> TypeProduits { get => typeProduits; set => typeProduits = value; }
        public List<CategorieProduit> CategorieProduits { get => categorieProduits; set => categorieProduits = value; }
        public List<CouleurProduit> CouleurProduits { get => couleurProduits; set => couleurProduits = value; }
        public ObservableCollection<Produit> Produits { get => produits; set => produits = value; }
        public ObservableCollection<Commande> Commandes { get => commandes; set => commandes = value; }
        public ObservableCollection<Revendeur> Revendeurs { get => revendeurs; set => revendeurs = value; }

        public Core(Employe employeConnecte)
        {
            Instance ??= this;

            EmployeConnecte = employeConnecte;
        }

        public void ChargerDonnees()
        {

            ModeTransports = ModeTransport.GetAll();
            TypePointes = TypePointe.GetAll();
            CategorieProduits = CategorieProduit.GetAll();
            TypeProduits = TypeProduit.GetAll(CategorieProduits);
            CouleurProduits = CouleurProduit.GetAll();

            Revendeurs = Revendeur.GetAll();

            Produits = Produit.GetAll(TypePointes, TypeProduits, CouleurProduits);
            Commandes = Commande.GetAll(ModeTransports, Revendeurs, Produits);
        }
    }
}
