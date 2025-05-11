using AffinitySetter.Utility.Abstract;
using Base.Constants;
using DSCPSetter.Helper.Abstract;
using DSCPSetter.Utility.Abstract;
using Microsoft.Win32;

namespace DSCPSetter.Helper.Concrete;
public class DSCPSetterHelper : IDSCPSetterHelper, IDisposable
{
    private readonly IDSCPMiscUtilityService _dscpMiscUtilityService;
    private readonly IVDFConverterUtilityService _vdfConverterUtilityService;
    private readonly IPathHelperService _pathHelperService;
    private readonly IReadJsonHelperService _readJsonHelperService;
    private readonly IDSCPRegistryUtilityService _dscpRegistryUtilityService;
    private readonly IIFEORegistryUtilityService _ifeoRegistryUtilityService;
    private readonly IGPUPreferenceUtilityService _gpupreferenceUtilityService;
    private readonly IRegistryUtilityService _registryUtilityService;
    private bool disposedValue;

    public DSCPSetterHelper(
        IDSCPMiscUtilityService dscpMiscUtilityService,
        IVDFConverterUtilityService vdfConverterUtilityService,
        IPathHelperService pathHelperService,
        IReadJsonHelperService readJsonHelperService,
        IDSCPRegistryUtilityService dscpRegistryUtilityService,
        IIFEORegistryUtilityService ifeoRegistryUtilityService,
        IGPUPreferenceUtilityService gpuPreferenceUtilityService,
        IRegistryUtilityService registryUtilityService

        )
    {
        _dscpMiscUtilityService = dscpMiscUtilityService;
        _vdfConverterUtilityService = vdfConverterUtilityService;
        _pathHelperService = pathHelperService;
        _readJsonHelperService = readJsonHelperService;
        _dscpRegistryUtilityService = dscpRegistryUtilityService;
        _ifeoRegistryUtilityService = ifeoRegistryUtilityService;
        _gpupreferenceUtilityService = gpuPreferenceUtilityService;
        _registryUtilityService = registryUtilityService;
    }

    public void RunDSCP()
    {
        try
        {
            RunVDFConverter();
            ReadJsonFiles();
            AddDSCPRegistrySettings();
            AddScheduledTask();
            AddIFEORegistrySettings();
            ReadGPUPreferenceRegistrySettings();
            AddGPUPreferenceRegistrySettings();
        }
        finally
        {
            Dispose();
        }
    }
    public void RunVDFConverter()
    {
        _vdfConverterUtilityService.Read(Path.Combine(_pathHelperService.GetSteamPath(), "appcache", "appinfo.vdf"));
        _vdfConverterUtilityService.Dispose();          // Need to dispose here because of file access problem
    }
    public void ReadJsonFiles()
    {
        _readJsonHelperService.ReadAppInfo(Path.Combine(Path.GetTempPath(), "appinfo.json"));
        _readJsonHelperService.ReadLibraryFolder(Path.Combine(_pathHelperService.GetSteamPath(), "steamapps", "libraryfolders.vdf"));
    }

    public void AddDSCPRegistrySettings()
    {
        _dscpRegistryUtilityService.Create(
            RegistryPaths.RegistryPathMappings[7],
            RegistryKeyPermissionCheck.ReadWriteSubTree,
            _registryUtilityService.CreateRegistrySecurity()
        );
    }

    public void AddScheduledTask()
    {
        _dscpMiscUtilityService.AddSchedule();
    }
    public void AddIFEORegistrySettings()
    {
        _ifeoRegistryUtilityService.Create(
            RegistryPaths.RegistryPathMappings[9],
            RegistryKeyPermissionCheck.ReadWriteSubTree,
            _registryUtilityService.CreateRegistrySecurity()
        );
    }
    public void ReadGPUPreferenceRegistrySettings()
    {
        _gpupreferenceUtilityService.ReadEntry(
            RegistryPaths.RegistryPathMappings[10]);

    }
    public void AddGPUPreferenceRegistrySettings()
    {
        _gpupreferenceUtilityService.Create(
            RegistryPaths.RegistryPathMappings[10],
            RegistryKeyPermissionCheck.ReadWriteSubTree,
            _registryUtilityService.CreateRegistrySecurity()
        );
    }
    ~DSCPSetterHelper()
    {
        Dispose(false);
    }
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
