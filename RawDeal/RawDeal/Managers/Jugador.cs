namespace RawDeal;
using RawDealView;
using RawDealView.Options;
using RawDealView.Formatters;
public class Jugador
{
    private JugadorData jugadorData;
    private View view;
    public List<Card> playableCards;
    private List<int> indexesInHandOfPlayableCards;
    private List<string> strPlaysInfo;
    private int IndexSelectedCardToPlay;
    private Card chosedCardToPlay;
    private bool endOfTurn;
    private Hability hability;
    private int usesOfHabilityDuringTurn;
    private int maxUses = 100;



    public Jugador(JugadorData jugadorData, View view)
    {
        this.jugadorData = jugadorData;
        this.view = view;
        InstantiateSuperstarHability();

    }

    
        
    public void CheckIfThePlayerLostDuringHisOwnTurn()
    {
        
        GetPlayableCards();
        if (jugadorData.arsenal.Count == 0)
        {
            MakeThePlayerLose();
            FinishGame();
        }
    }
    
    
    public void DrawCardsFromArsenalToHand(int cantidad)
    {

        List<Card> drawedCards = PickCardsToTakeFromArsenal(cantidad);
        RemoveCardsFromArsenal(drawedCards);
        AddCardsToHand(drawedCards);

    }
    

    public void RemoveCardsFromArsenal(List<Card> drawedCards)
    {

        foreach (Card drawedCard in drawedCards)
        {
            jugadorData.arsenal.RemoveAt(jugadorData.arsenal.Count-1);

        } 
    }


    public void MakeThePlayerLose()
    {
        jugadorData.lost = true;
      
    }
    public List<Card> PickCardsToTakeFromArsenal(int cantidad)
    {
        
        List<Card> cardsTakenFromArsenal = new List<Card>();

        int indexLastCardTaken;

        if (jugadorData.arsenal.Count - cantidad >= 0)
        {

            indexLastCardTaken = jugadorData.arsenal.Count - cantidad;
        }
        else
        {
            indexLastCardTaken = 0;
            MakeThePlayerLose();
        }

        for (int i = indexLastCardTaken; i  < jugadorData.arsenal.Count ; i++)
        {
            
            cardsTakenFromArsenal.Insert(0,jugadorData.arsenal[i]);

        }

        return cardsTakenFromArsenal;

    }
    
    

 

    public void AddCardsToHand(List<Card> drawedCards)
    {
        foreach (Card cartaRobada in drawedCards)
        {
            jugadorData.hand.Add(cartaRobada);
        }

    }

    public void ExecuteTurn()
    {
        usesOfHabilityDuringTurn = 0;
        ExecuteBeforeDrawingSegment();
        if (jugadorData.manKindType)
        {
            DrawCardsFromArsenalToHand(2);

        }
        else
        {
            DrawCardsFromArsenalToHand(1);

        }
        
        endOfTurn = false;
        while (endOfTurn == false)
        {
            ShowGameInformation();
            
            ExecuteMainSegment();
        }

    }

    public void ExecuteBeforeDrawingSegment()
    {
        view.SayThatATurnBegins(jugadorData.superstar.Name);
        if(hability.ApplyBeforeDrawing())
        {
            if (hability.IsGoingToUseHability())
            {
                hability.ApplyEffect();
            }
        }
    }

    public void InstantiateSuperstarHability()
    {
        Superstar specificSuperStar = jugadorData.superstar;

        switch (specificSuperStar.Name)
        {
            case "HHH":
                hability = new HhhHability(jugadorData, this, view);
                break;
            
            case "THE UNDERTAKER":
                hability = new UndertakerHability(jugadorData, this, view);
                maxUses = 1;
                break;
            
            case "THE ROCK":
                hability = new TheRockHability(jugadorData, this, view);
                break;
            
            case "KANE":
                hability = new KaneHability(jugadorData, this, view);
                break;
            
            case "CHRIS JERICHO":
                hability = new JerichoHability(jugadorData, this, view);
                maxUses = 1;
                break;
            
            case "MANKIND":
                hability = new MankindHability(jugadorData, this, view);
                jugadorData.manKindType = true;
                break;
            case "STONE COLD STEVE AUSTIN":
                hability = new StoneColdHability(jugadorData, this, view);
                maxUses = 1;
                break;

            
            default:
                throw new InvalidOperationException($"Unhandled superstar: {specificSuperStar.Name}");
        }

    }

