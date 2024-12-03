namespace hallawayApp;

public class Room
{
    public double _price;
    public int _size;
    public bool _isAvailable;
 
    public Room(double price, int size, bool isAvailable)
    {
        this._price = price;
        this._size = size;
        this._isAvailable = isAvailable;
    }
}