using SAE2._01_Pilot.Models;
using SAE2._01_Pilot.UserControls;
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
    /// Logique d'interaction pour ListeProduitWindow.xaml
    /// </summary>
    public partial class ListeProduitWindow : Window
    {
        public UCProduits UCProduit { get; private set; }

        public ListeProduitWindow(ObservableCollection<LigneCommande> ligneCommandesActuelles)
        {
            InitializeComponent();

            UCProduit = new UCProduits(ligneCommandesActuelles
                .Select(lc => lc.Produit)
                .ToList());

            Grid.SetRow(UCProduit, 1);
            grid.Children.Add(UCProduit);

            UCProduit.dgProduits.MouseDoubleClick += dgProduit_MouseDoubleClick;
            UCProduit.dgProduits.SelectionChanged += dgProduit_SelectionChanged;
            UCProduit.butAddProduit.Click += butAddProduit;

            UCProduit.butAddProduit.Visibility = Visibility.Hidden;
            UCProduit.butAddProduit.Content = "Valider";
        }

        private void dgProduit_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
        }

        private void dgProduit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UCProduit.dgProduits.SelectedItem is not Produit)
                return;

            UCProduit.butAddProduit.Visibility = Visibility.Visible;
        }

        private void butAddProduit(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
