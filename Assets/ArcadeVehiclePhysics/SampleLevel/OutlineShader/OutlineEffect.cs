using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class OutlineEffect : MonoBehaviour
{
    Camera camera;
    public Material material;

    public List<ShaderVariable> shaderVariables = new List<ShaderVariable>();

    private void Awake()
    {
        if (camera == null)
        {
            camera = GetComponent<Camera>();
            camera.depthTextureMode = DepthTextureMode.DepthNormals;
        }
    }

    // Postprocess the image
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        foreach (ShaderVariable shaderVariable in shaderVariables)
        {
            if (shaderVariable.varName != "")
            {
                material.SetFloat(shaderVariable.varName, shaderVariable.varValue);
            }
        }

        Graphics.Blit(source, null, material);
    }

    [System.Serializable]
    public class ShaderVariable
    {
        public string varName;
        public float varValue;
    }
}
