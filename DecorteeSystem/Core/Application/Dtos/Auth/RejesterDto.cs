using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Auth
{
    public record  RejesterDto(string Name, string UserName, string Password, string Phone, string Email);
   
}
