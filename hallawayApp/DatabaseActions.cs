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
    public async Task<Address> GetAddress(int locationId)
    {
        await using (var cmd = _db.CreateCommand("SELECT * FROM public.\"address\" WHERE \"location_id\" = @locationId"))
        {
            cmd.Parameters.AddWithValue("locationId", locationId);

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
    public async Task<List<Room>> GetRooms(int roomId)
    {
        List<Room> rooms = new List<Room>();

        // Correctly reference the "room_id" column
        await using (var cmd = _db.CreateCommand("SELECT * FROM public.\"room\" WHERE \"room_id\" = @roomId"))
        {
            // Add parameter for room_id
            cmd.Parameters.AddWithValue("roomId", roomId);

            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Room room = new Room(
                        reader.GetInt32(1),   // Room number
                        reader.GetDouble(2),  // Price
                        true
                    );

                    rooms.Add(room);
                }
            }
        }

        return rooms;
    }
    
    // Method that takes int value as id and returns a list with addons
    public async Task<List<Addon>> GetAddons(int addonId)
    {
        List<Addon> addons = new List<Addon>();
        await using (var cmd = _db.CreateCommand($"SELECT * FROM public.\"addon\" WHERE \"addon_id\" = @addonId"))
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
        // Use a parameterized query to avoid SQL injection
        await using (var cmd = _db.CreateCommand("SELECT * FROM public.\"hotel\" WHERE \"hotel_name\" = @hotelName"))
        {
            cmd.Parameters.AddWithValue("hotelName", hotelName); // Pass the hotel name safely

            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    // Extract column values
                    int addressId = reader.GetInt32(2); // Address ID
                    Address address = await GetAddress(addressId);

                    var rooms = await GetRooms(reader.GetInt32(0)); // Use hotel_id for room lookup

                    // Construct the Hotel object
                    Hotel hotel = new Hotel(
                        reader.GetString(1),  // hotel_name
                        address,              // Address object
                        reader.GetBoolean(3), // pool
                        reader.GetBoolean(4), // restaurant
                        reader.GetBoolean(5), // kidsClub
                        reader.GetInt32(7),   // number_of_rooms
                        reader.GetInt32(8),   // stars
                        reader.GetBoolean(9), // is_active
                        rooms                 // List<Room>
                    );

                    return hotel;
                }
            }
        }

        return null; // Return null if the hotel is not found
    }

    public async Task<int> AddEmptyParty()
    {
        try
        {
            // Insert a new empty party and return the auto-generated id
            await using (var cmd = _db.CreateCommand(
                             "INSERT INTO public.\"party\" (\"organizer_id\") VALUES (NULL) RETURNING \"id\""))
            {
                // Execute the command and get the generated id
                var partyId = (int)await cmd.ExecuteScalarAsync();
                Console.WriteLine($"Empty party created successfully. Party ID: {partyId}");
                return partyId;
            }
        }
        catch (Exception ex)
        {
            // Log the error
            Console.WriteLine($"Error creating empty party: {ex.Message}");
            throw;
        }
    }
    public async Task RemoveAllPersonsFromParty(int partyId)
    {
        try
        {
            // Delete all entries from person_X_party where the party_id matches
            await using (var cmd = _db.CreateCommand(
                             "DELETE FROM public.\"person_X_party\" WHERE \"party_id\" = @partyId"))
            {
                cmd.Parameters.AddWithValue("partyId", partyId);

                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Successfully removed {rowsAffected} persons linked to Party ID {partyId}.");
                }
                else
                {
                    Console.WriteLine($"No persons were linked to Party ID {partyId}.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing persons from party: {ex.Message}");
            throw;
        }
    }
    
    public async Task RemovePersonFromParty(int userId, int partyId)
    {
        try
        {
            // Delete the entry from person_X_party where the user_id and party_id match
            await using (var cmd = _db.CreateCommand(
                             "DELETE FROM public.\"person_X_party\" WHERE \"person_id\" = @userId AND \"party_id\" = @partyId"))
            {
                // Add parameters for the user_id and party_id
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.AddWithValue("partyId", partyId);

                // Execute the query
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Successfully removed user ID {userId} from Party ID {partyId}.");
                }
                else
                {
                    Console.WriteLine($"No connection found between user ID {userId} and Party ID {partyId}.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing user ID {userId} from Party ID {partyId}: {ex.Message}");
            throw;
        }
    }
    
    public async Task<List<(int UserId, string Name)>> GetPartyMembers(int partyId)
    {
        var partyMembers = new List<(int UserId, string Name)>();

        try
        {
            await using (var cmd = _db.CreateCommand(
                             "SELECT p.\"user_id\", p.\"name\" " +
                             "FROM public.\"person\" p " +
                             "INNER JOIN public.\"person_X_party\" pxp ON p.\"user_id\" = pxp.\"person_id\" " +
                             "WHERE pxp.\"party_id\" = @partyId"))
            {
                // Add parameter for party ID
                cmd.Parameters.AddWithValue("partyId", partyId);

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        // Get user_id and name from the query result
                        int userId = reader.GetInt32(reader.GetOrdinal("user_id"));
                        string name = reader.GetString(reader.GetOrdinal("name"));

                        // Add the user_id and name to the list
                        partyMembers.Add((userId, name));
                    }
                }
            }

            Console.WriteLine($"{partyMembers.Count} members found in Party ID {partyId}.");
            return partyMembers;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving party members: {ex.Message}");
            throw;
        }
    }
    
    public async Task UpdatePartyOrganizer(int partyId, int organizerId)
    {
        try
        {
            // Update the organizer for the given party ID
            await using (var cmd = _db.CreateCommand(
                             "UPDATE public.\"party\" SET \"organizer_id\" = @organizerId WHERE \"id\" = @partyId"))
            {
                // Add parameters for party ID and organizer ID
                cmd.Parameters.AddWithValue("partyId", partyId);
                cmd.Parameters.AddWithValue("organizerId", organizerId);

                // Execute the query
                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                // Confirm success
                if (rowsAffected > 0)
                {
                    Console.WriteLine($"Party {partyId} updated with organizer ID {organizerId}.");
                }
                else
                {
                    Console.WriteLine($"No party found with ID {partyId} to update.");
                }
            }
        }
        catch (Exception ex)
        {
            // Log any errors
            Console.WriteLine($"Error updating party organizer: {ex.Message}");
            throw;
        }
    }
    public async Task AddPersonXParty(int personId, int partyId)
    {
        try
        {
            await using (var cmd = _db.CreateCommand(
                             "INSERT INTO public.\"person_X_party\" (\"person_id\", \"party_id\") " +
                             "VALUES (@personId, @partyId)"))
            {
                // Add parameters
                cmd.Parameters.AddWithValue("personId", personId);
                cmd.Parameters.AddWithValue("partyId", partyId);

                // Execute the query
                await cmd.ExecuteNonQueryAsync();

                Console.WriteLine($"Person ID {personId} successfully linked to Party ID {partyId}.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error linking person ID {personId} to party ID {partyId}: {ex.Message}");
            throw;
        }
    }

    public async Task<int> AddPersonToDataBase(Person person, int partyId)
{
    try
    {
        // Debug log to check the person's data
        Console.WriteLine($"Inserting Person: Name={person.name}, Phone={person.phone}, Email={person.email}, DateOfBirth={person.dateOfBirth}, PartyID={partyId}");

        int personId;

        // Step 1: Insert the person into the "Person" table
        await using (var cmd = _db.CreateCommand(
                             "INSERT INTO public.\"person\" (\"name\", \"phone\", \"email\", \"date_of_birth\") " +
                             "VALUES (@name, @phone, @email, @dateOfBirth) RETURNING \"user_id\""))
        {
            // Add parameters
            cmd.Parameters.AddWithValue("name", person.name);
            cmd.Parameters.AddWithValue("phone", person.phone);
            cmd.Parameters.AddWithValue("email", person.email);
            cmd.Parameters.AddWithValue("dateOfBirth", person.dateOfBirth);

            // Execute the query and get the generated ID
            personId = (int)await cmd.ExecuteScalarAsync();

            // Debug log to confirm insertion
            Console.WriteLine($"{person.name} added to database successfully with ID: {personId}.");
        }

        // Step 2: Link the person to the party in the "PersonXParty" table
        await using (var cmd = _db.CreateCommand(
                             "INSERT INTO public.\"person_X_party\" (\"person_id\", \"party_id\") " +
                             "VALUES (@personId, @partyId)"))
        {
            // Add parameters
            cmd.Parameters.AddWithValue("personId", personId);
            cmd.Parameters.AddWithValue("partyId", partyId);

            // Execute the query
            await cmd.ExecuteNonQueryAsync();

            // Debug log to confirm the relationship
            Console.WriteLine($"Person ID {personId} linked to Party ID {partyId} successfully.");
        }

        return personId;
    }
    catch (Exception ex)
    {
        // Log any exceptions
        Console.WriteLine($"Error adding person {person.name}: {ex.Message}");
        throw;
    }
}
    public async Task<List<Person>> GetAllPersons()
    {
        var persons = new List<Person>();

        try
        {
            await using (var cmd = _db.CreateCommand(
                             "SELECT \"user_id\", \"name\", \"phone\", \"email\", \"date_of_birth\"" +
                             "FROM public.\"person\" ORDER BY \"user_id\""))
            {
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        // Create a Person object from the row data
                        var person = new Person(
                            name: reader.GetString(reader.GetOrdinal("name")),
                            phone: reader.GetString(reader.GetOrdinal("phone")),
                            email: reader.GetString(reader.GetOrdinal("email")),
                            dateOfBirth: reader.GetDateTime(reader.GetOrdinal("date_of_birth"))
                        );

                        // Add the person to the list
                        persons.Add(person);
                    }
                }
            }

            Console.WriteLine($"{persons.Count} persons retrieved from the database.");
            return persons;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving persons: {ex.Message}");
            throw;
        }
    }
    
    public async Task<int> GetPersonId(string name)
    {
        try
        {
            await using (var cmd = _db.CreateCommand("SELECT \"user_id\" FROM public.\"person\" WHERE \"name\" = @name"))
            {
                // Safely add the name parameter
                cmd.Parameters.AddWithValue("name", name);

                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync()) // Check if a row is available
                    {
                        // Get the user_id from the first column
                        return reader.GetInt32(0);
                    }
                    else
                    {
                        Console.WriteLine($"No person found with the name: {name}");
                        return -1; // Or throw an exception, depending on your use case
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving user_id for name '{name}': {ex.Message}");
            throw;
        }
    }

    

    public async Task AddOrder(int partyId, int adminId, int hotelId, DateTime orderDate, double totalPrice)
    {
        // Define the SQL query for inserting a new order
        await using (var cmd = _db.CreateCommand(
                         "INSERT INTO public.\"order\" (\"party\", \"admin\", \"hotel\", \"date\", \"totalprice\") " +
                         "VALUES (@partyId, @adminId, @hotelId, @orderDate, @totalPrice)"))
        {
            // Add parameters to the query
            cmd.Parameters.AddWithValue("partyId", partyId);       // Foreign key: Party ID
            cmd.Parameters.AddWithValue("adminId", adminId);       // Foreign key: Admin ID
            cmd.Parameters.AddWithValue("hotelId", hotelId);       // Foreign key: Hotel ID
            cmd.Parameters.AddWithValue("orderDate", orderDate);   // Date of the order
            cmd.Parameters.AddWithValue("totalPrice", totalPrice); // Total price of the order

            // Execute the command
            await cmd.ExecuteNonQueryAsync();
            Console.WriteLine("Order added successfully.");
        }
    }
    
    //add new Hotel to DB
    public async void AddHotel(string name, Address address, bool pool, bool resturant, bool kidsClub, Rating rating, int distanceBeach, int distanceCityCenter, bool evningEntertainment)
    {
        int address_id = await GetAddressId(address.City, address.Street);
        await using (var cmd = _db.CreateCommand(
                         "INSERT INTO hotel (hotel_name, address, pool, resturant, kidsclub, rating, distancebeach, distancecitycenter, evningentertainment) VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9)"))
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
        await using (var cmd = _db.CreateCommand(" INSERT INTO room (price, size, is_available) VALUES ($1, $2, $3)"))
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
                         "INSERT INTO addon (name, description, price, hotel,) VALUES ($1, $2, $3, $4)"))
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
        await using(var cmd = _db.CreateCommand($"SELECT location_id FROM address WHERE city = {city} AND wHERE street = {street}"))
        await using(var reader = await cmd.ExecuteReaderAsync())
        {
            address_id = reader.GetInt32(0);
        }
           
        return address_id;
    }
    
}