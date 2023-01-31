using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW6Project.Core.Models;
using HW6Project.Utility;
using System.Numerics;
using System.Xml.Linq;

namespace HW6Project.Core
{
    public class UserRepository : IUserRepository
    {
        public string RepositoryFileName { get; set; }
        public User[]? CachedUsers { get; set; }
        public bool ShouldExit { get; set; }

        public UserRepository(string fileName = "FileDataStorage.csv") 
        {
            RepositoryFileName = fileName;

            try
            {
                CachedUsers = GetAllUsers();
            }
            catch (FileNotFoundException)
            {
                // Create the file if it doesn't exist.
                File.Create(fileName).Dispose();
                CachedUsers = Array.Empty<User>();
            }


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
            AddUser(new()
            {
                Name = InputHelper.GetConsoleInput("Enter user's name: "),
                Phone = InputHelper.GetConsoleInput("Enter phone:"),
                BirthdayDate =
                InputHelper.GetConsoleInput("Enter Birthday date: ", Convert.ToDateTime)
            });


        public void ShowEditUserMenu()
        {
            var userId = InputHelper.GetConsoleInput("User's ID: ", int.Parse);

            var user = GetUser(userId)!;
            user.Name = InputHelper.GetConsoleInput("Enter user's name [or empty]: ", 0);
            user.Phone = InputHelper.GetConsoleInput("Enter phone:", 0);
            user.BirthdayDate = InputHelper.GetConsoleInput("Enter Birthday date: ", Convert.ToDateTime);

            SaveUsers();
        }

        public void ShowListUserMenu()
        {
            if (CachedUsers == null || CachedUsers.Length == 0)
            {
                Console.WriteLine("User list is empty!");
                return;
            }

            foreach (var currentUser in CachedUsers)
            {
                Console.WriteLine(currentUser);
            }
        }

        public void ShowDeleteUserMenu()
        {
            var userId = InputHelper.GetConsoleInput("User's ID: ", int.Parse);
            DeleteUser(userId);
        }

        public static User ParseFromCsv(string[] csvData) =>
            new()
            {
                UserId = Convert.ToInt32(csvData[0]),
                Name = Convert.ToString(csvData[1]),
                Phone = Convert.ToString(csvData[2]),
                BirthdayDate = Convert.ToDateTime(csvData[3]),
                CreatedAt = Convert.ToDateTime(csvData[4]),
            };

        private string FormatCsvUsers()
        {
            var sb = new StringBuilder();

            foreach (var currentUser in CachedUsers!)
            {
                sb.AppendLine($"{currentUser.UserId},{currentUser.Name}," +
                $"{currentUser.Phone},{currentUser.BirthdayDate},{currentUser.CreatedAt}");
            }

            return sb.ToString();
        }

        public User[] ReadAllUsers(string fileName) =>
            File.ReadAllLines(
                string.IsNullOrEmpty(fileName) ? RepositoryFileName : 
                fileName)
            .Select(s => 
            ParseFromCsv(s.Split(',', StringSplitOptions.RemoveEmptyEntries)))
            .ToArray();
        public User[] GetAllUsers() =>
            ReadAllUsers(RepositoryFileName);
        public User[] GetUsers() =>
            CachedUsers ??= GetAllUsers();
        public User AddUser(User user)
        {
            SetAndSaveUsers(CachedUsers?.Append(user).ToArray() ?? new[] { user });
            return user;
        }
        public User? GetUser(int id) =>
            CachedUsers?.First(u => u.UserId == id) ?? null;
        public User[]? GetUsers(string name) =>
            CachedUsers?.Where(
                u => u.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
            .ToArray()
            ?? null;
        public void WriteUsers(string fileName) =>
            File.WriteAllText(fileName, FormatCsvUsers());
        public void DeleteUser(int id) =>
            SetAndSaveUsers(CachedUsers?.Where(u => u.UserId != id).ToArray() ?? null);
        public void SetAndSaveUsers(User[]? users)
        {
            CachedUsers = users;
            SaveUsers();
        }
        public void SaveUsers() =>
            WriteUsers(RepositoryFileName);
        public void RefereshUsers() =>
            ReadAllUsers(RepositoryFileName);
    }
}
