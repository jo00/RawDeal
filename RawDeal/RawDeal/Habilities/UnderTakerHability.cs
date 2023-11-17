using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal;

public class UndertakerHability : Hability
{
    private int _uses;

    public UndertakerHability(Player player, PlayerTurnsManager playerTurnsManager, View view, DecksManager decksManager) : base(player, playerTurnsManager, view, decksManager)
    {
    }

   

    public override bool AskBeforeUsingIt()
    {
        return false;
    }
    
    public override bool CanPlayerUseHability()
    {
        if (_uses == 0)
        {
            if (player.hand.Count >= 2)
            {
                return true;
            }
            else
            {
                return false;
            }   
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
        DiscardCardFromHand(2);
        DiscardCardFromHand(1);

        //no se estan agregando a las opciones de recuperacion las que acaba de descartar
        RescueCardFromRingArea();
    }

    public void DiscardCardFromHand(int numberOfCardsLeftToDiscard)
    {
        List<string> strHand = _decksManager.FormatCardSetIntoString(player.hand);
        int idOfCardToDiscard =
            _view.AskPlayerToSelectACardToDiscard(strHand, thisSuperstar.Name, thisSuperstar.Name, numberOfCardsLeftToDiscard);
        Card discardedCard = player.hand[idOfCardToDiscard];
        player.ringside.Add(discardedCard);
        player.hand.RemoveAt(idOfCardToDiscard);

    }

    public void RescueCardFromRingArea()
    {
        List<string> strRingside = _decksManager.FormatCardSetIntoString(player.ringside);
        int idCardToRecover = _view.AskPlayerToSelectCardsToPutInHisHand(thisSuperstar.Name, 1, strRingside);
        Card pickedCard = player.ringside[idCardToRecover];
        player.hand.Add(pickedCard);
    }
    
    



    
}