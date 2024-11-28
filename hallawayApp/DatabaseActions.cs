using Npgsql;

namespace hallawayApp;

public class DatabaseActions
{
    NpgsqlDataSource _db;

    public DatabaseActions(NpgsqlDataSource db)
    {
        _db = db;
    }
    
    public async void ShowAllHotels()
    {
        await using (var cmd = _db.CreateCommand("SELECT * FROM Hotel"))
        await using (var reader = await cmd.ExecuteReaderAsync())
        {
            while (await reader.ReadAsync())
            {
                // Hotel constructor and add to list
            }
        }
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