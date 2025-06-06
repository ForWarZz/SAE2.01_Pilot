using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Pilot.Models
{
    public class Role
    {
        public int Id { get; set; }
        public LibelleRole Libelle { get; set; }

        public Role(int id, LibelleRole libelle)
        {
            Id = id;
            Libelle = libelle;
        }
    }

    public enum LibelleRole
    {
        Commercial,
        ResponsableProduction
    }
}
