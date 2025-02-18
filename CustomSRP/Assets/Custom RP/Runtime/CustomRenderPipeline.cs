using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomRenderPipeline : RenderPipeline
{
    private CameraRenderer _cameraRender = new CameraRenderer();
    
    
    
    protected override void Render(ScriptableRenderContext context, List<Camera> cameras)
    {
        for (int i = 0; i < cameras.Count; i++)
        {
            _cameraRender.Render(context, cameras[i]);
        }
    }

    protected override void Render(ScriptableRenderContext context, Camera[] cameras)
    {
        
    }
}
