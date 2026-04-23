using System;
using System.Collections.Generic;
using System.Data;
using snake_the_game.controllers;
using snake_the_game.models;

namespace snake_the_game.controllers
{
    class nTabla
    {
        public Tabla ImprimirPuntajes(int puntajeFinal, int nivelAlcanzado)
        {
            //Console.Clear();
            //Console.ResetColor();

            string[,] datos = new string[2, 2];

            //int puntajeFinal = controladorNivel.nivel.ComidaConsumida;
            //int nivelAlcanzado = controladorNivel.nivel.Numero; //esta variable la agregue para que tambien se añada a la tabla

            //titulos
            datos[0, 0] = "Nivel alcanzado";
            datos[0, 1] = "Puntaje Total";

            //puntaje y nivel
            datos[1, 0] = nivelAlcanzado.ToString();
            datos[1, 1] = puntajeFinal.ToString() + " pts";

            return new Tabla(datos);
        }
        public void DibujarTabla(Tabla tabla)
        {
            string[,] matriz = tabla.Datos;

            int filas = matriz.GetLength(0);
            int columnas = matriz.GetLength(1);

            int[] anchos = new int[columnas];

            for (int c = 0; c < columnas; c++)
            {
                for (int f = 0; f < filas; f++)
                {
                    if (matriz[f, c].Length > anchos[c])
                        anchos[c] = matriz[f, c].Length;
                }
                anchos[c] += 2;
            }

            //linea superior de la fila
            ImprimirLinea(anchos, "top");

            //dibujo de la tabla
            for (int f = 0; f < filas; f++)
            {    
                Console.Write("║"); //borde vertical

                for (int c = 0; c < columnas; c++)
                {
                    if (f == 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write($" {matriz[f, c].PadRight(anchos[c] - 1)}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write($" {matriz[f, c].PadRight(anchos[c] - 1)}");
                    }
                    Console.Write("║");
                }
                Console.WriteLine();

                //separa despues de los titulos
                if (f == 0)
                {
                    ImprimirLinea(anchos, "mid");
                }
            }

            //linea de cierre final
            ImprimirLinea(anchos, "bottom");
        }
    
        private void ImprimirLinea(int[] anchos, string tipo)
        {
            char izq;
            char medio;
            char der;

            switch (tipo)
            {
                case "top": 
                    izq = '╔'; medio = '╦'; der = '╗'; 
                    break;
                case "mid":
                    izq = '╠'; medio = '╬'; der = '╣';
                    break;
                case "bottom":
                    izq = '╚'; medio = '╩'; der = '╝';
                    break;
                default:
                    izq = medio = der = ' ';
                    break;
            }

            Console.Write(izq);

            for (int i = 0; i < anchos.Length; i++)
            {
                Console.Write(new string('═', anchos[i]));

                if (i < anchos.Length - 1) {
                    Console.Write(medio);
                }
            }
            Console.WriteLine(der);
        }
    }
}