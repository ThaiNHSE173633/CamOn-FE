using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects {
    public class Camera : BaseEntity {
        public string? Name { get; set; }
        public string? IpAddress { get; set; }
        public string? AccountId { get; set; }
        public ICollection<CaptureImage>? CaptureImages { get; set; }
        public ICollection<Notification>? Notifications { get; set; }
    }
}
