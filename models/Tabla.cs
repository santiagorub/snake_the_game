namespace snake_the_game.models
{
    class Tabla
    {
        public string [,] Datos {get; set;}
        
        public Tabla(string[,] datos)
        {
            Datos = datos;
        }
    }
}