using SAE2._01_Pilot.Models;
using SAE2._01_Pilot.Utils;
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
    /// Logique d'interaction pour ListeRevendeurWindow.xaml
    /// </summary>
    public partial class ListeRevendeurWindow : Window
    {
        public ListeRevendeurWindow()
        {
            InitializeComponent();

            ucRevendeurs.dgRevendeur.MouseDoubleClick += dgRevendeur_MouseDoubleClick;
            ucRevendeurs.dgRevendeur.SelectionChanged += dgRevendeur_SelectionChanged;
            ucRevendeurs.butAddRevendeur.Click += butAddRevendeur_Click;

            ucRevendeurs.butAddRevendeur.Visibility = Visibility.Hidden;
            ucRevendeurs.butAddRevendeur.Content = "Valider";
        }

        private void dgRevendeur_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;
        }

        private void dgRevendeur_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ucRevendeurs.dgRevendeur.SelectedItem is not Revendeur)
                return;

            ucRevendeurs.butAddRevendeur.Visibility = Visibility.Visible;
        }

        private void butAddRevendeur_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
