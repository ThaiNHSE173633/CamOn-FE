using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects {
    public class Package : BaseEntity {
        public string? Name { get; set; }
        public int MonthValue { get; set; }
        public int CameraValue { get; set; }
        public double? Price { get; set; }
    }

    public class UserPackage
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int PackageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual Account User { get; set; }
        public virtual Package Package { get; set; }
    }

    public class PackageTransaction : BaseEntity {
        public string? TransactionId { get; set; }
        public string? AccountId { get; set; }
        public int? PackageId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual Account User { get; set; }
    }
}
