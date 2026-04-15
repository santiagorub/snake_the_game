using System;
using System.Collections.Generic;
using System.Data;

namespace snake_the_game.models
{
    class Snake
    {
        public List<(int x, int y)> Cuerpo {get; set;}
        public string Direccion {get; set;} //direccion actual de la serpiente

        public Snake()
        {
            Cuerpo = new List<(int, int)>();

            //posición inicial de la serpiente
            Cuerpo.Add((10, 10));
            Cuerpo.Add((9, 10));
            Cuerpo.Add((8, 10));

            Direccion = "right";
        }

        //devuelve la posicion d la cabeza
        public(int x, int y) ObtenerCabeza()
        {
            return Cuerpo[0];
        }

        //la serpiente se mueve en la direccion actual
        public void Mover()
        {
            var cabeza = ObtenerCabeza();
            int x = cabeza.x;
            int y = cabeza.y;

            switch (Direccion)
            {
                case "up": y--; break;
                case "down": y++; break;
                case "left": x--; break;
                case "right": x++; break;
            }

            Cuerpo.Insert(0, (x, y)); //agrega nueva cabeza
            Cuerpo.RemoveAt(Cuerpo.Count - 1); //elimina la ultima parte de la cola
        }

        public void Crecer() //la serpiente crece cuando come
        {
            var cabeza = ObtenerCabeza();
            Cuerpo.Insert(0, cabeza); //duplica la cabeza y la cola no se elimina en el proximo movimiento
        }
    }
}