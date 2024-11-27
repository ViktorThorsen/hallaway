namespace hallawayApp;

public class SQLcreater
{
    private string query;
    
    public void InsertPerson(Person person)//Add new person to DB
    {
        query = $"INSERT INTO Person (name, phone, email, date_of_birth) VALUES {person.name}, {person.phone}, {person.email}, {person.dateOfBirth}";
    }

    public void InsertParty(Party party)//Add new party to DB
    {
/*DB get*/         int organizerID = Convert.ToInt32($"SELECT MAX(user_id) FROM Person WHERE name = {party.organizer.name}");
        
        query = $"Insert INTO Party (organizer_id) VALUES {organizerID}";
        foreach (var person in party.persons )
        {
/*DB get*/            int partyID = Convert.ToInt32("SELECT MAX(id) FROM Party");
            query = $"UPDATE Person SET party_id = {partyID}";

        }
    }

    public void InsertAdmin(Admin admin)//Add new admin to DB
    {
        query = $"INSERT INTO Admin (name, phone, email, date_of_birth) VALUES {admin.name}, {admin.phone}, {admin.email}, {admin.dateOfBirth}";
    }

    public void InsertAddon(Addon addOn)
    {
        query = $"INSERT INTO AddON (name, description, price) VALUES {addOn.name}, {addOn.description}, {addOn.price}";
    }

    public void InsertAddress(Address address)
    {
        query = $"INSERT INTO Address (city, street) VALUES {address.City}, {address.Street}";
    }

    public void InsertRoom(Room room)
    {
        query = $"INSERT INTO Room (price, size) VALUES {room.price}, {room.size}";
    }

    public void InsertHotel(Hotel hotel)
    {
        query = $"INSERT INTO Hotel (hotel_name, address, pool, resturant, kidsclub, rating, distancebeach, distancecitycenter, evningentertainment) VALUES {hotel.hotelName}, /*hotel.address.id, only in DB*/ {hotel.pool} ,{hotel.resturante} ,{hotel.kidsClub} ,{hotel.rating} ,{hotel.distanceBeach} ,{hotel.distanceCityCenter} ,{hotel.eveningEntertainment}";
/*DB get*/   int hotelID = Convert.ToInt32("SELECT MAX(hotel_id) FROM Hotel");
        foreach (var room in hotel.roomList)
        {
            query = $"UPDATE Room SET hotel_id = {hotelID}";

        }
        foreach (var addOn in hotel.addonList )
        {
            query = $"UPDATE AddOn SET hotel = {hotelID}";

        }
    }

    public void InsertOrder(Order order)
    {
/*DB get*/         int partyID = Convert.ToInt32("SELECT MAX(id) FROM Party");//Cant ID Party
/*DB get*/         int adminID = Convert.ToInt32($"SELECT MAX(admin_id) FROM Admin WHERE name = {order.admin.name}");
/*DB get*/         int hotelID = Convert.ToInt32($"SELECT MAX(hotel_id) FROM Hotel WHERE hotel_name = {order.hotel.hotelName}");
        
        query = $"INSERT INTO Order (date, totalprice) VALUES {order.date}, {order.totalPrice}";
/*DB get*/         int orderID = Convert.ToInt32("SELECT MAX(order_id) FROM Order");
        foreach (var addOn in order.addonList)
        {
            query = $"UPDATE AddOn SET order = {orderID}";
        }
    }
    
}