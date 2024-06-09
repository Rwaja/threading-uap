using System.Threading;

namespace ClaseHilos
{
    internal class Producto
    {
        public string Nombre { get; set; }
        public decimal PrecioUnitarioDolares { get; set; }
        public int CantidadEnStock { get; set; }

        public Producto(string nombre, decimal precioUnitario, int cantidadEnStock)
        {
            Nombre = nombre;
            PrecioUnitarioDolares = precioUnitario;
            CantidadEnStock = cantidadEnStock;
        }
    }
    internal class Solution //reference: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/lock
    {

        static List<Producto> productos = new List<Producto>
        {
            new Producto("Camisa", 10, 50),
            new Producto("Pantalón", 8, 30),
            new Producto("Zapatilla/Champión", 7, 20),
            new Producto("Campera", 25, 100),
            new Producto("Gorra", 16, 10)
        };
        static SemaphoreSlim semaphore = new SemaphoreSlim(2, 2);

        static int precio_dolar = 500;

        static void Tarea1()
        {
            Console.WriteLine("Tarea 1 en fila");
            semaphore.Wait();
            Console.WriteLine("Tarea 1 en ejecucion");

            lock (productos)
            {
                foreach (var producto in productos)
                {
                    producto.CantidadEnStock += 10;
                    Console.WriteLine($"Stock de {producto.Nombre} actualizado a {producto.CantidadEnStock} unidades");
                }
            }
            Console.WriteLine("\n---------------------------------------------------------------\n");
            Thread.Sleep(500);
            semaphore.Release();
        }
        static void UpdateDolarPrice()
        {
            Console.WriteLine("Tarea 2 en fila");
            semaphore.Wait();
            Console.WriteLine("Tarea 2 en ejecucion");

            lock (productos)
            {
                foreach (var producto in productos)
                {
                    producto.PrecioUnitarioDolares += precio_dolar;
                    Console.WriteLine($"Precio de {producto.Nombre} actualizado a ${producto.PrecioUnitarioDolares} /// Diferencia neta: {precio_dolar}");
                }
            }
            Console.WriteLine("\n---------------------------------------------------------------\n");
            Thread.Sleep(3000);
            semaphore.Release();
        }
   
        

        static void Tarea3()
      {
            Console.WriteLine("Tarea 3 en fila");
            semaphore.Wait();
            Console.WriteLine("Tarea 3 en ejecucion");

            lock (productos)
            {
                decimal total = 0;
                foreach (var producto in productos)
                {
                    total += producto.PrecioUnitarioDolares * producto.CantidadEnStock;
                    Console.WriteLine($"Producto: {producto.Nombre} - Stock: {producto.CantidadEnStock} - Precio: ${producto.PrecioUnitarioDolares} - Total: {producto.PrecioUnitarioDolares * producto.CantidadEnStock}");
                }
                Console.WriteLine($"Total del inventario: {total}");
            }
            Console.WriteLine("\n---------------------------------------------------------------\n");
            Thread.Sleep(12000);
            semaphore.Release();
        }

        static void Tarea4()
        {
            Console.WriteLine("Tarea 4 en fila");
            semaphore.Wait();
            Console.WriteLine("Tarea 4 en ejecucion");
            lock (productos)
            {
                foreach (var producto in productos)
                {
                    producto.PrecioUnitarioDolares *= 1.1m;
                    Console.WriteLine($"Precio de {producto.Nombre} actualizado a ${producto.PrecioUnitarioDolares} (+10% por inflación)");
                }
            }
            Console.WriteLine("\n---------------------------------------------------------------\n");
            Thread.Sleep(8000);
            semaphore.Release();
        }
        internal static void Excecute()
      { 
         Thread task1 = new Thread(Tarea1);    
            task1.Name = "Hilo 1";
         Thread task2 = new Thread(UpdateDolarPrice);
            task2.Name = "Hilo 2";
        Thread task4 = new Thread(Tarea4);
            task4.Name = "Hilo 4";
        Thread task3 = new Thread(Tarea3);
            task3.Name = "Hilo 3";

            task1.Start();
            task2.Start();
            task4.Start();
            task3.Start();

            Console.ReadLine();
        }
   }
}