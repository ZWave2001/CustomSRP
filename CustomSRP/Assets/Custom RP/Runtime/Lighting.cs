using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Lighting
{
    private const string _bufferName = "Lighting";

    private static int _dirLightColorId = Shader.PropertyToID("_DirectionalLightColor");
    private static int _dirLightDirectionId = Shader.PropertyToID("_DirectionalLightDirection");
    private CullingResults _cullingResults;

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
    
    private void SetupDirectionalLight()
    {
        Light light = RenderSettings.sun;
        _buffer.SetGlobalVector(_dirLightColorId, light.color.linear * light.intensity);
        _buffer.SetGlobalVector(_dirLightDirectionId, -light.transform.forward);
    }

    private void SetupLights()
    {
        NativeArray<VisibleLight> visibleLights = _cullingResults.visibleLights;
    }
}