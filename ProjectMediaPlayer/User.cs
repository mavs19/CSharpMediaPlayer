namespace ProjectMediaPlayer
{
    class User
    {
        // Fields for the username, password hash and salt with getters and setters
        public string UserId { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
    }
}
