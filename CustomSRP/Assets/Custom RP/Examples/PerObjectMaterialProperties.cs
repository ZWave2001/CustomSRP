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
        private static int _metallicId = Shader.PropertyToID("_Metallic");
        private static int _smoothnessId = Shader.PropertyToID("_Smoothness");


        [SerializeField]
        private Color _baseColor = Color.white;

        [SerializeField, Range(0, 1)]
        private float _cutoff = 0.5f, _metallic = 0, _smoothness = 0.5f; 

        private static MaterialPropertyBlock block;

        private void OnValidate()
        {
            if (block == null)
            {
                block = new MaterialPropertyBlock();
            }

            block.SetFloat(_metallicId, _metallic);
            block.SetFloat(_smoothnessId, _smoothness);
            block.SetColor(_baseColorId, _baseColor);
            block.SetFloat(_cutoffId, _cutoff);
            
            GetComponent<Renderer>().SetPropertyBlock(block);
        }
    }
}