using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Serializable]
    public class Team : Entity<long>
    {
        public string Name { get; set; }

        public Team(): base(0)
        {
            Name = "";
        }

        public Team(string name): base(0)
        {
            Name = name;
        }

        public override string ToString()
        {
            return "Team: " + base.Id + " " + Name;
        }

        public override bool Equals(object obj)
        {
            return obj is Team team &&
                   Id == team.Id &&
                   Name == team.Name;
        }

        public override int GetHashCode()
        {
            int hashCode = -1919740922;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            return hashCode;
        }
    }
}
