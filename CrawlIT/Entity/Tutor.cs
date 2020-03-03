using System;
using System.Collections.Generic;
using System.Text;
using CrawlIT.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

class Tutor : Enemy
{
    public Tutor(Texture2D texture, Texture2D closeUpTexture, float posx, float posy, int fightsLeft, int questionPerFight)
        : base(texture, closeUpTexture, posx, posy, fightsLeft, questionPerFight)
    {
        
    }

    public override void BeatenBy(Player player)
    {
        player.LifeCount++;
        this.FightsLeft = Math.Max(0, this.FightsLeft - 1);
        if (this.FightsLeft == 0)
        {
            // Behaviour when no more questions
        }
    }

    public override void Beat(Player player)
    {
        // TODO: Insert dialogue when you lose against tutor
    }
}
