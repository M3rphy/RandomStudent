using CommunityToolkit.Maui.Views;
using randomStudentOH.Models;
using randomStudentOH.ViewModels;

namespace randomStudentOH.Views;

public partial class EditPage : Popup
{
    public EditPage(Class SelectedClass, MainViewModel vm)
    {
        InitializeComponent();
        BindingContext = new EditViewModel(SelectedClass, vm);
    }
}