using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects {
    public class CaptureImage : BaseEntity {
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public int? CameraId { get; set; }
    }
}
