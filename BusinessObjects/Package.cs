using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects {
    public class Package : BaseEntity {
        public string? Name { get; set; }
        public int MonthValue { get; set; }
        public double? Price { get; set; }
    }

    public class PackageTransaction : BaseEntity {
        public string? TransactionId { get; set; }
        public string? AccountId { get; set; }
        public int? PackageId { get; set; }
    }
}
