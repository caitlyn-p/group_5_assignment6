using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace group_5_assignment6;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    
    private Texture2D _particleTexture;
    private List<Sparkler> _particles;// list so more sparkles can be continuously added
    private Texture2D _wand;
    
    //floats for sparkler location
    private float _emitterX;
    private float _emitterY;
    
    //variables for fountain firework
    private Texture2D _fountainParticle;

    private List<FountainParticle> _fountainParticles;
    
    private bool fountainOn = true;
    private KeyboardState previousKeyboard;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here
        
        _particles = new List<Sparkler>();

        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 600;
        _graphics.ApplyChanges();

        _emitterX = 550f; 
        _emitterY = 400f;

        base.Initialize();
        
        //list of particles for fountain firework
        _fountainParticles = new List<FountainParticle>();

    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        //sparkler images
        _particleTexture = Content.Load<Texture2D>("sparkle");
        _wand = Content.Load<Texture2D>("wand");
        
        //using circles for fountain firework
        _fountainParticle = CreateCircleTexture(12);

        //spawning in particles on bottom left of window, adjusted to match caitlyn's buffer size
        Vector2 spawnPos = new Vector2(400, 300);
        
        //adding in particles to list for fountain
        _fountainParticles = new List<FountainParticle>();

        for (int i = 0; i < 120; i++)
        {
            _fountainParticles.Add(
                new FountainParticle(_fountainParticle, spawnPos)
            );
        }
    }
    
    //creating a particle shape for fountain fireworks
    private Texture2D CreateCircleTexture(int radius)
    {
        int diameter = radius * 2;

        Texture2D texture =
            new Texture2D(GraphicsDevice, diameter, diameter);

        Color[] data = new Color[diameter * diameter];

        Vector2 center = new Vector2(radius);

        for (int y = 0; y < diameter; y++)
        {
            for (int x = 0; x < diameter; x++)
            {
                float distance =
                    Vector2.Distance(new Vector2(x,y), center);

                if (distance <= radius)
                    data[y * diameter + x] = Color.White;
                else
                    data[y * diameter + x] = Color.Transparent;
            }
        }

        texture.SetData(data);

        return texture;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        //add three sparkles every frame
        for (int i = 0; i < 3; i++)
        {
            _particles.Add(new Sparkler(_particleTexture, _emitterX, _emitterY));
        }
        //update all existing particles
        for (int i = _particles.Count - 1; i >= 0; i--)
        {
            _particles[i].Update(gameTime);
            //remove particles after lifespan is ended
            if (!_particles[i].IsAlive)
                _particles.RemoveAt(i);
        }
        
        //adding in keyboard inputs so that the fountain pauses and resumes when the F key is pressed
        KeyboardState keyboard = Keyboard.GetState();

        if (keyboard.IsKeyDown(Keys.F) &&
            previousKeyboard.IsKeyUp(Keys.F))
        {
            fountainOn = !fountainOn;
        }

        previousKeyboard = keyboard;

        if (fountainOn)
        {
            foreach (FountainParticle p in _fountainParticles)
            {
                //applying 2 forces to the particles
                p.ApplyGravity();
                p.Update();
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        //draw sparkler wand
        _spriteBatch.Draw(
            _wand,
            new Vector2(400, 400),
            null,
            Color.White ,
            0f,
            Vector2.Zero,
            0.5f,
            SpriteEffects.None,
            0f
        );
        //draw sparklers
        foreach (Sparkler part in _particles)
        {
            part.Draw(_spriteBatch);
        }
        
        //drawing fountain particles
        foreach (FountainParticle p in _fountainParticles)
        {
            p.Draw(_spriteBatch);
        }
        
        _spriteBatch.End();
        

        base.Draw(gameTime);
    }
}