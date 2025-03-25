using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Custom_RP.Examples
{
    public class MeshBall : MonoBehaviour
    {
        static int baseColorId = Shader.PropertyToID("_BaseColor");
        static int metallicID = Shader.PropertyToID("_Metallic");
        static int smoothnessID = Shader.PropertyToID("_Smoothness");


        [SerializeField]
        private Mesh _mesh = default;

        [SerializeField]
        private Material _material = default;

        private Matrix4x4[] _matrices = new Matrix4x4[1023];
        private Vector4[] _baseColors = new Vector4[1023];
        float[] metallic = new float[1023];
        float[] smoothness = new float[1023];

        private MaterialPropertyBlock _block;


        private void Awake()
        {
            for (int i = 0; i < _matrices.Length; i++)
            {
                _matrices[i] = Matrix4x4.TRS(Random.insideUnitSphere * 10f,
                    Quaternion.Euler(Random.value * 360f, Random.value * 360f, Random.value * 360f),
                    Vector3.one * Random.Range(0.5f, 1.5f));
                _baseColors[i] = new Vector4(Random.value, Random.value, Random.value, Random.Range(0.5f, 1f));
                metallic[i] = Random.value < 0.25f ? 1 : 0f;
                smoothness[i] = Random.Range(0.05f, 0.95f);
            }
        }

        private void Update()
        {
            if (_block == null)
            {
                _block = new MaterialPropertyBlock();
                _block.SetVectorArray(baseColorId, _baseColors);
                _block.SetFloatArray(metallicID, metallic);
                _block.SetFloatArray(smoothnessID, smoothness);
            }

            Graphics.DrawMeshInstanced(_mesh, 0, _material, _matrices, _matrices.Length, _block);
        }
    }
}