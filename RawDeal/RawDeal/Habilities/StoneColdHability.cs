using System.Security.Cryptography;
using RawDealView;

namespace RawDeal;

public class StoneColdHability : Hability
{

    public StoneColdHability(JugadorData jugadorData, Jugador jugador, View view) : base(jugadorData, jugador, view)
    {
    }

   

    public override bool AskBeforeUsingIt()
    {
        return false;
    }

    public override bool CanPlayerUseHability()
    {
        if (_jugadorData.arsenal.Count > 0)
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
        _jugador.DrawCardsFromArsenalToHand(1);
        _view.SayThatPlayerDrawCards(thisSuperstar.Name, 1);
        ReturnCardFromHandToArsenal();
    }
    
    public void ReturnCardFromHandToArsenal()
    {
        List<string> strHand = _jugador.FormatCardSetIntoString(_jugadorData.hand);
        int idOfCardToReturn =
            _view.AskPlayerToReturnOneCardFromHisHandToHisArsenal(thisSuperstar.Name, strHand);
        Card returnedCard = _jugadorData.hand[idOfCardToReturn];
        _jugadorData.arsenal.Insert(0, returnedCard);
        _jugadorData.hand.RemoveAt(idOfCardToReturn);

    }


    
}