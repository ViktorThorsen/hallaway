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

    public async Task<Address> GetAddress(int locationId)
    {
        await using (var cmd = _db.CreateCommand("SELECT * FROM address WHERE location_id = $1"))
        {
            cmd.Parameters.AddWithValue(locationId);

            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                if (await reader.ReadAsync())
                {
                    Console.WriteLine($"{reader.GetString(1)}");
                    return new Address(reader.GetString(1), reader.GetString(2));
                }
            }
        }

        Console.WriteLine("RETURN NULL");
        return null;
    }

    public async Task<List<Room>> GetRoomsByHotelId(int hotelId)
    {
        var rooms = new List<Room>();
        const string query = "SELECT room_id, room_name, price, size FROM public.room WHERE hotel_id = $1";

        await using (var cmd = _db.CreateCommand(query))
        {
            cmd.Parameters.AddWithValue(hotelId);

            try
            {
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var room = new Room(
                            roomId: reader.GetInt32(reader.GetOrdinal("room_id")),
                            roomName: reader.GetString(reader.GetOrdinal("room_name")),
                            price: reader.GetDouble(reader.GetOrdinal("price")),
                            size: reader.GetInt32(reader.GetOrdinal("size"))
                        );

                        rooms.Add(room);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching rooms for hotel ID {hotelId}: {ex.Message}");
                throw;
            }
        }

        return rooms;
    }

    public async Task<List<Reservation>> GetReservationsForHotel(int hotelId)
    {
        var reservations = new List<Reservation>();
        const string query = @"
        SELECT room_id, start_date, end_date 
        FROM public.reservation 
        WHERE room_id IN (SELECT room_id FROM public.room WHERE hotel_id = $1)
          AND ((start_date >= '2024-12-01' AND start_date <= '2025-01-31') 
            OR (end_date >= '2024-12-01' AND end_date <= '2025-01-31'))";

        await using (var cmd = _db.CreateCommand(query))
        {
            cmd.Parameters.AddWithValue(hotelId);

            try
            {
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var reservation = new Reservation
                        {
                            RoomId = reader.GetInt32(reader.GetOrdinal("room_id")),
                            StartDate = reader.GetDateTime(reader.GetOrdinal("start_date")),
                            EndDate = reader.GetDateTime(reader.GetOrdinal("end_date"))
                        };

                        reservations.Add(reservation);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching reservations for hotel ID {hotelId}: {ex.Message}");
                throw;
            }
        }

        return reservations;
    }

    public async Task<int> AddReservation(Reservation reservation)
    {
        const string query = @"
    INSERT INTO public.reservation (room_id, start_date, end_date) 
    VALUES ($1, $2, $3) 
    RETURNING id";

        await using (var cmd = _db.CreateCommand(query))
        {
            cmd.Parameters.AddWithValue(reservation.RoomId);
            cmd.Parameters.AddWithValue(reservation.StartDate);
            cmd.Parameters.AddWithValue(reservation.EndDate);

            try
            {
                int reservationId = (int)await cmd.ExecuteScalarAsync();
                Console.WriteLine(
                    $"Reservation added successfully with ID {reservationId} for Room ID {reservation.RoomId}.");
                return reservationId;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding reservation: {ex.Message}");
                throw;
            }
        }
    }

    public async Task<List<Room>> GetRooms(int roomID)
    {
        List<Room> rooms = new List<Room>();

        await using (var cmd = _db.CreateCommand("SELECT * FROM public.room WHERE room_id = $1"))
        {
            cmd.Parameters.AddWithValue(roomID);

            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    Room room = new Room(
                        roomId: reader.GetInt32(0),
                        roomName: reader.GetString(4),
                        price: reader.GetDouble(1),
                        size: reader.GetInt32(2)
                    );

                    rooms.Add(room);
                }
            }
        }

        return rooms;
    }

    public async Task<int> AddEmptyParty()
    {
        try
        {
            await using (var cmd = _db.CreateCommand(
                             "INSERT INTO public.party (organizer_id) VALUES (NULL) RETURNING id"))
            {
                var partyId = (int)await cmd.ExecuteScalarAsync();
                Console.WriteLine($"Empty party created successfully. Party ID: {partyId}");
                return partyId;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating empty party: {ex.Message}");
            throw;
        }
    }

    public async Task UpdatePartyOrganizer(int partyId, int organizerId)
    {
        try
        {
            await using (var cmd = _db.CreateCommand(
                             "UPDATE public.party SET organizer_id = $1 WHERE id = $2"))
            {
                cmd.Parameters.AddWithValue(organizerId);
                cmd.Parameters.AddWithValue(partyId);

                int rowsAffected = await cmd.ExecuteNonQueryAsync();
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
            Console.WriteLine($"Error updating party organizer: {ex.Message}");
            throw;
        }
    }

    public async Task AddPersonXParty(int personId, int partyId)
    {
        try
        {
            await using (var cmd = _db.CreateCommand(
                             "INSERT INTO public.person_x_party (person_id, party_id) " +
                             "VALUES ($1, $2)"))
            {
                cmd.Parameters.AddWithValue(personId);
                cmd.Parameters.AddWithValue(partyId);
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
            Console.WriteLine(
                $"Inserting Person: Name={person.name}, Phone={person.phone}, Email={person.email}, DateOfBirth={person.dateOfBirth}, PartyID={partyId}");

            int personId;

            await using (var cmd = _db.CreateCommand(
                             "INSERT INTO public.person (name, phone, email, date_of_birth) " +
                             "VALUES ($1, $2, $3, $4) RETURNING user_id"))
            {
                cmd.Parameters.AddWithValue(person.name);
                cmd.Parameters.AddWithValue(person.phone);
                cmd.Parameters.AddWithValue(person.email);
                cmd.Parameters.AddWithValue(person.dateOfBirth);
                personId = (int)await cmd.ExecuteScalarAsync();

                Console.WriteLine($"{person.name} added to database successfully with ID: {personId}.");
            }

            await using (var cmd = _db.CreateCommand(
                             "INSERT INTO public.person_x_party (person_id, party_id) " +
                             "VALUES ($1, $2)"))
            {
                cmd.Parameters.AddWithValue(personId);
                cmd.Parameters.AddWithValue(partyId);

                await cmd.ExecuteNonQueryAsync();

                Console.WriteLine($"Person ID {personId} linked to Party ID {partyId} successfully.");
            }

            return personId;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding person {person.name}: {ex.Message}");
            throw;
        }
    }

    public async Task RemovePersonFromParty(int userId, int partyId)
    {
        try
        {
            await using (var cmd = _db.CreateCommand(
                             "DELETE FROM public.person_x_party WHERE person_id = $1 AND party_id = $2"))
            {
                cmd.Parameters.AddWithValue(userId);
                cmd.Parameters.AddWithValue(partyId);

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

    public async Task RemoveAllPersonsFromParty(int partyId)
    {
        try
        {
            await using (var cmd = _db.CreateCommand(
                             "DELETE FROM public.person_x_party WHERE party_id = $1"))
            {
                cmd.Parameters.AddWithValue(partyId);

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

    public async Task<List<Person>> GetAllPersons()
    {
        var persons = new List<Person>();

        try
        {
            await using (var cmd = _db.CreateCommand(
                             "SELECT user_id, name, phone, email, date_of_birth FROM public.person ORDER BY user_id"))
            {
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var person = new Person(
                            name: reader.GetString(reader.GetOrdinal("name")),
                            phone: reader.GetString(reader.GetOrdinal("phone")),
                            email: reader.GetString(reader.GetOrdinal("email")),
                            dateOfBirth: reader.GetDateTime(reader.GetOrdinal("date_of_birth"))
                        );

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

    public async Task<int> GetPersonId(Person person)
    {
        const string query = @"
        SELECT user_id 
        FROM public.person 
        WHERE name = @name AND phone = @phone AND email = @email AND date_of_birth = @dateOfBirth
        LIMIT 1";

        await using (var cmd = _db.CreateCommand(query))
        {
            cmd.Parameters.AddWithValue("@name", person.name);
            cmd.Parameters.AddWithValue("@phone", person.phone);
            cmd.Parameters.AddWithValue("@email", person.email);
            cmd.Parameters.AddWithValue("@dateOfBirth", person.dateOfBirth);

            try
            {
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return reader.GetInt32(0);
                    }
                    else
                    {
                        Console.WriteLine("No matching person found in the database.");
                        return -1;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching Person ID: {ex.Message}");
                throw;
            }
        }
    }



    public async Task<int> AddOrder(int partyId, int adminId, int hotelId, double totalPrice, int reservertion_id)
    {
        int orderId = -1;
        const string query = @"
        INSERT INTO public.order (party, admin, hotel, totalprice, reservation_id)
        VALUES ($1, $2, $3, $4, $5)
        RETURNING order_id";

        await using (var cmd = _db.CreateCommand(query))
        {
            cmd.Parameters.AddWithValue(partyId);
            cmd.Parameters.AddWithValue(adminId);
            cmd.Parameters.AddWithValue(hotelId);
            cmd.Parameters.AddWithValue(totalPrice);
            cmd.Parameters.AddWithValue(reservertion_id);
            try
            {
                orderId = (int)await cmd.ExecuteScalarAsync();
                Console.WriteLine($"Order added successfully. Order ID: {orderId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding order: {ex.Message}");
            }
        }

        return orderId;
    }

    public async void AddHotel(string name, Address address, bool pool, bool resturant, bool kidsClub, Rating rating,
        int distanceBeach, int distanceCityCenter, bool evningEntertainment)
    {
        int address_id = await GetAddressId(address.City, address.Street);
        await using (var cmd = _db.CreateCommand(
                         "INSERT INTO public.hotel (hotel_name, address, pool, resturant, kidsclub, rating, distancebeach, distancecitycenter, evningentertainment) VALUES ($1, $2, $3, $4, $5, $6, $7, $8, $9)"))
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

    public async Task<List<Hotel>> GetAllHotels()
    {

        var hotels = new List<Hotel>();

        try
        {
            await using (var cmd = _db.CreateCommand(
                             "SELECT hotel_id, hotel_name, address, pool,resturant,kidsclub, rating, distancebeach, " +
                             "distancecitycenter, eveningentertainment FROM hotel ORDER BY hotel_id"))
            {
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int hotelId = reader.GetInt32(reader.GetOrdinal("hotel_id"));
                        string hotelName = reader.GetString(reader.GetOrdinal("hotel_name"));
                        int addressId = reader.GetInt32(reader.GetOrdinal("address"));
                        bool hasPool = reader.GetBoolean(reader.GetOrdinal("pool"));
                        int dbRating = reader.GetInt32(reader.GetOrdinal("rating"));
                        bool restaurante = reader.GetBoolean(reader.GetOrdinal("resturant"));
                        bool kidsClub = reader.GetBoolean(reader.GetOrdinal("kidsclub"));
                        Rating rating = (Rating)dbRating;
                        int distancebeach = reader.GetInt32(reader.GetOrdinal("distancebeach"));
                        int distanceCityCenter = reader.GetInt32(reader.GetOrdinal("distancecitycenter"));
                        bool eveningEntertainment = reader.GetBoolean(reader.GetOrdinal("eveningentertainment"));
                        Address address = await GetAddress(addressId);
                        var rooms = await GetRooms(hotelId);

                        var hotel = new Hotel(
                            hotelId: hotelId,
                            hotelName: hotelName,
                            address: address,
                            pool: hasPool,
                            restaurante: restaurante,
                            kidsClub: kidsClub,
                            ratingEnum: rating,
                            distanceBeach: distancebeach,
                            distanceCityCenter: distanceCityCenter,
                            eveningEntertainment: eveningEntertainment,
                            roomList: rooms
                        );
                        hotels.Add(hotel);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while fetching hotels: {ex.Message}");
        }

        return hotels;
    }

    public async void AddRoom(double price, int size, bool avalible)
    {
        await using (var cmd = _db.CreateCommand(
                         " INSERT INTO public.room (price, size, isAvalible) VALUES ($1, $2, $3)"))
        {
            cmd.Parameters.AddWithValue(price);
            cmd.Parameters.AddWithValue(size);
            cmd.Parameters.AddWithValue(avalible);
            await cmd.ExecuteReaderAsync();
        }
    }

    public async void AddAddon(string name, string description, double price, int hotel_id)
    {
        await using (var cmd = _db.CreateCommand(
                         "INSERT INTO public.addon (name, description, price, hotel) VALUES ($1, $2, $3, $4)"))
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
        await using (var cmd = _db.CreateCommand(
                         "SELECT location_id FROM public.address WHERE city = $1 AND street = $2"))
        {
            cmd.Parameters.AddWithValue(city);
            cmd.Parameters.AddWithValue(street);
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                address_id = reader.GetInt32(0);
            }
        }


        return address_id;
    }

    public async Task<List<Admin>> GetAllAdmin()
    {
        var admins = new List<Admin>();
        await using (var cmd = _db.CreateCommand(
                         "SELECT admin_id, name, phone, email, date_of_birth FROM public.admin ORDER BY admin_id"))
        {
            await using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    var admin = new Admin(
                        Name: reader.GetString(reader.GetOrdinal("name")),
                        phone: reader.GetString(reader.GetOrdinal("phone")),
                        email: reader.GetString(reader.GetOrdinal("email")),
                        dateOfBirth: reader.GetDateTime(reader.GetOrdinal("date_of_birth"))
                    );
                    admins.Add(admin);
                }
            }
        }

        return admins;
    }

    public async Task<List<Addon>> GetAddons(int hotelId)
    {
        var addons = new List<Addon>();

        try
        {
            const string query = @"
            SELECT addon.addon_id, addon.name, addon.description, addon.price
            FROM addon
            JOIN addon_x_hotel ON addon.addon_id = addon_x_hotel.addon_id
            WHERE addon_x_hotel.hotel_id = @hotelId";

            await using (var cmd = _db.CreateCommand(query))
            {
                cmd.Parameters.AddWithValue("@hotelId", hotelId);
                await using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        int addonID = reader.GetInt32(reader.GetOrdinal("addon_id"));
                        string name = reader.GetString(reader.GetOrdinal("name"));
                        string description = reader.GetString(reader.GetOrdinal("description"));
                        double price = reader.GetDouble(reader.GetOrdinal("price"));

                        var addon = new Addon(
                            addon_id: addonID,
                            name: name,
                            description: description,
                            price: price
                        );

                        addons.Add(addon);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while fetching addons: {ex.Message}");
        }

        return addons;
    }

    public async Task AddtoAddonXOrder(List<Addon> addonList, int order_id)
    {
        foreach (Addon addon in addonList)
        {
            int addon_id = addon.addonID;

            try
            {
                await using (var cmd = _db.CreateCommand(
                                 "INSERT INTO public.addon_x_order (addon_id, order_id) " +
                                 "VALUES ($1, $2)"))
                {
                    cmd.Parameters.AddWithValue(addon_id);
                    cmd.Parameters.AddWithValue(order_id);

                    await cmd.ExecuteNonQueryAsync();

                    ;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error linking addon ID {addon_id} to order ID {order_id}: {ex.Message}");
                throw;
            }
        }
    }

    public async Task RemoveOrder(int orderID)
    {
        const string fetchDetailsQuery = @"
            SELECT reservation_id, party 
            FROM public.order 
            WHERE order_id = $1";

        const string deletePersonXPartyQuery = @"
            DELETE FROM public.person_x_party 
            WHERE party_id = $1";

        const string deleteAddonsQuery = @"
            DELETE FROM public.addon_x_order 
            WHERE order_id = $1";

        const string deleteOrderQuery = @"
            DELETE FROM public.order 
            WHERE order_id = $1";

        const string deleteReservationQuery = @"
            DELETE FROM public.reservation 
            WHERE id = $1";

        const string deletePartyQuery = @"
            DELETE FROM public.party 
            WHERE id = $1";

        await using (var fetchCmd = _db.CreateCommand(fetchDetailsQuery))
        {
            fetchCmd.Parameters.AddWithValue(orderID);

            try
            {
                int? reservationID = null;
                int? partyID = null;

                // Fetch reservation ID and party ID associated with the order
                await using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        reservationID = reader.IsDBNull(0) ? null : reader.GetInt32(0);
                        partyID = reader.IsDBNull(1) ? null : reader.GetInt32(1);
                    }
                    else
                    {
                        Console.WriteLine("Order not found.");
                        return;
                    }
                }

                // Delete addons associated with the order
                await using (var deleteAddonsCmd = _db.CreateCommand(deleteAddonsQuery))
                {
                    deleteAddonsCmd.Parameters.AddWithValue(orderID);
                    await deleteAddonsCmd.ExecuteNonQueryAsync();
                }

                // Delete the order
                await using (var deleteOrderCmd = _db.CreateCommand(deleteOrderQuery))
                {
                    deleteOrderCmd.Parameters.AddWithValue(orderID);
                    await deleteOrderCmd.ExecuteNonQueryAsync();
                }

                // Delete the reservation if it exists
                if (reservationID.HasValue)
                {
                    await using (var deleteReservationCmd = _db.CreateCommand(deleteReservationQuery))
                    {
                        deleteReservationCmd.Parameters.AddWithValue(reservationID.Value);
                        await deleteReservationCmd.ExecuteNonQueryAsync();
                    }
                }

                // Delete connections between persons and the party
                if (partyID.HasValue)
                {
                    await using (var deletePersonXPartyCmd = _db.CreateCommand(deletePersonXPartyQuery))
                    {
                        deletePersonXPartyCmd.Parameters.AddWithValue(partyID.Value);
                        await deletePersonXPartyCmd.ExecuteNonQueryAsync();
                    }
                }

                // Delete the party if it exists
                if (partyID.HasValue)
                {
                    await using (var deletePartyCmd = _db.CreateCommand(deletePartyQuery))
                    {
                        deletePartyCmd.Parameters.AddWithValue(partyID.Value);
                        await deletePartyCmd.ExecuteNonQueryAsync();
                    }
                }

                Console.WriteLine($"Order (ID: {orderID}) and all associated data were removed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while removing the order: {ex.Message}");
            }
        }
    }
    public async Task EditPartyByOrder(int orderID)
    {
        const string fetchPartyQuery = @"SELECT party FROM public.order WHERE order_id = $1";
        const string deletePersonXPartyQuery = @"DELETE FROM public.persons_x_party WHERE party_id = $1 AND person_id = $2";

        try
        {
            int partyID;
            await using (var fetchCmd = _db.CreateCommand(fetchPartyQuery))
            {
                fetchCmd.Parameters.AddWithValue(orderID);

                await using (var reader = await fetchCmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        partyID = reader.GetInt32(reader.GetOrdinal("party"));
                    }
                    else
                    {
                        throw new Exception("Order not found.");
                    }
                }
            }
            Console.WriteLine("Enter the person ID to be removed from the party:");
            if (int.TryParse(Console.ReadLine(), out int personID))
            {
                await using (var deleteCmd = _db.CreateCommand(deletePersonXPartyQuery))
                {
                    deleteCmd.Parameters.AddWithValue(partyID);
                    deleteCmd.Parameters.AddWithValue(personID);
                    await deleteCmd.ExecuteNonQueryAsync();
                }

                Console.WriteLine($"Person (ID: {personID}) removed from Party (ID: {partyID}) for Order (ID: {orderID}). \nPress Enter to continue...");
                Console.ReadLine();
            }
            else
            {
                Console.WriteLine("Invalid person ID. Please enter a valid number.");
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating party: {ex.Message} \nPress Enter to continue...");
            Console.ReadLine();
        }
    }
    
}
