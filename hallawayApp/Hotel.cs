using Npgsql;

namespace hallawayApp;

public class Hotel
{
    public string hotelName;
    public Address address;
    public bool pool;
    private enum ratingEnum;

    public bool restaurante;
    public bool kidsClub;
    public int distanceBeach;
    public int distanceCityCenter;
    public bool eveningEntertainment;
    private List<Room> roomList;
    private List<Addon> addonList;

  
  
    

    //Constructor Hotel
    public Hotel(string hotelName, Address address, bool pool, bool restaurante, bool kidsClub,
        int distanceBeach, int distanceCityCenter, bool eveningEntertainment, List<Room> roomList,
        List<Addon> addonList)
    {
        this.hotelName = hotelName;
        this.address = address;
        
        this.pool = pool;
        this.restaurante = restaurante;
        this.kidsClub = kidsClub;
        this.distanceBeach = distanceBeach;
        this.distanceCityCenter = distanceCityCenter;
        this.eveningEntertainment = eveningEntertainment;
        this.roomList = roomList;
        this.addonList = addonList;
        
    }
    

    
    
    
}