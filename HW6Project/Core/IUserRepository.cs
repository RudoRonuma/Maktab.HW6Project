using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HW6Project.Core.Models;

namespace HW6Project.Core
{
    public interface IUserRepository
    {
        User[] ReadAllUsers(string fileName);
        User[] GetAllUsers();
        User[] GetUsers();
        User? GetUser(int id);
        User[]? GetUsers(string name);
        User AddUser(User user);
        void WriteUsers(string fileName);
        void DeleteUser(int id);
        void SaveUsers();
        void RefereshUsers();
        
    }
}
