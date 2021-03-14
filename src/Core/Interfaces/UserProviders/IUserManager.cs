using Core.Dtos.CommonDtos;
using Core.Dtos.UserDtos;
using Core.Models.Manager;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Interfaces.UserProviders
{
    public interface IUserManager
    {
        Task<(List<UserGridDto> Items, int TotalCount)> GetAllAsync(UserGridDto dto);

        Task<UserUpdateDto> GetAsync(string id);

        Task<DeleteResultDto> DeleteAsync(string id);

        Task<ChangeStatusResultDto> ChangeStatusAsync(string id);

        Task<CommandResultDto> CreateAsync(UserCreateDto dto);

        Task<CommandResultDto> SetPasswordAsync(UserSetPasswordDto dto);

        Task<CommandResultDto> UpdateAsync(UserUpdateDto dto);

        Task<CommandResultDto> UpdateMyProfileAsync(Manager me, UserUpdateDto dto);

        Task<CommandResultDto> ChangeMyPasswordAsync(Manager me, UserChangePasswordDto dto);
    }
}
