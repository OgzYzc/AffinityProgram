namespace AffinityProgram.Model
{
    internal class Model_PciBridgeDevices
    {
        public Model_PciBridgeDevices(string pciBridgeId)
        {
            this.PciBridgeId = pciBridgeId;
        }

        private string PciBridgeId { get; }

        public override string ToString()
        {
            return PciBridgeId;
        }
    }
}
