using GestionClientesEFCore.Data;
using GestionClientesEFCore.Models;
using Microsoft.EntityFrameworkCore;

namespace GestionClientesEFCore.Services
{
    public class ClienteService
    {
        private readonly ClientesContext _context;

        // Constructor: Inicializa el contexto de la base de datos
        public ClienteService()
        {
            _context = new ClientesContext();
        }

        /// <summary>
        /// Transfiere saldo de un cliente origen a un cliente destino usando una transacción.
        /// Si ocurre cualquier error (cliente no existe, saldo insuficiente, etc.), la transacción se revierte.
        /// </summary>
        /// <param name="idOrigen">Id del cliente que envía el saldo</param>
        /// <param name="idDestino">Id del cliente que recibe el saldo</param>
        /// <param name="monto">Monto a transferir</param>
        public async Task TransferirSaldoAsync(int idOrigen, int idDestino, decimal monto)
        {
            // Iniciar una transacción
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Buscar los clientes origen y destino en la base de datos
                var clienteOrigen = await _context.Clientes.FindAsync(idOrigen);
                var clienteDestino = await _context.Clientes.FindAsync(idDestino);

                // Verificar que ambos clientes existen
                if (clienteOrigen == null)
                    throw new Exception($"Cliente origen con Id={idOrigen} no existe.");
                if (clienteDestino == null)
                    throw new Exception($"Cliente destino con Id={idDestino} no existe.");

                // Verificar que el cliente origen tiene suficiente saldo
                if (clienteOrigen.Saldo < monto)
                    throw new Exception("Saldo insuficiente para realizar la transferencia.");

                // Realizar la transferencia de saldos
                clienteOrigen.Saldo -= monto;
                clienteDestino.Saldo += monto;

                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();
                // Confirmar (commit) la transacción
                await transaction.CommitAsync();

                Console.WriteLine($"Transferencia de {monto:C} realizada con éxito de {clienteOrigen.Nombre} a {clienteDestino.Nombre}.");
            }
            catch (Exception ex)
            {
                // Si ocurre algún error, revertir (rollback) la transacción
                await transaction.RollbackAsync();
                Console.WriteLine($"Error en la transferencia: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina un cliente de la base de datos solo si su saldo es 0, usando una transacción.
        /// Si el saldo no es 0, lanza una excepción y revierte la operación.
        /// </summary>
        /// <param name="idCliente">Id del cliente a eliminar</param>
        public async Task EliminarClienteAsync(int idCliente)
        {
            // Iniciar una transacción
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Buscar el cliente en la base de datos
                var cliente = await _context.Clientes.FindAsync(idCliente);
                if (cliente == null)
                    throw new Exception($"Cliente con Id={idCliente} no existe.");

                // Verificar que el saldo sea 0
                if (cliente.Saldo != 0)
                    throw new Exception("No se puede eliminar un cliente con saldo diferente de 0.");

                // Eliminar el cliente
                _context.Clientes.Remove(cliente);

                // Guardar cambios y confirmar la transacción
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                Console.WriteLine($"Cliente '{cliente.Nombre}' eliminado correctamente.");
            }
            catch (Exception ex)
            {
                // Si hay error, revertir la transacción
                await transaction.RollbackAsync();
                Console.WriteLine($"Error al eliminar cliente: {ex.Message}");
            }
        }
    }
}