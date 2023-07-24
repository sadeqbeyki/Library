using LibIdentity.DomainContracts.RoleContracts;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.adminPanel.Controllers;
[Area("adminPanel")]
public class RolesController : Controller
{
    private readonly IRoleService _roleService;

    public RolesController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    public async Task<ActionResult<List<RoleDto>>> Index()
    {
        var roles = await _roleService.GetRoles();
        return View(roles);
    }
    public async Task<ActionResult<RoleDto>> Details(int id)
    {
        var role = await _roleService.GetRole(id);
        return View(role);
    }
    #region Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(RoleDto model)
    {
        if (ModelState.IsValid)
        {
            var result = await _roleService.CreateRole(model);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
            }
        }
        return View(model);
    }
    #endregion

    #region Update
    public async Task<ActionResult> Update(int id)
    {
        var role = await _roleService.GetRole(id);
        if (role != null)
        {
            return View(role);
        }
        return RedirectToAction("Index");
    }
    [HttpPost]
    public async Task<ActionResult<RoleDto>> Update(int id, RoleDto model)
    {
        var role = await _roleService.GetRole(id);
        if (role != null)
        {
            var result = await _roleService.UpdateRole(model);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
            }
            return View(model);
        }
        return NotFound();
    }
    #endregion

    #region Delete
    public async Task<ActionResult> Delete(RoleDto dto)
    {
        var role = await _roleService.GetRole(dto.Id);
        if (role != null)
        {
            var result = await _roleService.DeleteRole(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(item.Code, item.Description);
                }
            }
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }
    #endregion
}
