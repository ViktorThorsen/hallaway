namespace hallawayApp;

public class Order
{
    private string orderName;
    public Party party;
    public Admin admin;
    public Hotel hotel;
    public DateTime date;
    public double totalPrice;
    public List<Addon> addonList;

    public Order(Party party, Admin admin)
    {
        this.party = party;
        this.admin = admin;
        
    }

    private void AddHotel()
    {
        
    }

    private void AddParty()
    {
        
    }

    private void CalculatePrice()
    {
        
    }
    
}