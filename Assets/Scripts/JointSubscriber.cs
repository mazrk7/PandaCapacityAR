using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Sensor;
using UnityEngine;
using System.Collections;

public class JointSubscriber : MonoBehaviour
{
    [SerializeField]
    string topicName = "/joint_states";
    [SerializeField]
    GameObject robot;

    // ROS Connector
    ROSConnection mRos;

    private ArticulationBody[] articulationChain;

    const int activeDof = 7;

    void Start()
    {
        // Get ROS connection static instance
        mRos = ROSConnection.GetOrCreateInstance();
        articulationChain = robot.GetComponentsInChildren<ArticulationBody>();
        mRos.Subscribe<JointStateMsg>(topicName, ReceiveJoints);
    }

    private void ReceiveJoints(JointStateMsg jntsMsg)
    {
        StartCoroutine(SetJointValues(jntsMsg));
    }

    IEnumerator SetJointValues(JointStateMsg jntsMsg)
    {

        for (int i = 0; i < activeDof; i++)
        {
            var joint = articulationChain[i + 1];
            var jointDrive = joint.xDrive;
            jointDrive.target = (float)(jntsMsg.position[i]) * Mathf.Rad2Deg;
            joint.xDrive = jointDrive;
        }

        yield return new WaitForSeconds(0.5f);
    }
}