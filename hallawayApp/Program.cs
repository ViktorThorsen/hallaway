// See https://aka.ms/new-console-template for more information
using Npgsql;
using hallawayApp;

Console.WriteLine("Welcome! Add a party and Desitination to Create a order");

Database database = new Database();
DatabaseActions databaseActions = new DatabaseActions(database.Connection());
Menu mainMenu = new Menu(databaseActions);

