using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mono;

public class CoinCounter
{   
    List<Sprite> sprites;
    Dictionary<string, dynamic> assets;
    int coins = 0;
    int totalCoins = 0;

    public CoinCounter(List<Sprite> sprites, Dictionary<string, dynamic> assets) 
    {
        this.sprites = sprites;
        this.assets = assets;
    }

    public void Update()
    {
        totalCoins = 0;
        foreach (dynamic sprite in sprites)
        {
            if (sprite.type == "player")
            {
                coins = sprite.coinsCollected;
            }
            if (sprite.type == "coin")
            {
                totalCoins += 1;
            }
        }
    }

    public void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics)
    {
        string text = coins + "/" + totalCoins;
        _spriteBatch.DrawString(
            assets["font"], 
            text, 
            new Vector2(
                _graphics.PreferredBackBufferWidth - assets["font"].MeasureString(text).X/2 - 30,
                assets["font"].MeasureString(text).X/2
            ), 
            Color.Black
        );
        _spriteBatch.Draw(
            assets["coin"], 
            new Rectangle(
                _graphics.PreferredBackBufferWidth - 100,
                12,
                assets["coin"].Width * 4,
                assets["coin"].Height * 4
            ),
            Color.White
        );
    }
}