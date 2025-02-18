using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraRender
{
    ScriptableRenderContext context;
    Camera camera;

    private const string _bufferName = "Render Camera";

    private CommandBuffer _buffer = new CommandBuffer()
    {
        name = _bufferName
    };
    private CullingResults _cullingResults;
    
    /// <summary>
    /// Draw all geometry that camera can see  
    /// </summary>
    public void Render(ScriptableRenderContext context, Camera camera)
    {
        this.context = context;
        this.camera = camera;

        if (!Cull())
            return;
        
        Setup();
        DrawVisibleGeometry();
        Submit();
    }
    
    void Setup()
    {
        // Apply the camera's properties to context, including matrix as well as some other properties
        context.SetupCameraProperties(camera);
        //Clear the earlier render target
        //TODO: what will happen if remove this
        _buffer.ClearRenderTarget(true, true, Color.clear);
        
        _buffer.BeginSample(_bufferName);
        ExecuteBuffer();
    }
    void DrawVisibleGeometry()
    {
        context.DrawSkybox(camera);
    }

    /// <summary>
    /// Submit the actual rendering
    /// </summary>
    void Submit()
    {
        _buffer.EndSample(_bufferName);
        ExecuteBuffer();
        context.Submit();
    }


    void ExecuteBuffer()
    {
        context.ExecuteCommandBuffer(_buffer);
        _buffer.Clear();
    }


    bool Cull()
    {
        if (camera.TryGetCullingParameters(out var p))
        {
            _cullingResults = context.Cull(ref p);
            return true;
        }

        return false;
    }
}


   