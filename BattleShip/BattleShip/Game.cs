using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship
{
    public class Game
    {
        private Board playerBoard;
        private Board enemyBoard;
        private Random random = new Random();

        // ПУНКТ 6: Коллекция Shots, где хранится полная история всех выстрелов игры
        public List<Shot> Shots { get; }

        public Game()
        {
            Shots = new List<Shot>();
            
            // Инициализируем доски размером, например, 10х10
            playerBoard = new Board("Игрок", 10);
            enemyBoard = new Board("Компьютер", 10);

            // Наполняем тестовыми кораблями (в реальном проекте тут будет автогенерация)
            playerBoard.AddShip(new Ship("Эсминец", new List<Position> { new Position(0, 0), new Position(0, 1) }));
            enemyBoard.AddShip(new Ship("Линкор", new List<Position> { new Position(2, 2), new Position(2, 3), new Position(2, 4) }));
        }

        public void Play()
        {
            Console.WriteLine("=== МОРСКОЙ БОЙ НАЧАЛСЯ ===");
            bool isGameActive = true;

            while (isGameActive)
            {
                // --- ХОД ИГРОКА ---
                bool isUserTurnValid = false;
                while (!isUserTurnValid)
                {
                    try
                    {
                        Console.Write("\nВаш ход. Введите X и Y через пробел (или 'exit' для выхода): ");
                        string input = Console.ReadLine();
                        
                        if (input?.Trim().ToLower() == "exit")
                        {
                            isGameActive = false;
                            break;
                        }

                        string[] coordinates = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                        if (coordinates.Length != 2) 
                            throw new FormatException("Неверный формат! Нужно ввести ровно два числа через пробел.");

                        int x = int.Parse(coordinates[0]);
                        int y = int.Parse(coordinates[1]);

                        Position userTarget = new Position(x, y);

                        // ПУНКТ 5: Вынос логики выстрела по доске компьютера в отдельный метод
                        ExecuteShot(enemyBoard, userTarget);
                        isUserTurnValid = true; // Выстрел успешен, выходим из цикла ожидания ввода
                    }
                    // ПУНКТ 2: Обрабатываем исключения так, чтобы игра не завершалась аварийно
                    catch (FormatException ex)
                    {
                        Console.WriteLine($"Ошибка ввода: {ex.Message}");
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine($"Ошибка координат: {ex.Message}");
                    }
                    catch (ArgumentException ex)
                    {
                        // Сюда попадет и попытка выстрелить повторно (Пункт 7)
                        Console.WriteLine($"Невозможно сделать ход: {ex.Message}");
                    }
                }

                if (!isGameActive) break;

                // --- ХОД КОМПЬЮТЕРА ---
                bool isEnemyTurnValid = false;
                while (!isEnemyTurnValid)
                {
                    try
                    {
                        // Генерируем случайную клетку на поле игрока
                        int compX = random.Next(0, playerBoard.Size);
                        int compY = random.Next(0, playerBoard.Size);
                        Position enemyTarget = new Position(compX, compY);

                        ExecuteShot(playerBoard, enemyTarget);
                        isEnemyTurnValid = true;
                    }
                    catch (ArgumentException)
                    {
                        // Если компьютер кинул исключение "уже стреляли", он просто идет на новую итерацию генерации random
                        continue;
                    }
                }

                // ПУНКТ 8: Вывод LINQ статистики в конце каждого раунда
                PrintRoundStatistics();

                // Проверка на окончание игры (если у кого-то потоплены все корабли)
                if (IsFleetDestroyed(enemyBoard))
                {
                    Console.WriteLine("\nПоздравляем! Вы уничтожили весь флот противника!");
                    break;
                }
                if (IsFleetDestroyed(playerBoard))
                {
                    Console.WriteLine("\nУвы! Компьютер разбил ваши корабли!");
                    break;
                }
            }
        }

        // ПУНКТ 5: Вынесенный изолированный метод для обработки выстрела
        private void ExecuteShot(Board targetBoard, Position position)
        {
            // Проверка границ доски перед выстрелом
            if (position.X >= targetBoard.Size || position.Y >= targetBoard.Size)
            {
                throw new ArgumentOutOfRangeException(nameof(position), $"Выстрел {position} выходит за пределы поля {targetBoard.Size}x{targetBoard.Size}.");
            }

            // ПУНКТ 7: Если игрок пытается выстрелить в клетку, куда уже стреляли, бросаем исключение
            bool isAlreadyTargeted = Shots.Any(s => s.TargetBoard == targetBoard && s.Position.Equals(position));
            if (isAlreadyTargeted)
            {
                throw new ArgumentException($"В ячейку {position} на доске '{targetBoard.OwnerName}' уже производился выстрел.");
            }

            // Находим корабль (Пункт 4)
            Ship targetShip = targetBoard.FindShip(position);

            // ПУНКТ 5 и 6: Создаем объект Shot и добавляем в общую историю
            Shot shotResult = new Shot(targetBoard, position, targetShip);
            Shots.Add(shotResult);

            // Информируем о результате
            if (shotResult.IsHit)
            {
                Console.WriteLine($"[{targetBoard.OwnerName}] ПОПАДАНИЕ в {position}! Задет корабль: {targetShip.Name}");
            }
            else
            {
                Console.WriteLine($"[{targetBoard.OwnerName}] ПРОМАХ в {position}.");
            }
        }

        // ПУНКТ 8: Метод вывода статистики с использованием LINQ
        private void PrintRoundStatistics()
        {
            Console.WriteLine("\n--- СТАТИСТИКА ТЕКУЩЕГО РАУНДА ---");

            // Выделяем доски, которые участвуют в игре
            Board[] boardsInGame = { playerBoard, enemyBoard };

            foreach (var board in boardsInGame)
            {
                // Выбираем из истории все выстрелы по конкретно этой доске
                var boardShots = Shots.Where(s => s.TargetBoard == board).ToList();

                Console.WriteLine($"\nДоска: {board.OwnerName}");
                
                // 8.1. Общее количество выстрелов
                Console.WriteLine($"  8.1. Общее кол-во выстрелов: {boardShots.Count}");

                // 8.2. Количество попаданий
                Console.WriteLine($"  8.2. Кол-во попаданий: {boardShots.Count(s => s.IsHit)}");

                // 8.3. Количество промахов
                Console.WriteLine($"  8.3. Кол-во промахов: {boardShots.Count(s => !s.IsHit)}");

                // 8.4. Был ли хотя бы один промах
                Console.WriteLine($"  8.4. Был ли хоть один промах: {(boardShots.Any(s => !s.IsHit) ? "Да" : "Нет")}");

                // 8.5. Первый успешный выстрел
                var firstSuccessfulShot = boardShots.FirstOrDefault(s => s.IsHit);
                Console.WriteLine($"  8.5. Первый успешный выстрел: {(firstSuccessfulShot != null ? firstSuccessfulShot.Position.ToString() : "Отсутствует")}");

                // 8.6. Список координат всех попаданий
                var hitCoords = boardShots.Where(s => s.IsHit).Select(s => s.Position);
                Console.WriteLine($"  8.6. Координаты попаданий: {(hitCoords.Any() ? string.Join(", ", hitCoords) : "Нет")}");

                // 8.7.* Статистика для каждого корабля на доске
                Console.WriteLine("  8.7.* Состояние флота:");
                foreach (var ship in board.Ships)
                {
                    // Считаем через LINQ, сколько выстрелов из истории попало именно в этот корабль
                    int hitsOnShip = boardShots.Count(s => s.HitShip == ship);
                    bool isSunk = hitsOnShip >= ship.Cells.Count;

                    Console.WriteLine($"    - Корабль '{ship.Name}' | Попаданий: {hitsOnShip}/{ship.Cells.Count} | Статус: {(isSunk ? "ПОТОПЛЕН" : "На плаву")}");
                }
            }
            Console.WriteLine("----------------------------------");
        }

        private bool IsFleetDestroyed(Board board)
        {
            // Считаем сумму палуб всех кораблей на доске
            int totalShipCells = board.Ships.Sum(s => s.Cells.Count);
            // Считаем сколько раз по этой доске успешно попали
            int totalHits = Shots.Count(s => s.TargetBoard == board && s.IsHit);

            return totalHits >= totalShipCells;
        }
    }
}
