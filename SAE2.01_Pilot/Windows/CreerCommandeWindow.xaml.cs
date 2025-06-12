using SAE2._01_Pilot.Models;
using SAE2._01_Pilot.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SAE2._01_Pilot.Windows
{
    /// <summary>
    /// Logique d'interaction pour CreerCommandeWindow.xaml
    /// </summary>
    public partial class CreerCommandeWindow : Window
    {
        private Commande nouvelleCommande;
        private LigneCommande ligneCommandeCourante;

        public CreerCommandeWindow(Commande nouvelleCommande)
        {
            this.nouvelleCommande = nouvelleCommande;
            ligneCommandeCourante = new LigneCommande();

            InitializeComponent();

            spFormulaire.DataContext = nouvelleCommande;
            spFormLigne.DataContext = ligneCommandeCourante;

            cbTransport.ItemsSource = Core.Instance.ModeTransports;
            dgLignesCommande.ItemsSource = nouvelleCommande.LigneCommandes;
        }

        private void btnAjouterLigne_Click(object sender, RoutedEventArgs e)
        {
            bool ok = Core.ValiderFormulaire(spFormLigne);

            if (ligneCommandeCourante.Produit == null)
            {
                Core.MessageBoxErreur("Veuillez sélectionner un produit.");
                return;
            }

            if (!ok)
                return;

            LigneCommande ligneCommande = new LigneCommande(ligneCommandeCourante.Produit, ligneCommandeCourante.Quantite);
            nouvelleCommande.LigneCommandes.Add(ligneCommande);

            inputQuantite.Clear();
            btnSelectProduit.Content = "Sélectionner un produit";
            btnSelectProduit.DataContext = null;
        }

        private void btnCreer_Click(object sender, RoutedEventArgs e)
        {
            bool ok = Core.ValiderFormulaire(spFormulaire);

            if (nouvelleCommande.Revendeur == null)
            {
                Core.MessageBoxErreur("Veuillez sélectionner un revendeur.");
                return;
            }

            if (nouvelleCommande.LigneCommandes.Count == 0)
            {
                Core.MessageBoxErreur("Veuillez ajouter au moins une ligne de commande.");
                return;
            }

            if (!ok)
                return;

            nouvelleCommande.EmployeId = Core.Instance.EmployeConnecte.Id;
            nouvelleCommande.Create();

            DialogResult = true;
        }

        private void btnSelectRevendeur_Click(object sender, RoutedEventArgs e)
        {
            ListeRevendeurWindow listeRevendeurWindow = new ListeRevendeurWindow();
            bool dialogResult = listeRevendeurWindow.ShowDialog() ?? false;

            if (!dialogResult)
                return;

            Revendeur? revendeur = listeRevendeurWindow.ucRevendeurs.dgRevendeur.SelectedItem as Revendeur;
            nouvelleCommande.Revendeur = revendeur;

            btnSelectRevendeur.Content = revendeur.RaisonSociale;
        }

        private void btnSelectProduit_Click(object sender, RoutedEventArgs e)
        {
            ListeProduitWindow listeProduitsWindow = new ListeProduitWindow();
            bool dialogResult = listeProduitsWindow.ShowDialog() ?? false;

            if (!dialogResult)
                return;

            Produit? produit = listeProduitsWindow.ucProduits.dgProduits.SelectedItem as Produit;

            ligneCommandeCourante.Produit = produit;
            btnSelectProduit.Content = produit.Nom;
        }

        private void btnSupprLigne_Click(object sender, RoutedEventArgs e)
        {
            Button? button = sender as Button;
            if (button == null) 
                return;

            LigneCommande ligneASupprimer = button.DataContext as LigneCommande;
            nouvelleCommande.LigneCommandes.Remove(ligneASupprimer);
        }
    }
}
