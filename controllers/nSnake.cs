using System;
using System.Threading;
using snake_the_game.models;

namespace snake_the_game.controllers
{
    class nSnake
    {
        public void IniciarJuego()
        {
            Console.CursorVisible = false;
            Snake snake = new Snake();

            while(true)
            {
                if(Console.KeyAvailable) //lee el teclado sin que el juego se frene
                {
                    ConsoleKey tecla = Console.ReadKey(true).Key;
                    switch(tecla)
                    {
                        case ConsoleKey.W:
                            if(snake.Direccion != "down") snake.Direccion = "up"; break;
                        case ConsoleKey.S:
                            if(snake.Direccion != "up") snake.Direccion = "down"; break;
                        case ConsoleKey.A:
                            if(snake.Direccion != "right") snake.Direccion = "left"; break;
                        case ConsoleKey.D:
                            if(snake.Direccion != "left") snake.Direccion = "right"; break;
                    }
                }

                snake.Mover();
                Dibujar(snake);
                Thread.Sleep(150); //velocidad del juego
            }
        }

        private void Dibujar(Snake snake)
        {
            Console.Clear();

            foreach(var parte in snake.Cuerpo)
            {
                Console.SetCursorPosition(parte.x, parte.y);
                Console.Write("O");
            }
        }
    }
}