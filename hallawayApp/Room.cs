namespace hallawayApp;

public class Room
{
    private int _price;
    private double _size;
    private bool _isAvailable;
 
    private Room(int price, double size, bool isAvailable)
    {
        this._price = price;
        this._size = size;
        this._isAvailable = isAvailable;
    }
}