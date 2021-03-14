using Core.Dtos.CommonDtos;
using Core.Dtos.Settings;
using Core.Dtos.UserDtos;
using Core.FilterAttributes;
using Core.Helpers;
using Core.Interfaces.UserProviders;
using Core.Models.Manager;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Services.UserProviders
{
    public class UserManager : IUserManager
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<Manager> _userManager;
        private readonly SignInManager<Manager> _signInManager;
        private readonly PanelAppSettings _appSettings;
        public UserManager(
            ApplicationDbContext dbContext,
            UserManager<Manager> userManager,
            SignInManager<Manager> signInManager,
            IOptions<PanelAppSettings> appSettings)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _appSettings = appSettings.Value;
        }

        public async Task<(List<UserGridDto> Items, int TotalCount)> GetAllAsync(UserGridDto dto)
        {
            var query = _dbContext.Users.Where(u=> !u.Deleted);
            if (dto.PageOrder != null)
            {
                query = query.OrderByField(dto.PageOrderBy, dto.PageOrder.ToLower() == "asc");
            }
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.UserName), c => c.UserName.Contains(dto.UserName.Trim()));
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.Email), c => c.Email.Contains(dto.Email.Trim()));
            query = query.WhereIf(!string.IsNullOrWhiteSpace(dto.PhoneNumber), c => c.PhoneNumber.Contains(dto.PhoneNumber.Trim()));
            query = query.WhereIf(dto.Gender.HasValue, c => c.Gender == dto.Gender);
            query = query.WhereIf(dto.IsActive == true, c => !c.LockoutEnd.HasValue);
            query = query.WhereIf(dto.IsActive == false, c => c.LockoutEnd.HasValue);

            var totalCount = query.Count();

            var take = _appSettings.PageSize;
            var skip = (dto.ThisPageIndex - 1) * take;
            query = query.Skip(skip).Take(take);
            var items = await query.Select(user => new UserGridDto
            {
                Id = user.Id,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName,
                IsActive = !user.LockoutEnd.HasValue,
                Gender = user.Gender
            }).ToListAsync();

            return (items, totalCount);
        }

        public async Task<UserUpdateDto> GetAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }
            return new UserUpdateDto 
            { 
                Id = id, 
                Email = user.Email, 
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Gender = user.Gender
            };
        }

        public async Task<DeleteResultDto> DeleteAsync(string id)
        {
            var user = await _dbContext.Users
                 .AsNoTracking()
                 .FirstOrDefaultAsync(evnt => evnt.Id == id);

            if (user == null)
            {
                return DeleteResultDto.NotFound(id);
            }

            try
            {
                user.Deleted = true;
                user.DeletedOn = DateTime.Now;
                _dbContext.Update(user);
                await _dbContext.SaveChangesAsync();
                return DeleteResultDto.Successful(id);
            }
            catch
            {
                return DeleteResultDto.UnknownError(id);
            }
        }

        public async Task<ChangeStatusResultDto> ChangeStatusAsync(string id)
        {
            var propertyInfo = typeof(UserGridDto).GetProperty(nameof(UserGridDto.IsActive));
            if (!propertyInfo.IsDefined(typeof(BooleanAttribute), false))
            {
                return ChangeStatusResultDto.NotPossible(id);
            }
            var booleanAttribute = propertyInfo.GetCustomAttributes(typeof(BooleanAttribute), false).Cast<BooleanAttribute>().SingleOrDefault();

            var user = await _dbContext.Users
                 .AsNoTracking()
                 .FirstOrDefaultAsync(u => u.Id == id && !u.Deleted);

            if (user == null)
            {
                return ChangeStatusResultDto.NotFound(id);
            }

            try
            {
                var userWasActive = !user.LockoutEnd.HasValue;

                if (userWasActive)
                {
                    user.LockoutEnd = DateTime.Now.AddYears(100);
                }
                else
                {
                    user.LockoutEnd = null;
                }
                _dbContext.Update(user);
                await _dbContext.SaveChangesAsync();

                if (userWasActive)
                {
                    return ChangeStatusResultDto.Successful(id, booleanAttribute.FalseBadge, booleanAttribute.FalseText);
                }
                return ChangeStatusResultDto.Successful(id, booleanAttribute.TrueBadge, booleanAttribute.TrueText);
            }
            catch
            {
                return ChangeStatusResultDto.UnknownError(id);
            }
        }

        public async Task<CommandResultDto> CreateAsync(UserCreateDto dto)
        {
            var user = new Manager 
            {
                UserName = dto.UserName,
                Email = dto.Email,
                Gender = dto.Gender,
                PhoneNumber = dto.PhoneNumber,
                CreatedOn = DateTime.Now 
            };
            var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
                return CommandResultDto.Successful();
            
            var errors = result.Errors.Where(e => !string.IsNullOrWhiteSpace(e.Description)).Select(e => e.Description);
            var message = string.Join("<br/>",errors);
            return CommandResultDto.Failed(message);
        }

        public async Task<CommandResultDto> SetPasswordAsync(UserSetPasswordDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id);
            if (user == null)
            {
                return CommandResultDto.NotFound();
            }

            var newPassword = _userManager.PasswordHasher.HashPassword(user, dto.Password);
            user.PasswordHash = newPassword;
            return await UpdateUser(user);
        }

        public async Task<CommandResultDto> UpdateAsync(UserUpdateDto dto)
        {
            var user = await _userManager.FindByIdAsync(dto.Id);
            if (user == null)
            {
                return CommandResultDto.NotFound();
            }
            user.Email = dto.Email;
            user.UserName = dto.UserName;
            user.Gender = dto.Gender;
            user.PhoneNumber = dto.PhoneNumber;
            return await UpdateUser(user);
        }

        public async Task<CommandResultDto> UpdateMyProfileAsync(Manager me, UserUpdateDto dto)
        {
            if (dto.UserName != me.UserName)
            {
                var setUserNameResult = await _userManager.SetUserNameAsync(me, dto.UserName);
                if (!setUserNameResult.Succeeded)
                {
                    return CommandResultDto.Failed( $"خطایی در تغییر نام کاربری رخ داده است.." + string.Join(". ", setUserNameResult.Errors.Select(p => p.Description)));
                }
            }

            if (dto.Email != me.Email)
            {
                var setEmailResult = await _userManager.SetEmailAsync(me, dto.Email);
                if (!setEmailResult.Succeeded)
                {
                    return CommandResultDto.Failed("خطایی در تغییر ایمیل رخ داده است. یک حساب دیگر با این ایمیل وجود دارد.");
                }
            }

            if (dto.PhoneNumber != me.PhoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(me, dto.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    return CommandResultDto.Failed($"خطایی در تغییر شماره تلفن رخ داده است.");
                }
            }

            if(dto.Gender != me.Gender)
            {
                me.Gender = dto.Gender;
                await UpdateUser(me);
            }

            return CommandResultDto.Successful();
        }

        private async Task<CommandResultDto> UpdateUser(Manager user)
        {
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return CommandResultDto.Successful();
            }

            var errors = result.Errors.Where(e => !string.IsNullOrWhiteSpace(e.Description)).Select(e => e.Description);
            var message = string.Join("<br/>", errors);
            return CommandResultDto.Failed(message);
        }

        public async Task<CommandResultDto> ChangeMyPasswordAsync(Manager me, UserChangePasswordDto dto)
        {
            var result = await _userManager.ChangePasswordAsync(me, dto.CurrentPassword, dto.NewPassword);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(me, isPersistent: false);
                return CommandResultDto.Successful();
            }
            var errors = result.Errors.Where(e => !string.IsNullOrWhiteSpace(e.Description)).Select(e => e.Description);
            var message = string.Join("<br/>", errors);
            return CommandResultDto.Failed(message);
        }
    }
}
