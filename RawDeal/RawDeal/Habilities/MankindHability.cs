using System.Security.Cryptography;
using RawDealView;

namespace RawDeal;

public class MankindHability : Hability
{

    public MankindHability(JugadorData jugadorData, Jugador jugador, View view) : base(jugadorData, jugador, view)
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
        return false;
    }

    public override bool ApplyDuringMainSegment()
    {
        return false;
    }
    
    public override void ApplyEffect()
    {
        Console.WriteLine("su habilidad es automatica");
    }

    
}