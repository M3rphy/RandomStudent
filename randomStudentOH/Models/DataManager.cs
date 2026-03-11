using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace randomStudentOH.Models
{
    public class DataManager
    {
        public ObservableCollection<Class> Classes = new ObservableCollection<Class>();
        private  readonly string fileName = "students.txt";
        private  string pathFile => Path.Combine(FileSystem.AppDataDirectory, fileName);

        public async Task save(ObservableCollection<Class> classes)
        {
         
            string content = "";
            for (int i = 0; i < classes.Count; i++)
            {
                content += $"{classes[i].Symbol}|";
                for (int j = 0; j < classes[i].Students.Count; j++)
                {
                    content += $"{classes[i].Students[j].Number}.{classes[i].Students[j].Name},";
                }
                content += "\n";
            }
            await File.WriteAllTextAsync(pathFile, content);
        }
        public async Task<ObservableCollection<Class>> load()
        {
            Debug.WriteLine(FileSystem.AppDataDirectory);

            Classes.Clear();
            if (File.Exists(pathFile))
            {
                string[] lines =  await File.ReadAllLinesAsync(pathFile);
                for (int i = 0; i < lines.Length; i++)
                {
                    string[] currentClass = lines[i].Split("|");
                    string[] students = currentClass[1].Split(",");
                    ObservableCollection<Student> studentsInClass = new ObservableCollection<Student>();
                    for (int j = 0; j < students.Length; j++)
                    {
                        string[] studentInfo = students[j].Split(".");
                        if (studentInfo[0] != "" && studentInfo[1] != "")
                            studentsInClass.Add(new Student(int.Parse(studentInfo[0]), studentInfo[1]));
                    }
                    Classes.Add(new Class { Symbol = currentClass[0], Students = studentsInClass });
                }
            }
            return Classes;
        }
    }
}
