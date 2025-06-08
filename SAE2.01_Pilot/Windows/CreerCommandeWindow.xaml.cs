using SAE2._01_Pilot.Models;
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
        private Employe EmployeConnecte;

        public CreerCommandeWindow(List<ModeTransport> modeTransports, Commande nouvelleCommande, Employe employeConnecte)
        {
            this.nouvelleCommande = nouvelleCommande;

            InitializeComponent();

            cbTransport.ItemsSource = modeTransports;
            cbTransport.DisplayMemberPath = "Libelle";
            cbTransport.SelectedValuePath = "Id";

            dgLignesCommande.DataContext = nouvelleCommande.LigneCommandes;
            EmployeConnecte = employeConnecte;
        }

        private void btnAjouterLigne_Click(object sender, RoutedEventArgs e)
        {
            bool quantiteOk = int.TryParse(inputQuantite.Text, out int quantite);
            if (String.IsNullOrEmpty(inputQuantite.Text) || !quantiteOk)
            {
                MessageBox.Show("Veuillez entrer une quantité.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (btnSelectProduit.DataContext is not Produit)
            {
                MessageBox.Show("Veuillez sélectionner un produit.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Produit produitSelect = (btnSelectProduit.DataContext as Produit)!;
            LigneCommande nouvelleLigne = new LigneCommande(produitSelect, quantite);

            nouvelleCommande.LigneCommandes.Add(nouvelleLigne);
        }

        private void btnCreer_Click(object sender, RoutedEventArgs e)
        {
            if (nouvelleCommande.LigneCommandes.Count == 0)
            {
                MessageBox.Show("Veuillez ajouter au moins une ligne de commande.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cbTransport.SelectedItem is not ModeTransport transport)
            {
                MessageBox.Show("Veuillez sélectionner un mode de transport.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (btnSelectRevendeur.DataContext is not Revendeur revendeur)
            {
                MessageBox.Show("Veuillez sélectionner un revendeur.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Revendeur revendeurSelect = (btnSelectRevendeur.DataContext as Revendeur)!;

            nouvelleCommande.ModeTransport = transport;
            nouvelleCommande.Revendeur = revendeurSelect;
            nouvelleCommande.EmployeId = EmployeConnecte.Id;

            nouvelleCommande.Create();

            DialogResult = true;
        }
    }
}