    public NextPlay SelectNextPlay()
    {
        NextPlay selectedOption;
        if (hability.CanPlayerUseHability() && usesOfHabilityDuringTurn < maxUses && hability.ApplyDuringMainSegment())
        {
            selectedOption = view.AskUserWhatToDoWhenUsingHisAbilityIsPossible();

        }
        else
        {
            selectedOption = view.AskUserWhatToDoWhenHeCannotUseHisAbility();

        }

        return selectedOption;
    }
    public void ExecuteMainSegment()
    {
        NextPlay selectedOption = SelectNextPlay();

        if (selectedOption == NextPlay.ShowCards)
        {
            ViewCards();
        }
        if (selectedOption == NextPlay.PlayCard)
        {
            PlayCard();
        }
        if (selectedOption == NextPlay.EndTurn)
        {
            endOfTurn = true;

            
            if (jugadorData.oponenteData.lost)
            {
                FinishGame();


            }

            if (jugadorData.arsenal.Count <= 0)
            {
                FinishGame();
            }
        }
        if (selectedOption == NextPlay.GiveUp)
        {
            Console.WriteLine("Selecciono giveup");

            FinishGame();
            endOfTurn = true;

        }

        if (selectedOption == NextPlay.UseAbility)
        {
            hability.ApplyEffect();
            usesOfHabilityDuringTurn = 1;

        }
    }

    public void FinishGame()
    {
        jugadorData.canKeepPlaying = false;
        
        view.CongratulateWinner(jugadorData.oponenteData.superstar.Name);
    }

    public void ShowGameInformation()
    {
        PlayerInfo playerInfo1 = new PlayerInfo(jugadorData.superstar.Name, jugadorData.fortitudRating, jugadorData.hand.Count(),
            jugadorData.arsenal.Count);
        PlayerInfo playerInfo2 = new PlayerInfo(jugadorData.oponenteData.superstar.Name, jugadorData.oponenteData.fortitudRating, jugadorData.oponenteData.hand.Count,
            jugadorData.oponenteData.arsenal.Count);
        view.ShowGameInfo(playerInfo1, playerInfo2);
        
        jugadorData.oponente.GetPlayableCards();

    }

