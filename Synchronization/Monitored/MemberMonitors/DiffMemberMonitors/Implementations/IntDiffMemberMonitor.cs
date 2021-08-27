using System.Reflection;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors.DiffMemberMonitors.Implementations
{
    public class IntDiffMemberMonitor : DiffMemberMonitorBase<int>
    {
        public IntDiffMemberMonitor(string name, object memberHolder, MemberInfo memberInfo) : base(name, memberHolder, memberInfo)
        {
        }

        public override int TypedAddition(int a, int b)
        {
            return a + b;
        }

        public override bool TypedEquality(int a, int b)
        {
            return (a - b) < 0.1f;
        }

        public override int TypedSubtraction(int a, int b)
        {
            return a - b;
        }
    }
}
