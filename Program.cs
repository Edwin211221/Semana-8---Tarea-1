using GestionClientesEFCore.Services;

/*
 * Proyecto: Práctica de Transacciones con EF Core y MySQL
 * Autor: Edwin Barrazueta
 * Descripción:
 *  Este programa prueba el manejo de transacciones en Entity Framework Core,
 *  permitiendo transferencias y eliminaciones de clientes en una base de datos MySQL,
 *  mostrando mensajes de éxito o error en consola según los requerimientos de la tarea.
 */


class Program
{
    static async Task Main(string[] args)
    {
        var servicio = new ClienteService();

        // 1. Probar transferencia de saldo (ajusta los Ids y el monto según tus datos de prueba)
        Console.WriteLine("Probando transferencia de saldo...");
        await servicio.TransferirSaldoAsync(1, 2, 100m); // Transfiere 100 de Bruce (Id=1) a Clark (Id=2)

        // 2. Probar transferencia fallida (por saldo insuficiente)
        Console.WriteLine("\nProbando transferencia fallida...");
        await servicio.TransferirSaldoAsync(2, 1, 10000m); // Intenta transferir más de lo que tiene Clark

        // 3. Probar eliminación exitosa (Arthur debe tener saldo 0)
        Console.WriteLine("\nProbando eliminación de cliente...");
        await servicio.EliminarClienteAsync(5); // Elimina a Arthur (Id=5)

        // 4. Probar eliminación fallida (cliente con saldo diferente de 0)
        Console.WriteLine("\nProbando eliminación fallida...");
        await servicio.EliminarClienteAsync(1); // Bruce tiene saldo > 0

        Console.WriteLine("\nPruebas finalizadas. Presiona cualquier tecla para salir.");
        Console.ReadKey();
    }
}
