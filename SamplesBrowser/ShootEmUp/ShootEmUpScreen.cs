using GameFramework;
using GameFramework.Cameras;
using GameFramework.Inputs;
using GameFramework.Scenes;
using GameFramework.Screens;
using GameFramework.Sprites;

namespace SamplesBrowser.ShootEmUp
{
    public class ShootEmUpScreen : ScreenBase
    {
        private readonly ScreenNavigation screenNavigation;

        private Camera camera;

        private InputConfiguration inputConfiguration;

        private GameResourceManager gameResourceManager;

        private Scene scene;

        private SpriteLayer entityLayer;

        public ShootEmUpScreen(ScreenNavigation screenNavigation)
        {
            this.screenNavigation = screenNavigation;
        }

        public override void Initialize(Viewport viewport)
        {
            this.camera = new Camera(viewport);
            this.camera.Center = CameraCenter.WindowTopLeft;

            this.CreateInputConfiguration();
        }

        public override void LoadContent(GameResourceManager theResourceManager)
        {
            this.gameResourceManager = theResourceManager;
            this.CreateScene();
        }

        private PlayerShipEntity playerShipEntity;

        public override void Update(InputContext inputContext, IGameTiming gameTime)
        {
            this.inputConfiguration.Update(inputContext, gameTime);
            this.playerShipEntity.Update(gameTime);
        }

        public override int Draw(DrawContext drawContext)
        {
            drawContext.Camera = this.camera;
            return this.scene.Draw(drawContext);
        }

        private void CreateInputConfiguration()
        {
            this.inputConfiguration = new InputConfiguration();

            this.inputConfiguration.AddDigitalButton("Back").Assign(KeyboardKeys.Escape)
                .MapClickTo(gt => this.screenNavigation.NavigateBack());

            this.inputConfiguration.AddDigitalButton("Left").Assign(KeyboardKeys.Left);
            this.inputConfiguration.AddDigitalButton("Right").Assign(KeyboardKeys.Right);
            this.inputConfiguration.AddDigitalButton("Up").Assign(KeyboardKeys.Up);
            this.inputConfiguration.AddDigitalButton("Down").Assign(KeyboardKeys.Down);
            this.inputConfiguration.AddDigitalButton("Fire Weapon").Assign(KeyboardKeys.Space);
        }

        private void CreateScene()
        {
            this.scene = new Scene("MainScene");

            var spriteSheet = this.CreateSpriteSheet();

            this.entityLayer = new SpriteLayer("EntityMap");
            this.scene.Add(this.entityLayer);

            this.playerShipEntity = new PlayerShipEntity(this.entityLayer, spriteSheet);
            this.playerShipEntity.BindController(this.inputConfiguration);

            var yellowSprite = new Sprite(spriteSheet, "YellowEnemy") { Position = new Vector(250, 100) };
            var redSprite = new Sprite(spriteSheet, "RedEnemy") { Position = new Vector(300, 100) };
            var blueSprite = new Sprite(spriteSheet, "BlueEnemy") { Position = new Vector(350, 100) };

            this.entityLayer.AddSprite(yellowSprite);
            this.entityLayer.AddSprite(redSprite);
            this.entityLayer.AddSprite(blueSprite);
        }

        private SpriteSheet CreateSpriteSheet()
        {
            var spriteTexture = this.gameResourceManager.GetTexture(@"ShootEmUp\Sprites.png");
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