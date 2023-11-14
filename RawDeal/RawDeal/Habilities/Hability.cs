using RawDealView;

namespace RawDeal;

public abstract class Hability
{
    protected JugadorData _jugadorData;
    protected Jugador _jugador;
    protected View _view;
    protected Superstar thisSuperstar;

    

    public Hability(JugadorData jugadorData, Jugador jugador, View view)
    {
        _jugadorData = jugadorData;
        _jugador = jugador;
        _view = view;
        thisSuperstar = _jugadorData.superstar;
    }
    
    public abstract bool CanPlayerUseHability();
    public abstract bool ApplyBeforeDrawing();
    public abstract bool ApplyDuringMainSegment();
    public abstract void ApplyEffect();
    public abstract bool AskBeforeUsingIt();
    
    public bool IsGoingToUseHability()
    {
        if (CanPlayerUseHability())
        {
            if (AskBeforeUsingIt())
            {
                return _view.DoesPlayerWantToUseHisAbility(_jugadorData.superstar.Name);
            }

            else
            { 
                return true;
            }
        }

        return false;
    }



}