using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace randomStudentOH.Models
{
    public class Class
    {
        public string Symbol { get; set; }  
        public ObservableCollection<Student> Students { get; set; } = new ObservableCollection<Student>();
        
    }
}
