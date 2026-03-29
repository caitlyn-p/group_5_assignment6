using System;

namespace group_5_assignment6;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

public class Sparkler
{
    //display varibales
    private Texture2D sprite;
    private Vector2 _position;
    private Vector2 _velocity;
    private Vector2 _acceleration;
    
    //forces variables
    private float _mass;
    private float _life;//how long sparkle stays on screen
    private float _time;
    private float _oscillationStrength;
    private float _oscillationSpeed;
    //random so sparkles can have different behaviours
    private static Random rand = new Random();

    public bool IsAlive => _life > 0;

    public Sparkler(Texture2D texture, float x, float y)
    {
        sprite = texture;
        
        _position = new Vector2(x, y);

        _acceleration = Vector2.Zero;
        _mass = 1f;
        
        _life = 60;

        _velocity = new Vector2(
            rand.Next(-10, 11) / 10f,
            rand.Next(-40, -15) / 10f
        );

        _oscillationStrength = 0.4f + (float)rand.NextDouble() * 3f;
        _oscillationSpeed = 6f + (float)rand.NextDouble() * 6f;

        _time = 0f;
    }

    public void ApplyForce(Vector2 force)
    {
        _acceleration += force / _mass;
    }

    public void Update()
    {
        _time += 0.1f;//psuedo time. time increases by 1/10 every frame
        
        //gravity force
        Vector2 gravity = new Vector2(0, 0.15f * _mass);
        ApplyForce(gravity);
        
        // horizontal oscillation force
        float oscillationX = (float)Math.Sin(_time * _oscillationSpeed) * _oscillationStrength;
        ApplyForce(new Vector2(oscillationX, 0));
        
        //update sparkles on forces
        _velocity += _acceleration;
        _position += _velocity;
        
        //reset acceleration
        _acceleration = Vector2.Zero;
        
        //update life
        _life -= 1f;
    }

    public void Draw(SpriteBatch sb)
    {
        //check life cycle of sparkle
        if (!IsAlive) return;
        
        //draw sparkle
        sb.Draw(
            sprite,
            _position,
            null,
            Color.Pink ,
            0f,
            Vector2.Zero,
            0.1f,
            SpriteEffects.None,
            0f
        );
    }
}