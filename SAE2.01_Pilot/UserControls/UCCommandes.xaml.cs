using SAE2._01_Pilot.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logique d'interaction pour UCCommandes.xaml
    /// </summary>
    public partial class UCCommandes : UserControl
    {
        public ObservableCollection<Commande> Commandes { get; set; }

        public UCCommandes()
        {
            // ChargerCommandes();
            InitializeComponent();

            // dgCommandes.DataContext = Commandes;
        }

        private void ChargerCommandes()
        {
            Commandes = new ObservableCollection<Commande>(Commande.GetAll());
        }
    }
}
