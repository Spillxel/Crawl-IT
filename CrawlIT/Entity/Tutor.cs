using System;
using CrawlIT.Shared;
using Microsoft.Xna.Framework.Content;

class Tutor : Enemy
{
    public Tutor(ContentManager content, string texture, string closeUpTexture, float posx, float posy, int fightsLeft, int questionPerFight)
        : base(content, texture, closeUpTexture, posx, posy, fightsLeft, questionPerFight)
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
