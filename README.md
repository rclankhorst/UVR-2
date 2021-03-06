# UVR-2

An experiment in Unity with Oculus Quest (Oculus Link) grabbing items with hand tracking.

## How to download

Note: creating an archive from Github will **not** download any asset files (LFS) which are committed to this repository (as of now, this is 368 MB).

In order to get these asset files, clone the repository on your local system instead (ensure you have Git + Git LFS installed).

## Built with

- Unity 2020.3.0f1 (LTS release as of early 2021)
- A development environment: MS Visual Studio, Visual Studio Code, or JetBrains Rider

## What am I looking at?

An empty scene with the only setup being proper hand tracking.

The floating sphere in front of you you can grab using your right hand, pinching the index finger.

The floating sphere carries the `GrabbableObject` component.

The right hand has the `PinchController` component, and does some magic.

## What needs to be done?

- Retaining velocity from the hand's rigidbody at the time of releasing the grabbed object.
- (Optionally) remove the rigidbody's `isKinematic` assignment in `PinchController.OnPinchEnd`.
