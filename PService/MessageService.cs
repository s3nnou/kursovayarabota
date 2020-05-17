using System.Windows.Forms;

namespace PService
{
    public class MessageService
    {
        public static void SuccsesfulLogin()
        {
            MessageBox.Show("You are in!");
        }

        public static void SuccsesfulRegister()
        {
            MessageBox.Show("You are in!");
        }

        public static void NameIsNotVacant()
        {
            MessageBox.Show("There are existen user with such name");
        }


        public static void PasswordMatch()
        {
            MessageBox.Show("Seems like passwords are not similar");
        }

        public static void FailedRegistration()
        {
            MessageBox.Show("Something gone wrong");
        }

        public static void FailedLogin()
        {
            MessageBox.Show("There are no such user!");
        }

        public static void FailedClientIdSearch()
        {
            MessageBox.Show("Invalid client ID");
        }

        public static void SuccsesfulChange()
        {
            MessageBox.Show("Changes Saved");
        }
    }
}
