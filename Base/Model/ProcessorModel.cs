namespace Base.Model;
public class ProcessorModel
{
    public class ProcessorInfoModel
    {
        public int LogicalProcessorCount { get; set; }
        public int PhysicalProcessorCount { get; set; }
        public bool IsSMTEnabled { get; set; }
    }
}
