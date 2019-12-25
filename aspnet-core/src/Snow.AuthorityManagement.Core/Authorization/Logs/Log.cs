using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Anc.Domain.Entities;

namespace Snow.AuthorityManagement.Core.Authorization.Logs
{
    public class Log : Entity<long>
    {
        public string Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
        public string Properties { get; set; }
        public DateTime CreationTime { get; set; }
    }
}