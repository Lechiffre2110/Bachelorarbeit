using Unity.VisualScripting;

public class Location {

    //Location has Name, Description, Address, Phone, Website, Longitude, Latitude
    public string Name { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public string Phone { get; set; }
    public string Website { get; set; }
    public float Longitude { get; set; }
    public float Latitude { get; set; }
    public float Distance { get; set; }

    public Location(string name, string description, string address, string phone, string website, float longitude, float latitude)
    {
        Name = name;
        Description = description;
        Address = address;
        Phone = phone;
        Website = website;
        Longitude = longitude;
        Latitude = latitude;
        Distance = float.MaxValue;
    }
}