using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Pilot.Models
{
    public class Adresse
    {
        private string rue;
        private string ville;
        private string codePostal;

        public Adresse(string rue, string codePostal, string ville)
        {
            Rue = rue;
            CodePostal = codePostal;
            Ville = ville;
        }

        public Adresse()
        {
            
        }

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

        public string Rue { 
            get => rue; 
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("La rue ne peut pas être vide.");
                }

                rue = value;
            }
        }
        public string Ville { 
            get => ville;
            set
            {
                if (String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("La ville ne peut pas être vide.");
                }

                ville = value;
            }
        }
    }
}
