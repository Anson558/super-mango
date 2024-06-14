using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace mono;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private LevelLoader levelLoader;
    private CoinCounter coinCounter;

    Dictionary<string, dynamic> assets;
    Dictionary<string, dynamic> audio;

    Menu menu;
    string state = "menu";

    Vector2 scroll = Vector2.Zero;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        _graphics.PreferredBackBufferWidth = 1280;
        _graphics.PreferredBackBufferHeight = 720;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        audio = new()
        {
            ["music"] = Content.Load<Song>("audio/music"),
            ["jump"] = Content.Load<SoundEffect>("audio/jump").CreateInstance(),
            ["die"] = Content.Load<SoundEffect>("audio/die").CreateInstance(),
            ["coin"] = Content.Load<SoundEffect>("audio/coin").CreateInstance(),
            ["level_complete"] = Content.Load<SoundEffect>("audio/level_complete").CreateInstance()
        };

        SoundEffect.MasterVolume = 1;
        MediaPlayer.Volume = 1;

        audio["jump"].Volume = 0.3f;
        audio["coin"].Volume = 0.4f;
        audio["die"].Volume = 0.5f;
        audio["level_complete"].Volume = 0.6f;
        MediaPlayer.IsRepeating = true;
        MediaPlayer.Play(audio["music"]);

        assets = new()
        {
            ["transparent"]= Content.Load<Texture2D>("images/transparent"),
            ["player"] = new Dictionary<string, Animation>(){
                ["idle"] = new Animation(Content.Load<Texture2D>("images/player_idle"), 0.4f),
                ["run"] = new Animation(Content.Load<Texture2D>("images/player_run"), 0.12f),
                ["jump"] = new Animation(Content.Load<Texture2D>("images/player_jump"), 1)
            },
            ["spider"] = new Dictionary<string, Animation>(){
                ["idle"] = new Animation(Content.Load<Texture2D>("images/spider"), 0.2f)
            },
            ["bird"] = new Dictionary<string, Animation>(){
                ["idle"] = new Animation(Content.Load<Texture2D>("images/bird"), 0.2f)
            },
            ["terrain"] = Content.Load<Texture2D>("images/terrain"),
            ["star"] = Content.Load<Texture2D>("images/star"),
            ["coin"] = Content.Load<Texture2D>("images/coin"),
            ["spikes"] = Content.Load<Texture2D>("images/spikes"),
            ["background"] = Content.Load<Texture2D>("images/background"),
            ["title"] = Content.Load<Texture2D>("images/title"),
            ["font"] = Content.Load<SpriteFont>("font")
        };

        menu = new Menu(assets, audio);
        levelLoader = new LevelLoader(assets, audio);
        levelLoader.LoadContent();
        coinCounter = new CoinCounter(levelLoader.sprites, assets);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        if (state == "menu")
        {
            KeyboardState kState = Keyboard.GetState();
            if (kState.IsKeyDown(Keys.Space))
                state = "game";
        }
        else if (state == "game")
        {
            levelLoader.Update();
            coinCounter.Update();

            foreach (Sprite sprite in levelLoader.sprites)
            {
                sprite.Update(gameTime);
            }

            ApplyScroll();
        }    

        base.Update(gameTime);
    }

    void ApplyScroll()
    {
        foreach (Sprite sprite in levelLoader.sprites){
            if (sprite.type == "player") {
                scroll.X += (sprite.rect.Center.X - _graphics.PreferredBackBufferWidth/2 - scroll.X)/18;
                scroll.Y += (sprite.rect.Center.Y - _graphics.PreferredBackBufferHeight/2 - scroll.Y)/18;
            }
        }
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(new Color(35, 35, 35));
 
        // TODO: Add your drawing code here
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(assets["background"], new Rectangle(0, 0, 1280, 720), Color.White);
        if (state == "menu")
            menu.Draw(_spriteBatch, _graphics);
        else if (state == "game")
        {
            foreach (var sprite in levelLoader.sprites)
            {
                sprite.Draw(_spriteBatch, scroll);
            }
            coinCounter.Draw(_spriteBatch, _graphics);
        }
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
 