using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
    private CameraRenderer _cameraRender = new CameraRenderer();

    private bool _useDynamicBatching, _useGPUInstancing;
    

    public CustomRenderPipeline(bool useDynamicBatching, bool useGPUInstancing, bool useSRPBatcher)
    {
        _useDynamicBatching = useDynamicBatching;
        _useGPUInstancing = useGPUInstancing;
        GraphicsSettings.useScriptableRenderPipelineBatching = useSRPBatcher;
    }
    protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            _cameraRender.Render(context, cameras[i], _useDynamicBatching, _useGPUInstancing);
        }
    }

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        
    }
}
