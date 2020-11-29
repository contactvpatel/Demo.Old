using System;

namespace Demo.UI.ViewModels
{
    public class BaseViewModel
    {
        public DateTime Created { get; set; }
        public int CreatedBy { get; set; }
        public DateTime LastUpdated { get; set; }
        public int LastUpdatedBy { get; set; }
    }
}
