using Spectre.Console;
using System;

namespace TablaDeAmortizacion
{
    // Definición de la clase
    public class solicitudDatos
    {
        public decimal montoPrestamo { get; set; }
        public decimal interesAnual { get; set; }
        public int plazoMeses { get; set; }

        // Método para mostrar información
        public void titulo()
        {
            Console.WriteLine("MI TABLA DE AMORTIZACIÓN DE DEUDAS");
            Console.WriteLine();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Crea el objeto de solicitudDatos
            var datos = new solicitudDatos();

            // Usa el método titulo
            datos.titulo();

            // Solicitar datos al usuario
            // El parse sirve para convertir un string (texto) a un tipo de dato numérico
            Console.Write("Ingrese el monto del prestamo: ");
            datos.montoPrestamo = decimal.Parse(Console.ReadLine());

            Console.Write("Ingrese la tasa de interes anual (%): ");
            datos.interesAnual = decimal.Parse(Console.ReadLine());

            Console.Write("Ingrese el plazo del prestamo (en meses): ");
            datos.plazoMeses = int.Parse(Console.ReadLine());

            Console.WriteLine();

            // Llamar al método para calcular amortización
            CalcularAmortizacion(datos);
        }

        // Método para calcular la amortización
        //Lo que esta entre parentesis sirve para recibir toda la información en usn solo paquete
        static void CalcularAmortizacion(solicitudDatos datos)
        {
            // Calcula tasa de interés mensual
            decimal interesMensual = (datos.interesAnual / 100) / 12;

            // Calcular la cuota fija mensual 
            double unoMasInteresMensual = 1 + (double)interesMensual;
            double potencia = Math.Pow(unoMasInteresMensual, datos.plazoMeses);

            decimal cuotaFija = datos.montoPrestamo * (interesMensual * (decimal)potencia) / ((decimal)potencia - 1);
            cuotaFija = Math.Round(cuotaFija, 2);

            // Llamar al método para generar la tabla
            GenerarTabla(datos, interesMensual, cuotaFija);
        }

        // Método para generar la tabla
        static void GenerarTabla(solicitudDatos datos, decimal interesMensual, decimal cuotaFija)
        {
            // Crear la tabla de amortización
            var table = new Table();

            // Para definir y agregar columnas
            table.AddColumn("No. de cuota");
            table.AddColumn("Pago de cuota");
            table.AddColumn("Interés a pagar");
            table.AddColumn("Abono a capital");
            table.AddColumn("Saldo pendiente");

            // Variables para el cálculo 
            decimal saldoPendiente = datos.montoPrestamo;
            decimal totalIntereses = 0;

            // Bucle for para calcular cada período
            for (int i = 1; i <= datos.plazoMeses; i++)
            {
                decimal interesPeriodo = Math.Round(saldoPendiente * interesMensual, 2);
                decimal abonoCapital = Math.Round(cuotaFija - interesPeriodo, 2);
                decimal cuotaActual = cuotaFija;

                // if, else para que se ajuste el último período y el saldo sea 0
                if (i == datos.plazoMeses && saldoPendiente != 0)
                {
                    abonoCapital += saldoPendiente;
                    cuotaActual = interesPeriodo + abonoCapital;
                    saldoPendiente = 0;
                }
                else
                {
      
                    // Else sirve para actualizar el saldo pendiente restando el abono a capital en cada mes que no es el último.
                    saldoPendiente = Math.Round(saldoPendiente - abonoCapital, 2);
                }

                totalIntereses = totalIntereses + interesPeriodo;

                // Agregar fila a la tabla
                /* N2 significa numero con 2 plasas decimales */

                table.AddRow(
                    i.ToString(),
                    $"{cuotaActual:N2}",
                    $"{interesPeriodo:N2}",
                    $"{abonoCapital:N2}",
                    $"{saldoPendiente:N2}"
                );
            }

            // Llamar al método para mostrar resultados
            MostrarResultados(table, datos.montoPrestamo, totalIntereses);
        }

        // Método para mostrar resultados
        static void MostrarResultados(Table table, decimal montoPrestamo, decimal totalIntereses)
        {
            // Mostrar la tabla
            AnsiConsole.Write(table);

            // Mostrar totales finales
            Console.WriteLine();
            Console.WriteLine($"Total a pagar: {(montoPrestamo + totalIntereses):N2}");
            Console.WriteLine($"Total intereses: {totalIntereses:N2}");
        }
    }
}