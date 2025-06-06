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
using System.Windows.Shapes;

namespace SAE2._01_Pilot.Windows
{
    /// <summary>
    /// Logique d'interaction pour Connexion.xaml
    /// </summary>
    public partial class Connexion : Window
    {
        public Employe EmployeResult {  get; set; }

        public Connexion()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string identifiant = inputLogin.Text;
            string password = inputPassword.Password.ToString();

            if (String.IsNullOrEmpty(identifiant))
            {
                MessageBox.Show("Veuillez entrez un identifiant pour vous connecter.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (String.IsNullOrEmpty(password))
            {
                MessageBox.Show("Veuillez entrez un mot de passe pour vous connecter.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                inputPassword.Clear();
                return;
            }

            Employe? employe = Employe.FindByCredentials(identifiant, password);
            if (employe == null)
            {
                MessageBox.Show("Les identifiants entrés sont incorrect. Veuillez réessayer.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                inputPassword.Clear();
                return;
            }

            EmployeResult = employe;
            DialogResult = true;
        }
    }
}
