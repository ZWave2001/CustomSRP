using System;
using UnityEngine;

namespace Custom_RP.Examples
{
    [DisallowMultipleComponent]
    public class PerObjectMaterialProperties : MonoBehaviour
    {
        static int baseColorId = Shader.PropertyToID("_BaseColor");

        [SerializeField]
        private Color baseColor = Color.white;

        private static MaterialPropertyBlock block;

        private void OnValidate()
        {
            if (block == null)
            {
                block = new MaterialPropertyBlock();
            }
            block.SetColor(baseColorId, baseColor);
            GetComponent<Renderer>().SetPropertyBlock(block);
        }
    }
}