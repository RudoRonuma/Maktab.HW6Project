using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HW6Project.Core.Models
{
    public class User
    {
        private static readonly object _locker = new();
        private static int _lastUserId = 0;

        private int _userId;
        private string _name;
        private string _phone;
        private DateTime _birthdayDate;

        public int UserId
        {
            get { return _userId; }
            set
            {
                if (value <= 0)
                    return;
                
                _userId = value;
            }
        }
        public string Name
        {
            get { return _name; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;

                _name = value;
            }
        }
        public string Phone
        {
            get { return _phone; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    return;

                _phone = value;
            }
        }
        public DateTime BirthdayDate
        {
            get { return _birthdayDate; }
            set
            {
                if (value == DateTime.MinValue)
                    return;

                _birthdayDate = value;
            }
        }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public User()
        {
            if (UserId == 0)
            {
                lock (_locker)
                {
                    _lastUserId++;
                    UserId = _lastUserId;
                }
            }

            _name = string.Empty;
            _phone = string.Empty;
        }

        public override string ToString()
        {
            return $"{UserId}: {Name} ({Phone}) at {CreatedAt} " +
                $"Birthday: {BirthdayDate}";
        }

        public string FormatCsv() =>
            $"{UserId},{Name}," +
                $"{Phone},{BirthdayDate},{CreatedAt}";

        public static User ParseFromCsv(string[] csvData) =>
            new()
            {
                UserId = Convert.ToInt32(csvData[0]),
                Name = Convert.ToString(csvData[1]),
                Phone = Convert.ToString(csvData[2]),
                BirthdayDate = Convert.ToDateTime(csvData[3]),
                CreatedAt = Convert.ToDateTime(csvData[4]),
            };
    }
}
