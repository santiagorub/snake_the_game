using System;
using System.Reflection.Metadata;
using System.Runtime.InteropServices.Marshalling;
using System.Threading;
using snake_the_game.models;

namespace snake_the_game.controllers
{
    class nSnake
    {
        //medidas de donde se mueve la serpiente
        private int ancho = 50;
        private int alto = 15; //min 12 porque sino empieza a fallar ya que la consola de windows tiene un minimo permitido

        nComida controladorComida = new nComida();
        Comida ?comida;

        public void IniciarJuego()
        {
            Console.Clear();
            Console.CursorVisible = false;

            //Console.SetBufferSize(ancho + 2, alto + 2); //fija el tamaño del buffer para evitar scroll
            //Console.SetWindowSize(ancho + 2, alto + 2); //ajusta el tamaño de la ventana para que se ajuste al area de juego

            Snake snake = new Snake();
            bool juegoFunca = true; //es la variable para ver si el juego sigue en marcha o se acaba

            DibujarBordes();

            //genera la comida del inicio
            comida = controladorComida.GenerarComida(snake, ancho, alto);
            controladorComida.DibujarComida(comida);

            while(juegoFunca)
            {
                if(Console.KeyAvailable) //lee el teclado sin que el juego se frene
                {
                    ConsoleKey tecla = Console.ReadKey(true).Key;
                    switch(tecla)
                    {
                        case ConsoleKey.W: case ConsoleKey.UpArrow://funciona con las teclas wasd y las flechitas
                            if(snake.Direccion != "down") snake.Direccion = "up"; break;
                        case ConsoleKey.S: case ConsoleKey.DownArrow:  
                            if(snake.Direccion != "up") snake.Direccion = "down"; break;
                        case ConsoleKey.A: case ConsoleKey.LeftArrow:
                            if(snake.Direccion != "right") snake.Direccion = "left"; break;
                        case ConsoleKey.D: case ConsoleKey.RightArrow:
                            if(snake.Direccion != "left") snake.Direccion = "right"; break;
                    }
                }

                snake.Mover();

                 //colision con el cuerpo
                if (snake.ChocoConSiMisma())
                {
                    juegoFunca = false; //el juego termina
                    break;
                }


                //para comprobar cuando la cabeza choca con los bordes
                var cabeza = snake.ObtenerCabeza();

                if ( cabeza.x < 1 || cabeza.x >= ancho-1 || cabeza.y < 1 || cabeza.y >= alto-1)
                {
                    juegoFunca = false; //el juego termina
                    break;
                }

                DibujarSerpiente(snake);

                //cuando la serpiente come genera una nueva comida
                if (cabeza.x == comida.x && cabeza.y == comida.y)
                {
                    snake.Crecer();
                    comida = controladorComida.GenerarComida(snake, ancho, alto);
                    controladorComida.DibujarComida(comida);
                }

                Thread.Sleep(150); //velocidad del juego
            }
            GameOver();
            ImprimirPuntajes(snake);
    
            Console.WriteLine("\nPresiona cualquier tecla para cerrar...");
            Console.ReadKey(true);
        }

        private void DibujarSerpiente(Snake snake)
        {
            //borrar la cola
            var cola = snake.Cuerpo[snake.Cuerpo.Count - 1];
            Console.SetCursorPosition(cola.x, cola.y);
            Console.Write(" ");

            //dibujar una nueva cabeza
            var cabeza = snake.ObtenerCabeza();
            Console.SetCursorPosition(cabeza.x, cabeza.y);
            Console.Write("O");
        }

        private void DibujarBordes()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            //esquinas
            Console.SetCursorPosition(0, 0); Console.Write("╔");
            Console.SetCursorPosition(ancho - 1, 0); Console.Write("╗");
            Console.SetCursorPosition(0, alto - 1); Console.Write("╚");
            Console.SetCursorPosition(ancho - 1, alto - 1); Console.Write("╝");

            //dibujo de bordes de arriba y abajo
            for (int i = 1; i < ancho-1; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("═");
                Console.SetCursorPosition(i, alto - 1);
                Console.Write("═");
            }
            //dibuja bordes de izquierda y derecha
            for (int i = 1; i < alto-1; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("║");
                Console.SetCursorPosition(ancho - 1, i);
                Console.Write("║");
            }
        }

        private void GameOver()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(ancho / 2 - 5, alto / 2);
            Console.Write("Game Over!!!");
            Console.SetCursorPosition(ancho - 5, alto);
            Console.ResetColor();
            Console.WriteLine("\nToca cualquier tecla para ir a la tabla...");
            Console.ReadKey(true); //si pongo false la letra aparece en pantalla y no quiero eso.
        }
        
        public void ImprimirPuntajes(Snake snake)
        {
            Console.Clear();
            Console.ResetColor();

            int puntajeFinal = snake.Cuerpo.Count - 1; 

            string[,] tablaResultados = new string[2, 2];

            // Fila 0: Títulos
            tablaResultados[0, 0] = "Concepto";
            tablaResultados[0, 1] = "Detalle";

            // Fila 1: Datos de la partida
            tablaResultados[1, 0] = "Puntaje Total";
            tablaResultados[1, 1] = puntajeFinal.ToString() + " pts";

            DibujaTabla(tablaResultados);
        }
        private void DibujaTabla(string[,] matriz)
        {
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

            // Dibujo de la tabla
            for (int f = 0; f < filas; f++)
            {    
                // Línea superior de la fila
                ImprimirLineaBorde(anchos);

                for (int c = 0; c < columnas; c++)
                {
                    Console.Write("║"); // Borde vertical

                    if (f == 0) // Si es la fila de títulos
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow; // Color de títulos
                        Console.Write($" {matriz[f, c].PadRight(anchos[c] - 1)}");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write($" {matriz[f, c].PadRight(anchos[c] - 1)}");
                    }
                }
                Console.WriteLine("║");
            }
            // Línea de cierre final
            ImprimirLineaBorde(anchos);
        }
    
        private void ImprimirLineaBorde(int[] anchos)
        {
            foreach (int ancho in anchos)
            {
                Console.Write("╬" + new string('═', ancho));
            }
            Console.WriteLine("╬");
        }
    }
}
