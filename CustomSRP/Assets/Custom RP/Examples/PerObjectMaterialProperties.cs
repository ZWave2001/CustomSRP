using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Custom_RP.Examples
{
    [DisallowMultipleComponent]
    public class PerObjectMaterialProperties : MonoBehaviour
    {
        private static int _baseColorId = Shader.PropertyToID("_BaseColor");
        private static int _cutoffId = Shader.PropertyToID("_Cutoff");

        [SerializeField]
        private Color _baseColor = Color.white;

        [SerializeField, Range(0, 1)]
        private float _cutoff = 0.5f; 

        private static MaterialPropertyBlock block;

        private void OnValidate()
        {
            if (block == null)
            {
                block = new MaterialPropertyBlock();
            }
            block.SetColor(_baseColorId, _baseColor);
            block.SetFloat(_cutoffId, _cutoff);
            
            GetComponent<Renderer>().SetPropertyBlock(block);
        }
    }
}