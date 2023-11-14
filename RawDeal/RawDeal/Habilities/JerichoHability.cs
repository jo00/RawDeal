using System.Security.Cryptography;
using RawDealView;

namespace RawDeal;

public class JerichoHability : Hability
{

    public JerichoHability(JugadorData jugadorData, Jugador jugador, View view) : base(jugadorData, jugador, view)
    {
    }

   

    public override bool AskBeforeUsingIt()
    {
        return true;
    }

    public override bool CanPlayerUseHability()
    {
        if (_jugadorData.hand.Count >= 1)
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
        Superstar opponentSuperstar = _jugadorData.oponenteData.superstar;

        List<string> strHand = _jugadorData.oponente.FormatCardSetIntoString(_jugadorData.oponenteData.hand);
        int idOfCardToDiscard =
            _view.AskPlayerToSelectACardToDiscard(strHand, opponentSuperstar.Name, opponentSuperstar.Name, 1);
        Card discardedCard = _jugadorData.oponenteData.hand[idOfCardToDiscard];
        _jugadorData.oponenteData.ringside.Add(discardedCard);
        _jugadorData.oponenteData.hand.RemoveAt(idOfCardToDiscard);
    }
    public void DiscardCardFromHand()
    {
        List<string> strHand = _jugador.FormatCardSetIntoString(_jugadorData.hand);
        int idOfCardToDiscard =
            _view.AskPlayerToSelectACardToDiscard(strHand, thisSuperstar.Name, thisSuperstar.Name, 1);
        Card discardedCard = _jugadorData.hand[idOfCardToDiscard];
        _jugadorData.ringside.Add(discardedCard);
        _jugadorData.hand.RemoveAt(idOfCardToDiscard);
    }


}