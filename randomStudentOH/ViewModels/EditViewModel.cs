using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Input;
using randomStudentOH.Models;
using randomStudentOH.Views;

namespace randomStudentOH.ViewModels
{
    public class EditViewModel
    {
        public Class _selectedClass { get; set; } = new();
        private MainViewModel _mainViewModel;
        public IAsyncRelayCommand SaveCommand { get; }
        public IRelayCommand AddStudentCommand { get; }
        public IRelayCommand<Student> RemoveStudentCommand { get; }

        public EditViewModel(Class SelectedClass, MainViewModel mainViewModel)
        {
            _selectedClass.Symbol = SelectedClass.Symbol;
            foreach (var s in SelectedClass.Students)
            {
                _selectedClass.Students.Add(s);
            }
            _mainViewModel = mainViewModel;
            SaveCommand = new AsyncRelayCommand(async () =>{
                int i = 1;
                foreach (var c in _mainViewModel.Classes)
                {
                    if(c == SelectedClass)
                    {
                        c.Students = _selectedClass.Students;
                        foreach (var s in c.Students)
                        {
                            s.Number = i;
                            i++;
                        }
                        c.Symbol = _selectedClass.Symbol;
                    }
                }   
                await _mainViewModel._dataManager.save(_mainViewModel.Classes);
                await _mainViewModel.InitializeAsync();
            });
            AddStudentCommand = new RelayCommand(() =>
            {
                _selectedClass.Students.Add(new Student(_selectedClass.Students.Count+1, "") );
            });
            RemoveStudentCommand = new RelayCommand<Student>((student) =>
            {
                if(_selectedClass.Students.Count > 0)
                {
                    _selectedClass.Students.Remove(student);
                }
            });
        }
        public EditViewModel()
        {
        }
        
    }
}
