using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mono;

public class Spider : PhysicsSprite
{
    protected float speed = 250;
    float direction = 1;
    float timeSinceSwitch = 0;

    public Spider(Dictionary<string, Animation> animations, Rectangle rectangle, List<Sprite> sprites, string type) : base(animations, rectangle, sprites, type)
    {
        this.sprites = sprites;
    }

    public override void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        base.Update(gameTime);
        velocity.X = speed * direction * dt;
        foreach (Sprite sprite in sprites) {
            if (sprite.type == "enemy_barrier") {
                if (rect.Intersects(sprite.rect) && timeSinceSwitch > 1) {
                    timeSinceSwitch = 0;
                    direction *= -1;
                }
            }
        }
        timeSinceSwitch += dt;
    }
}

public class Bird : Spider
{
    public Bird(Dictionary<string, Animation> animations, Rectangle rectangle, List<Sprite> sprites, string type) : base(animations, rectangle, sprites, type)
    {
        gravity = 0;
        speed = 120;
    }
}
