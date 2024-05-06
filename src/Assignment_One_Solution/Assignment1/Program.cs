using Assignment1;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        Student student = new Student();
        student.Id = 11808021;
        student.Name = "Shakil";

        Console.WriteLine(JsonFormatter.Convert(student));
    }
}

public class Student
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int[,] TwoDArray { get; set; }
    public int[,,] ThreeDArray { get; set; }
}