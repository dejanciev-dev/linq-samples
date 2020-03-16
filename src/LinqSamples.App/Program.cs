using LinqSamples.App.Ordering;
using System;
using LinqSamples.Common.Extensions;

namespace LinqSamples.App
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("=============== Original list ================");
            Console.WriteLine(LinqOrdering.models.ConvertToString());
            Console.WriteLine("=============== Order by specific values list ================");
            Console.WriteLine(LinqOrdering.LinqOrderBySpecificValues().ConvertToString());
            Console.ReadLine();
        }
    }
}