using System.Reflection;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors.DiffMemberMonitors
{
    public class FloatDiffMemberMonitor : DiffMemberMonitorBase<float>
    {
        public FloatDiffMemberMonitor(string name, object memberHolder, MemberInfo memberInfo) : base(name, memberHolder, memberInfo)
        {
        }

        public override float TypedAddition(float a, float b)
        {
            return a + b;
        }

        public override bool TypedEquality(float a, float b)
        {
            return (a - b) < 0.1f;
        }

        public override float TypedSubtraction(float a, float b)
        {
            return a - b;
        }
    }
}
