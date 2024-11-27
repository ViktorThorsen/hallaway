using System.Diagnostics;
namespace hallawayApp;
public class Party
{
    public Person organizer;
    public List<Person> persons = new List<Person>();
    private Menu _paryMenu = new Menu();

    public Party()
    {
        Console.Clear();
        AddPartyMenu();
    }
    private void AddPerson()
    {
        string name = AddPersonNameMenu();
        string phone = AddPersonNumberMenu();
        string email = AddPersonEmailMenu();
        DateTime date = AddPersonDoBMenu();
        Person person = new Person(name, phone, email, date);
        persons.Add(person);
        Console.WriteLine($"{name} was added to the party");
    }

    public void AddPartyMenu()
    {
        Console.WriteLine($"===========================" +                      
                          $"Add a party!" +                      
                          $"===========================" +                      
                          $"\n1) Add person \n2) Delete person \n3) Edit person \n4) Done \n0) Quit");    
        int input = Int32.Parse(Console.ReadLine());    
        Debug.Assert(input != null);
        
        switch (input)
        {
            case 1:
                AddPerson();
                break;
            case 2:
                
                break;
            case 3:
                
                break;
            case 4:
                break;
            case 0:
                Console.WriteLine("Goodbye!"); // Quit!
                break;
            default:
                break;
        }
    }

    public string AddPersonNameMenu()
    {
        Console.WriteLine($"===========================" +
                          $"Add Person: " +                   
                          $"===========================" +               
                          $"\nEnter person name: ");   
        string input = Console.ReadLine();    
        Debug.Assert(input != null);
        return input;
    }

    public string AddPersonNumberMenu()
    {
        Console.WriteLine($"===========================" +
                          $"Add Person: " +                   
                          $"===========================" +               
                          $"\nEnter persons phone number: ");   
        string input = Console.ReadLine();    
        Debug.Assert(input != null);
        return input;
    }
    
    public string AddPersonEmailMenu()
    {
        Console.WriteLine($"===========================" +
                          $"Add Person: " +                   
                          $"===========================" +               
                          $"\nEnter persons email: ");   
        string input = Console.ReadLine();    
        Debug.Assert(input != null);
        return input;
    }
    
    public DateTime AddPersonDoBMenu()
    {
        Console.WriteLine("===========================");
        Console.WriteLine("Add Person");
        Console.WriteLine("===========================");
        Console.WriteLine("Enter Date of Birth (e.g., 2000-12-31): ");

        while (true) // Loop until the user enters a valid date
        {
            string input = Console.ReadLine();
            if (DateTime.TryParse(input, out DateTime dateOfBirth))
            {
                return dateOfBirth; // Return the valid date
            }
            else
            {
                Console.WriteLine("Invalid date format. Please try again (e.g., 2000-12-31):");
            }
        }
    }
}