using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW6Project.Core;
using HW6Project.Utility;

namespace HW6Project.UI
{
    public class ConsoleUI
    {
        public UserRepository Repository { get; set; }
        public bool ShouldExit { get; set; }
        public ConsoleUI() 
        {
            Repository = new();
        }


        public void StartUI()
        {
            while (!ShouldExit)
            {
                try
                {
                    ShowMenu();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error of {ex} happened.");
                    Console.WriteLine("Please try again.");
                }
            }
        }

        public void ShowMenu()
        {
            Console.WriteLine("Select one of the following option: ");
            Console.WriteLine("1. Create a new user");
            Console.WriteLine("2. Edit a user");
            Console.WriteLine("3. List all users");
            Console.WriteLine("4. Delete a user");
            Console.WriteLine("5. Exit the program");

            var userInput = InputHelper.GetConsoleInput("Your choice: ", int.Parse);
            switch (userInput)
            {
                default:
                    Console.WriteLine($"Invalid option selected: {userInput}");
                    return;

                case 1:
                    ShowCreateUserMenu();
                    break;
                case 2:
                    ShowEditUserMenu();
                    break;
                case 3:
                    ShowListUserMenu();
                    break;
                case 4:
                    ShowDeleteUserMenu();
                    break;
                case 5:
                    ShouldExit = true;
                    return;
            }
        }

        public void ShowCreateUserMenu() =>
            Repository.AddUser(new()
            {
                Name = InputHelper.GetConsoleInput("Enter user's name: "),
                Phone = InputHelper.GetConsoleInput("Enter phone:"),
                BirthdayDate =
                InputHelper.GetConsoleInput("Enter Birthday date: ", Convert.ToDateTime)
            });


        public void ShowEditUserMenu()
        {
            var userId = InputHelper.GetConsoleInput("User's ID: ", int.Parse);

            var user = Repository.GetUser(userId)!;
            user.Name = InputHelper.GetConsoleInput("Enter user's name [or empty]: ", 0);
            user.Phone = InputHelper.GetConsoleInput("Enter phone:", 0);
            user.BirthdayDate = InputHelper.GetConsoleInput("Enter Birthday date: ", Convert.ToDateTime);

            Repository.SaveUsers();
        }

        public void ShowListUserMenu()
        {
            if (Repository.CachedUsers == null || Repository.CachedUsers.Length == 0)
            {
                Console.WriteLine("User list is empty!");
                return;
            }

            foreach (var currentUser in Repository.CachedUsers)
            {
                Console.WriteLine(currentUser);
            }
        }

        public void ShowDeleteUserMenu()
        {
            var userId = InputHelper.GetConsoleInput("User's ID: ", int.Parse);
            Repository.DeleteUser(userId);
        }

    }
}
