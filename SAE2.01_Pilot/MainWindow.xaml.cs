using SAE2._01_Pilot.Models;
using SAE2._01_Pilot.UserControls;
using SAE2._01_Pilot.Utils;
using SAE2._01_Pilot.Windows;
using System.DirectoryServices.ActiveDirectory;
using System.Reflection;
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
        public Employe EmployeConnecte { get; set; }

        public MainWindow()
        {
            Start();
        }

        private void Charger()
        {
            new Core(EmployeConnecte).ChargerDonneesStatiques();
        }

        private void miCommandes_Click(object sender, RoutedEventArgs e)
        {
            ChangerStyleMenu(sender);
            ccMain.Content = new UserControls.UCCommandes();
        }
        private void miRevendeurs_Click(object sender, RoutedEventArgs e)
        {
            ChangerStyleMenu(sender);
            ccMain.Content = new UserControls.UCRevendeurs();
        }
        private void miProduits_Click(object sender, RoutedEventArgs e)
        {
            ChangerStyleMenu(sender);
            ccMain.Content = new UserControls.UCProduits();
        }

        private void ChangerStyleMenu(object sender)
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

        private void Start()
        {
            Connexion connexionWindow = new Connexion();
            bool? result = connexionWindow.ShowDialog();

            if (result != true)
            {
                Application.Current.Shutdown();
                return;
            }

            EmployeConnecte = connexionWindow.EmployeResult;

            InitializeComponent();

            new Core(EmployeConnecte).ChargerDonneesStatiques();
            spTop.DataContext = Core.Instance.EmployeConnecte;

            if (EmployeConnecte.EstResponsableProduction)
            {
                ChangerStyleMenu(miProduits);
                ccMain.Content = new UserControls.UCProduits();

                miCommandes.Visibility = Visibility.Hidden;
                miRevendeurs.Visibility = Visibility.Visible;
            }
            else
            {
                ChangerStyleMenu(miCommandes);
                ccMain.Content = new UserControls.UCCommandes();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            Core.Instance = null;
            Start();
            Show();
        }
    }
}