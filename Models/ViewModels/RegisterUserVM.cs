using Microsoft.AspNetCore.Http;
using Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class RegisterUserVM: Users
    {
        public string? PublisherName { get; set; }

        public IFormFile? uploadedImage { get; set; }
    }
}
