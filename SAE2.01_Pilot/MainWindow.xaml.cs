using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAE2._01_Pilot
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void mi_Click(object sender, RoutedEventArgs e)
        {
            // Détecte le MenuItem (mi) cliqué - change le style - remet le style de base aux autres mi
            MenuItem itemClique = sender as MenuItem;
            itemClique.Style = (Style)FindResource("MenuItemSelectStyle");
            foreach (MenuItem item in MenuTop.Items)
            {
                if (item != itemClique)
                    ResetMenuItemStyle(item);
            }
        }

        private void ResetMenuItemStyle(MenuItem mi)
        {
            mi.Style = (Style)FindResource("MenuItemStyle");
        }
    }
}