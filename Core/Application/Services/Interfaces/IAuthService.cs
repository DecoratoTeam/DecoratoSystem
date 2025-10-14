using Application.Dtos;
using Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<GeneralResponseDto<bool>> RegisterAsync(RejesterDto rejesterDto,CancellationToken cancellationToken);
        Task<GeneralResponseDto<AuthDto>> LoginAsync(LoginDto loginDto,CancellationToken cancellationToken);
    }
}
