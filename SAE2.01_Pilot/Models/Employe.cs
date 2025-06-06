using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Pilot.Models
{
    public class Employe
    {
        public int Id { get; set; }
        public string Nom {  get; set; }
        public string Prenom { get; set; }
        public Role Role { get; set; }

        public Employe()
        {
            
        }

        public Employe(int id, string nom, string prenom, Role role)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Role = role;
        }
    }
}
