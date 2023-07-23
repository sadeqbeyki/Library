using LibBook.DomainContracts.Lend;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.adminPanel.Controllers;
[Area("adminPanel")]
public class LendsController : Controller
{
    private readonly ILendService _lendService;
    
    public LendsController(ILendService lendService)
    {
        _lendService = lendService;
    }
   
    public async Task<ActionResult<List<LendDto>>> Index()
    {
        List<LendDto> lends = await _lendService.GetAllLends();
        return View("Index", lends);
    }
}
