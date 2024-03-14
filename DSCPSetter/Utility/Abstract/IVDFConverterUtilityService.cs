namespace DSCPSetter.Utility.Abstract;
public interface IVDFConverterUtilityService
{
    void Transform();
    void Transform(HashSet<uint>? ids);
    void Dispose();
}
