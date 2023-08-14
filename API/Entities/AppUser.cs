using Dapper.Contrib.Extensions;

namespace API.Entities;

// p'q Dapper sepa el nombre de la tabla, ya que buscaba a "AppUsers" xdefault
[Dapper.Contrib.Extensions.Table("Users")]
public class AppUser
{
    [Key]
    public int Id { get; set; }
    public string UserName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}
