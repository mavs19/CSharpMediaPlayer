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
        string username;
        string password;
        static MockUserRepository userRepo = new MockUserRepository();
        static PasswordManager pwManager = new PasswordManager();


        public FormLogin()
        {
            InitializeComponent();
            CreateAdminUser();
        }

        private void BtnCreateUser_Click(object sender, EventArgs e)
        {
            username = textUsername.Text;
            password = textPassword.Text;
            string passwordHash = pwManager.GeneratePasswordHash(password, out string salt);
            User user = new User
            {
                UserId = username,
                PasswordHash = passwordHash,
                Salt = salt

            };
            userRepo.AddUser(user);
            //toolStripStatusLabel.Text = "User : " + username + " created";
            MessageBox.Show("Username : " + username + " created.\n" +
                "Salt : " + salt + "\n" + "Password Hash : " + passwordHash);
            textUsername.Clear();
            textPassword.Clear();
        }

        private void BtnSignIn_Click(object sender, EventArgs e)
        {
            username = textUsername.Text;
            password = textPassword.Text;
            User user = userRepo.GetUser(username);
            bool result = pwManager.IsPasswordMatch(password, user.Salt, user.PasswordHash);
            if (result)
            {
                //toolStripStatusLabel.Text += "Login successfull";
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

        private void CreateAdminUser()
        {
            username = "admin";
            password = "admin";
            string passwordHash = pwManager.GeneratePasswordHash(password, out string salt);
            User user = new User
            {
                UserId = username,
                PasswordHash = passwordHash,
                Salt = salt

            };
            userRepo.AddUser(user);
        }

        private void ButtonClear_Click(object sender, EventArgs e)
        {
            textUsername.Clear();
            textPassword.Clear();
        }
    }
}
