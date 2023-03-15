using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.db.ef
{
    public class DataContext:DbContext
    {
        private static bool created = false;

        public DataContext()
        {
            if (!created)
            {
                created = true;
                Database.EnsureCreated();
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=D:\\Facultate\\Semestrul 4\\MPP\\mpp-proiect-repository-mariaruncan\\proiect.db");
            SQLitePCL.Batteries.Init();
        }

        public DbSet<Emp> Employees { get; set; }
    }

    [Table("Employees")]
    public class Emp
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long id { get; set; }
        
        [Required]
        [MaxLength(30)]
        public string firstname { get; set; }

        [Required]
        [MaxLength(30)]
        public string lastname { get; set; }

        [Required]
        [MaxLength(30)]
        public string username { get; set; }

        [Required]
        [MaxLength(50)]
        public string hashedPassword { get; set; }

    }
}
