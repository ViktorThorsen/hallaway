namespace hallawayApp;

public class Hotel
{
    public int hotelID;
    public string hotelName;
    public Address address;
    public bool pool;
    public Rating ratingEnum;
    public bool restaurante;
    public bool kidsClub;
    public int distanceBeach;
    public int distanceCityCenter;
    public bool eveningEntertainment;
    public List<Room> roomList;
    private List<Addon> addonList;
    
    public Hotel(int hotelId, string hotelName, Address address, bool pool,Rating ratingEnum, bool restaurante, bool kidsClub,
        int distanceBeach, int distanceCityCenter, bool eveningEntertainment, List<Room> roomList)
    {
        this.hotelID = hotelId;
        this.hotelName = hotelName;
        this.address = address;
        
        this.pool = pool;
        this.ratingEnum = ratingEnum;
        this.restaurante = restaurante;
        this.kidsClub = kidsClub;
        this.distanceBeach = distanceBeach;
        this.distanceCityCenter = distanceCityCenter;
        this.eveningEntertainment = eveningEntertainment;
        this.roomList = roomList;
        this.addonList = addonList;
        
        Console.Clear();
        SelectDestinationMenu();
    }
    

    private void SelectDestinationMenu()
    {
       
        
    }
    
    
    private void GetHotel()
    {
        
    }
}