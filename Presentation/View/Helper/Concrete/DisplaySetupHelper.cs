using Presentation.View.Helper.Abstract;
using Presentation.View.Utility.Abstract;

namespace Presentation.View.Helper.Concrete;
internal class DisplaySetupHelper : IDisplaySetupHelperService
{
    private readonly IDisplaySetupUtilityService _displaySetupUtility;

    public DisplaySetupHelper(IDisplaySetupUtilityService displaySetupUtility)
    {
        _displaySetupUtility = displaySetupUtility;
    }

    public void ConfigureConsoleFrame()
    {
        _displaySetupUtility.DisableConsoleFrameSettings();
    }
}


