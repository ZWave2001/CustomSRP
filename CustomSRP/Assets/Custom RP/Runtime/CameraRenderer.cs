using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer
{
    public ScriptableRenderContext context;
    public Camera camera;

    private const string _bufferName = "Render Camera";

    private CommandBuffer _buffer = new CommandBuffer()
    {
        name = _bufferName
    };
    private CullingResults _cullingResults;
    
    private static ShaderTagId _unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");

   
    
    
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
        DrawUnsupportedShaders();
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
        var sortSettings = new SortingSettings(camera)
        {
            criteria = SortingCriteria.CommonOpaque 
        };
        var drawingSettings = new DrawingSettings(_unlitShaderTagId, sortSettings);
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
        
        //Draw opaque first, then the skybox, finally transparent things
        context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);

        context.DrawSkybox(camera);
        
        sortSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;
        
        context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
        
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


   