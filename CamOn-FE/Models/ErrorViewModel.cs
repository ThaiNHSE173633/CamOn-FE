using BusinessObjects;

namespace CamOn_FE.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
    public class AdminAssignPackageViewModel
    {
        public List<Account> Users { get; set; }
        public List<Package> Packages { get; set; }

        public string SelectedUserId { get; set; }
        public int SelectedPackageId { get; set; }
    }
    public class AdminDashboardViewModel
    {
        public Dictionary<string, int> PackagesBoughtPerMonth { get; set; }
    }

}