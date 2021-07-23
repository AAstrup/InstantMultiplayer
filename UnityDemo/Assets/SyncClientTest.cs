using Assets;
using InstantMultiplayer.Synchronization.Delta;
using InstantMultiplayer.UnityIntegration;
using UnityEngine;

namespace Assets
{
    public class SyncClientTest : MonoBehaviour
    {
        public SyncClient SyncClient;

        private DeltaContainer _delta;
        private TestCommunicationClient _comClient;

        private void Start()
        {
            _comClient = new TestCommunicationClient();
            SyncClient._client = _comClient;
            SyncClient.Connect();
        }
     
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Return))
            {
                var count = _comClient.Flush();
                Debug.Log("Flushed " + count);
            }
        }
    }
}