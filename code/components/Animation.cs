using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mono;

public class Animation
{
    public Texture2D texture;
    public int image;
    int imageSize;
    float frameTime;
    int textureIndex = 0;
    float timer = 0;
    readonly int TILESIZE = 16;

    public Animation(Texture2D texture, float frameTime)
    {   
        this.texture = texture;
        this.frameTime = frameTime;
        imageSize = this.texture.Width / TILESIZE;
    }

    public void Update(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        timer += dt;
        image = textureIndex * TILESIZE;
        if (timer > frameTime)
        {
            if (textureIndex < imageSize - 1) {
                textureIndex += 1;
            }
            else {
                textureIndex = 0;
            }
            timer = 0;
        }
    }
}
