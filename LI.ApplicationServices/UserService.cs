using AutoMapper;
using LI.ApplicationContracts.UserContracts;
using LI.Domain.UserAgg;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LI.ApplicationServices;

public class UserService : IUserService
{
    private readonly UserManager<User> _userManager;

    private readonly IMapper _mapper;

    public UserService(UserManager<User> userManager, IMapper mapper)
    {
        _userManager = userManager;
        _mapper = mapper;
    }
    #region Get
    public async Task<UserDto> GetUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            throw new InvalidOperationException("The desired user does not exist");
        }
        var userMapp = _mapper.Map<UserDto>(user);
        return userMapp;

    }
    #endregion

    #region GetAll
    public async Task<List<UserDto>> GetUsers()
    {
        List<User> users = await _userManager.Users.Take(50).ToListAsync();
        if (users == null || users.Count == 0)
        {
            throw new ArgumentException("Users List Is Empty!");
        }
        var usersMap = _mapper.Map<List<UserDto>>(users);
        return usersMap;
    }
    #endregion

    #region Create
    public async Task<IdentityResult> CreateUser(UserDto model)
    {
        var userMap = _mapper.Map<User>(model);
        var result = await _userManager.CreateAsync(userMap, model.Password);
        return result;
    }
    #endregion

    #region Update
    public async Task<IdentityResult> UpdateUser(UserDto user)
    {
        var userMapp = _mapper.Map<User>(user);
        var result = await _userManager.UpdateAsync(userMapp);
        return result;
    }
    #endregion

    #region Delete
    public async Task<IdentityResult> DeleteUser(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        if (user == null)
        {
            throw new InvalidOperationException("The desired user does not exist");
        }
        var result = await _userManager.DeleteAsync(user);
        return result;
    }
    #endregion
}
