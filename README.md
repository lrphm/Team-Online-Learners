# Team-Online-Learners

## Overview
This readme provides a hyperlinked summary of the project structure, including project documentation. In addition to this, detailed instructions have been included on how to use the prototype builds and (if necessary) build a desired prototype from the corresponding code repository commit.    

## Team
Name | Experience
------------ | -------------
Laura Pham (@lrphm)| Research, analysis, user interface design, UX and front end programming
Jessica Jenkinson | Research, analysis, user interface design and UX
Carl Shield (@flambosa) | Research, analysis, user interface design, UX, front end & back end programming
Jason Yang | Research, analysis, user interface design and UX

## Project Structure
* Main Project Wiki - https://github.com/lrphm/Team-Online-Learners/wiki
* Documentation - 
* Prototype (build) - https://drive.google.com/file/d/1UNf3IWX-QUM4t-EAuVX0bW4gyam9_pWB/view?usp=sharing
* Prototype (code) - https://github.com/lrphm/Team-Online-Learners.git

## Prototype Use
Prototype | Application URL | Source Code URL
------------ | ------------- | -------------
High-Fid (final) | https://drive.google.com/file/d/1UNf3IWX-QUM4t-EAuVX0bW4gyam9_pWB/view?usp=sharing | https://github.com/lrphm/Team-Online-Learners.git  
Low-Fid | https://drive.google.com/file/d/1UNf3IWX-QUM4t-EAuVX0bW4gyam9_pWB/view?usp=sharing | https://github.com/lrphm/Team-Online-Learners/tree/3c8740fab2ffd97b6911932636073059749a0a54

Use the links in the above table in the corresponding placeholders in the below instructions to use or build the application for the desired prototype. 

*Application Use Instructions*
1. Download/unzip the zipped application folder from {*Application URL*}.
2. Go inside the unzipped folder and launch the application file *Team Online Learners.exe*.
3. Select the meeting you would like to join from the dropdown and click "Join" (if using the high-fid prototype, you can check the "Use Webcam" box to use your webcam).
4. When you are finished with the meeting, click the red hangup button in the bottom right of the screen and repeat step 3 until you have participated in all 3 meetings.

*Application Build Instructions*
1. Go to the code repositority commit for the target prototype on GitHub using {*Source Code URL*}.
2. To the top-right of the tables displaying the commit files, click the green "Code" button -> Download Zip.
3. Unzip the file, open Unity Hub and add the folder **indside** the unzipped folder to Unity Hub as a project.
4. Set the appropriate Unity version and open the project.
5. **If building mid-fid:** open the Scenes folder under Project Assets and double-click on the scene *Team Online Learners*. 
6. Select File -> Build Settings
7. Ensure config is as follows:
   * Platform = PC, Max & Linux Standalone
   * Target Platform = Windows
   * Architecture = x86_64
   * All check boxes should be unchecked
   * Compression Method = Default
8. Click Build.
9. Once the build has finished, to run the application execute the output application file and go to step 3 of *Application Use Instructions*. 
