using UnityEngine;
using UnityEngine.Rendering;

public partial class CameraRenderer
{
    partial void DrawUnsupportedShaders ();
    
#if UNITY_EDITOR
    private static ShaderTagId[] _legacyShaderTagIds =
    {
        new ShaderTagId("Always"),
        new ShaderTagId("ForwardBase"),
        new ShaderTagId("PrepassBase"),
        new ShaderTagId("Vertex"),
        new ShaderTagId("VertexLM"),
        new ShaderTagId("VertexLMRGBM"),
    };

    private static Material _errorMaterial;


    partial void DrawUnsupportedShaders()
    {
        if (_errorMaterial == null)
        {
            _errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
        }

        var drawingSettings = new DrawingSettings(_legacyShaderTagIds[0], new SortingSettings(camera))
        {
            overrideMaterial = _errorMaterial,
        };
        for (int i = 1; i < _legacyShaderTagIds.Length; i++)
        {
            drawingSettings.SetShaderPassName(i, _legacyShaderTagIds[i]);
        }

        var filteringSettings = FilteringSettings.defaultValue;
        context.DrawRenderers(_cullingResults, ref drawingSettings, ref filteringSettings);
    }
#endif
}