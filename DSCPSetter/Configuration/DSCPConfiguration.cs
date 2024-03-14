namespace DSCPSetter.Configuration;
internal class DSCPConfiguration
{
    internal Dictionary<string, string> DSCPValues = new Dictionary<string, string>()
    {
        { "Application Name", string.Empty},
        { "DSCP Value", "46"},
        { "Local IP", "*"},
        { "Local IP Prefix Length", "*"},
        { "Local Port", "*"},
        { "Protocol", "*"},
        { "Remote IP", "*"},
        { "Remote IP Prefix Length", "*"},
        { "Remote Port", "*"},
        { "Throttle Rate", "-1"},
        { "Version", "1.0"},
    };
}
