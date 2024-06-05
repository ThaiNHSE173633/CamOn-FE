
using Microsoft.AspNetCore.Identity;

namespace BusinessObjects;

public class Account : IdentityUser
{
    public ICollection<Camera>? Cameras { get; set; }
}