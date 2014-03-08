## Features
### TileLayer
 - Map drawing utilities: DrawRectangle, FillRectangle, DrawLine
 - CompositeTile stencil to help map's designing
### SpriteLayer
### HexLayer
### Layer
### Scene
 - P3 UnitTesting Saving and Loading!
### Screen
 - ScreenNavigation should handle UnloadContent and manage available memory
### Input
 - Input configuration (mouse axes)
 - Input configuration (gamepad)
### Others
 - P4 Entities
### ContentSync
 - Validate current content Links
### IO
 - Validate Tiled load map/tileset support
 - Fix Repositories to use proper constructor injection patterns

## Bugs
- ! CameraMode.Fix to be implemented on all Gfx 
- ! Touch gesture get stuck, should we implement our own configurable gesture? (add-on utility)

## Refactorings

## Quality
- Activate more StyleCop rules
- FxCop

## Samples ideas
- Shooter Game (top view)
- Tactical War Game (Hex map)
- Side scroller
- Infinite tile map (trick)
- Mouse GetHit

## Projects structure
- Base Launcher
 - Windows8Base (Windows 8.1 base game project)
 - WindowsBase (Windows desktop base game project)
 - WindowsPhoneBase (Windows Phone 8 base game project)
- Samples Launcher
 - Windows8Samples (Windows 8.1 game launcher for SamplesBrowser)
 - WindowsPhone8Samples (Windows Phone 8 game launcher for SamplesBrowser)
 - WindowsSamples (Windows desktop game launcher for SamplesBrowser)
- Utilities
 - ContentSync (Post-Build utility to sync projects Content folder)
 - ContentSyncTests (UnitTest project for ContentSync)
- Other
 - GameFramework (Framework abstraction - PCL)
 - GameFramework.IO (Framework utilities for loading/saving scenes - PCL)
 - GameNavigator (Debugging tools: screen navigator and object inspector)
 - SamplesBrowser (Samples game demo - PCL)

## InTray
- Document Glosary
- Fix WP8 Samples
- Fix Win8.1 Exit with Escape (can it be fixed?)