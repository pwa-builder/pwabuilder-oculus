# Next steps for getting your PWA into the Oculus Store
You've successfully generated an Oculus app package for your PWA. 😎 

Your next steps:
1. **Read Oculus PWA docs**
2. **Save your signing key** 
3. **Test your app** on your local Oculus device
4. **Submit your app package** to the Oculus Store or Oculus App Lab

Each step is explained below.

## 1. Read Oculus PWA docs

We strongly recommend reading Oculus' documentation on Oculus PWAs:

- [Overview](https://developer.oculus.com/pwa/)
- [Getting Started with Oculus PWAs](https://developer.oculus.com/documentation/web/pwa-gs/)
- [Upload and Submit PWAs](https://developer.oculus.com/documentation/web/pwa-submit-app/)

This documentation shows you how your app can be used in a VR environment, design principles for Oculus PWAs, how your apps can be discoverable through the Oculus Store or Oculus App Lab, integration into Oculus Home, and the VR/AR capabilities available to PWAs.

## 2. Save your signing key

If your zip file contains a `signing.keystore` file and a `signing-key-info.txt` file, save these files in a safe place.

You'll need these files later if you upload a new version of your app to the Oculus Store or Oculus App Lab.

If your zip download didn't contain these files, that means when you generated your Oculus app in PWABuilder, you chose `None` or `Existing` as your signing key option.

## 3. Test your app on your local Oculus device

For this step, you'll need:

- An Oculus device, such as an Oculus Quest 2
- A USB-C cable to connect your Oculus device to your PC or Mac
- The [Oculus Developer Hub app](https://developer.oculus.com/documentation/unity/ts-odh/) installed on your PC or Mac

First, verify your Oculus software is up-to-date. Turn on your Oculus device and open `Settings` -> `System` -> `Software Update`.  Your software version should be 31 or greater.

Second, enable Multitasking. In Oculus `Settings`, choose `Experimental`. Then enable `Multitasking`. (Multitasking not listed? It may already be enabled already in newer versions of Oculus software.)

Third, install [Oculus Developer Hub (ODH)](https://developer.oculus.com/documentation/unity/ts-odh/) on your PC or Mac. Follow [these steps](https://developer.oculus.com/documentation/unity/ts-odh/) to configure ODH to work with your Oculus device.

Fourth, install your PWA package onto your device. Open the Oculus Developer Hub (ODH) app on your PC or Mac, go to `My Device` -> `Apps` -> `Upload`. Choose the `.apk` file you downloaded from PWABuilder. This will install your PWA onto your Oculus device.

Once installed, you can launch your PWA from the `App Library` on your Oculus device. In the `App Library`, change the app filter to `Unknown Sources`, and you should see your app in the list. Click to launch the app.

## 4. Submit your app package to the Oculus Store or Oculus App Lab

Now that you've built and tested your PWA Oculus package, you're ready to upload your app package to the Oculus Store or Oculus App Lab.

See [Upload your PWA to the Oculus Developer Center](https://developer.oculus.com/documentation/web/pwa-submit-app/#upload-the-pwa-in-the-oculus-developer-center) for step-by-step instructions to upload your app to the Oculus Store or Oculus App Lab.

Oculus Store is currently available for Oculus-approved developers. [Oculus App Lab](https://developer.oculus.com/blog/introducing-app-lab-a-new-way-to-distribute-oculus-quest-apps/) is opened to all developers and discoverable through direct link, invitation, or exact match search. For more details, see [App Lab: A New WAy to Distribute Oculus Apps](https://developer.oculus.com/blog/introducing-app-lab-a-new-way-to-distribute-oculus-quest-apps/).

## Need more help?

If you're having trouble testing or submitting your Oculus PWA package, we're here to help. You can [open an issue](https://github.com/pwa-builder/pwabuilder/issues/new?&labels=oculus-platform,question%20%3Agrey_question%3A) and we'll help walk you through it.
