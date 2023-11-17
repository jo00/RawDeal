using System.Security.Cryptography;
using RawDealView;

namespace RawDeal;

public class StoneColdHability : Hability
{

    public StoneColdHability(Player player, PlayerTurnsManager playerTurnsManager, View view, DecksManager decksManager) : base(player, playerTurnsManager, view, decksManager)
    {
    }

   

    public override bool AskBeforeUsingIt()
    {
        return false;
    }

    public override bool CanPlayerUseHability()
    {
        if (player.arsenal.Count > 0)
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
        return false;
    }

    public override bool ApplyDuringMainSegment()
    {
        return true;
    }
    
    public override void ApplyEffect()
    {
        _view.SayThatPlayerIsGoingToUseHisAbility(thisSuperstar.Name, thisSuperstar.SuperstarAbility);
        _decksManager.DrawCardsFromArsenalToHand(1);
        _view.SayThatPlayerDrawCards(thisSuperstar.Name, 1);
        ReturnCardFromHandToArsenal();
    }
    
    public void ReturnCardFromHandToArsenal()
    {
        List<string> strHand = _decksManager.FormatCardSetIntoString(player.hand);
        int idOfCardToReturn =
            _view.AskPlayerToReturnOneCardFromHisHandToHisArsenal(thisSuperstar.Name, strHand);
        Card returnedCard = player.hand[idOfCardToReturn];
        player.arsenal.Insert(0, returnedCard);
        player.hand.RemoveAt(idOfCardToReturn);

    }


    
}