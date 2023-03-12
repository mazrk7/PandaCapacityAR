using RosMessageTypes.Sensor;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using Unity.Robotics.Visualizations;
using UnityEngine;

public class PolytopeVertexSubscriber : MonoBehaviour
{
    // Topic variables required for ROS communication
    [SerializeField]
    string topicName = "/panda/polytope_vertex";
    [SerializeField]
    GameObject parentObj;
    [SerializeField]
    Color vertexColor;
    [SerializeField]
    float pointRadius = 0.05f;

    Dictionary<string, Drawing3D> mDrawingNamespace = new Dictionary<string, Drawing3D>();

    // Start is called before the first frame update
    void Start()
    {
        // Get ROS connection static instance
        ROSConnection.GetOrCreateInstance().Subscribe<PointCloudMsg>(topicName, OnCloud);
    }

    // Update is called once per frame
    void Update()
    {
        if (mDrawingNamespace.Count == 0)
        {
            Debug.Log("Waiting for " + topicName + " messages...");
        }

        foreach (KeyValuePair<string, Drawing3D> element in mDrawingNamespace)
        {
            string key = element.Key;
            element.Value.gameObject.SetActive(true);
        }
    }

    void OnCloud(PointCloudMsg cloudMsg)
    {
        Drawing3D drawing;
        if (!mDrawingNamespace.TryGetValue(topicName, out drawing))
        {
            drawing = Drawing3D.Create();
            if (parentObj)
            {
                // Set parent frame of drawing cloud object if exists
                drawing.transform.SetParent(parentObj.transform);
                drawing.transform.SetPositionAndRotation(parentObj.transform.position, parentObj.transform.rotation);
            }
            mDrawingNamespace.Add(topicName, drawing);
        }

        // Redraw cloud drawing
        drawing.Clear();
        Draw<FLU>(cloudMsg, drawing, vertexColor, pointRadius);
    }

    public static void Draw<C>(PointCloudMsg cloudMsg, Drawing3D drawing, Color color, float radius) where C : ICoordinateSpace, new()
    {
        PCLDrawing cloud = drawing.AddPointCloud(cloudMsg.points.Length);
        for (int i = 0; i < cloudMsg.points.Length; ++i)
        {
            cloud.AddPoint(cloudMsg.points[i].From<C>(), color, radius);
        }
        cloud.Bake();
    }
}