namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors
{
    public interface IDiffMemberMonitor<T>: IMemberMonitor<T>
    {
        //T LastLocalValue { get; set; }
        //T AccumulatedForeignDiffValue { get; }
        //void ConsumeDiffValue(T deltaValue);
        //T GetDiffValue();
    }
}
