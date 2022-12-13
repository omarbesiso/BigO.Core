using BigO.Core.Validation;

try
{
    ValidateMethod(null);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

Console.WriteLine(Environment.NewLine);
try
{
    ValidateProperty(null);
}
catch (Exception ex)
{
    Console.WriteLine(ex);
}

Console.WriteLine("Done");
Console.ReadKey();

static void ValidateMethod(string name)
{
    Guard.NotNull(name, exceptionMessage: "The person name cannot be null.");
}

static void ValidateProperty(string name)
{
    var p = new Person();
    p.FirstName = name;
}

internal class Person
{
    private string _firstName;

    public string FirstName
    {
        get => _firstName;
        set
        {
            PropertyGuard.NotNull(value, exceptionMessage: "The first name cannot be null.");
            _firstName = value;
        }
    }
}