using RosMessageTypes.JskRecognition;
using System.Collections.Generic;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;

public class PolytopeSubscriber : MonoBehaviour
{
    // Topic variables required for ROS communication
    [SerializeField]
    string topicName = "/panda/polytope";
    [SerializeField]
    GameObject parentObj;
    [SerializeField]
    Color faceColor;
    [SerializeField]
    Color edgeColor;
    [SerializeField]
    float lineThickness;

    Dictionary<string, Drawing3D> mDrawingNamespace = new Dictionary<string, Drawing3D>();

    void Start()
    {
        // Get ROS connection static instance
        ROSConnection.GetOrCreateInstance().Subscribe<PolygonArrayMsg>(topicName, OnPolygonArray);
    }

    /*private void Update()
    {
        polytopeVisualizer = GetComponent<PolytopeVisualizer>();

        if (isReceived)
        {
            if (polytopeVisualizer != null)
            {
                polytopeVisualizer.SetPolygonsArray(polyArr);
                polytopeVisualizer.SetPolytopeMaterial(topicName, polytopeMaterial);
            }

            isReceived = false;
        }
    }*/

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

    private void OnPolygonArray(PolygonArrayMsg polyArrMsg)
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

        // Redraw polytope drawing
        drawing.Clear();
        Draw<FLU>(polyArrMsg, drawing, faceColor, edgeColor, lineThickness);
    }

    public static void Draw<C>(PolygonArrayMsg polyArrMsg, Drawing3D drawing, Color fColor, Color eColor, float thickness) where C : ICoordinateSpace, new()
    {
        int polyLength = polyArrMsg.polygons.Length;

        int numPoints = 0;
        for (int i = 0; i < polyLength; i++)
        {
            numPoints += polyArrMsg.polygons[i].polygon.points.Length;
        }

        if (numPoints > 0)
        {
            for (int i = 0; i < polyLength; i++)
            {
                var polygon = polyArrMsg.polygons[i].polygon;
                Vector3[] triPoints = new Vector3[polygon.points.Length];
                for (int j = 0; j < 3; j++)
                {
                    triPoints[j] = polygon.points[j].From<C>();
                }

                drawing.DrawTriangle(triPoints[0], triPoints[1], triPoints[2], fColor);
                drawing.DrawLines(triPoints, eColor, thickness);
                // Need to fill in Mesh, doesn't automatically in Unity so fill in connections
                Vector3[] fillInPoints = new Vector3[] { triPoints[0], triPoints[2], triPoints[1] };
                drawing.DrawTriangle(fillInPoints[0], fillInPoints[1], fillInPoints[2], fColor);
                drawing.DrawLines(fillInPoints, eColor, thickness);
            }
        }
    }
}