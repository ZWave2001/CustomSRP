using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Lighting
{
    private const int _maxDirLightCount = 4;
    private const string _bufferName = "Lighting";

    private static int _dirLightCountId = Shader.PropertyToID("_DirectionalLightCount");
    private static int _dirLightColorId = Shader.PropertyToID("_DirectionalLightColors");
    private static int _dirLightDirectionId = Shader.PropertyToID("_DirectionalLightDirections");
    private CullingResults _cullingResults;
    
    private static Vector4[] _dirLightColors = new Vector4[_maxDirLightCount];
    private static Vector4[] _dirLightDirections = new Vector4[_maxDirLightCount];

    private CommandBuffer _buffer = new CommandBuffer()
    {
        name = _bufferName,
    };


    public void Setup(ScriptableRenderContext context, CullingResults cullingResults)
    {
        _buffer.BeginSample(_bufferName);
        _cullingResults = cullingResults;
        // SetupDirectionalLight();
        SetupLights();
        _buffer.EndSample(_bufferName);
        context.ExecuteCommandBuffer(_buffer);
        _buffer.Clear();
    }
    
    private void SetupDirectionalLight(int index, ref VisibleLight visibleLight)
    {
        _dirLightColors[index] = visibleLight.finalColor;
        _dirLightDirections[index] = -visibleLight.localToWorldMatrix.GetColumn(2);
        
    }

    private void SetupLights()
    {
        NativeArray<VisibleLight> visibleLights = _cullingResults.visibleLights;

        int dirLightCount = 0;
        for (int i = 0; i < visibleLights.Length; i++)
        {
            
            var visibleLight = visibleLights[i];
            if (visibleLight.lightType == LightType.Directional)
            {
                dirLightCount++;
                if (dirLightCount >= _maxDirLightCount)
                {
                    break;
                }
                SetupDirectionalLight(i, ref visibleLight);
            }
        }

        _buffer.SetGlobalInt(_dirLightCountId, dirLightCount);
        _buffer.SetGlobalVectorArray(_dirLightColorId, _dirLightColors);
        _buffer.SetGlobalVectorArray(_dirLightDirectionId, _dirLightDirections);
    }
}