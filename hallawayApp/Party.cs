using System.Diagnostics;
namespace hallawayApp;
public class Party
{
    private int partyID;
    private Person _organizer;
    public List<Person> _persons = new List<Person>();
    //private Menu _paryMenu = new Menu();
    private DatabaseActions _databaseActions;

    public Party(DatabaseActions databaseActions)
    {
        _databaseActions = databaseActions;
        partyID = CreateEmptyParty();
    }
    
    private int CreateEmptyParty()
    {
        int id = Task.Run(async () => await _databaseActions.AddEmptyParty()).Result;
        Console.WriteLine($"Empty party created with Party ID: {id}");
        return id;
    }
    
    private async Task AddPerson()
    {
        string message = "";
        if (_organizer == null)
        {
            message = "PartyOrganizer";
        }
        else
        {
            message = "PartyMember";
        }
        string name = AddPersonNameMenu(message);
        string phone = AddPersonNumberMenu(message);
        string email = AddPersonEmailMenu(message);
        DateTime date = AddPersonDoBMenu(message);
        List<Person> personsInDatabase = await _databaseActions.GetAllPersons();
        Person person = new Person(name, phone, email, date);
        if (PersonExists(person, personsInDatabase))
        {
            Console.WriteLine($"A person with the same details already exists in the database.");
            return; // Exit without adding the person
        }
        
        
        _persons.Add(person);
        int id = await _databaseActions.AddPersonToDataBase(person, partyID);
        if (_organizer == null)
        {
            _organizer = person;
            await _databaseActions.UpdatePartyOrganizer(partyID, id);
        }
        
        Console.WriteLine($"{name} was added to the party");
        //Add This to Database
    }
    private bool PersonExists(Person newPerson, List<Person> personsInDatabase)
    {
        foreach (var person in personsInDatabase)
        {
            if (person.name == newPerson.name &&
                person.phone == newPerson.phone &&
                person.email == newPerson.email &&
                person.dateOfBirth == newPerson.dateOfBirth)
            {
                return true; // The person already exists
            }
        }
        return false; // No matching person found
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

    private async Task DisplayAndSelectPersonFromDatabase(int partyID)
{
    List<Person> personsInDataBase = await _databaseActions.GetAllPersons();

    if (personsInDataBase.Count == 0)
    {
        Console.WriteLine("No persons found in the database.");
        return; // Exit if there are no persons
    }
    Console.Clear();
    Console.WriteLine($"Menu> OrderMenu> PartyMenu" +
                      $"\n---------------------------"); 

    // Split the persons into two columns
    int halfCount = (personsInDataBase.Count + 1) / 2;

    for (int i = 0; i < halfCount; i++)
    {
        string firstColumn = $"{i + 1}) {personsInDataBase[i].name}";
        string secondColumn = (i + halfCount < personsInDataBase.Count)
            ? $"{i + 1 + halfCount}) {personsInDataBase[i + halfCount].name}"
            : ""; // Handle the case when the number of persons is odd
        Console.WriteLine($"{firstColumn,-30} {secondColumn}");
    }

    Console.WriteLine("0) Cancel");

    while (true)
    {
        Console.Write($"\nEnter the Person that you want to add: ");
        string input = Console.ReadLine();

        if (!int.TryParse(input, out int choice))
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
            continue;
        }

        if (choice == 0)
        {
            Console.WriteLine("Selection canceled.");
            return; // Exit function on cancel
        }

        if (choice < 1 || choice > personsInDataBase.Count)
        {
            Console.WriteLine($"Invalid choice. Please select a number between 1 and {personsInDataBase.Count}, or 0 to cancel.");
            continue;
        }

        // Calculate the corresponding user_id
        int userId = choice; // Since the list is ordered by user_id and 1-based indexing is used
        Person selectedPerson = personsInDataBase[choice - 1];

        Console.WriteLine($"You selected: {selectedPerson.name}");
        if (PersonExists(selectedPerson, _persons))
        {
            Console.WriteLine($"A person with the same details already exists in the party.");
            return; // Exit without adding the person
        }
        _persons.Add(selectedPerson); // Add the selected person to the party list

        // Link the selected person to the current party in the PersonXParty table
        try
        {
            await _databaseActions.AddPersonXParty(userId, partyID);
            Console.WriteLine($"Person {selectedPerson.name} (ID: {userId}) successfully linked to Party {partyID}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error linking person to party: {ex.Message}");
        }

        break; // Exit the loop after a valid choice
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
    
    private async Task DeletePerson()
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
            int personID = await _databaseActions.GetPersonId(_persons[selectedIndex].name);
            await _databaseActions.RemovePersonFromParty(personID, partyID);
            _persons.RemoveAt(selectedIndex);
            Console.WriteLine("Person successfully deleted.");
        }
        else
        {
            Console.WriteLine("Deletion canceled.");
        }
    }

    public async Task PartyMenu()
    {
        bool running = true;
        while (running)
        {
            Console.Clear();
            Console.WriteLine(
                              $"Menu> OrderMenu> PartyMenu" +
                              $"\n---------------------------" +
                              $"\n1) Add new person to party \n2) Add Already Registered Person to party \n3) Delete person from party\n4) Done \n0) Quit");
            Console.WriteLine("\nEnter your choice: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number between 0 and 4.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    await AddPerson();
                    break;
                case 2:
                   await DisplayAndSelectPersonFromDatabase(partyID);
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
                    await _databaseActions.RemoveAllPersonsFromParty(partyID);
                    running = false; // Exit the menu loop
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose a valid menu option.");
                    break;
            }
        }
    }

    public string AddPersonNameMenu(string message)
    {
        Console.Clear();
        Console.WriteLine(
                          $"Menu> OrderMenu> PartyMenu> Add{message}" +                   
                          $"\n---------------------------" +               
                          $"\nEnter Firstname and Lastname name of the {message}: ");
        string input = Console.ReadLine();    
        Debug.Assert(input != null);
        return input;
    }

    public string AddPersonNumberMenu(string message)
    {
        Console.Clear();
        Console.WriteLine(
            $"Menu> OrderMenu> PartyMenu> Add{message}" +                   
                          $"\n---------------------------" +               
                          $"\nEnter Phone number of the {message}: ");   
        string input = Console.ReadLine();    
        Debug.Assert(input != null);
        return input;
    }
    
    public string AddPersonEmailMenu(string message)
    {
        Console.Clear();
        Console.WriteLine(
            $"Menu> OrderMenu> PartyMenu> Add{message}" +                   
                          $"\n---------------------------" +               
                          $"\nEnter Email of the {message}: ");   
        string input = Console.ReadLine();    
        Debug.Assert(input != null);
        return input;
    }
    
    public DateTime AddPersonDoBMenu(string message)
    {
        Console.Clear();
        Console.WriteLine(
            $"Menu> OrderMenu> PartyMenu> Add{message}" +                   
            $"\n---------------------------" +               
            $"\nEnter Date of Birth (e.g., 2000-12-31) of the {message}: "); 
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