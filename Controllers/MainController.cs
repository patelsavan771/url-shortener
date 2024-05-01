using System.Data;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using MySql.Data.MySqlClient;
using url_shortener.Helpers;

namespace url_shortener.Controllers;

[ApiController]
[Route("")]
public class MainController : ControllerBase
{
    protected SqlConnectionFactory _sqlConnectionFactory;
    public MainController(SqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    // add new entry
    [HttpGet("shorten")]
    public async Task<ActionResult> Shorten([FromQuery] string url)
    {
        if (url is null) return NotFound();
        UrlShortenerHelper helper = new UrlShortenerHelper(_sqlConnectionFactory);
        return Ok(await helper.GenerateShortenUrl(url));
    }

    // redirect user
    [HttpGet("{code}")]
    public async Task<IResult> Post(string code)
    {
        UrlShortenerHelper helper = new UrlShortenerHelper(_sqlConnectionFactory);
        string url = await helper.GetFullUrlAsync(code);
        if (url == "" || url == null)
        {
            Results.NotFound();
        }
        return Results.Redirect(url);
    }
}