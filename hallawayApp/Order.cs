﻿namespace hallawayApp;
using System.Diagnostics;
public class Order
{
    private string orderName;
    private Party party = new Party();
    private Admin admin;
    private Hotel hotel;
    private DateTime date;
    private double totalPrice;
    private List<Addon> addonList;

    public Order()
    {
        CallCreateOrder();
    }

    public void CallCreateOrder()
    {
        bool running = true;

        while (running){
        Console.WriteLine(
                          $"===========================" +
                          $"===========================" + 
                          $"\n1) Manage party " +
                          $"\n2) Set date " +
                          $"\n3) Select destination " +
                          $"\n4) View details " +
                          $"\n5) Done " +
                          $"\n0) Quit");
        int input = Int32.Parse(Console.ReadLine());
        Debug.Assert(input != null);

        switch (input)
        {
            case 1:
                party.AddPartyMenu();
                break;
            case 2:
                // Pick a date
                break;
            case 3:
                
                break;
            case 4:
                ShowOrderDetails();
                break;
            case 5:
                running = false;
                break;
            case 0:
                running = false;
                Console.WriteLine("Goodbye!"); // Quit!
                break;
            default:
                break;
        }}
    }

    public void ShowOrderDetails()
    {
        Console.WriteLine($"===========================");
        foreach (Person person in party._persons)
        {
            Console.WriteLine($"{person.name}");
        }
        Console.WriteLine($"Start Date: ");
        Console.WriteLine($"Destination: ");
    }
    
}