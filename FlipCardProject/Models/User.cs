namespace FlipCardProject.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public ICollection<FlipcardSet> FlipcardSets { get; set; }
}

public class Usert
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string whatevz { get; set; }
}


