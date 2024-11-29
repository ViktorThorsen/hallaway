using Npgsql;

namespace hallawayApp;

public class Database
{
    
    // Missing information about the database
    private readonly string _host = "";
    private readonly string _port = "";
    private readonly string _username = "";
    private readonly string _password = "";
    private readonly string _database = "";
    private readonly string _schema = "";

    private NpgsqlDataSource _connection;

    public NpgsqlDataSource Connection()
    {
        return _connection;
    }

    public Database()
    {
        _connection = NpgsqlDataSource.Create($"Host={_host};Port={{_port}};Username={{_username}};Password={{_password}};Database={{_database}}; Schema={{_schema}};");
        
        using var conn = _connection.OpenConnection(); // Checks if the connection is successful 
    }
}