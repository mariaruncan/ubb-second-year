using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class Entity<ID>
    {
        public ID Id { get; set; }

        public Entity(ID id)
        {
            this.Id = id;
        }
    }
}
