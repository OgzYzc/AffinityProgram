using AffinitySetter.Controller.Concrete;
using Base.Model;
using Base.Utility.Abstract;
using DSCPSetter.Helper.Abstract;
using FindCore.Helper.Abstract;
using Presentation.View.Utility.Abstract;
using System.Reflection;

namespace Presentation.View.Utility.Concrete;
internal class DisplayMenuUtility : IDisplayMenuUtilityService
{
    public Stack<Dictionary<string, string[]>> menuStack;
    private readonly IProcessorUtilityService _processorUtility;
    private readonly IFindCoreHelperService _findCoreHelperService;
    private readonly IDSCPSetterHelper _dscpSetterHelper;
    private readonly NICManager _nicManager;
    private readonly PCIManager _pciManager;
    private readonly USBManager _usbManager;

    static string? firstOpt;
    static string? secondOpt;

    public DisplayMenuUtility(
        IProcessorUtilityService processorUtility,
        IFindCoreHelperService findCoreHelperService,
        IDSCPSetterHelper dscpSetterHelper,
        NICManager nicManager,
        PCIManager pciManager,
        USBManager usbManager
        )
    {
        menuStack = new Stack<Dictionary<string, string[]>>();
        _processorUtility = processorUtility;
        _findCoreHelperService = findCoreHelperService;
        _dscpSetterHelper = dscpSetterHelper;
        _nicManager = nicManager;
        _pciManager = pciManager;
        _usbManager = usbManager;
    }

    // https://stackoverflow.com/a/46909420
    public int MenuRenderer(Dictionary<string, string[]> menu)
    {

        Console.Clear();
        DisplayProcessorInformation();

        int startX = (Console.WindowWidth - menu.Keys.Max(k => k.Length)) / 2;      // Calculate left starting point for menu items
        int startY = (Console.WindowHeight - menu.Count) / 2;                       // Calculate top starting point for menu items
        const int optionsPerLine = 1;                                               // Make options displayed as column or row
        const int spacingPerLine = 1;                                               // Space between options if displayed as column

        int currentSelection = 0;

        menuStack.Push(menu);                                                       // Print the initial menu as main

        ConsoleKey key;

        List<string> options = menu.Keys.ToList();

        do
        {
            for (int i = 0; i < options.Capacity; i++)
            {
                Console.SetCursorPosition(startX + i % optionsPerLine * spacingPerLine, startY + i / optionsPerLine);

                if (i == currentSelection)
                    Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine(options[i]);

                Console.ResetColor();
            }

            key = Console.ReadKey(true).Key;
            currentSelection = ProcessUserInput(key, currentSelection, optionsPerLine, options, menu);

        } while (true);
    }

    public int ProcessUserInput(ConsoleKey key, int currentSelection, int optionsPerLine, List<string> options, Dictionary<string, string[]> menu)
    {
        switch (key)
        {
            /*case ConsoleKey.LeftArrow:
                {
                    if (currentSelection % optionsPerLine > 0)
                        return currentSelection--;
                    break;
                }
            case ConsoleKey.RightArrow:
                {
                    if (currentSelection % optionsPerLine < optionsPerLine - 1)
                        return currentSelection++;
                    break;
                }*/
            case ConsoleKey.UpArrow:
                {
                    if (currentSelection >= optionsPerLine)
                        return currentSelection -= optionsPerLine;
                    break;
                }
            case ConsoleKey.DownArrow:
                {
                    if (currentSelection + optionsPerLine < options.Count)
                        return currentSelection += optionsPerLine;
                    break;
                }
            case ConsoleKey.Enter:
                // This needs to change
                if (firstOpt == null)
                    firstOpt = options[currentSelection];
                else
                {
                    secondOpt = options[currentSelection];
                    InvokeMethod(firstOpt, secondOpt);
                    secondOpt = null;
                }
                string selectedOption = options[currentSelection];
                if (menuStack.Peek()[selectedOption].Length > 0)            // Check if option have anything inside it         //Delete after implement everything
                    if (menu.ContainsKey(selectedOption))
                        MenuRenderer(menu[selectedOption].ToDictionary(x => x, x => new string[] { }));                        // If selected option has a submenu, render it
                return currentSelection;
            case ConsoleKey.Backspace:
                {
                    firstOpt = null;
                    if (menuStack.Count > 1)
                    {
                        menuStack.Pop();
                        MenuRenderer(menuStack.Peek());
                    }
                    break;
                }
            case ConsoleKey.Escape:
                {
                    Environment.Exit(0);
                    break;
                }
            default:
                return currentSelection;
        }
        return currentSelection;
    }
    string InterceptMethod(string firstOpt)
    {
        switch (firstOpt)
        {
            case "NIC":
                {
                    return "_nicManager";
                }
            case "PCI":
                {
                    return "_pciManager";
                }
            case "USB":
                {
                    return "_usbManager";
                }
            case "Find Core":
                {
                    return "_findCoreHelperService";
                }
            case "DSCP Settings":
                {
                    return "_dscpSetterHelper";
                }
            default:
                return "Invalid";
        }
    }
    string InterceptSecondMethod(string secondOpt)
    {

        switch (secondOpt)
        {
            case "Add affinity":
                {
                    return "AffinityAdd";
                }
            case "Show devices":
                {
                    return "DeviceList";
                }
            case "Remove affinity":
                {
                    return "AffinityDelete";
                }
            case "CPPC":
                {
                    return "RunCPPC";
                }
            case "Registry settings":
                {
                    return "RegistrySettingsAdd";
                }
            case "Add DSCP":
                {
                    return "RunDSCP";
                }
            default:
                return "Invalid";
        }
    }
    void InvokeMethod(string firstOpt, string secondOpt)
    {
        string fieldName = InterceptMethod(firstOpt);
        string methodName = InterceptSecondMethod(secondOpt);

        if (fieldName != "Invalid")
        {
            FieldInfo? field = typeof(DisplayMenuUtility).GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);

            if (field != null)
            {
                object? instance = field.GetValue(this);
                MethodInfo? method = instance?.GetType().GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance);
                if (method != null)
                {
                    Console.Clear();
                    method.Invoke(instance, null);

                    while (Console.ReadKey(true).Key != ConsoleKey.Backspace);          // Wait for user to press Backspace to print previous menu.
                    Console.Clear();
                }
                else
                {
                    Console.WriteLine($"Method '{methodName}' not found in '{fieldName}'.");
                }
            }
            else
            {
                Console.WriteLine($"Field '{fieldName}' not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid field name.");
        }
    }
    public void DisplayProcessorInformation()
    {
        ProcessorModel.ProcessorInfoModel processorInfo = _processorUtility.GetProcessorInformation();

        ConsoleColor originalColor = Console.ForegroundColor;
        Console.Write($"P: {processorInfo.PhysicalProcessorCount} L: {processorInfo.LogicalProcessorCount} | ");

        Console.Write((Console.ForegroundColor = processorInfo.IsSMTEnabled ? ConsoleColor.Green : ConsoleColor.Red)
                                                   == ConsoleColor.Green ? "SMT is ENABLED" : "SMT is DISABLED");
        Console.ForegroundColor = originalColor;
    }
}
