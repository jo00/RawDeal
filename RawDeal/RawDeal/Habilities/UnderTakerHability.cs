using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using RawDealView;
using RawDealView.Formatters;

namespace RawDeal;

public class UndertakerHability : Hability
{
    private int _uses;

    public UndertakerHability(JugadorData jugadorData, Jugador jugador, View view) : base(jugadorData, jugador, view)
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
            if (_jugadorData.hand.Count >= 2)
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
        List<string> strHand = _jugador.FormatCardSetIntoString(_jugadorData.hand);
        int idOfCardToDiscard =
            _view.AskPlayerToSelectACardToDiscard(strHand, thisSuperstar.Name, thisSuperstar.Name, numberOfCardsLeftToDiscard);
        Card discardedCard = _jugadorData.hand[idOfCardToDiscard];
        _jugadorData.ringside.Add(discardedCard);
        _jugadorData.hand.RemoveAt(idOfCardToDiscard);

    }

    public void RescueCardFromRingArea()
    {
        List<string> strRingside = _jugador.FormatCardSetIntoString(_jugadorData.ringside);
        int idCardToRecover = _view.AskPlayerToSelectCardsToPutInHisHand(thisSuperstar.Name, 1, strRingside);
        Card pickedCard = _jugadorData.ringside[idCardToRecover];
        _jugadorData.hand.Add(pickedCard);
    }
    
    



    
}