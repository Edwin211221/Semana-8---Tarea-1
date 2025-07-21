namespace GestionClientesEFCore.Models
{
    public class Cliente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public decimal Saldo { get; set; }
    }
}