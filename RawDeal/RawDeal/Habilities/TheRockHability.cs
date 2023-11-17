using RawDeal;

using RawDealView;
using RawDealView.Formatters;

public class TheRockHability : Hability
{
    
    public TheRockHability(Player player, PlayerTurnsManager playerTurnsManager, View view, DecksManager decksManager) : base(player, playerTurnsManager, view, decksManager)
    {
       
    }

    public override bool AskBeforeUsingIt()
    {
        return true;
    }

    public override bool CanPlayerUseHability()
    {
        if (player.ringside.Count > 0)
        {
            return true;
 
        }
        else
        {
            return false;
        }
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
        List<string> strRingside = _decksManager.FormatCardSetIntoString(player.ringside);
        int recoveredCardId = _view.AskPlayerToSelectCardsToRecover(player.superstar.Name, 1, strRingside);
        _decksManager.MoveCardFromRingsideToArsenal(recoveredCardId);
        
    }
}