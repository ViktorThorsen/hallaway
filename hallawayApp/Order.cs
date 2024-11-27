namespace hallawayApp;

public class Order
{
    private string orderName;
    private Party party;
    private Admin admin;
    private Hotel hotel;
    private DateTime date;
    private double totalPrice;
    private List<Addon> addonList;

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