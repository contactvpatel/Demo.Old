using System;

namespace Demo.Api.Models
{
    public class BaseApiModel
    {
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastUpdated { get; set; }
        public int LastUpdatedBy { get; set; }
    }
}
