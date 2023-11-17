namespace RawDeal;
using RawDealView;
using RawDealView.Options;
using RawDealView.Formatters;

public class DecksManager
{
    private Player _player;
    private PlayerTurnsManager _playerTurnsManager;
    private View view;
    public List<Card> playableCards;
    private List<int> indexesInHandOfPlayableCards;
    private List<string> strPlayablesCardsInfo;
    private bool endOfTurn;
    private Hability hability;
    private int usesOfHabilityDuringTurn;
    private int maxUses = 100;
    
    public DecksManager(Player player, View view)
    {
        this._player = player;
        this.view = view;

    }
    
    public List<Card> GetPlayableCards()
    
    {
        playableCards = new List<Card>();
        indexesInHandOfPlayableCards = new List<int>();
        int counter = 0;

        foreach (Card card in _player.hand)
        {
            AddOnlyPlayableCards(card, counter);
            counter += 1;
        }

        return playableCards;
    }
    
    public List<Card> AddOnlyPlayableCards(Card card, int indexInHand)
    {
        if ((card.Types.Contains("Maneuver") || card.Types.Contains("Action")) &&
            (Int32.Parse(card.Fortitude) <= _player.fortitudRating))
        {
            playableCards.Add(card);
            indexesInHandOfPlayableCards.Add(indexInHand);

        }

        return playableCards;
    }

    public List<int> GetIdOfPlayableCards()
    {
        return indexesInHandOfPlayableCards;
    }

    public bool CheckIfThereAreNoCardsLeftInTheArsenal()
    {
        if (_player.arsenal.Count == 0)
        {
            return true;
        }

        return false;
    }

    public void DrawCardsAccordingToSupestar()
    {
        if (_player.manKindType)
        {
            DrawCardsFromArsenalToHand(2);

        }
        else
        {
            DrawCardsFromArsenalToHand(1);

        }
    }
    
    
    public void DrawCardsFromArsenalToHand(int cantidad)
    {

        List<Card> drawedCards = PickCardsToTakeFromArsenal(cantidad);
        RemoveCardsFromArsenal(drawedCards);
        AddCardsToHand(drawedCards);

    }
    
    
    public List<Card> PickCardsToTakeFromArsenal(int cantidad)
    {
        
        List<Card> cardsTakenFromArsenal = new List<Card>();

        int indexLastCardTaken;

        if (_player.arsenal.Count - cantidad >= 0)
        {

            indexLastCardTaken = _player.arsenal.Count - cantidad;
        }
        else
        {
            indexLastCardTaken = 0;
            _player.lost = true;
        }

        for (int i = indexLastCardTaken; i  < _player.arsenal.Count ; i++)
        {
            
            cardsTakenFromArsenal.Insert(0,_player.arsenal[i]);

        }

        return cardsTakenFromArsenal;

    }
    
    
    public void RemoveCardsFromArsenal(List<Card> drawedCards)
    {

        foreach (Card drawedCard in drawedCards)
        {
            _player.arsenal.RemoveAt(_player.arsenal.Count-1);

        } 
    }    
    
    
    public void AddCardsToHand(List<Card> drawedCards)
    {
        foreach (Card cartaRobada in drawedCards)
        {
            _player.hand.Add(cartaRobada);
        }

    }
    
    public void ViewCards()
    {
        CardSet setElegido = view.AskUserWhatSetOfCardsHeWantsToSee();
        if (setElegido == CardSet.Hand)
        {
            ShowSelectedSet(_player.hand);
        }

        if (setElegido == CardSet.RingArea)
        {
            ShowSelectedSet(_player.ringarea);

        }

        if (setElegido == CardSet.RingsidePile)
        {
            ShowSelectedSet(_player.ringside);

        }

        if (setElegido == CardSet.OpponentsRingArea)
        {
            ShowSelectedSet(_player.oponenteData.ringarea);
        }

        if (setElegido == CardSet.OpponentsRingsidePile)
        {
            ShowSelectedSet(_player.oponenteData.ringside);
        }
        
    }

    public void ShowSelectedSet(List<Card> selectedSet)
    {

        List<string> strSet = FormatCardSetIntoString(selectedSet);

        view.ShowCards(strSet);
       
    }
    
    public List<string> FormatCardSetIntoString(List<Card> cardSet)
    {
        List<string> strSet = new List<string>();

        foreach (Card card in cardSet)
        {
            IViewableCardInfo cardInfo = card;
            strSet.Add(Formatter.CardToString(cardInfo));
            Console.WriteLine(strSet[0]);
        }

        return strSet;
    }

    
    public List<string> TransformPlayableCardsIntoPlayerInfoFormat()
    {
        strPlayablesCardsInfo = new List<string>();
        foreach(Card playableCard in playableCards)
        {
            IViewablePlayInfo playableCardInfo = new PlayInfo();
            playableCardInfo.CardInfo = playableCard;
            playableCardInfo.PlayedAs = playableCard.Types[0].ToUpper();
            
            strPlayablesCardsInfo.Add(Formatter.PlayToString(playableCardInfo));
        }

        return strPlayablesCardsInfo;

    }

    
    public void MoveCardFromHandToRingArea(int indexSelectedCardToPlay, Card chosedCardToPlay)
    {
        List<int> indexesInHandOfPlayableCards = GetIdOfPlayableCards();
        int indexInHandSelectedCardToPlay = indexesInHandOfPlayableCards[indexSelectedCardToPlay];
        _player.hand.RemoveAt(indexInHandSelectedCardToPlay);
        _player.ringarea.Add(chosedCardToPlay);
    }
    
    public void MoveCardFromRingsideToArsenal(int recoveredCardIndex)
    {
        Card recoveredCard = _player.ringside[recoveredCardIndex];
        _player.arsenal.Insert(0, recoveredCard);

        _player.ringside.RemoveAt(recoveredCardIndex);
    }
    
    public void MoveCardsFromArsenalToRingSide(int numberOfCards)
    {
        List<Card>  lostCards = PickCardsToTakeFromArsenal(numberOfCards);
        RemoveCardsFromArsenal(lostCards);
        MoveCardsToRingside(lostCards, numberOfCards);
        
        if (_player.lost)
        {
            endOfTurn = true;
            
        }
    }
    
    public void MoveCardsToRingside(List<Card> lostCards, int numberOfCards)
    {
        int currentDamage = 0;

        foreach (Card lostCard in lostCards)
        {
            currentDamage += 1;
            _player.ringside.Add(lostCard);
            
            IViewableCardInfo cardInfo = lostCard;
            view.ShowCardOverturnByTakingDamage(Formatter.CardToString(cardInfo), currentDamage , numberOfCards );        }
       
    }

    
    


}