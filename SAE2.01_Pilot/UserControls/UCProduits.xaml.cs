using SAE2._01_Pilot.Models;
using SAE2._01_Pilot.Utils;
using SAE2._01_Pilot.Windows;
using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAE2._01_Pilot.UserControls
{
    /// <summary>
    /// Logique d'interaction pour UCProduits.xaml
    /// </summary>
    public partial class UCProduits : UserControl
    {
        private ListCollectionView produitView;

        public UCProduits()
        {
            InitializeComponent();

            ChargerProduits();
            InitComboBoxs();
        }

        private void InitComboBoxs()
        {
            // Catégories
            List<CategorieProduit> listeCategories = new List<CategorieProduit>(Core.Instance.CategorieProduits);
            listeCategories.Insert(0, new CategorieProduit(-1, "Toutes les catégories"));
            cbCategorie.ItemsSource = listeCategories;
            cbCategorie.DisplayMemberPath = "Libelle";
            cbCategorie.SelectedValuePath = "Id";
            cbCategorie.SelectedIndex = 0;

            // Couleurs
            List<CouleurProduit> listeCouleurs = new List<CouleurProduit>(Core.Instance.CouleurProduits);
            listeCouleurs.Insert(0, new CouleurProduit(-1, "Toutes les couleurs"));
            cbCouleur.ItemsSource = listeCouleurs;
            cbCouleur.DisplayMemberPath = "Libelle";
            cbCouleur.SelectedValuePath = "Id";
            cbCouleur.SelectedIndex = 0;

            // Types de pointe
            List<TypePointe> listePointes = new List<TypePointe>(Core.Instance.TypePointes);
            listePointes.Insert(0, new TypePointe(-1, "Toutes les pointes"));
            cbTypePointe.ItemsSource = listePointes;
            cbTypePointe.DisplayMemberPath = "Libelle";
            cbTypePointe.SelectedValuePath = "Id";
            cbTypePointe.SelectedIndex = 0;

            // Types de produit
            List<TypeProduit> listeTypes = new List<TypeProduit>(Core.Instance.TypeProduits);
            listeTypes.Insert(0, new TypeProduit(-1, "Tous les types"));
            cbType.ItemsSource = listeTypes;
            cbType.DisplayMemberPath = "Libelle";
            cbType.SelectedValuePath = "Id";
            cbType.SelectedIndex = 0;
        }

        private void ChargerProduits()
        {
            Core.Instance.RefreshProduits();

            produitView = new ListCollectionView(Core.Instance.Produits);
            produitView.Filter = FiltrerProduits;

            dgProduits.ItemsSource = produitView;
        }

        private bool FiltrerProduits(object obj)
        {
            return true;
        }

        private void HandleRole()
        {
            bool estResponsable = Core.Instance.EmployeConnecte.EstResponsableProduction;

            if (!estResponsable)
            {
                butModifierProduit.Visibility = Visibility.Hidden;
                butRendreIndisponibleProduit.Visibility = Visibility.Hidden;
            }
        }

        private void dgProduits_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object selectedItem = dgProduits.SelectedItem;
            bool estResponsable = Core.Instance.EmployeConnecte.EstResponsableProduction;

            if (selectedItem == null)
            {
                butRendreIndisponibleProduit.Visibility = Visibility.Hidden;
                butModifierProduit.Visibility = Visibility.Hidden;

                return;
            }

            if (estResponsable)
            {
                butModifierProduit.Visibility = Visibility.Visible;
                butRendreIndisponibleProduit.Visibility = Visibility.Visible;
            }
        }

        private void butAddProduit_Click(object sender, RoutedEventArgs e)
        {
            CreerProduitWindow creerProduitWindow = new CreerProduitWindow();
            bool dialogResult = creerProduitWindow.ShowDialog() ?? false;
        }
    }
}
