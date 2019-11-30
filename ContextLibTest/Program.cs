using System;
using jguerra.db.context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ContextLibTest
{
    class Program
    {
        static void Main(string[] args)
        {

            var optionsBuilder = new DbContextOptionsBuilder<NotasMiUmg>();
            optionsBuilder.UseMySQL("server=192.168.10.10;database=notasmiumg;user=appnotasmiumg;password=6y$EL&uk");

            using (var db = new NotasMiUmg(optionsBuilder.Options))
            {
                db.Database.EnsureCreated();
                Console.WriteLine(db.Estudiantes.AsEnumerable().Count());
            }
        }
    }
}
