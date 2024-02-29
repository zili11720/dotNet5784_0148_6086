using PL.Employee;
using PL.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL;

/// <summary>
/// Interaction logic for UserWindow.xaml
/// </summary>
public partial class PasswordWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public BO.User CurrentUser
    {
        get { return (BO.User)GetValue(CurrentUserProperty); }
        set { SetValue(CurrentUserProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentUser.This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrentUserProperty =
        DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(PasswordWindow), new PropertyMetadata(null));

    public PasswordWindow()
    {
        InitializeComponent();
        CurrentUser = new BO.User() { UserId = 0, UserName = "", Password = "",IsManager =false }; 
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
            CurrentUser = s_bl.User.Read(CurrentUser.UserName);
            if (CurrentUser.Password != password)
            {
                MessageBox.Show("Invalid password, Please try again", "worng password", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            MessageBox.Show("Login Successful!");
            if (CurrentUser.IsManager is true)
                new MainWindow().Show();
            else
                new AgentEmployeeWindow(CurrentUser.UserId).Show();

        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void btnSingUp_Click(object sender, RoutedEventArgs e)
    {
        new SignUpWindow().Show();
    }
}

