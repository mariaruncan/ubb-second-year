using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemaLab1
{
    internal class Column
    {   
        public string Name { get; set; }
        public bool IsPK { get; set; }
        public bool IsFK { get; set; }

        public Column(string name, bool isPK, bool isFK)
        {
            Name = name;
            IsPK = isPK;
            IsFK = isFK;
        }
    }
}
