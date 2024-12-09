using System.Diagnostics;
namespace hallawayApp;
public class Party
{
    public int partyID;
    private Person _organizer;
    public List<Person> _persons = new List<Person>();
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
    
    public async Task PartyMenu()
{
    bool running = true;

    while (running)
    {
        Console.Clear();

        string organizerMessage = _organizer != null ? _organizer.name : "(NOT set)";
        string personsMessage = _persons.Count > 0 ? $"({_persons.Count} person(s))" : "(No members yet)";

        Console.WriteLine(
            $"Menu> OrderMenu> PartyMenu" +
            $"\n---------------------------" +
            $"\n1) Set Organizer {organizerMessage}" +
            $"\n2) Add Party Members {personsMessage}" +
            $"\n3) Remove a Party Member" +
            $"\n4) View Party Details" +
            $"\n5) Done" +
            $"\n0) Cancel and Clear Party");
        Console.WriteLine("\nEnter your choice: ");

        string input = Console.ReadLine();

        if (!int.TryParse(input, out int choice))
        {
            Console.WriteLine("Invalid input. Please enter a number between 0 and 5.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            continue;
        }

        switch (choice)
        {
            case 1:
                await SetOrganizer();
                break;

            case 2:
                if (_organizer == null)
                {
                    Console.WriteLine("You must set the organizer before adding party members.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
                else
                {
                    await AddPartyMembers();
                }
                break;

            case 3:
                if (_persons.Count > 0)
                {
                    await DeletePerson();
                }
                else
                {
                    Console.WriteLine("There are no party members to remove.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
                break;

            case 4:
                ViewPartyDetails();
                break;

            case 5:
                if (_organizer == null)
                {
                    Console.WriteLine("You must set the organizer before completing the party.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("Party setup is complete!");
                    running = false;
                }
                break;

            case 0:
                Console.WriteLine("Are you sure you want to cancel and clear the party? (yes/no)");
                string confirm = Console.ReadLine()?.ToLower();
                if (confirm == "yes")
                {
                    await _databaseActions.RemoveAllPersonsFromParty(partyID);
                    _persons.Clear();
                    _organizer = null;
                    Console.WriteLine("Party cleared. Returning to the previous menu.");
                    running = false;
                }
                else
                {
                    Console.WriteLine("Party was not cleared. Returning to the menu...");
                }
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                break;

            default:
                Console.WriteLine("Invalid option. Please choose a valid menu option.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                break;
        }
    }
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
            return;
        }
        
        
        _persons.Add(person);
        int id = await _databaseActions.AddPersonToDataBase(person, partyID);
        if (_organizer == null)
        {
            _organizer = person;
            await _databaseActions.UpdatePartyOrganizer(partyID, id);
        }
        
        Console.WriteLine($"{name} was added to the party");
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
                return true;
            }
        }
        return false;
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
        return;
    }
    Console.Clear();
    Console.WriteLine($"Menu> OrderMenu> PartyMenu" +
                      $"\n---------------------------"); 
    
    int halfCount = (personsInDataBase.Count + 1) / 2;

    for (int i = 0; i < halfCount; i++)
    {
        string firstColumn = $"{i + 1}) {personsInDataBase[i].name}";
        string secondColumn = (i + halfCount < personsInDataBase.Count)
            ? $"{i + 1 + halfCount}) {personsInDataBase[i + halfCount].name}"
            : "";
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
            return;
        }

        if (choice < 1 || choice > personsInDataBase.Count)
        {
            Console.WriteLine($"Invalid choice. Please select a number between 1 and {personsInDataBase.Count}, or 0 to cancel.");
            continue;
        }
        
        int userId = choice;
        Person selectedPerson = personsInDataBase[choice - 1];

        Console.WriteLine($"You selected: {selectedPerson.name}");
        if (PersonExists(selectedPerson, _persons))
        {
            Console.WriteLine($"A person with the same details already exists in the party.");
            return;
        }
        _persons.Add(selectedPerson);
        
        try
        {
            await _databaseActions.AddPersonXParty(userId, partyID);
            Console.WriteLine($"Person {selectedPerson.name} (ID: {userId}) successfully linked to Party {partyID}.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error linking person to party: {ex.Message}");
        }

        break;
    }
}

    private int SelectPersonMenu()
    {
        DisplayAllPersons();
        Console.WriteLine("\nEnter the number of the person (or 0 to cancel):");

        string input = Console.ReadLine();
        if (int.TryParse(input, out int selectedIndex) && selectedIndex >= 0 && selectedIndex <= _persons.Count)
        {
            return selectedIndex - 1;
        }
        else
        {
            Console.WriteLine("Invalid input. Please try again.");
            return -1;
        }
    }
    
    private async Task DeletePerson()
    {
        int selectedIndex = SelectPersonMenu();

        if (selectedIndex == -1) return;
        if (selectedIndex == -2)
        {
            Console.WriteLine("No person was deleted. Returning to the menu.");
            return;
        }

        Console.WriteLine($"Are you sure you want to delete {_persons[selectedIndex].name}? (yes/no)");
        string confirmation = Console.ReadLine();

        if (confirmation?.ToLower() == "yes")
        {
            int personID = await _databaseActions.GetPersonId(_persons[selectedIndex]);
            await _databaseActions.RemovePersonFromParty(personID, partyID);
            _persons.RemoveAt(selectedIndex);
            _organizer = null;
            Console.WriteLine("Person successfully deleted.");
        }
        else
        {
            Console.WriteLine("Deletion canceled.");
        }
    }
    
    private async Task SetOrganizer()
    {
        Console.Clear();
        Console.WriteLine("Set Organizer:");
        Console.WriteLine("1) Add a new person");
        Console.WriteLine("2) Select from registered persons");
        Console.WriteLine("Enter your choice: ");

        string input = Console.ReadLine();

        if (input == "1")
        {
            await AddPerson();
            if (_organizer == null && _persons.Count > 0)
            {
                _organizer = _persons.Last();
                await _databaseActions.UpdatePartyOrganizer(partyID, await _databaseActions.GetPersonId(_organizer));
            }
        }
        else if (input == "2")
        {
            Console.Clear();
            await DisplayAndSelectPersonFromDatabase(partyID);

            if (_persons.Count > 0 && _organizer == null)
            {
                _organizer = _persons.Last();
                await _databaseActions.UpdatePartyOrganizer(partyID, await _databaseActions.GetPersonId(_organizer));
            }
        }
        else
        {
            Console.WriteLine("Invalid choice. Returning to the party menu...");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }
    }
    private async Task AddPartyMembers()
    {
        Console.WriteLine("Would you like to add:");
        Console.WriteLine("1) A new person");
        Console.WriteLine("2) A registered person from the database");
        Console.Write("Enter your choice: ");
        string subInput = Console.ReadLine();

        if (subInput == "1")
        {
            await AddPerson();
        }
        else if (subInput == "2")
        {
            await DisplayAndSelectPersonFromDatabase(partyID);
        }
        else
        {
            Console.WriteLine("Invalid choice. Returning to the party menu...");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
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
        while (true)
        {
            string input = Console.ReadLine();
            if (DateTime.TryParse(input, out DateTime dateOfBirth))
            {
                return dateOfBirth;
            }
            else
            {
                Console.WriteLine("Invalid date format. Please try again (e.g., 2000-12-31):");
            }
        }
    }
    private void ViewPartyDetails()
    {
        Console.Clear();
        Console.WriteLine("Party Details:");
        Console.WriteLine("---------------------------");

        if (_organizer != null)
        {
            Console.WriteLine($"Organizer: {_organizer.name}");
        }
        else
        {
            Console.WriteLine("Organizer: Not set");
        }

        if (_persons.Count > 0)
        {
            Console.WriteLine("Party Members:");
            foreach (var person in _persons)
            {
                Console.WriteLine($"- {person.name} ({person.email})");
            }
        }
        else
        {
            Console.WriteLine("Party Members: None added yet.");
        }

        Console.WriteLine("\nPress Enter to return...");
        Console.ReadLine();
    }
}