namespace hallawayApp;

public class Person
{
    public string name;
    public string phone;
    public string email;
    public DateTime dateOfBirth;
    
 
    public Person(string name, string phone, string email, DateTime dateOfBirth)
    {
        this.name = name;
        this.phone = phone;
        this.email = email;
        this.dateOfBirth = dateOfBirth;
      
    }
}