using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Extensions
{
    public static class ComponentExtensions
    {
        public static void InvokeForAll(Component synchronizedComponent, string methodName, params object[] arguments)
        {
            if (TryGetSynchronizer(synchronizedComponent, out var synchronizer))
            {
                synchronizer.InvokeForAll(synchronizedComponent, methodName, arguments);
                return;
            }
            Debug.LogError($"You need to add the {nameof(Component)} {synchronizedComponent.name} to a {nameof(Synchronizer)} before you can invoke methods!");
        }

        public static void InvokeForOwner(Component synchronizedComponent, string methodName, params object[] arguments)
        {
            if (TryGetSynchronizer(synchronizedComponent, out var synchronizer))
            {
                synchronizer.InvokeForOwner(synchronizedComponent, methodName, arguments);
                return;
            }
            Debug.LogError($"You need to add the {nameof(Component)} {synchronizedComponent.name} to a {nameof(Synchronizer)} before you can invoke methods!");
        }

        public static void InvokeForClients(Component synchronizedComponent, int[] clientIds, string methodName, params object[] arguments)
        {
            if (TryGetSynchronizer(synchronizedComponent, out var synchronizer))
            {
                synchronizer.InvokeForClients(synchronizedComponent, clientIds, methodName, arguments);
                return;
            }
            Debug.LogError($"You need to add the {nameof(Component)} {synchronizedComponent.name} to a {nameof(Synchronizer)} before you can invoke methods!");
        }

        public static void InvokeForClient(Component synchronizedComponent, int clientId, string methodName, params object[] arguments)
        {
            if (TryGetSynchronizer(synchronizedComponent, out var synchronizer))
            {
                synchronizer.InvokeForClient(synchronizedComponent, clientId, methodName, arguments);
                return;
            }
            Debug.LogError($"You need to add the {nameof(Component)} {synchronizedComponent.name} to a {nameof(Synchronizer)} before you can invoke methods!");
        }

        public static void InvokeForFilter(Component synchronizedComponent, int clientFilter, string methodName, params object[] arguments)
        {
            if (TryGetSynchronizer(synchronizedComponent, out var synchronizer))
            {
                synchronizer.InvokeForFilter(synchronizedComponent, clientFilter, methodName, arguments);
                return;
            }
            Debug.LogError($"You need to add the {nameof(Component)} {synchronizedComponent.name} to a {nameof(Synchronizer)} before you can invoke methods!");
        }

        private static bool TryGetSynchronizer(Component component, out Synchronizer synchronizer)
        {
            return SynchronizeStore.Instance.TryGetFromIID(component.gameObject.GetInstanceID(), out synchronizer);
        }
    }
}
