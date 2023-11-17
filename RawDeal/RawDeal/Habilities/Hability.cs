using RawDealView;

namespace RawDeal;

public abstract class Hability
{
    protected Player player;
    protected PlayerTurnsManager playerTurnsManager;
    protected View _view;
    protected Superstar thisSuperstar;
    protected DecksManager _decksManager;
    

    public Hability(Player player, PlayerTurnsManager playerTurnsManager, View view, DecksManager decksManager)
    {
        this.player = player;
        this.playerTurnsManager = playerTurnsManager;
        _view = view;
        _decksManager = decksManager;
        thisSuperstar = this.player.superstar;
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
                return _view.DoesPlayerWantToUseHisAbility(player.superstar.Name);
            }

            else
            { 
                return true;
            }
        }

        return false;
    }



}