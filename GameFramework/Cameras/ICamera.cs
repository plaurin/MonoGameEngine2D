namespace GameFramework.Cameras
{
    public interface ICamera
    {
        Vector Position { get; }
        
        float ZoomFactor { get; }

        Viewport Viewport { get; }
        
        Viewport SceneViewport { get; }
        
        Vector SceneTranslationVector { get; }
        Vector ViewPortCenter { get; }

        Vector GetSceneTranslationVector(Vector parallaxScrollingVector);
    }
}