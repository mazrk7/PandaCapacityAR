# PandaCapacityAR

Unity HoloLens project to visualize Panda capacity calculations in Augmented Reality (AR). Complements the [pycapacity](https://github.com/auctus-team/pycapacity) package and interfaces with ROS architectures using the [Unity Robotics Hub](https://github.com/Unity-Technologies/Unity-Robotics-Hub).

## Prerequisites

 * [HoloLens 2 dependencies](https://learn.microsoft.com/en-us/windows/mixed-reality/develop/install-the-tools)
 * [Unity 2021.3.18f1 LTS](https://learn.unity.com/tutorial/install-the-unity-hub-and-editor#)
 * [MRTK2](https://learn.microsoft.com/en-us/windows/mixed-reality/mrtk-unity/mrtk2/?view=mrtkunity-2022-05)
 * [Git LFS](https://git-lfs.com/) to track larger files

### Windows 10 vs. 11

The Unity app built by this project can be deployed through Visual Studio on both Windows 10 and 11. However, please note that it is simpler through Windows 10. 

If you are using Windows 10, then skip straight to the "Setup" instructions. Else consider the below points for Windows 11 as you navigate the "Setup":
- Make sure you are targeting a Windows 10 SDK (e.g., _10.0.19041.0_ or _10.0.18362.0_) in **both** your Unity UWP build and the Visual Studio debugging settings. Maybe things would also work if targeting a Windows 11 SDK, but this has not been tested yet.
- Errors will still possibly occur due to a **0x80070005 code** when deploying from the Visual Studio solution. Follow Step 1 in [this link](https://www.makeuseof.com/fix-the-windows-access-denied-error-0x80070005/) to give yourself full administrative permissions. You may to do this AFTER first building the Unity project (see below)
- If you encounter an error like "Unable to start debugging" or "Operation not supported. Unknown error: **0x80070057**" when deploying with debugging from Visual Studio, then see [this link](https://learn.microsoft.com/en-us/windows/mixed-reality/develop/advanced-concepts/using-visual-studio?tabs=hl2) on potential causes. You can probably still run the app without debugging, so don't worry too much about this.

## Setup & Installation

After acquiring all the prerequisites, you can begin by cloning this project repository.

Open the project in Unity and follow these steps:
1) If you have troubles opening the project on a first setup, with a "compilation errors" message and the option between entering in "Safe Mode" or "Ignore", then choose **Ignore**.
2) An MRTK window _may_ appear on your first setup. Simply skip through the steps, clicking only "Import TMP Essentials" when prompted.
3) Ensure that the "MainScene" is active. Open the "MainScene" in the Unity Editor by selecting it from "File->Open Scene".
4) **Important:** Errors will show up when you first configure this project. This is fine, but in the Unity Editor you will need to configure the project to target UWP ARM64. Go to "File->Build Settings", set the Platform as "UWP" and change the architecture type to "ARM64".
5) The project should currently be configured for deployment alongside ROS Noetic packages, with a connection bridge enabled by the [ROS-TCP-Connector](https://github.com/Unity-Technologies/ROS-TCP-Connector). Please note that if using ROS alongside this HoloLens app, then you will need to enable "Connect on Startup" under the Editor's "Robotics->ROS Settings" panel AND enter your ROS machine's IP address in the "ROS IP Address" field. You should also follow [these instructions](https://github.com/Unity-Technologies/Unity-Robotics-Hub/blob/main/tutorials/ros_unity_integration/setup.md#-ros-environment) on how to configure your ROS workspace.

Build the project (storing it in a "Builds" directory or equivalent).

In addition to these instructions, general instructions on deploying apps from Visual Studio to the HoloLens can be found [here](https://learn.microsoft.com/en-us/windows/mixed-reality/develop/advanced-concepts/using-visual-studio?tabs=hl2).