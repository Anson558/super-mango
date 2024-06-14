using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mono;

public class Menu
{
    Dictionary<string, dynamic> assets;
    Dictionary<string, dynamic> audio;

    public Menu(Dictionary<string, dynamic> assets, Dictionary<string, dynamic> audio) 
    {
        this.assets = assets;
        this.audio = audio;
    }

    public void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager _graphics)
    {
        _spriteBatch.Draw(
            assets["title"], 
            new Rectangle(
                _graphics.PreferredBackBufferWidth/2 - assets["title"].Width/2 * 6,
                _graphics.PreferredBackBufferHeight/2 - assets["title"].Height/2 * 6,
                assets["title"].Width * 6,
                assets["title"].Height * 6
            ),
            Color.White
        );

        string text = "Space to Start";
        _spriteBatch.DrawString(
            assets["font"], text, 
            new Vector2(
                _graphics.PreferredBackBufferWidth/2 - assets["font"].MeasureString(text).X/2, 
                _graphics.PreferredBackBufferHeight - 240
            ), 
            Color.Black
        );
    }
}
