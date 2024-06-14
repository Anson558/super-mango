using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Threading.Tasks.Dataflow;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mono;

public class Sprite
{
    public bool flip;
    public readonly int SCALE = 4;
    protected readonly int TILESIZE = 16;
    public string type;

    public Texture2D texture;
    public Rectangle rect;
    public Vector2 defaultPos;
    public Vector2 source;

    public Sprite(Texture2D texture, Rectangle rectangle, Vector2 source, string type)
    {
        this.source = source;
        this.texture = texture;
        this.type = type;
        rect = new Rectangle(rectangle.X * SCALE, rectangle.Y * SCALE, rectangle.Width * SCALE, rectangle.Height * SCALE);
        rect.Offset(new Point(0, TILESIZE - rect.Height));
        defaultPos = new Vector2(rect.X, rect.Y);
    }

    public virtual void Update(GameTime gameTime)
    {
  
    }

    public virtual void Draw(SpriteBatch _spriteBatch, Vector2 scroll)
    {
        _spriteBatch.Draw(
            texture: texture,
            position: new Vector2(rect.X - scroll.X, rect.Y - scroll.Y),
            sourceRectangle: new Rectangle((int)source.X, (int)source.Y, rect.Width / SCALE, rect.Height / SCALE),
            color: Color.White,
            rotation: 0,
            origin: Vector2.Zero,
            scale: SCALE,
            effects: flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
            layerDepth: 0
        );
    }
}

public class AnimatedSprite : Sprite
{
    public Dictionary<string, Animation> animations;
    public string state = "idle";

    public AnimatedSprite(Dictionary<string, Animation> animations, Rectangle rectangle, string type) : base(animations["idle"].texture, rectangle, new Vector2(0, 0), type)
    {
        this.animations = animations;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        animations[state].Update(gameTime);
    }

    public override void Draw(SpriteBatch _spriteBatch, Vector2 scroll)
    {
        _spriteBatch.Draw(
            texture: animations[state].texture,
            position: new Vector2(rect.X - scroll.X, rect.Y - scroll.Y),
            sourceRectangle: new Rectangle(animations[state].image, 0, rect.Width / SCALE, rect.Height / SCALE),
            color: Color.White,
            rotation: 0,
            origin: Vector2.Zero,
            scale: SCALE,
            effects: flip ? SpriteEffects.FlipHorizontally : SpriteEffects.None,
            layerDepth: 0
        );
    }
}
