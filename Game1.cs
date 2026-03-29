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
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        
        //sparkler images
        _particleTexture = Content.Load<Texture2D>("sparkle");
        _wand = Content.Load<Texture2D>("wand");
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
            _particles[i].Update();
            //remove particles after lifespan is ended
            if (!_particles[i].IsAlive)
                _particles.RemoveAt(i);
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

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}