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

        public CreerCommandeWindow(Commande nouvelleCommande)
        {
            this.nouvelleCommande = nouvelleCommande;
            DataContext = nouvelleCommande;

            InitializeComponent();

            cbTransport.ItemsSource = Core.Instance.ModeTransports;
            dgLignesCommande.ItemsSource = nouvelleCommande.LigneCommandes;
        }

        private void btnAjouterLigne_Click(object sender, RoutedEventArgs e)
        {
            bool quantiteOk = int.TryParse(inputQuantite.Text, out int quantite);
            if (String.IsNullOrEmpty(inputQuantite.Text) || !quantiteOk)
            {
                Core.MessageBoxErreur("Veuillez entrer une quantité valide.");
                return;
            }

            if (btnSelectProduit.DataContext is not Produit produit)
            {
                Core.MessageBoxErreur("Veuillez sélectionner un produit.");
                return;
            }

            LigneCommande nouvelleLigne = new LigneCommande(produit, quantite);
            nouvelleCommande.LigneCommandes.Add(nouvelleLigne);

            inputQuantite.Clear();
            btnSelectProduit.Content = "Sélectionner un produit";
            btnSelectProduit.DataContext = null;
        }

        private void btnCreer_Click(object sender, RoutedEventArgs e)
        {
            if (nouvelleCommande.LigneCommandes.Count == 0)
            {
                Core.MessageBoxErreur("Veuillez ajouter au moins une ligne de commande.");
                return;
            }

            if (cbTransport.SelectedItem is not ModeTransport transport)
            {
                Core.MessageBoxErreur("Veuillez sélectionner un mode de transport.");
                return;
            }

            if (nouvelleCommande.Revendeur == null)
            {
                Core.MessageBoxErreur("Veuillez sélectionner un revendeur.");
                return;
            }

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

            btnSelectProduit.DataContext = produit;
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
