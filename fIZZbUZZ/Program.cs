using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public interface IFizzBuzzRule
{
    string ApplyRule(int number);
}

public class FizzRule : IFizzBuzzRule
{
    public string ApplyRule(int number)
    {
        return (number % 3 == 0) ? "Fizz" : string.Empty;
    }
}

public class BuzzRule : IFizzBuzzRule
{
    public string ApplyRule(int number)
    {
        return (number % 5 == 0) ? "Buzz" : string.Empty;
    }
}

public class FizzBuzzRule : IFizzBuzzRule
{
    public string ApplyRule(int number)
    {
        if (number % 3 == 0 && number % 5 == 0)
            return "FizzBuzz";
        return string.Empty;
    }
}

public class DefaultRule : IFizzBuzzRule
{
    public string ApplyRule(int number)
    {
        return (number % 3 != 0 && number % 5 != 0) ? number.ToString() : string.Empty;
    }
}

public class FizzBuzzService
{
    private readonly List<IFizzBuzzRule> _rules;

    public FizzBuzzService()
    {
        _rules = new List<IFizzBuzzRule>();

        // Discover and add rules using reflection
        var ruleTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => typeof(IFizzBuzzRule).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);
        foreach (var type in ruleTypes)
        {
            _rules.Add((IFizzBuzzRule)Activator.CreateInstance(type));
        }
    }

    public string GetOutput(int number)
    {
        var output = _rules.Aggregate(string.Empty, (current, rule) => current + rule.ApplyRule(number));
        return string.IsNullOrEmpty(output) ? number.ToString() : output;
    }
}

public class Program
{
    public static void Main(string[] args)
    {
        var fizzBuzzService = new FizzBuzzService();
        for (int i = 1; i <= 100; i++)
        {
            Console.WriteLine(fizzBuzzService.GetOutput(i));
        }
    }
}
