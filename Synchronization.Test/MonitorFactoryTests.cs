using InstantMultiplayer.Synchronization.Attributes;
using InstantMultiplayer.Synchronization.Monitored;
using System.Linq;
using UnityEngine;
using Xunit;

namespace Synchronization.Test
{
    public class MonitorFactoryTests
    {
        [Fact]
        public void MemberTest()
        {
            var members = MonitorFactory.Instance.MonitorableMemberInfos(typeof(MonoTestClass)).ToList();
            Assert.Equal(3, members.Count);
            Assert.True(members.Exists(m => m.Name == nameof(MonoTestClass.TestField)));
            Assert.True(members.Exists(m => m.Name == nameof(MonoTestClass.TestProperty)));
            Assert.True(members.Exists(m => m.Name == nameof(MonoTestClass.ExposedField)));
        }

        private class MonoTestClass : MonoBehaviour
        {
            public float TestField = 10f;
            public int TestProperty { get; set; }

            [ExcludeSync]
            public bool ExcludedField = true;

            [SerializeField]
            internal bool ExposedField = false;

            internal readonly int HiddenField = 10;
            internal float HiddenProperty { get; set; }

            public MonoTestClass()
            {
                if(HiddenField > 0)
                {
                    HiddenField = 10;
                }
            }
        }
    }
}
