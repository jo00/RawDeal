using System.Security.Cryptography;
using RawDealView;

namespace RawDeal;

public class MankindHability : Hability
{

    public MankindHability(Player player, PlayerTurnsManager playerTurnsManager, View view, DecksManager decksManager) : base(player, playerTurnsManager, view, decksManager)
    {
    }

   

    public override bool AskBeforeUsingIt()
    {
        return false;
    }

    public override bool CanPlayerUseHability()
    {
        return true;
    }

    public override bool ApplyBeforeDrawing()
    {
        return false;
    }

    public override bool ApplyDuringMainSegment()
    {
        return false;
    }
    
    public override void ApplyEffect()
    {
        Console.WriteLine("su habilidad es automatica");
    }

    
}