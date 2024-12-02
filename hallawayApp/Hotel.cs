namespace hallawayApp;

public class Hotel
{
    public int hotelID;
    public string hotelName;
    public Address address;
    private bool pool;
    private Rating ratingEnum;
    public bool restaurante;
    public bool kidsClub;
    private int distanceBeach;
    public int distanceCityCenter;
    private bool eveningEntertainment;
    private List<Room> roomList;
    private List<Addon> addonList;
    
    public static List<Hotel> G;

    //Constructor Hotel
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