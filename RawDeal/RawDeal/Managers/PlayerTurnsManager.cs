using System.Runtime.CompilerServices;

namespace RawDeal;
using RawDealView;
using RawDealView.Options;
using RawDealView.Formatters;
public class PlayerTurnsManager
{
    private Player _player;
    private View view;
    public List<Card> playableCards;
    private List<string> strPlaysInfo;
    private int IndexSelectedCardToPlay;
    private Card chosedCardToPlay;
    private bool endOfTurn;
    private Hability hability;
    private int usesOfHabilityDuringTurn;
    private int maxUses;

    private HabilityManager _habilityManager;
    private DecksManager _decksManager;



    public PlayerTurnsManager(Player player, View view)
    {
        this._player = player;
        this.view = view;

        
        _decksManager = new DecksManager(player, view);

        _habilityManager = new HabilityManager(player, view, this, _decksManager);
        _habilityManager.LoadSuperstarHability();
        _habilityManager.AddHabilityToPlayer();

        hability = player.hability;
        maxUses = _habilityManager.GetHowManyTimesItCanBeUsed();
        

    }
    
    public void CheckIfThePlayerLostDuringHisOwnTurn()
    {
        if (_decksManager.CheckIfThereAreNoCardsLeftInTheArsenal())
        {
            LoseDuringThisTurn();
        }
    }

    public void TakeHandSize(int handSize)
    {
        _decksManager.DrawCardsFromArsenalToHand(handSize);
    }

    public void ExecuteTurn()
    {
        ExecuteBeforeDrawingSegment();
        _decksManager.DrawCardsAccordingToSupestar();
        ExecuteMainSegment();

    }

    public void ExecuteMainSegment()
    {
        while (endOfTurn == false)
        {
            ShowInformationForThisTurn();
            
            ExecuteMainSegmentMenu();
        }
    }
    
    public void ExecuteBeforeDrawingSegment()
    {
        usesOfHabilityDuringTurn = 0;
        view.SayThatATurnBegins(_player.superstar.Name);
        _habilityManager.UseHabilityIfApplicable();
        endOfTurn = false;
        
    }
    
    public NextPlay SelectNextPlay()
    {
        NextPlay selectedOption;
        if (_habilityManager.CheckIfUsingHabilityIsPossibleDuringMainSegment(usesOfHabilityDuringTurn))
        {
            selectedOption = view.AskUserWhatToDoWhenUsingHisAbilityIsPossible();

        }
        else
        {
            selectedOption = view.AskUserWhatToDoWhenHeCannotUseHisAbility();

        }

        return selectedOption;
    }
    
    
    public void ExecuteMainSegmentMenu()
    {
        NextPlay selectedOption = SelectNextPlay();

        switch (selectedOption)
        {
            case NextPlay.ShowCards:
                _decksManager.ViewCards();
                break;

            case NextPlay.PlayCard:
                PlayCard();
                break;

            case NextPlay.EndTurn:
                EndTurn();
                break;

            case NextPlay.GiveUp:
                LoseDuringThisTurn();
                break;

            case NextPlay.UseAbility:
                hability.ApplyEffect();
                usesOfHabilityDuringTurn += 1;
                break;
        }
    }

    public void LoseDuringThisTurn()
    {
        endOfTurn = true;
        _player.canKeepPlaying = false;
        Superstar oponentsSuperstar = _player.oponenteData.superstar;
        view.CongratulateWinner(oponentsSuperstar.Name);
    }

    public void EndTurn()
    {
        endOfTurn = true;

        if (_player.arsenal.Count <= 0)
        {
            LoseDuringThisTurn();
        }
    }
    public void ShowInformationForThisTurn() 
    {
        PlayerInfo playerInfo1 = new PlayerInfo(_player.superstar.Name, _player.fortitudRating, _player.hand.Count(),
            _player.arsenal.Count);
        PlayerInfo playerInfo2 = new PlayerInfo(_player.oponenteData.superstar.Name, _player.oponenteData.fortitudRating, _player.oponenteData.hand.Count,
            _player.oponenteData.arsenal.Count);
        view.ShowGameInfo(playerInfo1, playerInfo2);
    }

    public void PlayCard()
    {
        SelectPlayableCard();
        DoSelectedOption();
    }

    public void SelectPlayableCard()
    {
        playableCards = _decksManager.GetPlayableCards();       
        strPlaysInfo =  _decksManager.TransformPlayableCardsIntoPlayerInfoFormat();
        IndexSelectedCardToPlay = view.AskUserToSelectAPlay(strPlaysInfo);

    }


    public void DoSelectedOption()
    {
        if (IndexSelectedCardToPlay == -1)
        {
            ShowInformationForThisTurn();
            ExecuteMainSegmentMenu(); 
        }
        
        else
        {
            chosedCardToPlay = playableCards[IndexSelectedCardToPlay];
            ExecutePlaySelectedCard();
        }
    }

    public void ExecutePlaySelectedCard()
    {
        _decksManager.MoveCardFromHandToRingArea(IndexSelectedCardToPlay, chosedCardToPlay);
        
        ShowMessagesWhenPlayerIsPlayingACard();    
        
        int totalDamage = ChangeTotalDamageIfTheOpponentIsMankind();
        MakeDamage(totalDamage);
        
        EndTurnIfOpponentLost();

    }

    public void ShowMessagesWhenPlayerIsPlayingACard()
    {
        view.SayThatPlayerIsTryingToPlayThisCard(_player.superstar.Name, strPlaysInfo[IndexSelectedCardToPlay] );
        view.SayThatPlayerSuccessfullyPlayedACard();
    }
    

    public int ChangeTotalDamageIfTheOpponentIsMankind()
    {
        int damage;
        if (_player.oponenteData.manKindType)
        {
            damage = Int32.Parse(chosedCardToPlay.Damage) - 1;

        }
        else
        {
            damage = Int32.Parse(chosedCardToPlay.Damage);

        }

        return damage;
    }

    public void MakeDamage(int damage)
    {
        view.SayThatSuperstarWillTakeSomeDamage(_player.oponenteData.superstar.Name, damage);
        _player.oponente.ReceiveDamage(damage);
        UpdateFortitude();

    }

    public void ReceiveDamage(int damage)
    {
        _decksManager.MoveCardsFromArsenalToRingSide(damage);
    }

    public void EndTurnIfOpponentLost()
    {
        if (_player.oponenteData.lost)
        {
            endOfTurn = true;
        }
    }
    
    public void UpdateFortitude()
    {
        int totalFortitudRating= 0;
        foreach (Card card in _player.ringarea)
        {
            totalFortitudRating += Int32.Parse(card.Damage);
        }

        _player.fortitudRating = totalFortitudRating;
    }

}