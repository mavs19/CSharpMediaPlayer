using System.Collections.Generic;
using System.Linq;

namespace ProjectMediaPlayer
{
    class MockUserRepository
    {
        // List of the User class objects to store multiple users
        List<User> users = new List<User>();

        // Function to add the user to im memory dummy DB
        public void AddUser(User user)
        {
            users.Add(user);
        }
        // Function to retrieve the user based on user id
        public User GetUser(string userid)
        {
            try
            {
                return users.Single(u => u.Username == userid);
            }
            catch
            {
                return users.First();
            }
        }

    }
}
