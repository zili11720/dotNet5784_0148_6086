using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using PL.Tools;


namespace PL.Task;

/// <summary>
/// Interaction logic for DependencyListWindow.xaml
/// </summary>
public partial class DependencyListWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();
    private readonly int _id; // Declare id as a private field
    public DependencyListWindow(int id)
    {
        _id = id;
        DependencyList = s_bl.Task.GetDependenciesList(id).ToObservableCollection();
        int cnt = DependencyList.Count(t => true);
        InitializeComponent();
    }

    public ObservableCollection<BO.TaskInList> DependencyList
    {
        get { return (ObservableCollection<BO.TaskInList>)GetValue(DependencyListProperty); }
        set { SetValue(DependencyListProperty, value); }
    }

    // Dependency property for the list of dependencies
    public static readonly DependencyProperty DependencyListProperty =
        DependencyProperty.Register("DependencyListProperty", typeof(ObservableCollection<BO.TaskInList>), typeof(DependencyListWindow), new PropertyMetadata(null));

    // Property to bind to the TextBox for entering new dependency ID
    public int NewDependencyId
    {
        get { return (int)GetValue(NewDependencyIdProperty); }
        set { SetValue(NewDependencyIdProperty, value); }
    }

    // Dependency property for the new dependency ID
    public static readonly DependencyProperty NewDependencyIdProperty =
        DependencyProperty.Register("NewDependencyId", typeof(int), typeof(DependencyListWindow), new PropertyMetadata(0));


    private void btnDeleteDependency_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete the dependency?", "warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (MessageBoxResult.Yes == result)
            {     
                BO.TaskInList? dep = (sender as Button)!.CommandParameter as BO.TaskInList;
                s_bl.Task.RemoveDependency(_id, dep!.Id);
                DependencyList.Remove(dep);
                MessageBox.Show("The dependency was deleted successfuly", "Well done!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
       
    }

    private void btnAddDependency_Click(object sender, RoutedEventArgs e)
    {
        // לבדוק מה נסגר עם התז בהוספה המיידית
        try
        {
            int newId = NewDependencyId; // Get the new dependency ID from the bound property
            BO.TaskInList? newDep = s_bl.Task.AddDependency(_id, NewDependencyId);
            DependencyList.Add(newDep);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
