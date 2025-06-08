using SAE2._01_Pilot.Models;
using SAE2._01_Pilot.Windows;
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
        public List<TypePointe> TypePointes { get => typePointes; set => typePointes = value; }
        public List<TypeProduit> TypeProduits { get => typeProduits; set => typeProduits = value; }
        public List<CategorieProduit> CategorieProduits { get => categorieProduits; set => categorieProduits = value; }
        public List<CouleurProduit> CouleurProduits { get => couleurProduits; set => couleurProduits = value; }
        public List<ModeTransport> ModeTransports { get => modeTransports; set => modeTransports = value; }

        private List<TypePointe> typePointes;
        private List<TypeProduit> typeProduits;
        private List<CategorieProduit> categorieProduits;
        private List<CouleurProduit> couleurProduits;
        private List<ModeTransport> modeTransports;

        public MainWindow()
        {
            /*CheckLogin();*/
            InitializeComponent();
        }

        private void CheckLogin()
        {
            this.Hide();

            Connexion connexionWindow = new Connexion();
            connexionWindow.ShowDialog();

            if (connexionWindow.DialogResult == true)
            {
                EmployeConnecte = connexionWindow.EmployeResult;
                spTop.DataContext = EmployeConnecte;

                Charger();
                this.Show();
            } else
            {
                Application.Current.Shutdown();
            }
        }

        private void Charger()
        {
            TypePointes = TypePointe.GetAll();
            CouleurProduits = CouleurProduit.GetAll();
            CategorieProduits = CategorieProduit.GetAll();
            TypeProduits = TypeProduit.GetAll();
            ModeTransports = ModeTransport.GetAll();

            foreach (TypeProduit typeProduit in TypeProduits)
            {
                CategorieProduit? categorie = CategorieProduits.FirstOrDefault(c => c.Id == typeProduit.Categorie.Id);
                if (categorie == null)
                {
                    MessageBox.Show($"La catégorie du type de produit {typeProduit.Libelle} n'a pas été trouvée.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                typeProduit.Categorie = categorie;
            }
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
    }
}