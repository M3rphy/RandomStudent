using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using randomStudentOH.Models;
using randomStudentOH.Views; 

namespace randomStudentOH.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        public DataManager _dataManager = new();
        private ObservableCollection<Student> _displayStudents = new();

      
        public string DisplayNumber { get; set; }
        public string DisplayLuckyNumber { get; set; }
        public ObservableCollection<Student> DisplayStudents
        {
            get => _displayStudents;
            private set
            {
                if (_displayStudents != value)
                {
                    _displayStudents = value;
                    OnPropertyChanged();
                }
            }
        }

        public Class SelectedClass
        {
            get => _selectedClass;
            set
            {
                if (value != _selectedClass)
                {
                    _selectedClass = value;
                    OnPropertyChanged();
                    DisplayStudents = _selectedClass?.Students ?? new ObservableCollection<Student>();
                    if(DisplayStudents.Count > LuckyNumber)
                    {
                        DisplayStudents[LuckyNumber - 1].IsLucky = true;
                    }
                }
            }
        }
        public Class _selectedClass;
        public ObservableCollection<Class> Classes { get; } = new();
        public IRelayCommand GetRandomNumberCommand { get; }
        public IRelayCommand EditClassCommand { get; }
        public IRelayCommand AddClassCommand { get; }
        public IRelayCommand RemoveClassCommand { get; }
        public Action<int> Animate { get; internal set; }
        private List<Student> availbleStudents = new List<Student>();
        private List<int> wasAskedList = new List<int>();
        private int LuckyNumber { get; set; } = 0;
        public MainViewModel()
        {
            InitializeAsync();

            GetRandomNumberCommand = new RelayCommand(() =>
            {
                if (SelectedClass != null && SelectedClass.Students.Count > 0)
                {
                    foreach(var s in DisplayStudents)
                    {
                        s.WasAsked = false;
                    }
                    foreach (var askedNumber in wasAskedList)
                    {
                        var student = SelectedClass.Students.FirstOrDefault(s => s.Number == askedNumber);
                        if (student != null)
                        {
                            student.WasAsked = true;
                        }
                    }
                    availbleStudents = SelectedClass.Students.Where(s => s.IsPresent && !s.WasAsked && !s.IsLucky).ToList();
                    foreach(var s in availbleStudents)
                    {
                        Debug.WriteLine($"{s.Number}");
                    }
                    Debug.WriteLine("==============================");
                    if(availbleStudents.Count < 1)
                    {
                        return;
                    }
                   
                    var random = new Random();
                    int index = random.Next(availbleStudents.Count);
                    

                    var randomStudent = availbleStudents[index];
                    var selectedStudent = SelectedClass.Students.FirstOrDefault(s => s.Number == randomStudent.Number);
                    if (selectedStudent != null)
                    {
                        if (wasAskedList.Count < 3)
                        {
                            wasAskedList.Add(selectedStudent.Number);
                        }
                        else
                        {
                            wasAskedList.RemoveAt(0);
                            wasAskedList.Add(selectedStudent.Number);
                        }
                    }
                     
                    DisplayNumber = $"{randomStudent.Number}. {randomStudent.Name}";
                    OnPropertyChanged(nameof(DisplayNumber));
                    Animate?.Invoke(randomStudent.Number);
                }
            });
            EditClassCommand = new RelayCommand(() =>
            {
                if (SelectedClass != null)
                {
                    var editSheet = new EditPage(SelectedClass, this);
                    Application.Current.MainPage.ShowPopupAsync(editSheet);
                }
            });
            AddClassCommand = new RelayCommand(async () =>
            {
                string input = await Application.Current.MainPage.DisplayPromptAsync("Nawa Kategoria", "Podaj Nazwe Kategori");
                if (string.IsNullOrWhiteSpace(input))
                {
                    return; 
                }
                var newClass = new Class()
                {
                    Symbol = input,
                    Students = new ObservableCollection<Student>()
                };
                Classes.Add(newClass);
                    await _dataManager.save(Classes);
                    await InitializeAsync();
            });
            RemoveClassCommand = new RelayCommand(async () =>
            {
                if (SelectedClass != null)
                {
                        Classes.Remove(SelectedClass);
                        await _dataManager.save(Classes);
                        await InitializeAsync();
                }
            });
        }
    

        public async Task InitializeAsync()
        {
            Classes.Clear();
            int maxStudentsNumber = 0;
            var loaded = await _dataManager.load();
            foreach (var c in loaded)
            {
                Classes.Add(c);
                if (c.Students.Count > maxStudentsNumber)
                {
                    maxStudentsNumber = c.Students.Count;
                }
            }
            var random = new Random();
            LuckyNumber = random.Next(maxStudentsNumber)+1;   
            DisplayLuckyNumber = $"Szczęśliwy Numer: {LuckyNumber}";
            OnPropertyChanged(nameof(DisplayLuckyNumber));
            if (SelectedClass == null)
            {
                SelectedClass = Classes.FirstOrDefault();
            }
            if(SelectedClass != null)
            {
                DisplayStudents = SelectedClass.Students;
                OnPropertyChanged(nameof(DisplayStudents));
            }
            
        }
   
    }
}
