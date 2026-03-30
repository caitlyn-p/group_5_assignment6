using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace group_5_assignment6;

public class FountainParticle
{
    private Vector2 position;
    private Vector2 velocity;
    private Vector2 spawnPoint;

    private Texture2D texture;

    private float gravity = 0.1f;

    private float life;
    private float maxLife;

    private Color color;

    private static Random rand = new Random();
    
    public FountainParticle(Texture2D tex, Vector2 spawn)
    {
        texture = tex;
        spawnPoint = spawn;

        Reset();
    }

    private void Reset()
    {
        position = spawnPoint;

        // initial upward launch force
        float vx = (float)(rand.NextDouble() * 2 - 1);

        float vy = (float)(rand.NextDouble() * -8 - 4);

        velocity = new Vector2(vx, vy);

        maxLife = 60f + (float)rand.NextDouble() * 40f;
        life = maxLife;

        color = RandomColor();
    }

    public void ApplyGravity()
    {
        velocity.Y += gravity;
    }

    public void Update()
    {
        position += velocity;

        life--;

        // color gradually shifts
        float t = 1 - (life / maxLife);

        color = Color.Lerp(Color.Yellow, Color.Red, t);

        // reset particle
        if (life <= 0 || position.Y > 900)
            Reset();
    }

    public void Draw(SpriteBatch sb)
    {
        sb.Draw(
            texture,
            position,
            null,
            color,
            0f,
            new Vector2(texture.Width / 2f, texture.Height / 2f),
            1.0f,
            SpriteEffects.None,
            0f
        );
    }

    private Color RandomColor()
    {
        return new Color(
            rand.Next(200,255),
            rand.Next(120,255),
            rand.Next(80,255)
        );
    }
}