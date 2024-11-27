namespace hallawayApp;

public class Room
{
    public double price;
    public int size;
    private bool _isAvailable;
 
    private Room(double price, int size, bool isAvailable)
    {
        this.price = price;
        this.size = size;
        this._isAvailable = isAvailable;
    }
}