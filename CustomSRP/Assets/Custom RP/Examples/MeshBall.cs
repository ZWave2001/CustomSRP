﻿using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Custom_RP.Examples
{
    public class MeshBall : MonoBehaviour
    {
        static int baseColorId = Shader.PropertyToID("_BaseColor");

        [SerializeField]
        private Mesh _mesh = default;

        [SerializeField]
        private Material _material = default;

        private Matrix4x4[] _matrices = new Matrix4x4[1023];
        private Vector4[] _baseColors = new Vector4[1023];

        private MaterialPropertyBlock _block;


        private void Awake()
        {
            for (int i = 0; i < _matrices.Length; i++)
            {
                _matrices[i] = Matrix4x4.TRS(Random.insideUnitSphere * 10f, Quaternion.identity, Vector3.one);
                _baseColors[i] = new Vector4(Random.value, Random.value, Random.value, Random.Range(0.5f,1f));
            }
        }

        private void Update()
        {
            if (_block == null)
            {
                _block = new MaterialPropertyBlock();
                _block.SetVectorArray(baseColorId, _baseColors);
            }
            Graphics.DrawMeshInstanced(_mesh, 0, _material, _matrices, _matrices.Length, _block);
        }
    }
}