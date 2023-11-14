using RawDeal;

using RawDealView;
using RawDealView.Formatters;

public class TheRockHability : Hability
{
    
    public TheRockHability(JugadorData jugadorData, Jugador jugador, View view) : base(jugadorData, jugador, view)
    {
       
    }

    public override bool AskBeforeUsingIt()
    {
        return true;
    }

    public override bool CanPlayerUseHability()
    {
        if (_jugadorData.ringside.Count > 0)
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
        List<string> strRingside = _jugador.FormatCardSetIntoString(_jugadorData.ringside);
        int recoveredCardId = _view.AskPlayerToSelectCardsToRecover(_jugadorData.superstar.Name, 1, strRingside);
        _jugador.MoveCardFromRingsideToArsenal(recoveredCardId);
        
    }
}