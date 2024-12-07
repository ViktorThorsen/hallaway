namespace hallawayApp;

public class Room
{
    public int RoomId { get; set; } // Unique identifier for the room
    public double Price { get; set; }
    public int Size { get; set; }
    

    public Room(int roomId, double price, int size)
    {
        RoomId = roomId;
        Price = price;
        Size = size;
    }
}