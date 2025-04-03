using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkTea.Core.ViewModels
{
    public class UserBasicViewModel
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string? Email { get; set; }
        public string FullName { get; set; }
    }
}