    public void ViewCards()
    {
        CardSet setElegido = view.AskUserWhatSetOfCardsHeWantsToSee();
        if (setElegido == CardSet.Hand)
        {
            ShowSelectedSet(jugadorData.hand);
        }

        if (setElegido == CardSet.RingArea)
        {
            ShowSelectedSet(jugadorData.ringarea);

        }

        if (setElegido == CardSet.RingsidePile)
        {
            ShowSelectedSet(jugadorData.ringside);

        }

        if (setElegido == CardSet.OpponentsRingArea)
        {
            ShowSelectedSet(jugadorData.oponenteData.ringarea);
        }

        if (setElegido == CardSet.OpponentsRingsidePile)
        {
            ShowSelectedSet(jugadorData.oponenteData.ringside);
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

    public void PlayCard()
    {
        AllowToSelectPlayableCard();
        DoSelectedOption();
    }

    public void AllowToSelectPlayableCard()
    {
        GetPlayableCards();
        TransformPlayableCardsIntoPlayerInfoFormat();
        IndexSelectedCardToPlay = view.AskUserToSelectAPlay(strPlaysInfo);

    }

    public void GetPlayableCards()
    
    {
        playableCards = new List<Card>();
        indexesInHandOfPlayableCards = new List<int>();
        int counter = 0;

        foreach (Card card in jugadorData.hand)
        {
            AddOnlyPlayableCards(card, counter);
            counter += 1;
        }
    }

    public void AddOnlyPlayableCards(Card card, int indexInHand)
    {
        if ((card.Types.Contains("Maneuver") || card.Types.Contains("Action")) &&
            (Int32.Parse(card.Fortitude) <= jugadorData.fortitudRating))
        {
            playableCards.Add(card);
            indexesInHandOfPlayableCards.Add(indexInHand);

        }
    }

    public void TransformPlayableCardsIntoPlayerInfoFormat()
    {
        strPlaysInfo = new List<string>();
        foreach(Card playableCard in playableCards)
        {
            IViewablePlayInfo playableCardInfo = new PlayInfo();
            playableCardInfo.CardInfo = playableCard;
            playableCardInfo.PlayedAs = playableCard.Types[0].ToUpper();
            
            strPlaysInfo.Add(Formatter.PlayToString(playableCardInfo));
        }

    }

    public void DoSelectedOption()
    {
        if (IndexSelectedCardToPlay == -1)
        {
            ShowGameInformation();
            ExecuteMainSegment(); 
        }
        
        else
        {
            
            ExecutePlaySelectedCard();
        }
    }

    public void ExecutePlaySelectedCard()
    {
        chosedCardToPlay = playableCards[IndexSelectedCardToPlay];
        
        MoveCardFromHandToRingArea(IndexSelectedCardToPlay);
        
        view.SayThatPlayerIsTryingToPlayThisCard(jugadorData.superstar.Name, strPlaysInfo[IndexSelectedCardToPlay] );
        view.SayThatPlayerSuccessfullyPlayedACard();
        int damage;
        if (jugadorData.oponenteData.manKindType)
        {
            damage = Int32.Parse(chosedCardToPlay.Damage) - 1;

        }
        else
        {
            damage = Int32.Parse(chosedCardToPlay.Damage);

        }
        MakeDamage(damage);
    }

    public void MakeDamage(int damage)
    {
        view.SayThatSuperstarWillTakeSomeDamage(jugadorData.oponenteData.superstar.Name, damage);
        jugadorData.oponente.MoveCardsFromArsenalToRingSide(damage);
        UpdateFortitude();
        if (jugadorData.oponenteData.lost)
        {
            Console.WriteLine("EL OPONETE PERDIO");

            endOfTurn = true;
        }


    }

    public void MoveCardFromHandToRingArea(int indexSelectedCardToPlay)
    {
        int indexInHandSelectedCardToPlay = indexesInHandOfPlayableCards[indexSelectedCardToPlay];
        jugadorData.hand.RemoveAt(indexInHandSelectedCardToPlay);
        jugadorData.ringarea.Add(chosedCardToPlay);
    }
    
    public void MoveCardFromHandToRingSide(int indexSelectedCardToRemove)
    {
        jugadorData.hand.RemoveAt(indexSelectedCardToRemove);
        jugadorData.ringside.Add(chosedCardToPlay);
    }
    

    public void MoveCardFromRingsideToArsenal(int recoveredCardIndex)
    {
        Card recoveredCard = jugadorData.ringside[recoveredCardIndex];
        jugadorData.arsenal.Insert(0, recoveredCard);

        jugadorData.ringside.RemoveAt(recoveredCardIndex);
    }

    public void MoveCardsFromArsenalToRingSide(int numberOfCards)
    {
        List<Card>  lostCards = PickCardsToTakeFromArsenal(numberOfCards);
        RemoveCardsFromArsenal(lostCards);
        MoveCardsToRingside(lostCards, numberOfCards);
        
        if (jugadorData.lost)
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
            jugadorData.ringside.Add(lostCard);
            
            IViewableCardInfo cardInfo = lostCard;
            view.ShowCardOverturnByTakingDamage(Formatter.CardToString(cardInfo), currentDamage , numberOfCards );        }
       
    }

    public void UpdateFortitude()
    {
        int totalFortitudRating= 0;
        foreach (Card card in jugadorData.ringarea)
        {
            totalFortitudRating += Int32.Parse(card.Damage);
        }

        jugadorData.fortitudRating = totalFortitudRating;
    }
    




}