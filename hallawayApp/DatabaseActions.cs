using System.Data;
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
    public async Task<Hotel> GetHotel(string hotelName)
    {
        await using (var cmd = _db.CreateCommand($"SELECT * FROM Hotel WHERE hotel_name = {hotelName}"))
        {
            await using (var reader = await cmd.ExecuteReaderAsync())
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
        return null;
    }

    public async void AddParty(Person organizer)
    {
        int organizer_id = await GetPersonId(organizer.name);
        await using (var cmd = _db.CreateCommand("INSERT INTO PARTY (organizer, persons) VALUES ($1, $2)"))
        {
            cmd.Parameters.AddWithValue(organizer); // organizer int
            await cmd.ExecuteReaderAsync();
        }
    }

    public async void AddPerson(List<Person> persons)
    {
        await using (var cmd = _db.CreateCommand(
                         "INSERT INTO PERSON (name, phone, email, dateOfBirth) VALUES ($1, $2, $3, $4)"))
        {
            foreach (var person in persons)
            {
                cmd.Parameters.AddWithValue(person.name); // Adds name
                cmd.Parameters.AddWithValue(person.phone);  // Adds phone
                cmd.Parameters.AddWithValue(person.email); // Adds email
                cmd.Parameters.AddWithValue(person.dateOfBirth); // Adds date of birth
                await cmd.ExecuteReaderAsync();
            }
        }
    }

    public void AddOrder()
    {
        
    }
    
    //add new Hotel to DB
    public async void AddHotel(string name, Address address, bool pool, bool resturant, bool kidsClub, Rating rating, int distanceBeach, int distanceCityCenter, bool evningEntertainment)
    {
        int address_id = await GetAddressId(address.City, address.Street);
        await using (var cmd = _db.CreateCommand(
                         "INSERT INTO Hotel (hotel_name, address, pool, resturant, kidsclub, rating, distancebeach, distancecitycenter, evningentertainment) VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9)"))
        {
            cmd.Parameters.AddWithValue(name);
            cmd.Parameters.AddWithValue(address_id);
            cmd.Parameters.AddWithValue(pool);
            cmd.Parameters.AddWithValue(resturant);
            cmd.Parameters.AddWithValue(kidsClub);
            cmd.Parameters.AddWithValue(rating);
            cmd.Parameters.AddWithValue(distanceBeach);
            cmd.Parameters.AddWithValue(distanceCityCenter);
            cmd.Parameters.AddWithValue(evningEntertainment);
                
        }
    }
    //add new Room to DB
    public async void AddRoom(double price, int size, bool avalible)
    {
        await using (var cmd = _db.CreateCommand(" INSERT INTO Room (price, size, isAvalible) VALUES ($1, $2, $3)"))
        {
            cmd.Parameters.AddWithValue(price);
            cmd.Parameters.AddWithValue(size);
            cmd.Parameters.AddWithValue(avalible);
            await cmd.ExecuteReaderAsync();
        }
    }

    // Add new AddOn to DB, missing order_id
    public async void AddAddon(string name, string description, double price, Hotel hotel)
    {
        int hotel_id = (GetHotel(hotel.hotelName).Id);
        await using (var cmd = _db.CreateCommand(
                         "INSERT INTO AddON (name, description, price, hotel,) VALUES ($1, $2, $3, $4)"))
        {
            cmd.Parameters.AddWithValue(name);
            cmd.Parameters.AddWithValue(description);
            cmd.Parameters.AddWithValue(price);
            cmd.Parameters.AddWithValue(hotel_id);
            await cmd.ExecuteReaderAsync();
        }
    }

    public async Task<int> GetAddressId(string city, string street)
    {
        int address_id;
        await using(var cmd = _db.CreateCommand($"SELECT location_id FROM Address WHERE city = {city} AND wHERE street = {street}"))
        await using(var reader = await cmd.ExecuteReaderAsync())
        {
            address_id = reader.GetInt32(0);
        }
           
        return address_id;
    }
    
    public async Task<int> GetPersonId(string name)
        {
            int person_id;
            await using(var cmd = _db.CreateCommand($"SELECT user_id FROM Person WHERE name = {name}"))
            await using(var reader = await cmd.ExecuteReaderAsync())
            {
                person_id = reader.GetInt32(0);
            }
               
            return person_id;
        }
}