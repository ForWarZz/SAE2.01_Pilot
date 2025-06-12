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
    /// Logique d'interaction pour UCVisualiserCommande.xaml
    /// </summary>
    public partial class UCVisualiserCommande : UserControl
    {
        private UserControl ucProduit;
        public UCVisualiserCommande(Commande commande, UserControl ucProduit)
        {
            this.ucProduit = ucProduit;

            DataContext = commande;
            InitializeComponent();
        }

        private void butRetour_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = ((MainWindow)App.Current.MainWindow);
            mainWindow.ccMain.Content = ucProduit;
        }
    }
}
