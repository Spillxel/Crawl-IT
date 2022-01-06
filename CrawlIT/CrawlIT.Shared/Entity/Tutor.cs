using System;
using CrawlIT.Shared;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

class Tutor : Enemy
{
    public Tutor(ContentManager content, string texture, string closeUpTexture, Vector2 position, Point frame, int fights, int questions)
        : base(content, texture, closeUpTexture, position, frame, fights, questions)
    {
    }

    public override void BeatenBy(Player player)
    {
        player.SetLifeCount(++player.LifeCount);
        this.Fights = Math.Max(0, this.Fights - 1);
        if (this.Fights == 0)
        {
            // Behaviour when no more questions
        }
    }

    public override void Beat(Player player)
    {
        // TODO: Insert dialogue when you lose against tutor
    }
}
