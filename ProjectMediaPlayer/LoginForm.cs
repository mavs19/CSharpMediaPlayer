using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectMediaPlayer
{
    public partial class FormLogin : Form
    {
        // Declaring variables used for the log in function
        string username;
        string password;
        User CurrentUser;
        static MockUserRepository userRepo = new MockUserRepository();
        static PasswordManager pwManager = new PasswordManager();

        //public string GetCurrentUser()
        //{
        //    return CurrentUser;
        //}

        // The create user method called upon opening of form
        public FormLogin()
        {
            InitializeComponent();
            CreateAdminUser();
        }

        // Button create user event, variable store text input
        // Password sent in the generate has method, hased passwrod returned to variable
        // The username, passoword hash and salt will create a new object User, added to repo 
        private void BtnCreateUser_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textUsername.Text) || !string.IsNullOrEmpty(textPassword.Text))
            {
                username = textUsername.Text;
                password = textPassword.Text;
                string passwordHash = pwManager.GeneratePasswordHash(password, out string salt);
                User user = new User
                {
                    Username = username,
                    PasswordHash = passwordHash,
                    Salt = salt

                };
                userRepo.AddUser(user);
                MessageBox.Show("Username : " + username + " created.\n" +
                    "Salt : " + salt + "\n" + "Password Hash : " + passwordHash);
                textUsername.Clear();
                textPassword.Clear();
                
            }
            else
            {
                MessageBox.Show("Please enter a username and password");
            }
        }

        // Button Sign in event, the text input variables which are used in teh methods
        // Get User returns to appropriate object's data, password input and returned data sent in method,
        // Password match to recieve a boolean result if matching or not
        private void BtnSignIn_Click(object sender, EventArgs e)
        {
            username = textUsername.Text;
            password = textPassword.Text;
            User user = userRepo.GetUser(username);
            bool result = pwManager.IsPasswordMatch(password, user.Salt, user.PasswordHash);
            CurrentUser = user;
            if (result)
            {
                this.Hide();
                FormMediaPlayer formMediaPlayer = new FormMediaPlayer();
                formMediaPlayer.ShowDialog();
            }
            else
            {
                MessageBox.Show("Username or password incorrect, try again.");
            }
            textUsername.Clear();
            textPassword.Clear();
        }

        // Mehtod to create a default admin user
        // Uses the same functions as the Create user method, but data is hard coded
        private void CreateAdminUser()
        {
            username = "admin";
            password = "admin";
            string passwordHash = pwManager.GeneratePasswordHash(password, out string salt);
            User user = new User
            {
                Username = username,
                PasswordHash = passwordHash,
                Salt = salt

            };
            userRepo.AddUser(user);
        }

        // Method to clear the text fields when button clicked
        private void ButtonClear_Click(object sender, EventArgs e)
        {
            textUsername.Clear();
            textPassword.Clear();
        }
    }
}
