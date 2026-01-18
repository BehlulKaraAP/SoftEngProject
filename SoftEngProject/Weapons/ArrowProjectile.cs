using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftEngProject.Levels;


namespace SoftEngProject.Weapons
{
    internal class ArrowProjectile
    {
        private readonly Texture2D texture;

        public Vector2 Position;
        public Vector2 Velocity;

        public int Damage { get; } = 1;
        public bool IsExpired { get; private set; }

        private float lifeTimer = 3.0f;

        private readonly int hitboxOffsetX;
        private readonly int hitboxOffsetY;
        private readonly int hitboxWidth;
        private readonly int hitboxHeight;

        public Rectangle Hitbox => new Rectangle(
            (int)Position.X + hitboxOffsetX,
            (int)Position.Y + hitboxOffsetY,
            hitboxWidth,
            hitboxHeight);

        public ArrowProjectile(Texture2D texture, Vector2 position, Vector2 velocity, int hitboxWidth = 40, int hitboxHeight = 8, int hitBoxOffsetX = 20, int hitboxOffsetY = 28)
        {
            this.texture = texture;
            Position = position;
            Velocity = velocity;
            this.hitboxWidth = hitboxWidth;
            this.hitboxHeight = hitboxHeight;

            this.hitboxOffsetX = hitBoxOffsetX;
            this.hitboxOffsetY = hitboxOffsetY;
        }

        public void DebugDrawHitbox(SpriteBatch spriteBatch, Texture2D pixel, Color color, int thickness = 2)
        {
            var r = Hitbox;
            // top
            spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Top, r.Width, thickness), color);
            // bottom
            spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Bottom - thickness, r.Width, thickness), color);
            // left
            spriteBatch.Draw(pixel, new Rectangle(r.Left, r.Top, thickness, r.Height), color);
            // right
            spriteBatch.Draw(pixel, new Rectangle(r.Right - thickness, r.Top, thickness, r.Height), color);
        }

        public void Update(GameTime gameTime, Level level)
        {
            float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;

            Position += Velocity * dt;

            lifeTimer -= dt;
            if (lifeTimer <= 0f)
                IsExpired = true;

            if (!level.WorldBounds.Intersects(Hitbox))
                IsExpired = true;

            if (TouchesSolidTile(level))
                IsExpired = true;
        }

        private bool TouchesSolidTile(Level level)
        {
            int tileSize = level.TileSize;
            int[,] map = level.Map;
            var hb = Hitbox;

            int leftTile = Clamp(hb.Left / tileSize, 0, map.GetLength(1) - 1);
            int rightTile = Clamp((hb.Right - 1) / tileSize, 0, map.GetLength(1) - 1);
            int topTile = Clamp(hb.Top / tileSize, 0, map.GetLength(0) - 1);
            int bottomTile = Clamp((hb.Bottom - 1) / tileSize, 0, map.GetLength(0) - 1);

            for (int y = topTile; y <= bottomTile; y++)
                for (int x = leftTile; x <= rightTile; x++)
                    if (map[y, x] == 1)
                        return true;

            return false;
        }

        private int Clamp(int v, int min, int max)
        {
            if (v < min) return min;
            if (v > max) return max;
            return v;
        }

        public void Draw(SpriteBatch spriteBatch, SpriteEffects fx = SpriteEffects.None)
        {
            spriteBatch.Draw(texture, Position, null, Color.White, 0f, Vector2.Zero, 1f, fx, 0f);
        }
    }
}
