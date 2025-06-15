using SAE2._01_Pilot.Models;
using SAE2._01_Pilot.Utils;
using SAE2._01_Pilot.Windows;
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
        private bool filtrerDisponible;

        public UCProduits()
        {
            InitializeComponent();

            filtrerDisponible = false;
            Loaded += UCProduits_Loaded;

            ChargerProduits();
            InitComboBoxs();

            dgProduits.UnselectAll();
        }

        private void UCProduits_Loaded(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            if (window is ListeProduitWindow)
            {
                butAddProduit.Click -= butAddProduit_Click;
                dgProduits.SelectionChanged -= dgProduits_SelectionChanged;

                filtrerDisponible = true;
            }
        }

        private void InitComboBoxs()
        {
            // Catégories
            List<CategorieProduit> listeCategories = new List<CategorieProduit>(Core.Instance.CategorieProduits);
            listeCategories.Insert(0, new CategorieProduit(-1, "Toutes les catégories"));
            cbCategorie.ItemsSource = listeCategories;
            cbCategorie.SelectedIndex = 0;

            // Couleurs
            List<CouleurProduit> listeCouleurs = new List<CouleurProduit>(Core.Instance.CouleurProduits);
            listeCouleurs.Insert(0, new CouleurProduit(-1, "Toutes les couleurs"));
            cbCouleur.ItemsSource = listeCouleurs;
            cbCouleur.SelectedIndex = 0;

            // Types de pointe
            List<TypePointe> listePointes = new List<TypePointe>(Core.Instance.TypePointes);
            listePointes.Insert(0, new TypePointe(-1, "Toutes les pointes"));
            cbTypePointe.ItemsSource = listePointes;
            cbTypePointe.SelectedIndex = 0;

            UpdateTypeProduit(null);
        }

        private void UpdateTypeProduit(int? categorieId)
        {
            List<TypeProduit> listeTypes = new List<TypeProduit>(Core.Instance.TypeProduits);

            if (categorieId != -1 && categorieId != null)
                listeTypes = listeTypes.Where(t => t.Id == categorieId).ToList();

            listeTypes.Insert(0, new TypeProduit(-1, "Tous les types"));
            cbType.ItemsSource = listeTypes;
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
            string recherche = txtRecherche.Text;
            TypeProduit typeProduit = cbType.SelectedItem as TypeProduit;
            TypePointe typePointe = cbTypePointe.SelectedItem as TypePointe;
            CouleurProduit couleurProduit = cbCouleur.SelectedItem as CouleurProduit;

            Produit produit = (Produit)obj;

            if (filtrerDisponible && !produit.Disponible)
                return false;

            if (!produit.Nom.ToLower().StartsWith(recherche.ToLower()))
                return false;

            if (typeProduit != null && typeProduit.Id != -1 && produit.Type != typeProduit)
                return false;

            if (typePointe != null && typePointe.Id != -1 && produit.TypePointe.Id != typePointe.Id)
                return false;

            if (typeProduit != null && typeProduit.Id == -1)
            {
                if (!cbType.ItemsSource.Cast<TypeProduit>().Contains(produit.Type))
                    return false;
            }

            if (couleurProduit != null && couleurProduit.Id != -1 && !produit.Couleurs.Contains(couleurProduit))
                return false;

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
            Produit? produitSelected = dgProduits.SelectedItem as Produit;
            bool estResponsable = Core.Instance.EmployeConnecte.EstResponsableProduction;

            if (produitSelected == null)
            {
                butRendreIndisponibleProduit.Visibility = Visibility.Hidden;

                butModifierProduit.Visibility = Visibility.Hidden;
                butVisualiserProduit.Visibility = Visibility.Hidden;

                return;
            }

            butVisualiserProduit.Visibility = Visibility.Visible;

            if (estResponsable)
            {
                butModifierProduit.Visibility = Visibility.Visible;
                butRendreIndisponibleProduit.Visibility = Visibility.Visible;

                UpdateBtnIndisponible(produitSelected.Disponible);
            }
        }

        private void UpdateBtnIndisponible(bool nouvelleDisponibilite)
        {
            if (!nouvelleDisponibilite)
            {
                butRendreIndisponibleProduit.Content = "Disponible";
                butRendreIndisponibleProduit.Style = (Style)Application.Current.FindResource("SuccesButtonStyle");
            }
            else
            {
                butRendreIndisponibleProduit.Content = "Indisponible";
                butRendreIndisponibleProduit.Style = (Style)Application.Current.FindResource("SupprButtonStyle");
            }
        }

        private void butAddProduit_Click(object sender, RoutedEventArgs e)
        {
            Produit nouveauProduit = new Produit();
            CreerProduitWindow creerProduitWindow = new CreerProduitWindow(nouveauProduit, Utils.Action.Ajouter);

            bool dialogResult = creerProduitWindow.ShowDialog() ?? false;

            if (!dialogResult)
                return;

            try
            {
                nouveauProduit.Create();

                Core.Instance.Produits.Add(nouveauProduit);
                Core.MessageBoxSucces("Le produit a été créé avec succès.");
            } catch (Exception ex)
            {
                Core.MessageBoxErreur($"Une erreur est survenue lors de la création du produit : {ex.Message}");
            }
        }

        private void butVisualiserProduit_Click(object sender, RoutedEventArgs e)
        {
            Produit? produitSelected = dgProduits.SelectedItem as Produit;

            if (produitSelected == null)
                return;

            MainWindow mainWindow = ((MainWindow)App.Current.MainWindow);
            mainWindow.ccMain.Content = new UCVisualiserProduit(produitSelected, this);
        }

        private void butModifierProduit_Click(object sender, RoutedEventArgs e)
        {
            Produit? produitSelected = dgProduits.SelectedItem as Produit;

            if (produitSelected == null)
                return;

            Produit produitModifie = new Produit(produitSelected.Id, produitSelected.TypePointe, produitSelected.Type, produitSelected.Code, produitSelected.Nom, produitSelected.PrixVente, produitSelected.QuantiteStock, produitSelected.Couleurs, produitSelected.Disponible);
            CreerProduitWindow creerProduitWindow = new CreerProduitWindow(produitModifie, Utils.Action.Modifier);

            bool dialogResult = creerProduitWindow.ShowDialog() ?? false;

            if (!dialogResult)
                return;

            try
            {
                produitSelected.Code = produitModifie.Code;
                produitSelected.Nom = produitModifie.Nom;
                produitSelected.PrixVente = produitModifie.PrixVente;
                produitSelected.QuantiteStock = produitModifie.QuantiteStock;
                produitSelected.TypePointe = produitModifie.TypePointe;
                produitSelected.Type = produitModifie.Type;
                produitSelected.Disponible = produitModifie.Disponible;
                produitSelected.Couleurs = produitModifie.Couleurs;

                produitSelected.Update();

                Core.MessageBoxSucces("Le produit a été modifié avec succès.");
            } catch (Exception ex)
            {
                ChargerProduits();
                Core.MessageBoxErreur($"Une erreur est survenue lors de la modification du produit : {ex.Message}");
            }
        }

        private void butRendreIndisponibleProduit_Click(object sender, RoutedEventArgs e)
        {
            if (dgProduits.SelectedItem is not Produit produitSelected)
                return;

            bool nouvelleDisponibilite = !produitSelected.Disponible;
            string messageConfirmation = nouvelleDisponibilite
                ? "Êtes-vous sûr de vouloir rendre ce produit disponible ?"
                : "Êtes-vous sûr de vouloir rendre ce produit indisponible ?";

            string messageSucces = nouvelleDisponibilite
                ? "Le produit a été rendu disponible avec succès."
                : "Le produit a été rendu indisponible avec succès.";

            if (!Core.MessageBoxConfirmation(messageConfirmation))
                return;

            try
            {
                produitSelected.Disponible = nouvelleDisponibilite;
                produitSelected.Update();

                UpdateBtnIndisponible(nouvelleDisponibilite);

                Core.MessageBoxSucces(messageSucces);
            }
            catch (Exception ex)
            {
                Core.MessageBoxErreur($"Une erreur est survenue lors de la mise à jour du produit : {ex.Message}");
            }
        }


        private void txtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            produitView.Refresh();
        }

        private void cbCategorie_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CategorieProduit categorie = cbCategorie.SelectedItem as CategorieProduit;

            UpdateTypeProduit(categorie.Id);
            cbType.SelectedIndex = 0;

            produitView.Refresh();
        }

        private void cbFilterChange(object sender, SelectionChangedEventArgs e)
        {
            produitView.Refresh();
        }
    }
}
