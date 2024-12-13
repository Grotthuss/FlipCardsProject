namespace FlipCardProject.Models;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    
    public string Password { get; set; }
    public ICollection<FlipcardSet> FlipcardSets { get; set; }
}

public class LoginModel
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class RegisterModel
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
public class DeleteUserModel
{
    public int UserId { get; set; }
}