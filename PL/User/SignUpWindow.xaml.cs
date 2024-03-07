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

    public int UserId
    {
        get { return (int)GetValue(UserIdProperty); }
        set { SetValue(UserIdProperty, value); }
    }

    // Using a DependencyProperty as the backing store for UserId.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty UserIdProperty =
        DependencyProperty.Register("UserId", typeof(int), typeof(SignUpWindow), new PropertyMetadata(0));


    public string UserName
    {
        get { return (string)GetValue(UserNameProperty); }
        set { SetValue(UserNameProperty, value); }
    }

    // Using a DependencyProperty as the backing store for UserName.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty UserNameProperty =
        DependencyProperty.Register("UserName", typeof(string), typeof(SignUpWindow), new PropertyMetadata(null));



    public SignUpWindow()
    {
        InitializeComponent();  
    }

    string? password = null;

    private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        password = (sender as PasswordBox)?.Password;
    }

    private void btnCreateAccount_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (s_bl.Agent.ReadAll().Any(a => a.Id == UserId) is false)
                MessageBox.Show("Could not find an agent with the given id", "worng input", MessageBoxButton.OK, MessageBoxImage.Error);
            if (s_bl.User.Read(UserId) is not null)
                MessageBox.Show("This agent allready has a user", "worng input", MessageBoxButton.OK, MessageBoxImage.Error);
            BO.User newUser = new() { UserId = UserId, UserName = UserName, Password = password };
            s_bl.User.Create(newUser);
            MessageBox.Show("User was successfuly updated", "success", MessageBoxButton.OK, MessageBoxImage.Information);
            this.Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

    }
}
