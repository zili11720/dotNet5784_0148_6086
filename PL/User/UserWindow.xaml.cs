using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;

namespace PL.User
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        static readonly BlApi.IBl s_bl = BlApi.Factory.Get();

        public UserWindow(int userId)
        {
            InitializeComponent();
            //DataContext = this;

        }

        private void btnLogIn_Click(object sender, RoutedEventArgs e)
        {
           try
            {
                Button button = (Button)sender; 
                Grid parentGrid= (Grid)button.Parent; //as Grid
                TextBox userNameTextBox=(TextBox)parentGrid.Children[2];
                PasswordBox passwordBox = (PasswordBox)parentGrid.Children[4];

                string userName = userNameTextBox.Text;
                string password = passwordBox.Password;
                user = s_bl.User.read(username);
                if(User.password != password)
                {
                    MessageBox.Show("Invalid password, Please try again","worng password", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                MessageBox.Show("Login Successful!");
                new MainWindow(user).Show();
                
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
}
//public BO.User CurrentUser
//{
//    get { return (BO.User)GetValue(CurrentUserProperty); }
//    set { SetValue(CurrentUserProperty, value); }
//}

//// Using a DependencyProperty as the backing store for CurrentAgent.  This enables animation, styling, binding, etc...
//public static readonly DependencyProperty CurrentUserProperty =
//    DependencyProperty.Register("CurrentUser", typeof(BO.User), typeof(UserWindow), new PropertyMetadata(null));

//try
//{

//    //Fetch the agent with the given id or create a new one with defult values if the id does not exist
//    CurrentUser = sender.UserId is not 0 ? new BO.User() { UserId = 0, UserName = "", Password = "", IsManager = false } : s_bl.User.Read(userId)!;
//}
//catch (BO.BlDoesNotExistException ex)
//{
//    MessageBox.Show(ex.Message, "Could not find an afent with the a given id", MessageBoxButton.OK, MessageBoxImage.Error);
//    this.Close();
//}