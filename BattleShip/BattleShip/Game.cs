using System;
using System.Collections.Generic;
using System.Linq;

namespace Battleship
{
    public class Game
    {
        // ПУНКТ 3: Сохранение настроек в поле класса
        private GameSettings settings;
        
        // Требование 1.3: Работа с игроками строго через тип IPlayer
        private IPlayer player1;
        private IPlayer player2;
        private Random random = new Random();

        // Занятие 6 / Требование 1.5: Коллекция Shots для полной истории всех выстрелов
        public List<Shot> Shots { get; } = new List<Shot>();

        // ПУНКТ 3: Принимаем GameSettings в构造тор
        public Game(GameSettings settings)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings));

            // Инициализация игрока-человека
            player1 = new HumanPlayer("Игрок", settings.BoardSize);
            
            // Ручная базовая расстановка флота игрока (в учебных целях)
            player1.MyBoard.AddShip(new HorizontalShip("Эсминец", 2, new Position(0, 0)));
            player1.MyBoard.AddShip(new VerticalShip("Крейсер", 3, new Position(2, 1)));

            // Инициализация компьютера и генерация его доски
            player2 = new ComputerPlayer("Компьютер", settings.BoardSize);
            GenerateOpponentBoard(player2.MyBoard);
        }

        // ПУНКТ 3: Автогенерация кораблей компьютера без пересечений
        private void GenerateOpponentBoard(Board opponentBoard)
        {
            // Создаем столько же кораблей, сколько находится на доске пользователя
            foreach (var userShip in player1.MyBoard.Ships)
            {
                bool shipPlaced = false;
                while (!shipPlaced)
                {
                    // Генерируем случайный корабль через метод расширения Random
                    Ship potentialShip = random.NextShip(settings, "Вражеский " + userShip.Name, userShip.Length);

                    // Проверяем на пересечение со всеми уже выставленными кораблями (Пункт 2 и 3)
                    bool hasIntersection = opponentBoard.Ships.Any(existingShip => potentialShip.IntersectsWith(existingShip));

                    if (!hasIntersection)
                    {
                        opponentBoard.AddShip(potentialShip);
                        shipPlaced = true;
                    }
                }
            }
        }

        public void Play()
        {
            IShooter shooter1 = (IShooter)player1;
            IShooter shooter2 = (IShooter)player2;

            Console.WriteLine("=== МОРСКОЙ БОЙ НАЧАЛСЯ ===");
            
            // Выводим изначальное поле игрока для ознакомления
            Console.WriteLine("\n--- НАЧАЛЬНОЕ РАСПОЛОЖЕНИЕ ВАШИХ КОРАБЛЕЙ ---");
            DrawBoard(player1.MyBoard, showShips: true);

            while (true)
            {
                // Шаг 1: КОРРЕКТНЫЙ ВЫСТРЕЛ ПОЛЬЗОВАТЕЛЯ (с циклом переигрывания)
                bool validUserTurn = false;
                while (!validUserTurn)
                {
                    try
                    {
                        Shot playerShot = shooter1.Shoot(player2.MyBoard);
                        VerifyAndRegisterShot(playerShot);
                        
                        Console.WriteLine($"Результат выстрела: {TranslateResult(playerShot.Result)}");
                        validUserTurn = true; 
                    }
                    // Занятие 6 / Пункт 2: Перехват ошибок для непрерывного процесса игры
                    catch (Exception ex) when (ex is ArgumentException || ex is FormatException || ex is ArgumentOutOfRangeException)
                    {
                        Console.WriteLine($"Ошибка хода: {ex.Message} Попробуйте снова.");
                    }
                }

                // Шаг 2: КОРРЕКТНЫЙ ОТВЕТНЫЙ ВЫСТРЕЛ КОМПЬЮТЕРА (с циклом автопереигрывания при повторах)
                bool validEnemyTurn = false;
                while (!validEnemyTurn)
                {
                    try
                    {
                        Shot computerShot = shooter2.Shoot(player1.MyBoard);
                        VerifyAndRegisterShot(computerShot);
                        
                        Console.WriteLine($"[Компьютер] выстрелил в {computerShot.Position} -> {TranslateResult(computerShot.Result)}");
                        validEnemyTurn = true; 
                    }
                    catch (ArgumentException)
                    {
                        // Бот попал в клетку, куда уже стрелял — повторяем попытку генерации
                        continue;
                    }
                }

                // Шаг 3: ВЫВОД ОБЕИХ ДОСОК ПОСЛЕ СОВЕРШЕНИЯ ОБОИХ ХОДОВ
                Console.WriteLine("\n================ ТЕКУЩЕЕ СОСТОЯНИЕ ПОЛЕЙ ================");
                Console.WriteLine("\n--- ВАША ДОСКА ---");
                DrawBoard(player1.MyBoard, showShips: true);

                Console.WriteLine("\n--- ДОСКА КОМПЬЮТЕРА (Скрытая) ---");
                DrawBoard(player2.MyBoard, showShips: false);
                Console.WriteLine("=========================================================");

                // Шаг 4: ВЫВОД КОЛИЧЕСТВА ПОТОПЛЕННЫХ КОРАБЛЕЙ (LINQ)
                PrintRoundSummary();

                // Шаг 5: ПРОВЕРКА ОКОНЧАНИЯ ИГРЫ (Когда флот одного из игроков полностью уничтожен)
                if (player2.MyBoard.Ships.All(s => s.IsSunk))
                {
                    Console.WriteLine($"\nПОБЕДА! {player1.Name} уничтожил все корабли противника!");
                    break;
                }

                if (player1.MyBoard.Ships.All(s => s.IsSunk))
                {
                    Console.WriteLine($"\nПОРАЖЕНИЕ! {player2.Name} полностью разбил ваш флот!");
                    break;
                }
            }
        }

        // Подсказка к 1.5: Проверяем дубликаты до сохранения в историю и в списки корабля
        private void VerifyAndRegisterShot(Shot newShot)
        {
            // Проверяем, был ли выстрел по этой же таргет-доске в эти же координаты (Пункт 7)
            bool isDuplicate = Shots.Any(s => s.TargetBoard == newShot.TargetBoard && s.Position.Equals(newShot.Position));
            if (isDuplicate)
            {
                throw new ArgumentException("В эту клетку на данной доске уже стреляли.");
            }

            // Добавляем выстрел в общую историю игры
            Shots.Add(newShot);

            // ПУНКТ 5: При попадании добавляем Shot в личную коллекцию корабля
            if (newShot.HitShip != null)
            {
                newShot.HitShip.ShipHits.Add(newShot);
            }
        }

        // ПУНКТ 4: Отрисовка поля через два вложенных цикла for
        private void DrawBoard(Board board, bool showShips)
        {
            // Вывод строки-индикатора номеров колонок
            Console.Write("  ");
            for (int col = 0; col < board.Size; col++) Console.Write(col + " ");
            Console.WriteLine();

            // Внешний цикл — строки (Y), Внутренний — столбцы (X)
            for (int y = 0; y < board.Size; y++)
            {
                Console.Write(y + " "); // Номер строки слева

                for (int x = 0; x < board.Size; x++)
                {
                    Position currentPos = new Position(x, y);

                    // Проверяем, стреляли ли сюда
                    Shot shot = Shots.FirstOrDefault(s => s.TargetBoard == board && s.Position.Equals(currentPos));
                    Ship shipOnCell = board.FindShip(currentPos);

                    if (shot != null)
                    {
                        // Попадание — X, Промах — O
                        Console.Write(shot.IsHit ? "X " : "O ");
                    }
                    else if (shipOnCell != null && showShips)
                    {
                        // Неповрежденная палуба на доске игрока — S
                        Console.Write("S ");
                    }
                    else
                    {
                        // Пустая клетка или скрытый корабль компьютера — точка
                        Console.Write(". ");
                    }
                }
                Console.WriteLine();
            }
        }

        // ПУНКТ 5: Подсчет потопленных кораблей на каждой доске через LINQ
        private void PrintRoundSummary()
        {
            int playerSunk = player1.MyBoard.Ships.Count(s => s.IsSunk);
            int enemySunk = player2.MyBoard.Ships.Count(s => s.IsSunk);

            Console.WriteLine($"\n=== КОЛИЧЕСТВО ПОТОПЛЕННЫХ КОРАБЛЕЙ ===");
            Console.WriteLine($"У Вас на доске уничтожено: {playerSunk} из {player1.MyBoard.Ships.Count}");
            Console.WriteLine($"У Компьютера уничтожено: {enemySunk} из {player2.MyBoard.Ships.Count}");
            Console.WriteLine("========================================");
        }

        private string TranslateResult(ShootResult result) => result switch
        {
            ShootResult.Miss => "Промах!",
            ShootResult.Hit => "Попадание (Корабль ранен)!",
            ShootResult.Sunk => "Потоплен! Корабль полностью уничтожен!",
            _ => "Неизвестный исход"
        };
    }
}
