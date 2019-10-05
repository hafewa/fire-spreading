
using UnityEngine;

namespace FireSimulation.Controls
{
    public class ControlsConfig : MonoBehaviour
    {
        [Header("Key bindings")]
        public KeyCode lbm = KeyCode.Mouse0;
        public KeyCode rbm = KeyCode.Mouse1;
        public KeyCode forward = KeyCode.W;
        public KeyCode backward = KeyCode.S;
        public KeyCode left = KeyCode.A;
        public KeyCode right = KeyCode.R;
        public KeyCode fastCamera = KeyCode.LeftShift;
        public KeyCode toggleFreeLook = KeyCode.F;

    }
}