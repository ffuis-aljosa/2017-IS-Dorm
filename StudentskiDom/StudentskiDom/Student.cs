using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace StudentskiDom
{
    class Student
    {

       public Student(string firstName, string lastName, string dateOfBirth, string gender,
                       string faculty, string year)
        {
            FirstName = firstName;
            LastName = lastName;
            DateOfBirth = dateOfBirth;
            Gender = gender;
            Faculty = faculty;
            Year = year;
        }

        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Faculty { get; set; }
        public string Year { get; set; }
        public string Room { get; set; }
        public string Warning { get; set; }
    }
}
