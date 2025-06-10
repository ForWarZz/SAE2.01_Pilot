using SAE2._01_Pilot.Models;
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
            bool ok = true;
            foreach (UIElement uie in spFormulaire.Children)
            {
                if (uie is TextBox)
                {
                    TextBox txt = (TextBox)uie;
                    txt.GetBindingExpression(TextBox.TextProperty).UpdateSource();
                }

                if (Validation.GetHasError(uie))
                {
                    ok = false;
                }
            }

            if (ok)
                DialogResult = true;
        }
    }
}
