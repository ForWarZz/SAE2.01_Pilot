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
    /// Logique d'interaction pour PopUp.xaml
    /// </summary>
    public partial class PopUp : Window
    {

        public enum TypePopUp
        {
            Confirmation,
            Succes,
            Erreur
        }
        private TypePopUp type;
        private string messagePopUp;
        private string titrePopUp;

        public TypePopUp Type { get => type; set => type = value; }
        public string MessagePopUp { get => messagePopUp; set => messagePopUp = value; }
        public string TitlePopUp { get => titrePopUp; set => titrePopUp = value; }

        public PopUp(TypePopUp type, string titre, string message)
        {
            InitializeComponent();
            labTitre.Content = titre;
            tbMessage.Text = message;

            if (type == TypePopUp.Confirmation)
                ImgagePopUp.Source = new BitmapImage(new Uri("pack://application:,,,/Images/valider.png"));

            else if (type == TypePopUp.Succes)
            {
                ImgagePopUp.Source = new BitmapImage(new Uri("pack://application:,,,/Images/valider.png"));
                butSecondaire.Visibility = Visibility.Collapsed;
                butPrincipale.Content = "OK";
            }
            else
            {
                ImgagePopUp.Source = new BitmapImage(new Uri("pack://application:,,,/Images/erreur.png"));
                butSecondaire.Visibility = Visibility.Collapsed;
                butPrincipale.Content = "OK";
            }

        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ButtonState == MouseButtonState.Pressed)
                DragMove();
        }

        private void butPrincipale_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void butSecondaire_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
