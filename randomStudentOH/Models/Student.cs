using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace randomStudentOH.Models
{
    public class Student
    {
        public int Number { get; set; }
        public string Name { get; set; }
        public bool IsPresent { get; set; } = true;
        public bool WasAsked { get; set; } = false;
        public bool IsLucky { get; set; } = false;
        public Student(int number, string name)
        {
            Number = number;
            Name = name;
        }
    }
}
