
using Microsoft.AspNetCore.Identity;

namespace BusinessObjects;

public class Account : IdentityUser
{
    public ICollection<Camera>? Cameras { get; set; }
    public ICollection<UserPackage>? UserPackages { get; set; }
    public ICollection<PackageTransaction>? Transactions { get; set; }
    public ICollection<Notification>? Notifications { get; set; }
}