## Features
### TileLayer
 - Map drawing utilities: DrawRectangle, FillRectangle, DrawLine
 - CompositeTile stencil to help map's designing
### SpriteLayer
 - P2 Scaling SpriteSheets
 - P3 AnimatedSpriteDefinition
 - P3 CompositeSprite
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
 - Audio?
 - P4 Entities
  - Link to sprites/tiles
  - Animations state machine
 - P3 Resources manager: Screen, Audio
### ContentSync
 - Validate current content Links
### IO
 - Validate Tiled load map/tileset support
 - Fix Repositories to use proper constructor injection patterns

## Bugs
- ! WPF, DrawString Zoom and Font
- ! Scene saving point/vector/rectangle precision
- ! CameraMode.Fix to be implemented on all Gfx 
- ! Touch gesture get stuck, should we implement our own configurable gesture? (add-on utility)

## Refactorings
- Should Draw/Hit be external to core classes?

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
 - SamplesBrowser (Samples game demo - PCL)

## InTray
- Implement Scene Navigation
- Document Glosary
- Fix WP8 Samples
- Fix Win8.1 Exit with Escape (can it be fixed?)
