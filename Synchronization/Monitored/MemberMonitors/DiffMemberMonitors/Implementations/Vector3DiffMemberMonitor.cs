using System.Reflection;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors.DiffMemberMonitors.Implementations
{
    public class Vector3DiffMemberMonitor : DiffMemberMonitorBase<Vector3>
    {
        public Vector3DiffMemberMonitor(string name, object memberHolder, MemberInfo memberInfo) : base(name, memberHolder, memberInfo)
        {
        }

        public override Vector3 TypedAddition(Vector3 a, Vector3 b)
        {
            return a + b;
        }

        public override bool TypedEquality(Vector3 a, Vector3 b)
        {
            return (a - b).magnitude < 0.1f;
        }

        public override Vector3 TypedSubtraction(Vector3 a, Vector3 b)
        {
            return a - b;
        }
    }
}
