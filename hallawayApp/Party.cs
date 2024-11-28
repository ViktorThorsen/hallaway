using System.Diagnostics;
namespace hallawayApp;
public class Party
{
    private Person _organizer;
    public List<Person> _persons = new List<Person>();
    private Menu _paryMenu = new Menu();
    
    private void AddPerson()
    {
        string name = AddPersonNameMenu();
        string phone = AddPersonNumberMenu();
        string email = AddPersonEmailMenu();
        DateTime date = AddPersonDoBMenu();
        Person person = new Person(name, phone, email, date);
        _persons.Add(person);
        Console.WriteLine($"{name} was added to the party");
        //Add This to Database
    }

    private void DisplayAllPersons()
    {
        Console.WriteLine("===========================");
        Console.WriteLine("All Persons:");
        Console.WriteLine("===========================");

        for (int i = 0; i < _persons.Count; i++)
        {
            Person person = _persons[i];
            Console.WriteLine($"{i + 1}. Name: {person.name}, Email: {person.email}, DateOfBirth: {person.dateOfBirth.ToShortDateString()}");
        }
    }
    
    private int SelectPersonMenu()
    {
        DisplayAllPersons();
        Console.WriteLine("\nEnter the number of the person (or 0 to cancel):");

        string input = Console.ReadLine();
        if (int.TryParse(input, out int selectedIndex) && selectedIndex >= 0 && selectedIndex <= _persons.Count)
        {
            return selectedIndex - 1; // Convert to 0-based index
        }
        else
        {
            Console.WriteLine("Invalid input. Please try again.");
            return -1; // Return -1 to indicate an invalid selection
        }
    }
    
    private void DeletePerson()
    {
        int selectedIndex = SelectPersonMenu();

        if (selectedIndex == -1) return; // Invalid input, return to menu
        if (selectedIndex == -2)
        {
            Console.WriteLine("No person was deleted. Returning to the menu.");
            return;
        }

        Console.WriteLine($"Are you sure you want to delete {_persons[selectedIndex].name}? (yes/no)");
        string confirmation = Console.ReadLine();

        if (confirmation?.ToLower() == "yes")
        {
            _persons.RemoveAt(selectedIndex);
            Console.WriteLine("Person successfully deleted.");
        }
        else
        {
            Console.WriteLine("Deletion canceled.");
        }
    }

    public void AddPartyMenu()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine($"===========================" +
                              $"Add a party!" +
                              $"===========================" +
                              $"\n1) Add new person to party \n2) Add Already Registered Person to party \n3) Delete person from party\n4) Done \n0) Quit");

            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number between 0 and 4.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    AddPerson();
                    break;
                case 2:
                   //Add From database
                    break;
                case 3:
                    DeletePerson();
                    break;
                case 4:
                    Console.WriteLine("Finished adding a party!");
                    running = false; // Exit the menu loop
                    break;
                case 0:
                    Console.WriteLine("Goodbye!");
                    running = false; // Exit the menu loop
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose a valid menu option.");
                    break;
            }
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