namespace hallawayApp;

public class Admin
{
    public string name;
    public string phone;
    public string email;
    public DateTime dateOfBirth;

    public Admin(string Name, string phone, string email, DateTime dateOfBirth)
    {
        this.name = Name;
        this.phone = phone;
        this.email = email;
        this.dateOfBirth = dateOfBirth;
    }
}