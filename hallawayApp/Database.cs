using Npgsql;

namespace hallawayApp;

public class Database
{
    
    // Missing information about the database
    private readonly string _host = "localhost";
    private readonly string _port = "5432";
    private readonly string _username = "postgres";
    private readonly string _password = "admin132";
    private readonly string _database = "hallawaydatabase";

    private NpgsqlDataSource _connection;

    public NpgsqlDataSource Connection()
    {
        return _connection;
    }

    public Database()
    {
        _connection = NpgsqlDataSource.Create($"Host={_host};Port={_port};Username={_username};Password={_password};Database={_database}");
        
        using var conn = _connection.OpenConnection(); // Checks if the connection is successful 
    }
}