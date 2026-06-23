Console.WriteLine("Hello this is Rock Paper Scissors");
Console.WriteLine("Enter your step");


var userWon = false;

while (userWon == false)
{
    Console.Clear();
    
    Console.WriteLine("1 - Rock");
    Console.WriteLine("2 - Paper");
    Console.WriteLine("3 - Scissors");
    Console.WriteLine("0 - Exit");
    
    var userInput = Console.ReadLine(); // "5"

    int userChoice;

    // parse user input = string into int 
    // put result to out result param
    // return if parse was successful

    if (!int.TryParse(userInput, out userChoice) || !(userChoice >= 0 && userChoice <= 3))
    {
        Console.WriteLine("Invalid input");
        continue;
    }

    if (userChoice == 0)
    {
        return;
    }

    var random = new Random();
    var computerChoice = random.Next(1, 4); // generate random number 1-3

    string userChoiceString;
    switch (userChoice)
    {
        case 1:
            Console.WriteLine("Rock");
            userChoiceString = "Rock";
            break;
        case 2:
            userChoiceString = "Paper";
            break;
        default:
            userChoiceString = "Scissors";
            break;
    }

    Console.WriteLine($"You chose {userChoiceString}");

    string computerChoiceString = computerChoice switch
    {
        1 => "Rock",
        2 => "Paper",
        _ => "Scissors"
    };

    Console.WriteLine($"Computer chose {computerChoiceString}");

    if (computerChoice == userChoice)
    {
        Console.WriteLine("Draw");
    }
    else if (userChoice == 1 && computerChoice == 3 || userChoice == 2 && computerChoice == 1 ||
             userChoice == 3 && computerChoice == 2)
    {
        Console.WriteLine("You win");
        userWon = true;
    }
    else
    {
        Console.WriteLine("You lose");
    }

    // Ctrl + K + D 
}


//data in C# 
//  int, long 5 10 11
//  float, double, decimal 56.132
//  chars - 'a'
//  strings - "text"
//  bool - true / false
//  class