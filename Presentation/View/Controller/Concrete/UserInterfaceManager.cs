using Presentation.View.Controller.Abstract;
using Presentation.View.Helper.Concrete;

namespace Presentation.View.Controller.Concrete;
internal class UserInterfaceManager : IUserInterfaceService
{
    private readonly DisplayMenuHelper _displayMenuHelper;
    private readonly DisplaySetupHelper _displaySetupHelper;
    public UserInterfaceManager(DisplayMenuHelper displayMenuHelper, DisplaySetupHelper displaySetupHelper)
    {
        _displayMenuHelper = displayMenuHelper;
        _displaySetupHelper = displaySetupHelper;
    }

    public void DisplayInterface()
    {
        _displaySetupHelper.ConfigureConsoleFrame();
        _displayMenuHelper.MainMenu();
    }
}