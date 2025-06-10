namespace baseballAPI.Models;

public class Country
{
    public int CountryId { get; set; } 
    public string? CountryName { get; set; } 

    public Country(int id, string name)
    {
        CountryId = id;
        CountryName = name;
    }
    
    public Country() { }
}