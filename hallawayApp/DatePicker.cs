using System;

namespace hallawayApp
{
    public class Reservation
    {
        public (DateTime StartDate, DateTime EndDate) PickDateRange()
        {
            Console.WriteLine("Start Date (yyyy-mm-dd):");
            DateTime startDate = GetValidDate();

            Console.WriteLine("End Date (yyyy-mm-dd):");
            DateTime endDate = GetValidDate();
            
            while (endDate < startDate)
            {
                Console.WriteLine("End Date can't be earlier than Start Date:");
                endDate = GetValidDate();
            }

            return (startDate, endDate);
        }

        private DateTime GetValidDate()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (DateTime.TryParse(input, out DateTime date))
                {
                    return date;
                }
                Console.WriteLine("Ogiltigt datum. Försök igen (yyyy-mm-dd):");
            }
        }
    }
    namespace hallawayApp;

public class AddonManager
{
    private DatabaseActions _databaseActions;
    private List<Addon> addonList;

    public AddonManager(DatabaseActions databaseActions)
    {
        _databaseActions = databaseActions;
    }

    public async Task<List<Addon>> FindAddonMenu(int hotelId, List<Addon> existingAddons)
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
                $"\n1) Show all addons \n2) View selected addons \n3) Remove an addon \n0) Done");
            Console.WriteLine("\nEnter your choice: ");
            string input = Console.ReadLine();

            if (!int.TryParse(input, out int choice))
            {
                Console.WriteLine("Invalid input. Please enter a number between 0 and 3.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
                continue;
            }

            switch (choice)
            {
                case 1:
                    ShowAndSelectAddons(selectedAddons, existingAddons);
                    break;
                case 2:
                    ViewSelectedAddons(existingAddons);
                    break;
                case 3:
                    RemoveAddon(existingAddons);
                    break;
                case 0:
                    running = false;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please choose a valid menu option.");
                    Console.WriteLine("Press Enter to continue...");
                    Console.ReadLine();
                    break;
            }
        }

        return selectedAddons;
    }

    private void ShowAndSelectAddons(List<Addon> selectedAddons, List<Addon> existingAddons)
{
    while (true)
    {
        Console.Clear();

        if (!addonList.Any())
        {
            Console.WriteLine("No addons available at the selected hotel.");
            Console.WriteLine("Press Enter to return...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Available Addons:");
        for (int i = 0; i < addonList.Count; i++)
        {
            var addon = addonList[i];
            Console.WriteLine($"{i + 1}) {addon.name} - {addon.description} (${addon.price})");
        }

        Console.WriteLine("\nSelected Addons:");
        if (existingAddons.Any())
        {
            foreach (var addon in existingAddons)
            {
                Console.WriteLine($"- {addon.name} (${addon.price})");
            }
        }
        else
        {
            Console.WriteLine("No addons selected yet.");
        }

        Console.WriteLine("\nEnter the number of the addon to select it, or press Enter to return:");
        string input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            return;
        }

        if (!int.TryParse(input, out int addonNumber) || addonNumber < 1 || addonNumber > addonList.Count)
        {
            Console.WriteLine("Invalid selection. Please enter a valid number.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            continue;
        }
        
        var selectedAddon = addonList[addonNumber - 1];

        if (existingAddons.Contains(selectedAddon) || selectedAddons.Contains(selectedAddon))
        {
            Console.WriteLine("This addon is already selected.");
        }
        else
        {
            existingAddons.Add(selectedAddon);
            selectedAddons.Add(selectedAddon);
            Console.WriteLine($"Addon '{selectedAddon.name}' added to your selection.");
        }

        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}

    private void ViewSelectedAddons(List<Addon> existingAddons)
    {
        Console.Clear();

        if (!existingAddons.Any())
        {
            Console.WriteLine("No addons selected.");
        }
        else
        {
            Console.WriteLine("Selected Addons:");
            foreach (var addon in existingAddons)
            {
                Console.WriteLine($"{addon.name} - {addon.description} (${addon.price})");
            }
        }

        Console.WriteLine("Press Enter to return...");
        Console.ReadLine();
    }
    private void RemoveAddon(List<Addon> existingAddons)
    {
        Console.Clear();

        if (!existingAddons.Any())
        {
            Console.WriteLine("No addons selected to remove.");
            Console.WriteLine("Press Enter to return...");
            Console.ReadLine();
            return;
        }

        Console.WriteLine("Selected Addons:");
        for (int i = 0; i < existingAddons.Count; i++)
        {
            var addon = existingAddons[i];
            Console.WriteLine($"{i + 1}) {addon.name} - {addon.description} (${addon.price})");
        }

        Console.WriteLine("\nEnter the number of the addon to remove, or press Enter to return:");
        string input = Console.ReadLine();

        if (string.IsNullOrEmpty(input))
        {
            return;
        }

        if (!int.TryParse(input, out int addonNumber) || addonNumber < 1 || addonNumber > existingAddons.Count)
        {
            Console.WriteLine("Invalid selection. Please enter a valid number.");
            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
            return;
        }
        
        var removedAddon = existingAddons[addonNumber - 1];
        existingAddons.Remove(removedAddon);

        Console.WriteLine($"Addon '{removedAddon.name}' has been removed from your selection.");
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }
}
}