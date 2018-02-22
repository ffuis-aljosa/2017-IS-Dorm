using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentskiDom
{
    class Employee
    {
        public Employee(string firstName, string lastName, string dateOfBirth, string position,
                        string username, string password)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Position = position;
            Username = username;
            Password = password;
        }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Position { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
