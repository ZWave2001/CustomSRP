using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

public class CustomShaderGUI : ShaderGUI
{
    private MaterialEditor _editor;
    private Object[] _materials;
    MaterialProperty[] _materialProperties;

    private bool _showPresets;

    bool Clipping
    {
        set => SetProperty("_Clipping", "_CLIPPING", value);
    }

    bool PremultiplyAlpha
    {
        set => SetProperty("_PremulAlpha", "_PREMULTIPLY_ALPHA", value);
    }


    BlendMode SrcBlendMode
    {
        set => SetProperty("_SrcBlend", (float)value);
    }

    BlendMode DstBlendMode
    {
        set => SetProperty("_DstBlend", (float)value);
    }


    bool ZWrite
    {
        set => SetProperty("_ZWrite", value ? 1 : 0);
    }


    RenderQueue RenderQueue
    {
        set
        {
            foreach (Material m in _materials)
            {
                m.renderQueue = (int)value;
            }
        }
    }
    
    bool HasPremultiplyAlpha => HasProperty("_PremulAlpha");


    bool PresetButton(string name)
    {
        if (GUILayout.Button(name))
        {
            _editor.RegisterPropertyChangeUndo(name);
            return true;
        }

        return false;
    }

    void OpaquePreset()
    {
        if (PresetButton("Opaque"))
        {
            Clipping = false;
            PremultiplyAlpha = false;
            SrcBlendMode = BlendMode.One;
            DstBlendMode = BlendMode.Zero;
            ZWrite = true;
            RenderQueue = RenderQueue.Geometry;
        }
    }

    void ClipPreset()
    {
        if (PresetButton("Clip"))
        {
            Clipping = true;
            PremultiplyAlpha = false;
            SrcBlendMode = BlendMode.One;
            DstBlendMode = BlendMode.Zero;
            ZWrite = true;
            RenderQueue = RenderQueue.AlphaTest;
        }
    }

    void FadePreset()
    {
        if (PresetButton("Fade"))
        {
            Clipping = false;
            PremultiplyAlpha = false;
            SrcBlendMode = BlendMode.SrcAlpha;
            DstBlendMode = BlendMode.OneMinusSrcAlpha;
            ZWrite = false;
            RenderQueue = RenderQueue.Transparent;
        }
    }


    void TransparentPreset()
    {
        if (HasPremultiplyAlpha && PresetButton("Transparent"))
        {
            Clipping = false;
            PremultiplyAlpha = true;
            SrcBlendMode = BlendMode.One;
            DstBlendMode = BlendMode.OneMinusSrcAlpha;
            ZWrite = false;
            RenderQueue = RenderQueue.Transparent;
        }
    }


    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        base.OnGUI(materialEditor, properties);
        _editor = materialEditor;
        _materialProperties = properties;
        _materials = materialEditor.targets;


        EditorGUILayout.Space();
        _showPresets = EditorGUILayout.Foldout(_showPresets, "Presets", true);
        if (_showPresets)
        {
            OpaquePreset();
            ClipPreset();
            FadePreset();
            TransparentPreset();
        }
    }

    private bool SetProperty(string name, float value)
    {
        var property = FindProperty(name, _materialProperties);
        if (property != null)
        {
            property.floatValue = value;
            return true;
        }

        return false;
    }

    void SetKeyword(string keyword, bool enabled)
    {
        if (enabled)
        {
            foreach (Material mat in _materials)
            {
                mat.EnableKeyword(keyword);
            }
        }
        else
        {
            foreach (Material mat in _materials)
            {
                mat.DisableKeyword(keyword);
            }
        }
    }

    void SetProperty(string name, string keyword, bool value)
    {
        if (SetProperty(name, value ? 1 : 0))
        {
            SetKeyword(keyword, value);
        }
    }
    
    bool HasProperty(string name) => FindProperty(name, _materialProperties) != null;
}