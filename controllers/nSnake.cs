using System;
using System.Reflection.Metadata;
using System.Threading;
using snake_the_game.models;

namespace snake_the_game.controllers
{
    class nSnake
    {
        //medidas de donde se mueve la serpiente
        private int ancho = 70;
        private int alto = 15; //min 12 porque sino empieza a fallar ya que la consola de windows tiene un minimo permitido

        public void IniciarJuego()
        {
            Console.CursorVisible = false;

            Console.SetWindowSize(ancho + 2, alto + 2); //ajuso el tama;o de la ventana para que se ajuste al area de juego
            Console.SetBufferSize(ancho + 2, alto + 2);// fija el tama;o del buffer para evitar scroll

            Snake snake = new Snake();
            bool juegoFunca = true; //es la variable para ver si el juego sigue en marcha o se acaba

            while(juegoFunca)
            {
                if(Console.KeyAvailable) //lee el teclado sin que el juego se frene
                {
                    ConsoleKey tecla = Console.ReadKey(true).Key;
                    switch(tecla)
                    {
                        case ConsoleKey.W:
                        case ConsoleKey.UpArrow://funciona tambien con las flechitas
                            if(snake.Direccion != "down") snake.Direccion = "up"; break;
                        case ConsoleKey.S:
                        case ConsoleKey.DownArrow:  
                            if(snake.Direccion != "up") snake.Direccion = "down"; break;
                        case ConsoleKey.A:
                        case ConsoleKey.LeftArrow:
                            if(snake.Direccion != "right") snake.Direccion = "left"; break;
                        case ConsoleKey.D:
                        case ConsoleKey.RightArrow:
                            if(snake.Direccion != "left") snake.Direccion = "right"; break;
                    }
                }

                snake.Mover();

                //para comprobar cuando la cabeza choca con los bordes
                var cabeza = snake.ObtenerCabeza();
                if ( cabeza.x < 0 || cabeza.x >= ancho || cabeza.y < 0 || cabeza.y >= alto)
                {
                    juegoFunca = false; //el juego termina
                    break;
                }
                Dibujar(snake);
                Thread.Sleep(150); //velocidad del juego
            }
            Console.Clear();
            Console.SetCursorPosition(ancho / 2 - 5, alto / 2);
            Console.Write("Game Over");
            Console.WriteLine("\nToca cuaqueri letra para salir...");
            Console.ReadKey(true); // si pongo false la letra aparece en pantalla y no quiero eso.
        }

        private void Dibujar(Snake snake)
        {
            Console.Clear();

            //dibujo de bordes de arriba y deabajo
            for (int i = 0; i < ancho; i++)
            {
                Console.SetCursorPosition(i, 0);
                Console.Write("#");
                Console.SetCursorPosition(i, alto - 1);
                Console.Write("#");
            }
            //dibuja bordes de izquierda y derecha
            for (int i = 0; i < alto; i++)
            {
                Console.SetCursorPosition(0, i);
                Console.Write("#");
                Console.SetCursorPosition(ancho - 1, i);
                Console.Write("#");
            }

            foreach(var parte in snake.Cuerpo)
            {
                Console.SetCursorPosition(parte.x, parte.y);
                Console.Write("O");
            }
        }

        //arreglar el el hecho de que se mueve todo
    }
}