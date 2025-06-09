using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAE2._01_Pilot.Database
{
    public interface ICrud<T>
    {
        public void Create();
        public void Update();
        public void Delete();
    }
}
