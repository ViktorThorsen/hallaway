namespace hallawayApp;

public class AddOn
{
    public string name;
    public string description;
    public double price;

    public void Addon(string name, string description, double price)
    {
        this.name = name;
        this.description = description;
        this.price = price;
    }
}