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
using Action = SAE2._01_Pilot.Utils.Action;

namespace SAE2._01_Pilot.Windows
{
    /// <summary>
    /// Logique d'interaction pour CreerProduitWindow.xaml
    /// </summary>
    public partial class CreerProduitWindow : Window
    {
        private Produit nouveauProduit;
        private Action action;

        public CreerProduitWindow(Produit nouveauProduit, Action action)
        {
            this.action = action;
            this.nouveauProduit = nouveauProduit;

            DataContext = nouveauProduit;

            InitializeComponent();

            InitComboBoxs();
            InitListBoxs();

            if (action == Action.Modifier)
            {
                foreach (CouleurProduit couleur in nouveauProduit.Couleurs)
                {
                    Console.WriteLine($"Couleur sélectionnée : {couleur.Libelle}");
                    lbCouleurs.SelectedItems.Add(couleur);
                }
            }

            btnCreer.Content = action == Utils.Action.Ajouter ? "Créer un produit" : "Modifier le produit";
            txtTitre.Text = btnCreer.Content.ToString();
        }

        private void InitComboBoxs()
        {
            cbTypePointe.ItemsSource = Core.Instance.TypePointes;
            cbTypeProduit.ItemsSource = Core.Instance.TypeProduits;
        }

        private void InitListBoxs()
        {
            lbCouleurs.ItemsSource = Core.Instance.CouleurProduits;
        }

        private void btnCreer_Click(object sender, RoutedEventArgs e)
        {
            bool ok = Core.ValiderFormulaire(spFormulaire);
            ObservableCollection<CouleurProduit> couleursSelected = new ObservableCollection<CouleurProduit>(lbCouleurs.SelectedItems.Cast<CouleurProduit>().ToList());

            if (couleursSelected.Count == 0)
            {
                Core.MessageBoxErreur("Veuillez sélectionner au moins une couleur pour le produit.");
                return;
            }

            nouveauProduit.Couleurs = couleursSelected;

            if (ok)
                DialogResult = true;
        }
    }
}
