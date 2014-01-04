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

        private SpriteMap entityMap;

        public ShootEmUpScreen(ScreenNavigation screenNavigation)
        {
            this.screenNavigation = screenNavigation;
        }

        public override void Initialize(Camera theCamera)
        {
            this.camera = theCamera;

            this.camera.Center = CameraCenter.WindowTopLeft;
        }

        public override InputConfiguration GetInputConfiguration()
        {
            this.inputConfiguration = new InputConfiguration();

            this.inputConfiguration.AddDigitalButton("Back").Assign(KeyboardKeys.Escape)
                .MapClickTo(gt => this.screenNavigation.NavigateBack());

            this.inputConfiguration.AddDigitalButton("Left").Assign(KeyboardKeys.Left);
            this.inputConfiguration.AddDigitalButton("Right").Assign(KeyboardKeys.Right);
            this.inputConfiguration.AddDigitalButton("Up").Assign(KeyboardKeys.Up);
            this.inputConfiguration.AddDigitalButton("Down").Assign(KeyboardKeys.Down);
            this.inputConfiguration.AddDigitalButton("Fire Weapon").Assign(KeyboardKeys.Space);

            return this.inputConfiguration;
        }

        public override void LoadContent(GameResourceManager theResourceManager)
        {
            this.gameResourceManager = theResourceManager;
        }

        private PlayerShipEntity playerShipEntity;

        public override void Update(IGameTiming gameTime)
        {
            this.playerShipEntity.Update(gameTime);
        }

        public override Scene GetScene()
        {
            this.scene = new Scene("MainScene");

            var spriteSheet = CreateSpriteSheet();

            entityMap = new SpriteMap("EntityMap");
            this.scene.AddMap(entityMap);

            this.playerShipEntity = new PlayerShipEntity(entityMap, spriteSheet);
            this.playerShipEntity.BindController(this.inputConfiguration);

            var yellowSprite = new Sprite(spriteSheet, "YellowEnemy") { Position = new Point(250, 100) };
            var redSprite = new Sprite(spriteSheet, "RedEnemy") { Position = new Point(300, 100) };
            var blueSprite = new Sprite(spriteSheet, "BlueEnemy") { Position = new Point(350, 100) };

            entityMap.AddSprite(yellowSprite);
            entityMap.AddSprite(redSprite);
            entityMap.AddSprite(blueSprite);

            return this.scene;
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