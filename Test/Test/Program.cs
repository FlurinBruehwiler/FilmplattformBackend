// See https://aka.ms/new-console-template for more information

using Test;

Console.WriteLine("Hello, World!");

List<(Person, string)> people = new();

foreach (var i in Enumerable.Range(0,5))
{
    people.Add((new Person(), "test"));
}




var test = people;

