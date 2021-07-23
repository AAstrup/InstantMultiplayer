using KrisCorner.Utilities.Extensions;
using UnityEngine;

namespace Assets
{
    public class Mover: MonoBehaviour
    {
        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void Update()
        {
            transform.position = new Vector3(
                (transform.position.x + Input.GetAxisRaw("Horizontal") * 4 * Time.deltaTime).MinMax(-4.5f, 4.5f),
                0,
                (transform.position.z + Input.GetAxisRaw("Vertical") * 4 * Time.deltaTime).MinMax(-4.5f, 4.5f)
            );
            _camera.transform.position += (transform.position + new Vector3(0, 7, -1.5f) - _camera.transform.position) * Time.deltaTime * 8;
        }
    }
}
