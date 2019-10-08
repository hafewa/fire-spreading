using FireSimulation.Core;
using UnityEngine;

namespace FireSimulation.Vegetation
{
    public class Flower : MonoBehaviour
    {

        /*
            Flower (Plant) uses custom shader with instancing buffer to the shader
            MaterialPropertyBlock is used to access color property from material per renderer
            I don't really understand GPU optimization but i figured out with this solution 
            to avoid massive FPS drops by huge amoutn of draw calls and batches. 
        */

        public bool usePropertyBlock = true;
        private Renderer _renderer;
        private MaterialPropertyBlock _propertyBlock;
        private Color currentColor;


        private void Awake()
        {
            _propertyBlock = new MaterialPropertyBlock(); // 
            _renderer = GetComponent<Renderer>();
            _renderer.SetPropertyBlock(_propertyBlock);
        }

        /* Called in Combustible class to determine color based on Combustion state */
        public void ChangeColor(Color color)
        {
            _propertyBlock.SetColor("_Color", color);
            _renderer.SetPropertyBlock(_propertyBlock);
            currentColor = color;
        }

    }
}

