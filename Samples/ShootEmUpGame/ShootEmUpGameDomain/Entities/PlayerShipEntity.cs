using System;
using System.Collections.Generic;
using GameFramework;
using GameFramework.Inputs;
using GameFramework.Sprites;

namespace ShootEmUpGameDomain.Entities
{
    public class PlayerShipEntity
    {
        private readonly SpriteMap entityMap;
        private readonly SpriteSheet shipSheet;

        private Vector shipPosition = new Vector(300, 400);
        private Vector shipVelocity = Vector.Zero;
        
        private float lastFiringTime;
        private List<BulletEntity> bullets;

        public PlayerShipEntity(SpriteMap entityMap, SpriteSheet shipSheet)
        {
            this.entityMap = entityMap;
            this.shipSheet = shipSheet;

            this.SpriteReference = new SpriteReference
            {
                Position = new Point(300, 400),
                SpriteName = "Player Ship",
                ReferencedSprite = new Sprite(this.shipSheet, "Ship")
            };

            this.entityMap.AddSprite(this.SpriteReference);

            this.bullets = new List<BulletEntity>();
        }

        public SpriteReference SpriteReference { get; private set; }

        public void Update(IGameTiming gameTime)
        {
            var velocityClampRectangle = Rectangle.FromBound(-8, -5, 8, 5);
            var gameAreaClampRectangle = Rectangle.FromBound(100, 10, 700, 480 - 32);

            this.shipVelocity = this.shipVelocity.Clamp(velocityClampRectangle);
            this.shipPosition = this.shipPosition.Translate(this.shipVelocity).Clamp(gameAreaClampRectangle);

            this.SpriteReference.Position = this.shipPosition.ToPoint();

            this.shipVelocity = Vector.Zero;

            foreach (var bullet in bullets)
            {
                bullet.Update(gameTime);
            }
        }

        public void BindController(InputConfiguration inputConfiguration)
        {
            inputConfiguration.GetDigitalButton("Left").MapTo(gt => this.shipVelocity = shipVelocity.Translate(gt.ElapsedSeconds * -250, 0));
            inputConfiguration.GetDigitalButton("Right").MapTo(gt => this.shipVelocity = shipVelocity.Translate(gt.ElapsedSeconds * 250, 0));
            inputConfiguration.GetDigitalButton("Up").MapTo(gt => this.shipVelocity = shipVelocity.Translate(0, gt.ElapsedSeconds * -150));
            inputConfiguration.GetDigitalButton("Down").MapTo(gt => this.shipVelocity = shipVelocity.Translate(0, gt.ElapsedSeconds * 150));
            inputConfiguration.GetDigitalButton("Fire Weapon").MapTo(this.FireWeapon);
        }

        private void FireWeapon(IGameTiming gameTime)
        {
            if (gameTime.TotalSeconds - this.lastFiringTime > 0.05f)
            {
                this.bullets.Add(BulletEntity.Create(entityMap, shipSheet, this.shipPosition, new Vector(0, -500)));

                this.lastFiringTime = gameTime.TotalSeconds;
            }
        }
    }

    public class BulletEntity
    {
        private readonly SpriteMap entityMap;
        private readonly SpriteSheet shipSheet;
        
        private Vector position;
        private Vector velocity;
        private bool isActive;

        private BulletEntity(SpriteMap entityMap, SpriteSheet shipSheet, Vector position, Vector velocity)
        {
            this.entityMap = entityMap;
            this.shipSheet = shipSheet;
            this.position = position;
            this.velocity = velocity;
            this.isActive = true;

            this.SpriteReference = new SpriteReference
            {
                Position = this.position.ToPoint(),
                SpriteName = "Bullet",
                ReferencedSprite = new Sprite(this.shipSheet, "YellowShot")
            };

            this.entityMap.AddSprite(this.SpriteReference);
        }

        public SpriteReference SpriteReference { get; private set; }

        public static BulletEntity Create(SpriteMap entityMap, SpriteSheet shipSheet, Vector position, Vector velocity)
        {
            return new BulletEntity(entityMap, shipSheet, position, velocity);
        }

        public void Update(IGameTiming gameTime)
        {
            if (this.isActive)
            {
                var gameAreaClampRectangle = Rectangle.FromBound(100, 10, 700, 480 - 32);

                var actualVelocity = this.velocity.Scale(gameTime.ElapsedSeconds);
                this.position = this.position.Translate(actualVelocity);

                this.SpriteReference.Position = this.position.ToPoint();

                this.isActive = gameAreaClampRectangle.Intercept(this.position.ToPoint());
            }
        }
    }
}
