using InstantMultiplayer.Communication.Serialization;
using InstantMultiplayer.Synchronization.Attributes;
using InstantMultiplayer.Synchronization.Monitored;
using System;
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

        [Fact]
        public void Sandbox()
        {
            var t = typeof(UnityEngine.Component).IsAssignableFrom(typeof(MonoBehaviour));

            var ind = new PairDummy { I1 = 5, I2 = 4 };
            var pair = (5, 4);
            var serializer = new BinarySerializer();
            var pairSer = serializer.Serialize(pair);
            var indSer = serializer.Serialize(ind);
            var o1 = serializer.Deserialize(indSer);
            var o2 = serializer.Deserialize(pairSer);

        }

        [Serializable]
        private class PairDummy
        {
            public int I1;
            public int I2;
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
