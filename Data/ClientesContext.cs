using Microsoft.EntityFrameworkCore;
using GestionClientesEFCore.Models;
using Microsoft.Extensions.Configuration;

namespace GestionClientesEFCore.Data
{
    public class ClientesContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }

        private readonly string _connectionString;

        public ClientesContext()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
        }
    }
}
