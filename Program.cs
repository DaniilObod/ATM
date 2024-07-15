using System;
using System.Collections.Generic;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        var users = new Dictionary<string, (string password, decimal balance)>();
        ShowMainMenu(users);
    }

    static void ShowMainMenu(Dictionary<string, (string password, decimal balance)> users)
    {
        Console.Clear();
        Console.OutputEncoding = Encoding.UTF8;
        Console.CursorVisible = false;
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Welcome to the ATM Machine");
        Console.ResetColor();
        Console.WriteLine("\nUse ⬆️  and ⬇️  to navigate and press \u001b[32mEnter/Return\u001b[0m to select:");

        var option = 1;
        var decorator = "✅ \u001b[32m";
        ConsoleKeyInfo key;
        bool isSelected = false;

        while (!isSelected)
        {
            Console.SetCursorPosition(0, 6);
            Console.WriteLine($"{(option == 1 ? decorator : "   ")}Log-In\u001b[0m");
            Console.WriteLine($"{(option == 2 ? decorator : "   ")}Sign-In\u001b[0m");
            Console.WriteLine($"{(option == 3 ? decorator : "   ")}Leave\u001b[0m");

            key = Console.ReadKey(true);

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:
                    option = option == 1 ? 3 : option - 1;
                    break;

                case ConsoleKey.DownArrow:
                    option = option == 3 ? 1 : option + 1;
                    break;

                case ConsoleKey.Enter:
                    isSelected = true;
                    break;
            }
        }

        switch (option)
        {
            case 1:
                LogIn(users);
                break;
            case 2:
                SignIn(users);
                break;
            case 3:
                Console.WriteLine("\nGoodbye!");
                return;
        }
    }

    static void SignIn(Dictionary<string, (string password, decimal balance)> users)
    {
        Console.Clear();
        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        if (users.ContainsKey(username))
        {
            Console.WriteLine("Username already exists. Try again.");
            Console.ReadKey();
            ShowMainMenu(users);
            return;
        }
        Console.Write("Enter password: ");
        string password = Console.ReadLine();
        users[username] = (password, 0m); // Initial balance is 0
        Console.WriteLine("Account created successfully!");
        Console.ReadKey();
        ShowMainMenu(users);
    }

    static void LogIn(Dictionary<string, (string password, decimal balance)> users)
    {
        Console.Clear();
        Console.Write("Enter username: ");
        string username = Console.ReadLine();
        if (!users.ContainsKey(username))
        {
            Console.WriteLine("Username does not exist. Try again.");
            Console.ReadKey();
            ShowMainMenu(users);
            return;
        }
        Console.Write("Enter password: ");
        string password = Console.ReadLine();
        if (users[username].password == password)
        {
            Console.WriteLine("Login successful!");
            ShowUserMenu(users, username);
        }
        else
        {
            Console.WriteLine("Incorrect password. Try again.");
            Console.ReadKey();
            ShowMainMenu(users);
        }
    }

    static void ShowUserMenu(Dictionary<string, (string password, decimal balance)> users, string username)
    {
        bool loggedIn = true;
        while (loggedIn)
        {
            Console.Clear();
            Console.WriteLine("1. Check Balance");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("3. Withdraw");
            Console.WriteLine("4. Log Out");

            var choice = Console.ReadKey().Key;
            switch (choice)
            {
                case ConsoleKey.D1:
                    Console.WriteLine($"\nYour balance is: {users[username].balance}");
                    break;
                case ConsoleKey.D2:
                    Console.Write("\nEnter amount to deposit: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal deposit))
                    {
                        users[username] = (users[username].password, users[username].balance + deposit);
                        Console.WriteLine("Deposit successful!");
                    }
                    else
                    {
                        Console.WriteLine("Invalid amount.");
                    }
                    break;
                case ConsoleKey.D3:
                    Console.Write("\nEnter amount to withdraw: ");
                    if (decimal.TryParse(Console.ReadLine(), out decimal withdraw))
                    {
                        if (withdraw <= users[username].balance)
                        {
                            users[username] = (users[username].password, users[username].balance - withdraw);
                            Console.WriteLine("Withdrawal successful!");
                        }
                        else
                        {
                            Console.WriteLine("Insufficient balance.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid amount.");
                    }
                    break;
                case ConsoleKey.D4:
                    loggedIn = false;
                    ShowMainMenu(users);
                    return;
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
