using SAE2._01_Pilot.Models;
using SAE2._01_Pilot.UserControls;
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
using System.Windows.Shapes;

namespace SAE2._01_Pilot.Windows
{
    /// <summary>
    /// Logique d'interaction pour ListeProduitWindow.xaml
    /// </summary>
    public partial class ListeProduitWindow : Window
    {
        public ListeProduitWindow()
        {
            InitializeComponent();

            ucProduits

            ucProduits.dgProduits.MouseDoubleClick += dgProduit_MouseDoubleClick;
            ucProduits.dgProduits.SelectionChanged += dgProduit_SelectionChanged;
            ucProduits.butAddProduit.Click += butAddProduit;

            ucProduits.butAddProduit.Visibility = Visibility.Hidden;
            ucProduits.butAddProduit.Content = "Valider";
        }

        private void dgProduit_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
        }

        private void dgProduit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ucProduits.dgProduits.SelectedItem is not Produit)
                return;

            ucProduits.butAddProduit.Visibility = Visibility.Visible;
        }

        private void butAddProduit(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
