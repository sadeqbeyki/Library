using AppFramework.Application.Email;
using AutoMapper;
using LibIdentity.Domain.UserAgg;
using LibIdentity.DomainContracts.UserContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Security.Policy;

namespace LibIdentity.ApplicationServices;

public class UserService : IUserService
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly IMapper _mapper;
    private readonly IEmailService _email;

    public UserService(UserManager<UserIdentity> userManager, IMapper mapper, IEmailService email)
    {
        _userManager = userManager;
        _mapper = mapper;
        _email = email;
    }
    #region Get
    public async Task<UpdateUserViewModel> GetUser(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        return _mapper.Map<UpdateUserViewModel>(user);
    }

    public UpdateUserViewModel GetAccountBy(int id)
    {
        var account = _userManager.FindByIdAsync(id.ToString());
        return _mapper.Map<UpdateUserViewModel>(account);

        //return new UpdateUserDto()
        //{
        //    FirstName = account.,
        //    Email = account.Mobile
        //};
    }
    #endregion

    #region GetAll
    public async Task<List<UpdateUserViewModel>> GetUsers()
    {
        List<UserIdentity> users = await _userManager.Users.Take(50).ToListAsync();
        return _mapper.Map<List<UpdateUserViewModel>>(users);
    }

    public async Task<List<UserRolesViewModel>> GetAllUsers()
    {
        List<UserIdentity> users = await _userManager.Users.Take(50).ToListAsync();

        var userViewModels = new List<UserRolesViewModel>();

        foreach (var user in users)
        {
            var userViewModel = _mapper.Map<UserRolesViewModel>(user);
            var roles = await _userManager.GetRolesAsync(user);
            userViewModel.RoleName = roles.ToList();
            userViewModels.Add(userViewModel);
        }

        return userViewModels;
    }
    #endregion

    #region Create
    public async Task<IdentityResult> CreateUser(CreateUserViewModel model)
    {
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            var error = new IdentityError
            {
                Code = "Duplicate Email",
                Description = "این ایمیل قبلاً ثبت شده است."
            };
            return IdentityResult.Failed(error);
        }
        var userMap = _mapper.Map<UserIdentity>(model);
        var result =  await _userManager.CreateAsync(userMap, model.Password);
        await _userManager.AddToRoleAsync(userMap, "member");
        return result;
    }

    public async Task<IdentityResult> Register(CreateUserViewModel model)
    {
        var existingUser = await _userManager.FindByEmailAsync(model.Email);
        if (existingUser != null)
        {
            var error = new IdentityError
            {
                Code = "Duplicate Email",
                Description = "این ایمیل قبلاً ثبت شده است."
            };
            return IdentityResult.Failed(error) ;
        }
        //jwt token
        string passwordHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

        var userMap = _mapper.Map<UserIdentity>(model);
        var result = await _userManager.CreateAsync(userMap, passwordHash);
        await _userManager.AddToRoleAsync(userMap, "member");

        //send mail
        //...

        return result;
    }
    #endregion

    #region Update
    public async Task<IdentityResult> Update(UpdateUserViewModel user)
    {
        //string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);

        //var userMapp = _mapper.Map<User>(user);
        var result = await _userManager.FindByIdAsync(user.Id.ToString());

        result.FirstName = user.FirstName;
        result.LastName = user.LastName;
        result.Email = user.Email;
        result.UserName = user.UserName;

        return await _userManager.UpdateAsync(result);


    }
    #endregion

    #region Delete
    public async Task<IdentityResult> DeleteUser(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        return await _userManager.DeleteAsync(user);
    }
    #endregion
}
