﻿
using Npgsql;
using hallawayApp;

Console.WriteLine("Welcome! Add a party and Desitination to Create a order");

Database database = new Database();
DatabaseActions databaseActions = new DatabaseActions(database.Connection());
Menu mainMenu = new Menu(databaseActions);
int admin_Id = await mainMenu.ShowAdminMenu();
if (admin_Id == 0)
{
    Console.Clear();
}
else
{
    await mainMenu.ShowMainMenu(1);//admin_Id);
}


