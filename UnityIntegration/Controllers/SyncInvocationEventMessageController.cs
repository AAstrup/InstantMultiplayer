using Communication.Synchronization.Events;
using InstantMultiplayer.Communication;
using InstantMultiplayer.Synchronization.ComponentMapping;
using InstantMultiplayer.UnityIntegration.Events;
using InstantMultiplayer.UnityIntegration.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InstantMultiplayer.UnityIntegration.Controllers
{
    public class SyncInvocationEventMessageController : BaseMessageController<SyncInvocationEventMessage>
    {
        public static SyncInstantiationEventMessageController Instance => _instance ?? (_instance = new SyncInstantiationEventMessageController());
        private static SyncInstantiationEventMessageController _instance;

        private Queue<InvocationEvent> _invocationEventQueue;

        public SyncInvocationEventMessageController()
        {
            _invocationEventQueue = new Queue<InvocationEvent>();
            EventHandlerProvider.Instance.InvocationEventHandler += (s, v) => _invocationEventQueue.Enqueue(v);
        }

        public override void HandleMessage(SyncInvocationEventMessage syncMessage)
        {
            if (!SynchronizeStore.Instance.TryGet(syncMessage.SynchronizerId, out var synchronizer))
                return;
            foreach(var comp in synchronizer.Components)
            {
                var type = comp.GetType();
                if (!(ComponentMapper.TryGetCIDFromType(type, out var cid) && cid == syncMessage.CompId))
                    continue;
                var methodInfo = type.GetMethod(syncMessage.MethodName);
                if (methodInfo == null)
                {
                    Debug.LogWarning($"Failed to invoke method named {syncMessage.MethodName} for component of type {type} on synchronizer with id {synchronizer.SynchronizerId}");
                    return;
                }
                try
                {
                    methodInfo.Invoke(comp, syncMessage.Arguments);
                }
                catch(Exception e)
                {
                    var args = string.Join(",", syncMessage.Arguments);
                    Debug.LogException(new Exception($"Failed to invoke method named {syncMessage.MethodName} for component of type {type} on synchronizer with id {synchronizer.SynchronizerId} using arguments [{args}]: ", e));
                }
            }
        }

        public override bool TryGetMessage(out IMessage message)
        {
            if(_invocationEventQueue.TryDequeue(out var eventMessage))
            {
                if (ComponentMapper.TryGetCIDFromType(eventMessage.Component.GetType(), out var cid))
                {
                    message = new SyncInvocationEventMessage()
                    {
                        ClientFilter = eventMessage.ClientFilter,
                        SynchronizerId = eventMessage.SynchronizerId,
                        CompId = cid,
                        MethodName = eventMessage.MethodName,
                        Arguments = eventMessage.Arguments
                    };
                    return true;
                }
            }
            message = null;
            return false;
        }
    }
}
