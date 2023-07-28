using AutoMapper;
using LibIdentity.Domain.UserAgg;
using LibIdentity.DomainContracts.UserContracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LibIdentity.ApplicationServices;

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
    public async Task<UserViewModel> GetUser(int id)
    {
        var user = await _userManager.FindByIdAsync(id.ToString());
        return _mapper.Map<UserViewModel>(user);
    }

    public UserViewModel GetAccountBy(int id)
    {
        var account = _userManager.FindByIdAsync(id.ToString());
        return _mapper.Map<UserViewModel>(account);

        //return new UpdateUserDto()
        //{
        //    FirstName = account.,
        //    Email = account.Mobile
        //};
    }
    #endregion

    #region GetAll
    public async Task<List<UserViewModel>> GetUsers()
    {
        List<User> users = await _userManager.Users.Take(50).ToListAsync();
        return _mapper.Map<List<UserViewModel>>(users);
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
    public async Task<IdentityResult> Update(UserViewModel user)
    {
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
