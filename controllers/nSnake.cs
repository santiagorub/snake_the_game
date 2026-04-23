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
        private int ancho = 40;
        private int alto = 25; //min 12 porque sino empieza a fallar ya que la consola de windows tiene un minimo permitido
        Snake snake = new Snake();
        nComida controladorComida = new nComida();
        List<Comida> comida = new List<Comida>();
        nNivel controladorNivel = new nNivel();

        public ResultadoJuego IniciarJuego()
        {
            Console.Clear();
            Console.CursorVisible = false;

            //Console.SetBufferSize(ancho + 2, alto + 2); //fija el tamaño del buffer para evitar scroll
            //Console.SetWindowSize(ancho + 2, alto + 2); //ajusta el tamaño de la ventana para que se ajuste al area de juego
            
            bool juegoFunca = true; //es la variable para ver si el juego sigue en marcha o se acaba

            DibujarBordes();

            for (int i = 0; i < controladorNivel.nivel.Numero; i++)
            {
                var nueva = controladorComida.GenerarComida(snake, ancho, alto);
                comida.Add(nueva);
                controladorComida.DibujarComida(nueva);
            }

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
                for (int i = 0; i < comida.Count; i++)
                {
                    if (cabeza.x == comida[i].x && cabeza.y == comida[i].y)
                    {
                        snake.Crecer();

                        //registra la comida para los niveles
                        bool subioNivel = controladorNivel.RegistrarComida();
                        //elimina la comida que ya se comio
                        comida.RemoveAt(i);
                        
                        //genera nueva comida manteniendo la cantidad
                        var nueva = controladorComida.GenerarComida(snake, ancho, alto);
                        comida.Add(nueva);
                        controladorComida.DibujarComida(nueva);

                        //si subio el nivel se agrega una comida extra
                        if (subioNivel)
                        {
                            var extra = controladorComida.GenerarComida(snake, ancho, alto);
                            comida.Add(extra);
                            controladorComida.DibujarComida(extra);
                        }

                        break;
                    }
                }

                Thread.Sleep(150); //velocidad del juego

                //hud de comida y nivel
                string texto = $"Nivel: {controladorNivel.nivel.Numero}  Comidas: {controladorNivel.nivel.ComidaConsumida}";
                int x = (ancho / 2) - (texto.Length / 2);

                Console.SetCursorPosition(x, alto);
                Console.Write(texto + "   ");

            }
            GameOver();
            Console.ReadKey(true);

            return new ResultadoJuego
            {
                Puntaje = controladorNivel.nivel.ComidaConsumida,
                Nivel = controladorNivel.nivel.Numero
            };
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
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("O");
            Console.ResetColor();
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
            Console.WriteLine("\nPresione cualquier tecla para regresar al menú...");
            //Console.ReadKey(true); //si pongo false la letra aparece en pantalla y no quiero eso.
        }
    }
}
