namespace hallawayApp;

public class Room
{
    private double _price;
    private int _size;
    private bool _isAvailable;
 
    public Room(double price, int size, bool isAvailable)
    {
        this._price = price;
        this._size = size;
        this._isAvailable = isAvailable;
    }
}