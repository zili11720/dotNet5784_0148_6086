using PL.Employee;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;

namespace PL.User;

/// <summary>
/// Interaction logic for UserWindow.xaml
/// </summary>
public partial class UserWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public BO.User CurrentUser
    {
        get { return (BO.User)GetValue(CurrentUserProperty); }
        set { SetValue(CurrentUserProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentUser.This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentUserProperty =
        DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(UserWindow), new PropertyMetadata(null));

    public UserWindow()
    {
        InitializeComponent();
    }

    string? password = null;

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is PasswordBox passwordBox)
        {
            password = passwordBox.Password;
        }
    }

    private void btnLogIn_Click(object sender, RoutedEventArgs e)
    {
       try
       {
            // CurrentUser = s_bl.User.Read(CurrentUser.UserName)!;
            CurrentUser = s_bl.User.Read(CurrentUser.UserName);
            if (CurrentUser.Password != password)
            {
                MessageBox.Show("Invalid password, Please try again","worng password", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MessageBox.Show("Login Successful!");
            if (CurrentUser.IsManager is true)
                new MainWindow().Show();
            else
                new AgentEmployeeWindow(CurrentUser.UserId).Show();
            
       }
        catch(Exception ex)
       {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void btnSingUp_Click(object sender, RoutedEventArgs e)
    {
        new SignUpWindow().Show();
    }
}
