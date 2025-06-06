using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Pilot.Models
{
    public class Adresse
    {
        public string Rue { get; set; }
        public string Ville { get; set; }

        private string codePostal;

        public string CodePostal { 
            get => codePostal;

            set
            {
                if (value.Length < 5)
                {
                    throw new ArgumentOutOfRangeException("Le code postal n'est pas valide.");
                }

                codePostal = value;
            }
        }
    }
}
