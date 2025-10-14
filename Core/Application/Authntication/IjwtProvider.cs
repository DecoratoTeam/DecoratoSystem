using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Authntication
{
    public interface IjwtProvider
    {
         (string Token,int ExpiersIn) GenerateJwtToken(User user);
    }
}
