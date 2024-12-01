// See https://aka.ms/new-console-template for more information
using Npgsql;
using hallawayApp;

Console.WriteLine("Welcome! Add a party and Desitination to Create a order");
Menu mainMenu = new Menu();
Database database = new Database();
DatabaseActions databaseActions = new DatabaseActions(database.Connection());
await mainMenu.CallMainMenu(databaseActions);
