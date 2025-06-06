using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Pilot.Models
{
    public class Revendeur
    {
        public int Id { get; set; }
        public string RaisonSociale { get; set; }
        public Adresse Adresse { get; set; }

        public Revendeur()
        {
            
        }

        public Revendeur(int id, string raisonSociale, Adresse adresse)
        {
            Id = id;
            RaisonSociale = raisonSociale;
            Adresse = adresse;
        }
    }
}
