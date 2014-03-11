using GameFramework;
using GameFramework.Cameras;
using GameFramework.Inputs;
using GameFramework.Layers;
using GameFramework.Scenes;
using GameFramework.Screens;
using GameFramework.Sprites;

namespace SamplesBrowser.ShootEmUp
{
    public class ShootEmUpScreen : SceneBasedScreen
    {
        private readonly ScreenNavigation screenNavigation;

        private SpriteLayer entityLayer;
        private PlayerShipEntity playerShipEntity;

        public ShootEmUpScreen(ScreenNavigation screenNavigation)
        {
            this.screenNavigation = screenNavigation;
        }

        protected override Camera CreateCamera(Viewport viewport)
        {
            return new Camera(viewport) { Center = CameraCenter.WindowTopLeft };
        }

        protected override InputConfiguration CreateInputConfiguration()
        {
            var inputConfiguration = new InputConfiguration();

            inputConfiguration.AddDigitalButton("Back").Assign(KeyboardKeys.Escape)
                .MapClickTo(gt => this.screenNavigation.NavigateBack());

            inputConfiguration.AddDigitalButton("Left").Assign(KeyboardKeys.Left);
            inputConfiguration.AddDigitalButton("Right").Assign(KeyboardKeys.Right);
            inputConfiguration.AddDigitalButton("Up").Assign(KeyboardKeys.Up);
            inputConfiguration.AddDigitalButton("Down").Assign(KeyboardKeys.Down);
            inputConfiguration.AddDigitalButton("Fire Weapon").Assign(KeyboardKeys.Space);

            return inputConfiguration;
        }

        protected override Scene CreateScene()
        {
            var scene = new Scene("MainScene");
            scene.Add(new ColorLayer("Background", Color.CornflowerBlue));

            var spriteSheet = this.CreateSpriteSheet();

            this.entityLayer = new SpriteLayer("EntityMap");
            scene.Add(this.entityLayer);

            this.playerShipEntity = new PlayerShipEntity(this.entityLayer, spriteSheet);
            this.playerShipEntity.BindController(this.InputConfiguration);

            scene.Add(this.playerShipEntity);

            var yellowSprite = new Sprite(spriteSheet, "YellowEnemy") { Position = new Vector(250, 100) };
            var redSprite = new Sprite(spriteSheet, "RedEnemy") { Position = new Vector(300, 100) };
            var blueSprite = new Sprite(spriteSheet, "BlueEnemy") { Position = new Vector(350, 100) };

            this.entityLayer.AddSprite(yellowSprite);
            this.entityLayer.AddSprite(redSprite);
            this.entityLayer.AddSprite(blueSprite);

            return scene;
        }

        private SpriteSheet CreateSpriteSheet()
        {
            var spriteTexture = this.ResourceManager.GetTexture(@"ShootEmUp\Sprites.png");
            var spriteSheet = new SpriteSheet(spriteTexture, "SpriteSheet");

            spriteSheet.AddSpriteDefinition("Ship", new RectangleInt(1, 1, 32, 32));
            spriteSheet.AddSpriteDefinition("YellowEnemy", new RectangleInt(44, 37, 15, 23));
            spriteSheet.AddSpriteDefinition("RedEnemy", new RectangleInt(79, 37, 15, 23));
            spriteSheet.AddSpriteDefinition("BlueEnemy", new RectangleInt(114, 37, 15, 23));

            spriteSheet.AddSpriteDefinition("YellowShot", new RectangleInt(54, 1, 5, 9));

            return spriteSheet;
        }
    }
}