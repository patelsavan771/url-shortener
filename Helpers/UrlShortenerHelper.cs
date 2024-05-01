using Dapper;
using MySql.Data.MySqlClient;
using url_shortener.Models;

namespace url_shortener.Helpers;

public class UrlShortenerHelper
{
    private SqlConnectionFactory _sqlConnectionFactory;

    public UrlShortenerHelper(SqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory; 
    }
    public async Task<string> GenerateShortenUrl(string url)
    {
        UniqueCodeGenerator generator = new UniqueCodeGenerator(_sqlConnectionFactory);
        string code = await generator.GenerateAsync();
        string shortenUrl = "http://localhost:5225/" + code;

        // save code to db
        await SaveToDb(new ShortenUrl
        {
            id = Guid.NewGuid(),
            code = code,
            url = url,
            shortUrl = shortenUrl,
            createdOn = DateTime.Now
        });

        return shortenUrl;
    }

    internal async Task<string> GetFullUrlAsync(string code)
    {
        using (MySqlConnection connection = _sqlConnectionFactory.Create())
        {
            string query = "SELECT url FROM url_master WHERE code = @code";
            string? result = await connection.QueryFirstOrDefaultAsync<string>(query, new { code });

            if (result != null)
            {
                return result.ToString();
            }
        }
        return "";
    }

    private async Task SaveToDb(ShortenUrl shortenUrl)
    {
        // TODO: add int id instead of guid
        using (MySqlConnection connection = _sqlConnectionFactory.Create())
        {
            string query = "INSERT INTO url_master (id, code, url, shortUrl, createdOn) VALUES (@id, @code, @url, @shortUrl, @createdOn);";
            await connection.ExecuteAsync(query, shortenUrl);
        }
    }
}