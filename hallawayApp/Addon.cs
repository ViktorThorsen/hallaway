namespace hallawayApp;

public class Addon
{
    public int addonID;
    public string name;
    public string description;
    public double price;
 
    public Addon(int addon_id, string name, string description, double price)
    {
        this.addonID = addon_id;
        this.name = name;
        this.description = description;
        this.price = price;
    }
}