using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


[CreateAssetMenu(fileName = "CustomSRP", menuName = "Rendering/Custom Render Pipeline Asset")]
public class CustomRenderPipelineAsset : RenderPipelineAsset
{
    [SerializeField]
    private bool _useDynamicBatching, _useGPUInstancing, _useSRPBatcher;
    
    protected override RenderPipeline CreatePipeline()
    {
        return new CustomRenderPipeline(_useDynamicBatching, _useGPUInstancing, _useSRPBatcher);
    }
}