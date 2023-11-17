namespace RawDeal;
using RawDealView;
using RawDealView.Options;
using RawDealView.Formatters;

public class HabilityManager
{
    private Player _player;
    private PlayerTurnsManager _playerTurnsManager;
    private View view;
    private Hability hability;
    private int maxUses = 100;

    private DecksManager _decksManager;
    public HabilityManager(Player player, View view, PlayerTurnsManager playerTurnsManager, DecksManager decksManager)
    {
        this._player = player;
        this.view = view;
        this._playerTurnsManager = playerTurnsManager;
        _decksManager = decksManager;



    }
    public void LoadSuperstarHability()
    {
        Superstar specificSuperStar = _player.superstar;

        switch (specificSuperStar.Name)
        {
            case "HHH":
                hability = new HhhHability(_player, _playerTurnsManager, view, _decksManager);
                break;
            
            case "THE UNDERTAKER":
                hability = new UndertakerHability(_player, _playerTurnsManager, view, _decksManager);
                maxUses = 1;
                break;
            
            case "THE ROCK":
                hability = new TheRockHability(_player, _playerTurnsManager, view, _decksManager);
                break;
            
            case "KANE":
                hability = new KaneHability(_player, _playerTurnsManager, view, _decksManager);
                break;
            
            case "CHRIS JERICHO":
                hability = new JerichoHability(_player, _playerTurnsManager, view, _decksManager);
                maxUses = 1;
                break;
            
            case "MANKIND":
                hability = new MankindHability(_player, _playerTurnsManager, view, _decksManager);
                _player.manKindType = true;
                break;
            case "STONE COLD STEVE AUSTIN":
                hability = new StoneColdHability(_player, _playerTurnsManager, view, _decksManager);
                maxUses = 1;
                break;

            
            default:
                throw new InvalidOperationException($"Unhandled superstar: {specificSuperStar.Name}");
        }

    }

    public void AddHabilityToPlayer()
    {
        _player.LoadHability(hability);
    }

    public int GetHowManyTimesItCanBeUsed()
    {
        return maxUses;
    }

    public void UseHabilityIfApplicable()
    {
        if(hability.ApplyBeforeDrawing())
        {
            if (hability.IsGoingToUseHability())
            {
                hability.ApplyEffect();
            }
        }
    }

    public bool CheckIfUsingHabilityIsPossibleDuringMainSegment(int usesOfHability)

    {
        if (hability.ApplyDuringMainSegment())
        {
            if (hability.CanPlayerUseHability() && usesOfHability < maxUses)
            {
                return true;
            }
        }

        return false;
    }


}