namespace hallawayApp;

public class Addon
{
    public int addonID;
    public string name;
    public string description;
    public double price;
 
    public Addon(string name, string description, double price)
    {
        this.name = name;
        this.description = description;
        this.price = price;
    }
}