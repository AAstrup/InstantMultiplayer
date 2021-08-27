using InstantMultiplayer.Initialization;
using System;
using UnityEngine;

namespace InstantMultiplayer.Synchronization.ComponentMapping
{
    internal class ComponentTypeInitializationHandler : ATypeInitializationHandler<Component>
    {
        public override void HandleGenericType(Type type)
        {
            ComponentMapper.Instance.RegisterType(type);
        }
    }
}
