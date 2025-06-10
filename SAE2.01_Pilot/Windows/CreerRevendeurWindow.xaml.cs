using SAE2._01_Pilot.Models;
using SAE2._01_Pilot.Utils;
using System.Windows;
using System.Windows.Controls;

namespace SAE2._01_Pilot.Windows
{
    /// <summary>
    /// Logique d'interaction pour CreerRevendeurWindow.xaml
    /// </summary>
    public partial class CreerRevendeurWindow : Window
    {
        public CreerRevendeurWindow(Revendeur nouveauRevendeur, Utils.Action action)
        {
            DataContext = nouveauRevendeur;

            InitializeComponent();

            btnCreer.Content = action == Utils.Action.Ajouter ? "Créer un revendeur" : "Modifier le revendeur";
            txtTitre.Text = btnCreer.Content.ToString();
        }

        private void btnCreer_Click(object sender, RoutedEventArgs e)
        {
            bool ok = Core.ValiderFormulaire(spFormulaire);

            if (ok)
                DialogResult = true;
        }
    }
}
