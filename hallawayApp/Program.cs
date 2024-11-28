// See https://aka.ms/new-console-template for more information
using Npgsql;
using hallawayApp;

Console.WriteLine("Welcome! Add a party and Desitination to Create a order");
Menu mainMenu = new Menu();
Database dataBase = new Database();
NpgsqlDataSource dataBaseConnection = dataBase.Connection();
DatabaseActions databaseActions = new DatabaseActions(dataBaseConnection);
mainMenu.CallMainMenu(databaseActions);
