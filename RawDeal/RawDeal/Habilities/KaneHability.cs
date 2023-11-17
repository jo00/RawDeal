using RawDealView;

namespace RawDeal;

public class KaneHability : Hability
{

    public KaneHability(Player player, PlayerTurnsManager playerTurnsManager, View view, DecksManager decksManager) : base(player, playerTurnsManager, view, decksManager)
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
        return true;
    }

    public override bool ApplyDuringMainSegment()
    {
        return false;
    }
    
    public override void ApplyEffect()
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(thisSuperstar.Name, thisSuperstar.SuperstarAbility);
        playerTurnsManager.MakeDamage(1);
    }

    
}