using InstantMultiplayer.Synchronization.Objects;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class ResourceTest: MonoBehaviour
    {
        public Text Text;
        private void Start()
        {
            Text.text = "";
            Text.text += nameof(ReferenceRepository) + "\n";
            foreach (var p in ReferenceRepository.Instance.MapCopy())
                foreach (var pp in p.Value)
                    Text.text += $"[{p.Key}] {pp.Key}: {pp.Value.name} \n";
            Text.text += "\n";
            Text.text += nameof(ResourceRepository) + "\n";
            foreach (var p in ResourceRepository.Instance.MapCopy())
                foreach (var pp in p.Value)
                    Text.text += $"[{p.Key}] {pp.Key}: {pp.Value} \n";
        }
    }
}
