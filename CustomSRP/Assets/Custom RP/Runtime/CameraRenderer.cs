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

    private CommandBuffer _buffer = new()
    {
        name = _bufferName
    };

    private CullingResults _cullingResults;

    private static ShaderTagId _unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");


    /// <summary>
    /// Draw all geometry that camera can see  
    /// </summary>
    public void Render(ScriptableRenderContext context, Camera camera, bool useDynamicBatching, bool useGPUInstancing)
    {
        this.context = context;
        this.camera = camera;

        PrepareBuffer();
        PrepareForSceneWindow();

        if (!Cull())
            return;

        Setup();
        DrawVisibleGeometry(useDynamicBatching, useGPUInstancing);
        DrawUnsupportedShaders();
        DrawGizmos();
        Submit();
    }

    void Setup()
    {
        // Apply the camera's properties to context, including matrix as well as some other properties
        context.SetupCameraProperties(camera);
        CameraClearFlags flags = camera.clearFlags;
        //Clear the earlier render target
        //TODO: what will happen if remove this
        _buffer.ClearRenderTarget(flags <= CameraClearFlags.Depth, flags <= CameraClearFlags.Color, flags == CameraClearFlags.Color ? camera.backgroundColor.linear : Color.clear);

        _buffer.BeginSample(SampleName);
        ExecuteBuffer();
    }

    void DrawVisibleGeometry(bool useDynamicBatching, bool useGPUInstancing)
    {
        var sortSettings = new SortingSettings(camera)
        {
            criteria = SortingCriteria.CommonOpaque
        };
        var drawingSettings = new DrawingSettings(_unlitShaderTagId, sortSettings)
        {
            enableDynamicBatching = useDynamicBatching,
            enableInstancing = useGPUInstancing,
        };
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
        _buffer.EndSample(SampleName);
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