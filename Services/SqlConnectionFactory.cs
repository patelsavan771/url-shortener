using MySql.Data.MySqlClient;

public class SqlConnectionFactory
{
    private readonly string _conectionString;
    public SqlConnectionFactory(string connectionString)
    {
        _conectionString = connectionString;
    }
    public MySqlConnection Create()
    {
        return new MySqlConnection(_conectionString);
    }
}