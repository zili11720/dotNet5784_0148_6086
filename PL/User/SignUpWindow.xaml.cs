using PL.Agent;
using System.Windows;
using System.Windows.Controls;

namespace PL.User;

/// <summary>
/// Interaction logic for SignUpWindow.xaml
/// </summary>
public partial class SignUpWindow : Window
{
    static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

    public BO.User NewUser
    {
        get { return (BO.User)GetValue(NewUserProperty); }
        set { SetValue(NewUserProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrentUser.This enables animation, styling, binding, etc...
    public static readonly DependencyProperty NewUserProperty =
        DependencyProperty.Register("NewUser", typeof(BO.User), typeof(SignUpWindow), new PropertyMetadata(null));
    public SignUpWindow()
    {
        InitializeComponent();
        NewUser=new BO.User();
    }

    string? password = null;

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is PasswordBox passwordBox)
        {
            password = passwordBox.Password;
        }
    }
    private void btnCreateAccount_Click(object sender, RoutedEventArgs e)
    {
        if (s_bl.Agent.ReadAll().Any(a => a.Id == NewUser.UserId) is false)
            MessageBox.Show("Could not find an agent with the given id","worng input" ,MessageBoxButton.OK, MessageBoxImage.Error);
        NewUser.Password = password;
        s_bl.User.Create(NewUser);
    }
}
