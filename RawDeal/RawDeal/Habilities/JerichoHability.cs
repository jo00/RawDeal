using System.Security.Cryptography;
using RawDealView;

namespace RawDeal;

public class JerichoHability : Hability
{

    public JerichoHability(Player player, PlayerTurnsManager playerTurnsManager, View view, DecksManager decksManager) : base(player, playerTurnsManager, view, decksManager)
    {
    }

   

    public override bool AskBeforeUsingIt()
    {
        return true;
    }

    public override bool CanPlayerUseHability()
    {
        if (player.hand.Count >= 1)
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
        DiscardCardFromHand();
        DiscardCardFromOpponentsHand();
        
        
    }

    public void DiscardCardFromOpponentsHand()
    {
        Superstar opponentSuperstar = player.oponenteData.superstar;

        List<string> strHand = _decksManager.FormatCardSetIntoString(player.oponenteData.hand);
        int idOfCardToDiscard =
            _view.AskPlayerToSelectACardToDiscard(strHand, opponentSuperstar.Name, opponentSuperstar.Name, 1);
        Card discardedCard = player.oponenteData.hand[idOfCardToDiscard];
        player.oponenteData.ringside.Add(discardedCard);
        player.oponenteData.hand.RemoveAt(idOfCardToDiscard);
    }
    public void DiscardCardFromHand()
    {
        List<string> strHand = _decksManager.FormatCardSetIntoString(player.hand);
        int idOfCardToDiscard =
            _view.AskPlayerToSelectACardToDiscard(strHand, thisSuperstar.Name, thisSuperstar.Name, 1);
        Card discardedCard = player.hand[idOfCardToDiscard];
        player.ringside.Add(discardedCard);
        player.hand.RemoveAt(idOfCardToDiscard);
    }


}