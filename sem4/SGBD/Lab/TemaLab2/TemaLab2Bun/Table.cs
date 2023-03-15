using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemaLab1
{
    internal class Table
    {
        public String Name { get; set; }
        public int NoFields { get; set; }
        public String SelectCmd { get; set; }
        public String UpdateCmd { get; set; }
        public String DeleteCmd { get; set; }
        public String InsertCmd { get; set; }
        public List<Column> Columns { get; set; }

        public Table(String name, int noFields)
        {
            Columns = new List<Column>();
            Name = name;
            NoFields = noFields;
        }
    }
}
