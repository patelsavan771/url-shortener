namespace url_shortener.Helpers;

using System.Threading.Tasks;
using Dapper;
using MySql.Data.MySqlClient;

public class UniqueCodeGenerator
{
    public const int Length = 6;
    public const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    private static readonly Random random = new();
    private SqlConnectionFactory _sqlConnectionFactory;

    public UniqueCodeGenerator(SqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory; 
    }

    public async Task<string> GenerateAsync()
    {
        char[] codeArr = new char[Length];
        while (true)
        {
            //generate unique code
            for (int i = 0; i < Length; i++)
            {
                int randomIndex = random.Next(Alphabet.Length);
                codeArr[i] = Alphabet[randomIndex];
            }

            string code = new string(codeArr);
            if (await IsUniqueAsync(code))
            {
                return code;
            }
        }
    }

    private async Task<bool> IsUniqueAsync(string code)
    {
        using (MySqlConnection connection = _sqlConnectionFactory.Create())
        {
            string query = "SELECT id FROM url_master where code = @code;";

            string? id = await connection.QueryFirstOrDefaultAsync<string>(query, new {code});
            if(id is null)
            {
                return true;
            }
            return false;
        }
    }
}