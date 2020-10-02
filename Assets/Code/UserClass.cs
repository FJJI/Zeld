public class User
{
    public string username;
    public string email;
    public string password;
    public int wins;
    public int loses;

    public User()
    {
    }

    public User(string username, string email, string password, int wins, int loses)
    {
        this.username = username;
        this.email = email;
        this.password = password;
        this.wins = wins;
        this.loses = loses;

    }
}
