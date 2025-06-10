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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SAE2._01_Pilot.UserControls
{
    /// <summary>
    /// Logique d'interaction pour UCRevendeurs.xaml
    /// </summary>
    public partial class UCRevendeurs : UserControl
    {
        private ListCollectionView revendeursView;

        public UCRevendeurs()
        {
            InitializeComponent();

            ChargerRevendeurs();
        }

        private void ChargerRevendeurs()
        {
            Core.Instance.RefreshRevendeurs();

            revendeursView = new ListCollectionView(Core.Instance.Revendeurs);
            revendeursView.Filter = FiltrerRevendeurs;

            dgRevendeur.ItemsSource = revendeursView;
        }

        private bool FiltrerRevendeurs(object obj)
        {
            return true;
        }
    }
}
