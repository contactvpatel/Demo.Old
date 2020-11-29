using System;

namespace Demo.Application.Models.Base
{
    public class BaseModel
    {
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastUpdated { get; set; }
        public int LastUpdatedBy { get; set; }
    }
}
