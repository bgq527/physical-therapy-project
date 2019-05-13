# physical-therapy-project

## Projects
### Arrows

![arrows_demo](https://i.imgur.com/EeSyDdf.gif)

- Description
  - Arrow perception test, currently implemented using Unity, ARCore, built with Gradle, and using an Android phone w/ Google Cardboard Generic standards.
- Dependencies
  - Unity 2018.1.9f2
  - Android SDK Tools version 25.5.2
  - Android Build Tools
  - Android Phone supporting ARCore, with Android 8.0 Oreo and newer versions
  - JDK and JRE Update 8 Version 211
  
### AR Arrows

![ar_arrows_demo](https://i.imgur.com/uyWFvuf.gif)

<sup>Demo above is found within the DaxDev branch.</sup>

- Description
  - Arrow perception test with AR included, currently implemented using Unity, Vuforia, built with Gradle, and using an Android phone w/ Google Cardboard v1 standards.
- Dependencies
  - Unity 2018.1.9f2
  - Vuforia 7+ 
  - Android SDK Tools version 25.5.2
  - Android Build Tools
  - Android Phone supporting Vuforia, with Android 8.0 Oreo and newer versions
  - JDK and JRE Update 8 Version 211

### PhysTherapy Proj

- Description
  - Motion and human pose comparison implemented in Unity, using the Microsoft KinectV2.
- Dependencies
  - Windows OS (for support of the Kinect SDK)
  - Unity 2018.1.9f2
  - Microsoft KinectV2 SDK
  - Microsoft KinectV2 Unity Pro Plugin
- Algorithm Walkthrough
  - Load scene
    - Place comparison model by loading action model from JSON
    - Mirror comparison model (unrendered) at player model location
    - Spawn joint markers (cubes) according to joints being tracked within jointlist.csv
  - Render and update scene
    - Animate walker
    - Adjust joint marker positions according to Kinect feed
    - Calculate displacement between joint marker locations and mirrored comparison model joint locations
    - Color joint markers: red -> displacement outside threshold, green -> displacement within threshold
    - Repeat
- To Do
  - GUI
    - Allow user to load or choose multiple animations
    - Allow user to choose player model colors
    - Allow user to specify output name or username and translate to output file names
    - Allow user to specify tracked joints from GUI
    - Allow user to specify error thresholding 
    - Potentially allow user to record new animations
  - Data Recording
    - Log joint positions and displacements to file
    - Log inside/outside threshold flag with data
    - Log timestamps/framestamps
  - Other
    - Adjust joint marker shape, size, color
    - Record Demo Video
