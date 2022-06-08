using System;
using CreditNamespace;

public class Program
{
    public static Credit[] Filling(int size)
    {
        Credit[] dataBase = new Credit[size];
        for (int i = 0; i < size; i++)
            dataBase[i] = new Credit();
        
        return dataBase;
    }

    public static void Main()
    {
        Credit request = new Credit("req");
        Credit[] result = request.Search(Filling(50000));

        Console.WriteLine("Предложения, найденные по вашим условиям:\n");
        foreach (var elem in result)
            elem.PrintAll();
    }
}