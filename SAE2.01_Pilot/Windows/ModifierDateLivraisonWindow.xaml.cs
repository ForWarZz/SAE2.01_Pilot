﻿using SAE2._01_Pilot.Models;
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
    /// Logique d'interaction pour ModifierDateLivraisonWindow.xaml
    /// </summary>
    public partial class ModifierDateLivraisonWindow : Window
    {
        public ModifierDateLivraisonWindow(Commande commande)
        {
            DataContext = commande;
            InitializeComponent();
        }

        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {
            bool ok = Core.ValiderFormulaire(spFormulaire);

            if (!ok)
                return;

            DialogResult = true;
        }
    }
}
