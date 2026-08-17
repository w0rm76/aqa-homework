using System;

namespace Battleship
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Задаем настройки (например, классическое поле 6х6 клеток)
                GameSettings settings = new GameSettings(6);

                Game game = new Game(settings);
                game.Play();
            }
            catch (Exception ex)
            {
                // Вывод пользователю сообщения критического сбоя
                Console.WriteLine($"Критический сбой инициализации игры: {ex.Message}");
            }
        }
    }
}