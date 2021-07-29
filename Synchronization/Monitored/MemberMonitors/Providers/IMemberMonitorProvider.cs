using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InstantMultiplayer.Synchronization.Monitored.MemberMonitors.Providers
{
    public interface IMemberMonitorProvider
    {
        bool IsApplicable(object memberHolder, MemberInfo memberInfo);
        int Precedence { get; }
        AMemberMonitorBase GetMonitor(object memberHolder, MemberInfo memberInfo);
    }
}
