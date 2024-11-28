using Npgsql;

namespace hallawayApp;

public class DatabaseActions
{
    NpgsqlDataSource _db;

    public DatabaseActions(NpgsqlDataSource db)
    {
        _db = db;
    }
    
    // Method that takes int value as id and returns the address as an object
    public async Task<Address> GetAddress(int id)
    {
        await using (var cmd = _db.CreateCommand($"SELECT * FROM Hotel.Address WHERE id = {id}"))
        {
            cmd.Parameters.AddWithValue(id); // Use parameterized query

            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync()) // Check if a row is available
                {
                    // Assuming Address takes two string parameters
                    return new Address(reader.GetString(1), reader.GetString(2));
                }
            }
        }
        // Return null if no row matches the ID
        return null;
    }
    
    // Method that takes int value as id and returns a list with rooms
    public async Task<List<Room>> GetRooms(int id)
    {
        List<Room> rooms = new List<Room>();
        await using (var cmd = _db.CreateCommand($"SELECT * FROM Room WHERE id = {id}"))
        await using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                Room room = new Room(reader.GetInt32(1), reader.GetDouble(2), reader.GetBoolean(3));
                rooms.Add(room);
            }
        }
        return rooms;
    }
    
    // Method that takes int value as id and returns a list with addons
    public async Task<List<Addon>> GetAddons(int id)
    {
        List<Addon> addons = new List<Addon>();
        await using (var cmd = _db.CreateCommand($"SELECT * FROM Addon WHERE id = {id}"))
        await using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                Addon addon = new Addon(reader.GetString(1), reader.GetString(2), reader.GetDouble(3));
                addons.Add(addon);
            }
        }
        return addons;
    }
    
    // Method that reads all the hotels from the database and returns them as objects
    public async Task<Hotel> GetHotel()
    {
        await using (var cmd = _db.CreateCommand("SELECT * FROM Hotel.Address WHERE id = $()"))
        {
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        // Address
                        int addressId = reader.GetInt32(2);
                        Address address = await GetAddress(addressId);

                        // Rooms
                        var rooms = await GetRooms(reader.GetInt32(0));

                        // Addons
                        var addons = await GetAddons(reader.GetInt32(9));

                        // Hotel constructor and add to list
                        Hotel hotel = new Hotel(reader.GetString(1), address, reader.GetBoolean(3),
                            reader.GetBoolean(4),
                            reader.GetBoolean(5), reader.GetInt32(6), reader.GetInt32(7),
                            reader.GetBoolean(8), rooms, addons);
                        return hotel; 
                    }
                }
            }
        }
        return null;
    }

    public void ShowAllPersons()
    {
        
    }

    public void GetOneHotel()
    {
        
    }

    public void GetOnePerson()
    {
        
    }
}