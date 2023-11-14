using RawDealView;

namespace RawDeal;

public class KaneHability : Hability
{

    public KaneHability(JugadorData jugadorData, Jugador jugador, View view) : base(jugadorData, jugador, view)
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
        _jugador.MakeDamage(1);
    }

    
}