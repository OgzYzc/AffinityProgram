namespace Presentation.View.Utility.Abstract;
internal interface IDisplayMenuUtilityService
{
    int MenuRenderer(Dictionary<string, string[]> menu);
    int ProcessUserInput(ConsoleKey key, int currentSelection, int optionsPerLine, List<string> options, Dictionary<string, string[]> menu);

}
