namespace Base.Constants;
public class MainMenu
{
    public static readonly Dictionary<string, string[]> menu = new Dictionary<string, string[]>
        {
            { "NIC", new string[] { "Add affinity", "Show devices", "Set interrupt priority", "Set message limit", "Registry settings", "Remove affinity" } },
            { "PCI", new string[] { "Add affinity", "Show devices", "Set interrupt priority", "Set message limit", "Remove affinity", "Remove pci bridge affinity" } },
            { "USB", new string[] { "Add affinity", "Show devices", "Set interrupt priority", "Set message limit", "Remove affinity" } },
            { "Find Core", new string[] { "Placeholder", "CPPC" } },
            { "DSCP Settings", new string[] { "Add DSCP", "Placeholder" } },
        };
}
