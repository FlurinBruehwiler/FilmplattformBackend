﻿using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPITest.Models;
using WebAPITest.Models.DB;
using WebAPITest.Models.DTO;
using WebAPITest.Models.TMDB;
using WebAPITest.TmdbImports;

namespace WebAPITest.Controllers;

[ApiController]
[Route("[controller]")]
public class MoviesController : ControllerBase
{
    private readonly filmplattformContext _db;
    private readonly IHttpClientFactory _clientFactory;
    private readonly string _apiKey;
    private readonly MovieImporter _movieImporter;

    public MoviesController(filmplattformContext db, IHttpClientFactory clientFactory, IConfiguration configuration)
    {
        _db = db;
        _clientFactory = clientFactory;
        _apiKey = configuration.GetValue<string>("TmdbApiKey");
        _movieImporter = new MovieImporter(clientFactory, configuration, db);
    }
    
    [HttpGet("SearchMovies/{searchString}")]
    public async Task<ActionResult<TMDBMovieSearchResult>> SearchMovies(string searchString)
    {
        searchString = Regex.Replace(searchString, @"\s+", "+");
        var url = $"search/movie?api_key={_apiKey}&query={searchString}";
        
        TMDBMovieSearchResult? tmdbSearcher = null;
        var client = _clientFactory.CreateClient("tmdb");
        
        try
        {
            tmdbSearcher = await client.GetFromJsonAsync<TMDBMovieSearchResult>(url);
            
            if (tmdbSearcher == null)
            {
                throw new Exception();
            }
        }
        catch (Exception e)
        {
            return NotFound();
        }

        return tmdbSearcher;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DtoMovie>> GetMovie(int id)
    {
        if (!MovieExists(id))
        {
            await _movieImporter.AddMovieToDb(id);
        }

        var film = await _db.Films.FindAsync(id);

        if (film != null)
            return new DtoMovie(film);

        return NotFound();
    }

    private bool MovieExists(int id)
    {
        return _db.Films.Any(x => x.Id == id);
    }
}