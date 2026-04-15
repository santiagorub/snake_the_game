using snake_the_game.controllers;

namespace snake_the_game;
class Program
{
    static void Main(string[] args)
    {
        nSnake juego = new nSnake();
        juego.IniciarJuego();
    }
}
