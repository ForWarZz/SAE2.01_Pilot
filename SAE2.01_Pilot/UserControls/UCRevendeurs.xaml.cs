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
    /// Logique d'interaction pour UCRevendeurs.xaml
    /// </summary>
    public partial class UCRevendeurs : UserControl
    {
        private ListCollectionView revendeursView;

        public UCRevendeurs()
        {
            InitializeComponent();

            ChargerRevendeurs();
        }

        private void ChargerRevendeurs()
        {
            Core.Instance.RefreshRevendeurs();

            revendeursView = new ListCollectionView(Core.Instance.Revendeurs);
            revendeursView.Filter = FiltrerRevendeurs;

            dgRevendeur.ItemsSource = revendeursView;
            dgRevendeur.UnselectAll();
        }

        private bool FiltrerRevendeurs(object obj)
        {
            string recherche = txtRecherche.Text;
            Revendeur revendeur = (Revendeur)obj;

            if (!revendeur.RaisonSociale.ToLower().StartsWith(recherche.ToLower()))
                return false;

            return true;
        }

        private void butAddRevendeur_Click(object sender, RoutedEventArgs e)
        {
            Revendeur revendeur = new Revendeur();
            CreerRevendeurWindow creerRevendeurWindow = new CreerRevendeurWindow(revendeur, Utils.Action.Ajouter);
            bool dialogResult = creerRevendeurWindow.ShowDialog() ?? false;

            if (!dialogResult)
                return;

            try
            {
                revendeur.Create();

                Core.Instance.Revendeurs.Add(revendeur);
                Core.MessageBoxSucces("Le revendeur a été créé avec succès.");

            } catch (Exception ex)
            {
                Core.MessageBoxErreur($"Une erreur est survenue lors de la création du revendeur : {ex.Message}");
            }
        }

        private void butModifierRevendeur_Click(object sender, RoutedEventArgs e)
        {
            Revendeur? revendeur = dgRevendeur.SelectedItem as Revendeur;

            if (revendeur == null)
                return;

            Revendeur revendeurModifie = new Revendeur(revendeur.Id, revendeur.RaisonSociale, revendeur.Adresse);

            CreerRevendeurWindow creerRevendeurWindow = new CreerRevendeurWindow(revendeurModifie, Utils.Action.Modifier);
            bool dialogResult = creerRevendeurWindow.ShowDialog() ?? false;

            if (!dialogResult)
                return;

            try
            {
                revendeur.RaisonSociale = revendeurModifie.RaisonSociale;
                revendeur.Adresse = revendeurModifie.Adresse;
                revendeur.Update();

                Core.MessageBoxSucces("Le revendeur a été modifié avec succès.");
            }
            catch (Exception ex)
            {
                Core.MessageBoxErreur($"Une erreur est survenue lors de la modification du revendeur : {ex.Message}");
            }
        }

        private void dgRevendeur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Revendeur? revendeur = dgRevendeur.SelectedItem as Revendeur;

            if (revendeur == null)
            {
                butModifierRevendeur.Visibility = Visibility.Hidden;
                return;
            }

            bool estCommercial = Core.Instance.EmployeConnecte.EstCommercial;

            if (estCommercial)
                butModifierRevendeur.Visibility = Visibility.Visible;
        }

        private void txtRecherche_TextChanged(object sender, TextChangedEventArgs e)
        {
            revendeursView.Refresh();
        }
    }
}
