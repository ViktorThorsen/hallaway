namespace hallawayApp;

public class Hotel
{
    public string hotelName;
    private Address address;
    private bool pool;
    private enum ratingEnum;

    private bool restaurante;
    private bool kidsClub;
    private int distanceBeach;
    private int distanceCityCenter;
    private bool eveningEntertainment;
    private List<Room> roomList;
    private List<Addon> addonList;
    
    public static List<Hotel> G;

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