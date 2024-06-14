using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mono;

public class PhysicsSprite : AnimatedSprite
{
    protected Vector2 velocity = new Vector2(0, 0);
    protected int gravity = 35;
    protected float airTime = 0;
    protected List<Sprite> sprites;

    public PhysicsSprite(Dictionary<string, Animation> animations, Rectangle rectangle, List<Sprite> sprites, string type) : base(animations, rectangle, type)
    {
        this.sprites = sprites;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        Move(gameTime);

        if (velocity.X > 0){
            flip = false;
        }
        if (velocity.X < 0) {
            flip = true;
        }
    }

    void Move(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        airTime += dt;

        rect.X += (int)velocity.X;
        foreach (Sprite terrainSprite in sprites)
        {
            if (terrainSprite.type == "terrain" && rect.Intersects(terrainSprite.rect))
            {
                if (velocity.X > 0) {
                    rect.X = terrainSprite.rect.Left - rect.Width;
                }
                else if (velocity.X < 0) {
                    rect.X = terrainSprite.rect.Right;
                }
            }
        }

        
        velocity.Y += gravity * dt;
        rect.Y += (int)velocity.Y;
        foreach (Sprite terrainSprite in sprites)
        {
            if (terrainSprite.type == "terrain" && rect.Intersects(terrainSprite.rect))
            {
                if (velocity.Y > 0) {
                    rect.Y = terrainSprite.rect.Top - rect.Height;
                    velocity.Y = 0;
                    airTime = 0;
                }
                else if (velocity.Y < 0) {
                    rect.Y = terrainSprite.rect.Bottom;
                    velocity.Y = 0;
                }
            }
        }
    }
}
