using Presentation.View.Helper.Abstract;
using Presentation.View.Utility.Abstract;

namespace Presentation.View.Helper.Concrete;
internal class DisplayMenuHelper : IDisplayMenuHelperService
{
    private readonly IDisplayMenuUtilityService _displayMenuUtilityService;


    public DisplayMenuHelper(IDisplayMenuUtilityService displayMenuUtilityService)
    {
        _displayMenuUtilityService = displayMenuUtilityService;


    }

    public void MainMenu()
    {
        _displayMenuUtilityService.MenuRenderer(Base.Constants.MainMenu.menu);
    }
}
