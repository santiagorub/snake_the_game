using System;

namespace snake_the_game;

public class Menu
{
    public static void Show()
    {
        string[] opciones = { "Jugar", "Ver Tabla", "Salir" };

        while (true)
        {
            int opcion = MenuSeleccionar(opciones, "SNAKE THE GAME");

            switch (opcion)
            {
                case 0:
                    var juego = new snake_the_game.controllers.nSnake();
                    juego.IniciarJuego();
                    break;

                case 1:
                    Console.WriteLine("Ver Tabla");
                    Console.ReadKey();
                    break;

                case 2:
                    return;
            }
        }
    }

    public static int MenuSeleccionar(string[] opciones, string titulo)
    {
        int seleccion = 0;
        ConsoleKey tecla;

        do
        {
            Console.Clear();
            Console.WriteLine("==== " + titulo + " ====\n");

            for (int i = 0; i < opciones.Length; i++)
            {
                if (i == seleccion)
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.WriteLine("> " + opciones[i]);
                }
                else
                {
                    Console.WriteLine("  " + opciones[i]);
                }

                Console.ResetColor();
            }

            tecla = Console.ReadKey(true).Key;

            if (tecla == ConsoleKey.UpArrow)
                seleccion = (seleccion == 0) ? opciones.Length - 1 : seleccion - 1;

            if (tecla == ConsoleKey.DownArrow)
                seleccion = (seleccion == opciones.Length - 1) ? 0 : seleccion + 1;

        } while (tecla != ConsoleKey.Enter);

        return seleccion;
    }
}