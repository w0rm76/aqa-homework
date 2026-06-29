string userInput;

// greeting and asking how many rounds
Console.WriteLine("Hello this is Rock Paper Scissors...and Well");
Console.WriteLine("How many steps do you want to play? ( 1 to 10 )");
userInput = Console.ReadLine();
// parsing the answer
if (!int.TryParse(userInput, out int rounds) || !(rounds > 0 && rounds <= 10) ) 
{
    Console.WriteLine("Exit the game");
    return;
}
Console.WriteLine($"You chose {rounds} round(s)");

int roundsCounter = 0;
int userWon = 0;
int pcWon = 0;

Console.Clear();

while (roundsCounter < rounds)
{
    Console.WriteLine("Please, choose your next step:");
    Console.WriteLine();
    Console.WriteLine("1 - Rock");
    Console.WriteLine("2 - Paper");
    Console.WriteLine("3 - Scissors");
    Console.WriteLine("4 - Well");
    Console.WriteLine();
    Console.WriteLine("0 - Exit");

    userInput = Console.ReadLine();
    
    Console.Clear();
    
    int userChoice;
    
    if (!int.TryParse(userInput, out userChoice) || !(userChoice >= 0 && userChoice <= 4))
    {
        Console.WriteLine("Invalid input");
        continue;
    }
    if (userChoice == 0)
    {
        return;
    }
    string userChoiceString;
    switch (userChoice)
    {
        case 1:
            userChoiceString = "Rock";
            break;
        case 2:
            userChoiceString = "Paper";
            break;
        case 3:
            userChoiceString = "Scissors";
            break;
        default:
            userChoiceString = "Well";
            break;
    }
    
    Console.WriteLine($"You chose {userChoiceString}");
    
    var random = new Random();
    var computerChoice = random.Next(1, 5); // generate random number 1-4
    string computerChoiceString = computerChoice switch
    {
        1 => "Rock",
        2 => "Paper",
        3 => "Scissors",
        _ => "Well"
    };
    Console.WriteLine($"Computer chose {computerChoiceString}");
    
    // logic
    if (computerChoice == userChoice)
    {
        Console.WriteLine("That's the Draw, try again");
    }
    else if (userChoice == 1 && computerChoice == 3 || userChoice == 2 && computerChoice == (1 | 4) ||
             userChoice == 3 && computerChoice == 2 || userChoice == 4 &&  computerChoice == (1 | 3))
    {
        userWon++;
        Console.WriteLine("Congrats! You won");
    }
    else
    {
        pcWon++;
        Console.WriteLine("Sorry, you lose");
    }

    Console.WriteLine();
    Console.WriteLine($"Current score is you {userWon} to pc {pcWon}");
    
    roundsCounter++;
    Console.WriteLine($"It were played {roundsCounter} round(s)");
    
    if ((pcWon - userWon > rounds - roundsCounter) || (userWon - pcWon > rounds - roundsCounter))
    {
        Console.WriteLine($"This is the end");
        return;
    }
}