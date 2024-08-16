using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Xml;
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
        CheckCollisions(gameTime);

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

    void CheckCollisions(GameTime gameTime)
    {
        float dt = (float)gameTime.ElapsedGameTime.TotalSeconds;
        foreach (Sprite sprite in sprites)
        {
            if (rect.Intersects(sprite.rect))
            {
                if (sprite.type == "spikes" || sprite.type == "enemy" || rect.Y > 5000) {
                    audio["die"].Play();
                    velocity = Vector2.Zero;
                    rect.X = (int)defaultPos.X;
                    rect.Y = (int)defaultPos.Y;
                }
            }
            if (rect.Intersects(sprite.rect))
            {
                if (sprite.type == "coin") {
                    audio["coin"].Play();
                    coinsCollected += 1;
                    sprite.rect.X = 10000;
                    sprite.rect.Y = 10000;
                }
            }
            if (rect.Intersects(sprite.rect))
            {
                if (sprite.type == "bouncepad") {
                    velocity.Y = -jumpHeight * 2f * dt;
                }
            }
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
