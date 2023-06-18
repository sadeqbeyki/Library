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
    public async Task<UpdateUserDto> GetUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return _mapper.Map<UpdateUserDto>(user);
    }
    #endregion

    #region GetAll
    public async Task<List<UpdateUserDto>> GetUsers()
    {
        List<User> users = await _userManager.Users.Take(50).ToListAsync();
        return _mapper.Map<List<UpdateUserDto>>(users);
    }
    #endregion

    #region Create
    public async Task<IdentityResult> CreateUser(UserDto model)
    {
        var userMap = _mapper.Map<User>(model);
        return await _userManager.CreateAsync(userMap, model.Password);
    }
    #endregion

    #region Update
    public async Task<IdentityResult> Update(UpdateUserDto user)
    {
        //var userMapp = _mapper.Map<User>(user);
        var result = await _userManager.FindByIdAsync(user.Id);
        result.FirstName = user.FirstName;
        result.LastName = user.LastName;
        result.Email = user.Email;
        result.UserName = user.UserName;

        return await _userManager.UpdateAsync(result);


    }
    #endregion

    #region Delete
    public async Task<IdentityResult> DeleteUser(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        return await _userManager.DeleteAsync(user);
    }
    #endregion
}
