using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderImportWorker.Models
{
    public class OrderImportJobMessage
    {
        public long JobId { get; set; }
        public int CustomerId { get; set; }
        public string FileHash { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public string CreatedAt { get; set; } = string.Empty;
    }
}
