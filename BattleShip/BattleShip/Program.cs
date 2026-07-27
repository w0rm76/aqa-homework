using System;

namespace Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Game game = new Game();
                game.Play();
            }
            catch (Exception ex)
            {
                // Вывод пользователю критических сообщений сбоя через Exception.Message
                Console.WriteLine($"Критическая ошибка инициализации игры: {ex.Message}");
            }
        }
    }
}