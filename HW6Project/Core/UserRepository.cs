using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW6Project.Core.Models;
using HW6Project.Utility;

namespace HW6Project.Core
{
    public class UserRepository : IUserRepository
    {
        public string RepositoryFileName { get; set; }
        public User[]? CachedUsers { get; set; }


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

        private string FormatCsvUsers()
        {
            var sb = new StringBuilder();

            foreach (var currentUser in CachedUsers!)
            {
                sb.AppendLine(currentUser.FormatCsv());
            }

            return sb.ToString();
        }

        public User[] ReadAllUsers(string fileName) =>
            File.ReadAllLines(
                string.IsNullOrEmpty(fileName) ? RepositoryFileName : 
                fileName)
            .Select(s => 
            User.ParseFromCsv(s.Split(',', StringSplitOptions.RemoveEmptyEntries)))
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
