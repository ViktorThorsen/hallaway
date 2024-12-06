namespace hallawayApp;

public class AddonManager
{
    private DatabaseActions _databaseActions;
    private List<Addon> addonList;

    public AddonManager(DatabaseActions databaseActions)
    {
        _databaseActions = databaseActions;
    }

    public async Task<List<Addon>> FindAddonMenu(int hotelId)
    {
        bool running = true;
        addonList = await _databaseActions.GetAddons(hotelId);
        List<Addon> selectedAddons = new List<Addon>();

        while (running)
        {
            Console.Clear();
            Console.WriteLine(
                $"Menu> OrderMenu> AddonMenu" +
                $"\n---------------------------" +
                $"\n1) Show all addons \n0) Return");
            Console.WriteLine("\nEnter your choice: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number between 0 and 1.");
                continue;
            }

            switch (choice)
            {
                case 1:
                    selectedAddons = ShowAddons();
                    if (selectedAddons != null)
                    {
                        return selectedAddons;
                    }
                    break;
                case 0:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose a valid menu option.");
                    break;
            }
        }
        return null; // Return null if no hotel is selected
    }

    private List<Addon> ShowAddons()
    {
        Console.Clear();
        
        if (!addonList.Any())
        {
            Console.WriteLine("No addons available at selected hotel");
            return null;
        }
        foreach (var addon in addonList)
        {
            Console.WriteLine($"{addon.addonID}) Addon: {addon.name}\nDescription: {addon.description} Price: {addon.price}");
        }
        Console.WriteLine("\nEnter addonID or leave empty to return: "); 
        string input = Console.ReadLine();
        if (!int.TryParse(input, out int id))
        {
            Console.WriteLine("Invalid input. Please enter a valid number."); 
            return null;
        }
        
        List<Addon> selectedAddons = new List<Addon>();
        var selectedAddon = addonList.FirstOrDefault(addon => addon.addonID == id); 
        selectedAddons.Add(selectedAddon);
        
        if (selectedAddon == null)
        {
            Console.WriteLine($"No addons corresponded with given input: {input}.");
            Console.ReadLine();
        }
        else
        {
            selectedAddons.Add(selectedAddon);
        }

        return selectedAddons;
    }
}