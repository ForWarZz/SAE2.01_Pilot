﻿using SAE2._01_Pilot.Models;
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
    /// Logique d'interaction pour UCCommandes.xaml
    /// </summary>
    public partial class UCCommandes : UserControl
    {
        private ListCollectionView commandesView;

        private string typeFiltreDateActuel;
        private DateTime? dateDebutFiltre;
        private DateTime? dateFinFiltre;

        public UCCommandes()
        {
            InitializeComponent();
            ChargerCommandes();

            cbTypeDateFiltre.SelectedIndex = 0;

            butSupprCommande.Visibility = Visibility.Hidden;
            butVisualiserCommande.Visibility = Visibility.Hidden; 
            butAddCommande.Visibility = Visibility.Visible;

            InitComboBoxs();

            dgCommandes.UnselectAll();
        }

        private void ChargerCommandes()
        {
            Core.Instance.RefreshCommandes();

            commandesView = new ListCollectionView(Core.Instance.Commandes);
            commandesView.Filter = FiltrerCommandes;

            dgCommandes.ItemsSource = commandesView;
        }

        private bool FiltrerCommandes(object obj)
        {
            Commande commande = (Commande)obj;

            string recherche = txtRechercher.Text;
            Revendeur? revendeur = cbRevendeur.SelectedItem as Revendeur;

            if (!commande.Id.ToString().StartsWith(recherche))
                return false;

            if (revendeur != null && revendeur.Id != -1 && commande.Revendeur != revendeur)
                return false;

            if (!FiltrerParDate(commande))
                return false;

            return true;
        }


        private bool FiltrerParDate(Commande commande)
        {
            if (typeFiltreDateActuel == "aucun" || dateDebutFiltre == null)
                return true;

            switch (typeFiltreDateActuel)
            {
                case "avant":
                    return commande.DateCreation.Date < dateDebutFiltre.Value.Date;

                case "apres":
                    return commande.DateCreation.Date > dateDebutFiltre.Value.Date;

                case "exacte":
                    return commande.DateCreation.Date == dateDebutFiltre.Value.Date;

                case "entre":
                    if (dateFinFiltre == null)
                        return commande.DateCreation.Date >= dateDebutFiltre.Value.Date;

                    DateTime dateDebut = dateDebutFiltre.Value.Date;
                    DateTime dateFin = dateFinFiltre.Value.Date;

                    if (dateDebut > dateFin)
                    {
                        (dateFin, dateDebut) = (dateDebut, dateFin);
                    }

                    return commande.DateCreation.Date >= dateDebut && commande.DateCreation.Date <= dateFin;

                default:
                    return true;
            }
        }

        private void InitComboBoxs()
        {
            List<Revendeur> revendeurs = new List<Revendeur>(Core.Instance.Revendeurs);
            revendeurs.Insert(0, new Revendeur(-1, "Tous"));

            cbRevendeur.ItemsSource = revendeurs;
            cbRevendeur.SelectedIndex = 0;
        }

        private void dgCommandes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object selectedItem = dgCommandes.SelectedItem;

            if (selectedItem == null)
            {
                butSupprCommande.Visibility = Visibility.Hidden;
                butVisualiserCommande.Visibility = Visibility.Hidden;
                butModifierDateLivraison.Visibility = Visibility.Hidden;
                return;
            }

            butSupprCommande.Visibility = Visibility.Visible;
            butVisualiserCommande.Visibility = Visibility.Visible;
            butModifierDateLivraison.Visibility = Visibility.Visible;
        }

        private void butVisualiserCommande_Click(object sender, RoutedEventArgs e)
        {
            Commande commandeSelected = dgCommandes.SelectedItem as Commande;
            if (commandeSelected == null)
                return;

            MainWindow mainWindow = ((MainWindow)App.Current.MainWindow);
            mainWindow.ccMain.Content = new UCVisualiserCommande(commandeSelected, this);
        }

        private void txtRechercher_TextChanged(object sender, TextChangedEventArgs e)
        {
            commandesView.Refresh();
        }

        private void cbRevendeur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            commandesView.Refresh();
        }

        private void butAddCommande_Click(object sender, RoutedEventArgs e)
        {
            Commande commande = new Commande();

            CreerCommandeWindow creerCommandeWindow = new CreerCommandeWindow(commande);
            bool dialogResult = creerCommandeWindow.ShowDialog() ?? false;

            if (!dialogResult)
                return;

            try
            {
                commande.Create();
                Core.Instance.Commandes.Add(commande);

                Core.MessageBoxSucces("La commande a été créée avec succès.");
            }
            catch (Exception ex)
            {
                Core.MessageBoxErreur($"Une erreur est survenue lors de la création de la commande : {ex.Message}");
            }
        }

        private void butModifierDateLivraison_Click(object sender, RoutedEventArgs e)
        {
            Commande commande = dgCommandes.SelectedItem as Commande;
            Commande commandeModifie = new Commande(commande.Id, commande.ModeTransport, commande.Revendeur, commande.EmployeId, commande.DateCreation, commande.DateLivraison);

            ModifierDateLivraisonWindow modifierWindow = new ModifierDateLivraisonWindow(commandeModifie);
            bool dialogResult = modifierWindow.ShowDialog() ?? false;

            if (!dialogResult) 
                return;

            try
            {
                commande.DateLivraison = commandeModifie.DateLivraison;
                commande.Update();

                Core.MessageBoxSucces("La date de livraison à bien été appliquée.");
            } catch (Exception ex)
            {
                ChargerCommandes();
                Core.MessageBoxErreur($"Une erreur est survenue lors de la modification de la commande : {ex.Message}");
            }
        }

        private void cbTypeDateFiltre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTypeDateFiltre.SelectedItem is ComboBoxItem selectedItem)
            {
                string tag = selectedItem.Tag?.ToString() ?? "aucun";
                typeFiltreDateActuel = tag;

                // Gestion de l'affichage des DatePicker selon le type de filtre
                switch (tag)
                {
                    case "aucun":
                        dpDateFiltre.IsEnabled = false;
                        dpDateFiltreFin.Visibility = Visibility.Collapsed;

                        dateDebutFiltre = null;
                        dateFinFiltre = null;
                        break;

                    case "avant":
                    case "apres":
                    case "exacte":
                        dpDateFiltre.IsEnabled = true;
                        dpDateFiltreFin.Visibility = Visibility.Collapsed;

                        dateFinFiltre = null;
                        break;

                    case "entre":
                        dpDateFiltre.IsEnabled = true;
                        dpDateFiltreFin.Visibility = Visibility.Visible;
                        break;
                }

                commandesView.Refresh();
            }
        }

        private void dpDateFiltre_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dateDebutFiltre = dpDateFiltre.SelectedDate;
            commandesView.Refresh();
        }

        private void dpDateFiltreFin_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            dateFinFiltre = dpDateFiltreFin.SelectedDate;
            commandesView.Refresh();
        }

        private void butSupprCommande_Click(object sender, RoutedEventArgs e)
        {
            Commande? commande = dgCommandes.SelectedItem as Commande;
            if (commande == null)
                return;

            if (!Core.MessageBoxConfirmation("Voulez-vous vraiment supprimer cette commande ?"))
                return;

            try
            {
                commande.Delete();
                Core.Instance.Commandes.Remove(commande);

                Core.MessageBoxSucces("La commande à bien été supprimée.");
            } catch (Exception ex)
            {
                Core.MessageBoxErreur($"Une erreur est survenue lors de la suppression de la commande : {ex.Message}");
            }
        }
    }
}
