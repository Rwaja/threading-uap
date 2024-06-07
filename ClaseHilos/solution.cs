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
        static Barrier barrera = new Barrier(2, (b) =>
        {
            Console.WriteLine($"Post-Phase action: {b.CurrentPhaseNumber}");
        });
        public static Barrier barrera_
        {
            get { return barrera; }
        }
        static int precio_dolar = 500;

      static void Tarea1()
      {
            lock (productos)
            {
                foreach (var producto in productos)
                {
                    producto.CantidadEnStock += 10;
                    Console.WriteLine($"Stock de {producto.Nombre} actualizado a {producto.CantidadEnStock}");
                }
            }
            Console.WriteLine("\n---------------------------------------------------------------\n");
            Thread.Sleep(3000);
            barrera.SignalAndWait();
        }
      static void UpdateDolarPrice()
        {
            foreach (var producto in productos)
            {
                Console.WriteLine($"{productos.IndexOf(producto) + 1}. {producto.Nombre} - {producto.PrecioUnitarioDolares} - {producto.CantidadEnStock} \n");
            }
            Console.WriteLine("OPCIONES \n");
            Console.WriteLine("1. Aumentar precio de todos los productos \n");
            Console.WriteLine("2. Actualizar precio de un producto \n");
            Console.WriteLine("3. Salir \n");

            int op = Convert.ToInt32(Console.ReadLine());
            switch (op)
            {
                case 1:
                    Console.WriteLine("Ingrese el nuevo precio en dolares: ");
                    decimal precio = Convert.ToDecimal(Console.ReadLine());
                    UpdateAllPrices();
                    break;
                case 2:
                    try
                    {
                    Console.WriteLine("Ingrese el ID del producto: ");
                        int id = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Ingrese el nuevo precio en dolares: ");
                        decimal precio2 = Convert.ToDecimal(Console.ReadLine());
                        UpdateDolarPriceById(id, precio2);
                        }
                    catch (Exception)
                    {
                        Console.WriteLine("Error: Ingrese un valor valido");
                    }
                    break;
            }
            static void UpdateAllPrices(decimal precio)
            {
                lock (productos)
                {
                    foreach (var producto in productos)
                    {
                        producto.PrecioUnitarioDolares += precio;
                        Console.WriteLine($"Precio de {producto.Nombre}-{producto.PrecioUnitarioDolares-=precio} actualizado a {producto.PrecioUnitarioDolares} /// Diferencia neta: {precio}");
                    }
                }
            }
            static void UpdateDolarPriceById(int id, decimal precio)
            {
                lock (productos)
                {
                    productos[id - 1].PrecioUnitarioDolares = precio;
                    Console.WriteLine($"Precio de {productos[id - 1].Nombre} actualizado a {productos[id - 1].PrecioUnitarioDolares} \n");
                }
            }

        }
        static void Tarea3()
      {
         throw new NotImplementedException();
      }

      internal static void Excecute()
      { 
         Thread task1 = new Thread(Tarea1);    
            task1.Name = "Hilo 1";
         Thread task2 = new Thread(UpdateDolarPrice);
            task2.Name = "Hilo 2";


            task1.Start();
            task2.Start();

            Console.ReadLine();
        }
   }
}