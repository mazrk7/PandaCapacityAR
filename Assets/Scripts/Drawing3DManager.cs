using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drawing3DManager : MonoBehaviour
{
    static Drawing3DManager s_Instance;
    public static Drawing3DManager instance
    {
        get
        {
#if UNITY_EDITOR
                if (s_Instance == null)
                {
                    s_Instance = FindObjectOfType<Drawing3DManager>();
                    if (s_Instance == null)
                    {
                        GameObject newDebugDrawObj = new GameObject("DrawingManager");
                        s_Instance = newDebugDrawObj.AddComponent<Drawing3DManager>();
                        s_Instance.m_UnlitVertexColorMaterial = new Material(Shader.Find("Unlit/VertexColor"));
                        s_Instance.m_UnlitColorMaterial = new Material(Shader.Find("Unlit/Color"));
                        s_Instance.m_UnlitColorAlphaMaterial = new Material(Shader.Find("Unlit/ColorAlpha"));
                        s_Instance.m_UnlitPointCloudMaterial = new Material(Shader.Find("Unlit/PointCloudCutout"));
                    }
                }
#endif
            return s_Instance;
        }
    }

    Camera m_Camera;
    List<Drawing3D> m_Drawings = new List<Drawing3D>();
    List<Drawing3D> m_Dirty = new List<Drawing3D>();

    [SerializeField]
    Material m_UnlitVertexColorMaterial;
    [SerializeField]
    Material m_UnlitColorMaterial;
    [SerializeField]
    Material m_UnlitColorAlphaMaterial;
    [SerializeField]
    Material m_UnlitPointCloudMaterial;

    public Material UnlitVertexColorMaterial => m_UnlitVertexColorMaterial;
    public Material UnlitColorMaterial => m_UnlitColorMaterial;
    public Material UnlitColorAlphaMaterial => m_UnlitColorAlphaMaterial;
    public Material UnlitPointCloudMaterial => m_UnlitPointCloudMaterial;

    void Awake()
    {
        s_Instance = this;
        m_Camera = Camera.main;
    }

    public void AddDirty(Drawing3D drawing)
    {
        m_Dirty.Add(drawing);
    }

    public void DestroyDrawing(Drawing3D drawing)
    {
        m_Drawings.Remove(drawing);
        GameObject.Destroy(drawing.gameObject);
    }

    public static Drawing3D CreateDrawing(float duration = -1, Material material = null)
    {
        GameObject newDrawingObj = new GameObject("Drawing");
        Drawing3D newDrawing = newDrawingObj.AddComponent<Drawing3D>();
        newDrawing.Init(material != null ? material : instance.UnlitVertexColorMaterial, duration);
        instance.m_Drawings.Add(newDrawing);
        return newDrawing;
    }


    void LateUpdate()
    {
        foreach (Drawing3D drawing in m_Dirty)
        {
            if (drawing != null)
                drawing.Refresh();
        }
        m_Dirty.Clear();
    }
}