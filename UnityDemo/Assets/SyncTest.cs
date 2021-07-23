using InstantMultiplayer.Synchronization;
using InstantMultiplayer.Synchronization.Delta;
using InstantMultiplayer.UnityIntegration;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SyncTest : MonoBehaviour
{
    public Text Text;
    public Synchronizer Synchronizer;

    private DeltaContainer _delta;

    private void Start()
    {
        var ts = ComponentMapper.Instance.IncludedTypes().ToList();

        Text.text = ComponentMapper.GetCIDFromType(typeof(Transform)).ToString();
    }
     
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
            _delta = Synchronizer.TryGetDeltaContainer(new DeltaProvider(), out var delta) ? delta : _delta;
        if (Input.GetKeyDown(KeyCode.L))
            Synchronizer.ConsumeDeltaContainer(new DeltaConsumer(), _delta);
    }
}
