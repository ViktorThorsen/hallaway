namespace hallawayApp;

public class SQLcreater
{
    private string query;
    
    public void InsertPerson(Person person)//Add new person to DB
    {
        query = $"INSERT INTO Person VALUES {person.name}, {person.phone}, {person.email}, {person.dateOfBirth}";
    }

    public void InsertParty(Party party)//Add new party to DB
    {
        query = $"Insert INTO Party VALUES {party._organizer}";
        foreach (var person in party._persons )
        {
            int partyID = Convert.ToInt32("SELECT MAX(id) FROM Party");
            query = $"UPDATE Person SET party_id = {partyID}";

        }
    }

    public void InsertAdmin(Admin admin)//Add new admin to DB
    {
        query = $"INSERT INTO Admin VALUES {admin.name}, {admin.phone}, {admin.email}, {admin.dateOfBirth}";
    }
}