using GameFramework;
using GameFramework.Cameras;
using GameFramework.Inputs;
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

        public override void Update(IGameTiming gameTime)
        {
            this.playerShipEntity.Update(gameTime);
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

            var spriteSheet = this.CreateSpriteSheet();

            this.entityLayer = new SpriteLayer("EntityMap");
            scene.Add(this.entityLayer);

            this.playerShipEntity = new PlayerShipEntity(this.entityLayer, spriteSheet);
            this.playerShipEntity.BindController(this.InputConfiguration);

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

            spriteSheet.CreateSpriteDefinition("Ship", new Rectangle(1, 1, 32, 32));
            spriteSheet.CreateSpriteDefinition("YellowEnemy", new Rectangle(44, 37, 15, 23));
            spriteSheet.CreateSpriteDefinition("RedEnemy", new Rectangle(79, 37, 15, 23));
            spriteSheet.CreateSpriteDefinition("BlueEnemy", new Rectangle(114, 37, 15, 23));

            spriteSheet.CreateSpriteDefinition("YellowShot", new Rectangle(54, 1, 5, 9));

            return spriteSheet;
        }
    }
}