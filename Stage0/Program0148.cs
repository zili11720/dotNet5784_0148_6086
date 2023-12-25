using System;

namespace Stage0;
partial class Program
{
    static void Main(string[] args)
    {
        Welcome0148();
        Welcome6086();
        Console.ReadKey(); 
    }

    private static void Welcome0148()
    {
        Console.Write("Enter your name: ");
        string? username = Console.ReadLine();
        Console.WriteLine("{0}, welcome to my first console application", username);
    }

    static partial void Welcome6086();


}