using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Image Effects/Custom/BloomLines")]
public class Effect_Bloom : UnityStandardAssets.ImageEffects.PostEffectsBase
{

    [Range(0, 2)]
    public int _downsample = 0;
    public Vector2 _size = Vector2.one;
    public Shader _shader = null;
    private Material _material = null;

    public Camera _cam = null;
    private RenderTexture _Addition = null;

    public Color color;

    public int _PatchSize = 5;
    public float _Quality = 2.5f;

    public new void Start()
    {
        base.CheckResources();
        if (_Addition != null)
        {
            _Addition.Release();
        }
        _Addition = new RenderTexture(Screen.width, Screen.height, 0);
        _cam.targetTexture = _Addition;
    }

    public override bool CheckResources() {
        CheckSupport(false);

        _material = CheckShaderAndCreateMaterial(_shader, _material);
        if(_material != null)
        {
        }

        if (!isSupported)
            ReportAutoDisable();
        return isSupported;
    }

    public void OnDisable() {
        if (_material)
            DestroyImmediate(_material);
    }

    public void OnRenderImage(RenderTexture source, RenderTexture destination) {
        
        if (CheckResources() == false) {
            Graphics.Blit(source, destination);
            return;
        }

        // setup shader params
        _material.SetVector("_Color", color);
        _material.SetVector("_Size", _size);
        _material.SetFloat("_PatchSize", _PatchSize);
        _material.SetFloat("_Quality", _Quality);
        _material.SetTexture("_Addition", _Addition);
        
        source.filterMode = FilterMode.Bilinear;

        int rtW = source.width >> _downsample;
        int rtH = source.height >> _downsample;

        // downsample
       // RenderTexture rt = RenderTexture.GetTemporary(rtW, rtH, 0, source.format);

        //rt.filterMode = FilterMode.Bilinear;
        Graphics.Blit(source, destination, _material, 0);

        //Graphics.Blit(rt, destination);

        //RenderTexture.ReleaseTemporary(rt);
    }
}
