using SAE2._01_Pilot.Models;
using SAE2._01_Pilot.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

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

        public static Core Instance { get; set; }

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

        public void ChargerDonneesStatiques()
        {
            try
            {
                ModeTransports = ModeTransport.GetAll();
                TypePointes = TypePointe.GetAll();
                CategorieProduits = CategorieProduit.GetAll();
                TypeProduits = TypeProduit.GetAll(CategorieProduits);
                CouleurProduits = CouleurProduit.GetAll();
            } catch (Exception ex)
            {
                MessageBoxErreur($"Erreur lors du chargement des données statiques : {ex.Message}");
            }
        }

        public void RefreshRevendeurs()
        {
            try
            {
                Revendeurs = Revendeur.GetAll();
            } catch (Exception ex)
            {
                MessageBoxErreur($"Erreur lors du chargement des revendeurs : {ex.Message}");
            }
        }

        public void RefreshProduits()
        {
            try
            {
                Produits = Produit.GetAll(TypePointes, TypeProduits, CouleurProduits);
            }
            catch (Exception ex)
            {
                MessageBoxErreur($"Erreur lors du chargement des produits : {ex.Message}");
            }
        }

        public void RefreshCommandes()
        {
            try
            {
                RefreshRevendeurs();
                RefreshProduits();

                Commandes = Commande.GetFromEmploye(ModeTransports, Revendeurs, Produits, EmployeConnecte);
            }
            catch (Exception ex)
            {
                MessageBoxErreur($"Erreur lors du chargement des commandes du commercial : {ex.Message}");
            }
        }

        public static void MessageBoxErreur(string message)
        {
            /*MessageBox.Show(message, "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);*/
            PopUp popUp = new PopUp(PopUp.TypePopUp.Succes, "Erreur", message);
            popUp.ShowDialog();
        }

        public static void MessageBoxSucces(string message)
        {
  /*          MessageBox.Show(message, "Succès", MessageBoxButton.OK, MessageBoxImage.Information);*/
            PopUp popUp = new PopUp(PopUp.TypePopUp.Succes, "Succès", message);
            popUp.ShowDialog();
        }

        public static bool MessageBoxConfirmation(string message)
        {
            /*return MessageBox.Show(message, "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes;*/
            PopUp popUp = new PopUp(PopUp.TypePopUp.Confirmation, "Confirmation", message);
            return (popUp.ShowDialog() ?? false) == true;
        }

        public static bool ValiderFormulaire(DependencyObject container)
        {
            bool ok = true;

            foreach (object child in LogicalTreeHelper.GetChildren(container))
            {
                if (child is UIElement uie)
                {
                    BindingOperations.GetBindingExpression(uie, TextBox.TextProperty)?.UpdateSource();
                    BindingOperations.GetBindingExpression(uie, ComboBox.SelectedItemProperty)?.UpdateSource();
                    BindingOperations.GetBindingExpression(uie, CheckBox.IsCheckedProperty)?.UpdateSource();
                    BindingOperations.GetBindingExpression(uie, ListBox.SelectedItemsProperty)?.UpdateSource();

                    if (Validation.GetHasError(uie))
                        ok = false;

/*                    if (uie is DependencyObject nestedContainer)
                    {
                        if (!ValiderFormulaire(nestedContainer))
                        {
                            ok = false;
                        }
                    }*/
                }
            }

            return ok;
        }
    }
}
