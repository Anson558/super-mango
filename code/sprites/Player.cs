using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace mono;

public class Player : PhysicsSprite
{
    int speed = 360;
    int jumpHeight = 850;
    Dictionary<string, dynamic> audio;
    public bool levelComplete = false;
    public int coinsCollected = 0;

    public Player(Dictionary<string, Animation> animations, Rectangle rectangle, List<Sprite> sprites, Dictionary<string, dynamic> audio, string type) : base(animations, rectangle, sprites, type) 
    { 
        this.animations = animations;
        this.audio = audio;
    }

    public override void Update(GameTime gameTime)
    {   
        CheckDeath();
        CheckCompletion();
        CheckCoins();

        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        KeyboardState kState = Keyboard.GetState();

        if (kState.IsKeyDown(Keys.D)) {
            velocity.X = speed * dt;
        }
        else if (kState.IsKeyDown(Keys.A)) {
            velocity.X = -speed * dt;
        }
        else {
            velocity.X = 0;
        }

        if (kState.IsKeyDown(Keys.W)) {
            if (airTime < dt){
                velocity.Y = -jumpHeight * dt;
                audio["jump"].Play();
            }
        }

        if (airTime > 0.1)
            state = "jump";
        else if (Math.Abs(velocity.X) > 0)
            state = "run";
        else
            state = "idle";

        base.Update(gameTime);
    }

    void CheckDeath()
    {
        foreach (Sprite sprite in sprites)
        {
            if (rect.Intersects(sprite.rect))
            {
                if (sprite.type == "spikes" || sprite.type == "enemy" || rect.Y > 2000) {
                    audio["die"].Play();
                    rect.X = (int)defaultPos.X;
                    rect.Y = (int)defaultPos.Y;
                }
            }
        }
    }

    void CheckCoins() 
    {
        for (var i = 0; i < sprites.Count; i++)
        {
            if (rect.Intersects(sprites[i].rect))
            {
                if (sprites[i].type == "coin") {
                    audio["coin"].Play();
                    coinsCollected += 1;
                    sprites[i].rect.X = 10000;
                    sprites[i].rect.Y = 10000;
                }
            }
        }
    }

    void CheckCompletion() 
    {
        foreach (Sprite sprite in sprites)
        {
            if (rect.Intersects(sprite.rect))
            {
                if (sprite.type == "star") {
                    audio["level_complete"].Play();
                    levelComplete = true;
                }
            }
        }
    }
}
