using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace mono;

public class LevelLoader
{   
    public List<Sprite> sprites = new();

    Dictionary<string, Dictionary<Vector2, int>> tilemaps;

    Dictionary<string, dynamic> assets;
    Dictionary<string, dynamic> audio;

    public int level = 0;
    public readonly List<Rectangle> tileSource = new() {
        new Rectangle(0, 0, 16, 16),
        new Rectangle(16, 0, 16, 16),
        new Rectangle(32, 0, 16, 16),

        new Rectangle(0, 16, 16, 16),
        new Rectangle(16, 16, 16 ,16),
        new Rectangle(32, 16, 16 ,16),

        new Rectangle(0, 32, 16, 16),
        new Rectangle(16, 32, 16, 16),
        new Rectangle(32, 32, 16, 16),

        new Rectangle(0, 48, 16, 16),
        new Rectangle(16, 48, 16, 16),
        new Rectangle(32, 48, 16, 16),

        new Rectangle(0, 64, 16, 16),
        new Rectangle(16, 64, 16 ,16),
        new Rectangle(32, 64, 16 ,16),

        new Rectangle(0, 80, 16, 16),
        new Rectangle(16, 80, 16, 16),
        new Rectangle(32, 80, 16, 16)
    };

    public LevelLoader(Dictionary<string, dynamic> assets, Dictionary<string, dynamic> audio)
    {
        this.assets = assets;
        this.audio = audio;
        ResetTilemaps();
    }

    public void Update() {
        foreach (dynamic sprite in sprites.ToList()) {
            if (sprite.type == "player") {
                if (sprite.levelComplete == true) {
                    try {
                        level += 1;
                        ResetTilemaps();
                        LoadContent();
                    }
                    catch {
                        level = 0;
                        ResetTilemaps();
                        LoadContent(); 
                    }
                    sprite.levelComplete = false;
                }
            }
        }
    }

    public void ResetTilemaps()
    {
        tilemaps = new() {
            ["terrain"] = LoadLevel("terrain"),
            ["spikes"] = LoadLevel("spikes"),
            ["player"] = LoadLevel("player"),
            ["spiders"] = LoadLevel("spiders"),
            ["birds"] = LoadLevel("birds"),
            ["enemyBarriers"] = LoadLevel("enemy_barrier"),
            ["stars"] = LoadLevel("stars"),
            ["bouncepads"] = LoadLevel("bouncepads"),
            ["coins"] = LoadLevel("coins")
        };
        sprites.Clear();
    }

    private Dictionary<Vector2, int> LoadLevel(string layer)
    {
        Dictionary<Vector2, int> result = new();
        StreamReader reader = new("levels/" + level + "/" + level + "_" + layer + ".csv");

        int y = 0;
        string line;
        while ((line = reader.ReadLine()) != null) {
            string[] items = line.Split(',');

            for (int x = 0; x < items.Length; x++) {
                if (Convert.ToInt32(items[x]) > -1) {
                    result[new Vector2(x, y)] = Convert.ToInt32(items[x]);
                }
            }

            y++;
        }

        return result;
    }

    public void LoadContent()
    {   
        foreach (var item in tilemaps["terrain"]) {
            sprites.Add(new Sprite(
                texture: assets["terrain"], 
                rectangle: new Rectangle((int)item.Key.X * 16, (int)item.Key.Y * 16, 16, 16), 
                source: new Vector2(tileSource[item.Value].X, tileSource[item.Value].Y), 
                type: "terrain"
            ));
        }

        foreach (var item in tilemaps["stars"]) {
            sprites.Add(new Sprite(
                texture: assets["star"], 
                rectangle: new Rectangle((int)item.Key.X * 16, (int)item.Key.Y * 16, 16, 16), 
                source: new Vector2(tileSource[item.Value].X, tileSource[item.Value].Y), 
                type: "star"
            ));
        }

        foreach (var item in tilemaps["coins"]) {
            sprites.Add(new Sprite(
                texture: assets["coin"], 
                rectangle: new Rectangle((int)item.Key.X * 16, (int)item.Key.Y * 16, 10, 10), 
                source: new Vector2(tileSource[item.Value].X, tileSource[item.Value].Y), 
                type: "coin"
            ));
        }

        foreach (var item in tilemaps["bouncepads"]) {
            sprites.Add(new Sprite(
                texture: assets["bouncepad"], 
                rectangle: new Rectangle((int)item.Key.X * 16, (int)item.Key.Y * 16, 16, 6), 
                source: new Vector2(tileSource[item.Value].X, tileSource[item.Value].Y), 
                type: "bouncepad"
            ));
        }

        foreach (var item in tilemaps["player"]) {
            sprites.Add(new Player(
                animations: assets["player"], 
                rectangle: new Rectangle((int)item.Key.X * 16, (int)item.Key.Y * 16, 16, 14),
                sprites: sprites,
                audio: audio,
                type: "player"
            ));
        }

        foreach (var item in tilemaps["spiders"]) {
            sprites.Add(new Spider(
                animations: assets["spider"], 
                rectangle: new Rectangle((int)item.Key.X * 16, (int)item.Key.Y * 16, 16, 10), 
                sprites: sprites,
                type: "enemy"
            ));
        }

        foreach (var item in tilemaps["birds"]) {
            sprites.Add(new Bird(
                animations: assets["bird"], 
                rectangle: new Rectangle((int)item.Key.X * 16, (int)item.Key.Y * 16, 15, 13), 
                sprites: sprites,
                type: "enemy"
            ));
        }

        foreach (var item in tilemaps["enemyBarriers"]) {
            sprites.Add(new Sprite(
                texture: assets["transparent"], 
                rectangle: new Rectangle((int)item.Key.X * 16 - 1, (int)item.Key.Y * 16 - 1, 18, 18), 
                source: new Vector2(tileSource[item.Value].X, tileSource[item.Value].Y), 
                type: "enemy_barrier"
            ));
        }

        foreach (var item in tilemaps["spikes"]) {
            sprites.Add(new Sprite(
                texture: assets["spikes"], 
                rectangle: new Rectangle((int)item.Key.X * 16, (int)item.Key.Y * 16, 16, 5), 
                source: new Vector2(tileSource[item.Value].X, tileSource[item.Value].Y), 
                type: "spikes"
            ));
        }
    }
}