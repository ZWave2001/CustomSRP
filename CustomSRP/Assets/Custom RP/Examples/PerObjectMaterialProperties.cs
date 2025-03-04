using UnityEngine;

namespace Custom_RP.Examples
{
    [DisallowMultipleComponent]
    public class PerObjectMaterialProperties : MonoBehaviour
    {
        static int baseColorId = Shader.PropertyToID("_BaseColor");

        [SerializeField]
        private Color baseColor = Color.white;
        
    }
}