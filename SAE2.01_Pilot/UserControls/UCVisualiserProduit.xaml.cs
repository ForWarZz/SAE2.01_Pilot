using SAE2._01_Pilot.Models;
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
    /// Logique d'interaction pour UCVisualiserProduit.xaml
    /// </summary>
    public partial class UCVisualiserProduit : UserControl
    {
        private UserControl ucProduits;

        public UCVisualiserProduit(Produit produitSelected, UserControl ucProduits)
        {
            DataContext = produitSelected;
            this.ucProduits = ucProduits;

            InitializeComponent();
        }

        private void butRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = ((MainWindow)App.Current.MainWindow);
            mainWindow.ccMain.Content = ucProduits;
        }
    }
}
