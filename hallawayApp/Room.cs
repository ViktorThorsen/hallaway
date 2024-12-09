namespace hallawayApp;

public class Room
{
    public string room_name;
    public int RoomId;
    public double Price;
    public int Size;
    

    public Room( int roomId, string roomName, double price, int size)
    {
        RoomId = roomId;
        room_name = roomName;
        Price = price;
        Size = size;
    }
}