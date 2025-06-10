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

        public UCCommandes()
        {
            InitializeComponent();

            ChargerCommandes();

            HandleRoleEmploye();
            InitComboBoxs();
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
            return true;
        }

        private void InitComboBoxs()
        {
            List<Revendeur> revendeurs = new List<Revendeur>(Core.Instance.Revendeurs);
            revendeurs.Insert(0, new Revendeur(-1, "Tous"));

            cbRevendeur.ItemsSource = revendeurs;
            cbRevendeur.SelectedIndex = 0;
        }

        private void HandleRoleEmploye()
        {
            bool estResponsableProd = Core.Instance.EmployeConnecte.EstResponsableProduction;

            if (!estResponsableProd)
            {
                butAddCommande.Visibility = Visibility.Hidden;
                butSupprCommande.Visibility = Visibility.Hidden;

                butVisualiserCommande.Style = (Style)FindResource("PrimaryButtonStyle");
            }
        }

        private void dgCommandes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool estResponsableProd = Core.Instance.EmployeConnecte.EstResponsableProduction;
            object selectedItem = dgCommandes.SelectedItem;

            if (selectedItem == null)
            {
                butSupprCommande.Visibility = Visibility.Hidden;
                return;
            }

            butSupprCommande.Visibility = estResponsableProd ? Visibility.Visible : Visibility.Hidden;
        }
    }
}
