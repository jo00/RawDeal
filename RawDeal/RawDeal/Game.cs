using RawDealView;
using RawDealView.Options;

namespace RawDeal;

public class Game
{
    private bool endOfTheGame;
    
    private View view;
    private string deckFolder;


    private List<string> mazoVersionString1;
    private List<string> mazoVersionString2;
    
    private Dictionary<string, Card> mazo1;
    private Dictionary<string, Card> mazo2;

    private Superstar superestrella1;
    private Superstar superestrella2;
    
    private Jugador jugador1;
    private Jugador jugador2;

    private JugadorData dataJugador1; 
    private JugadorData dataJugador2;
    
    private Builder builder;
    
    private DeckValidator deckValidation1;
    private DeckValidator deckValidation2;
    
    private Dictionary<string, Card> dictCards;
    private Dictionary<string, Superstar> dictSuperstars;
    




public Game(View view, string deckFolder)
    {
        this.view = view;
        this.deckFolder = deckFolder;
        builder = new Builder();
        endOfTheGame = false;
    }

    public void Play()
    {        
        CreateNecessaryElementsForDeckValidations();
        
        if (ValidateDecksForBothPlayers())
        {
            InicializarPartida();
            PlayTurns();
        }
        else
        {
            view.SayThatDeckIsInvalid();
        }
    }

    public void PlayTurns()
    {
        while (endOfTheGame == false)
        {
            RealizarTurnoJugador(jugador1, dataJugador1);
            RealizarTurnoJugador(jugador2, dataJugador2);  
        }
            
    }

    public void CreateNecessaryElementsForDeckValidations()
    {
        GenerateDataFromJsons();
        CreateValidators();
    }

    public void GenerateDataFromJsons()
    { 
        dictCards = builder.GenerarDatosCartas();
        dictSuperstars = builder.GenerarDatosSuperstars();

    }

    public void CreateValidators()
    {
        deckValidation1 = new DeckValidator(dictCards, dictSuperstars);
        deckValidation2 = new DeckValidator(dictCards, dictSuperstars);
    }

    public bool ValidateDecksForBothPlayers()
    {
        mazoVersionString1 = ElegirMazo(deckValidation1);
        bool validacionMazo1 = deckValidation1.ValidateDesk();
         
        if (validacionMazo1)
        {
            mazoVersionString2 = ElegirMazo(deckValidation2);
            bool validacionMazo2 = deckValidation2.ValidateDesk();

            if (validacionMazo2)
            {
                return (true);
            }
            else
            {
                return (false);
            }
        }
        else
        {
            return (false);
        }
    }

    public List<string> ElegirMazo(DeckValidator deckValidator)
    {
        string mazoAValidar = view.AskUserToSelectDeck(deckFolder);

        string rutaMazoAValidar = Path.Combine(mazoAValidar);
        List<string> mazo =  deckValidator.ReadDeskFile(rutaMazoAValidar);
        return (mazo);
    }

    public void InicializarPartida()
    {
        InstanciarCartasDeLosMazosComoObjetos();

        DefinirTurnos();
        
        jugador1.DrawCardsFromArsenalToHand(dataJugador1.superstar.HandSize);
        jugador2.DrawCardsFromArsenalToHand(dataJugador2.superstar.HandSize);


    }

    public void DefinirTurnos()
    {
        if (superestrella1.SuperstarValue > superestrella2.SuperstarValue)
        {
            HacerQueElPrimerJugadorEnIngresarDatosEmpiece();
        }
        else if (superestrella1.SuperstarValue < superestrella2.SuperstarValue)
        {
            HacerQueElSegundoJugadorEnIngresarDatosEmpiece();
        }
        
        else 
        {
            HacerQueElPrimerJugadorEnIngresarDatosEmpiece();
        }
    }

    public void HacerQueElPrimerJugadorEnIngresarDatosEmpiece()
    {
      
        dataJugador1 = new JugadorData(mazo1, superestrella1);
        dataJugador2 = new JugadorData(mazo2, superestrella2);
        InstanciarJugadores();
    }
    public void HacerQueElSegundoJugadorEnIngresarDatosEmpiece()
    {
        dataJugador1 = new JugadorData(mazo2, superestrella2);
        dataJugador2 = new JugadorData(mazo1, superestrella1);
        InstanciarJugadores();
        
    }

    public void CargarOponentes()
    {
        dataJugador1.CargarOponente(jugador2);
        dataJugador2.CargarOponente(jugador1);
        dataJugador1.CargarDataOponente(dataJugador2);
        dataJugador2.CargarDataOponente(dataJugador1);

    }

    public void InstanciarJugadores()
    {
        jugador1 = new Jugador(dataJugador1, view);
        jugador2 = new Jugador(dataJugador2, view);
        CargarOponentes();

    }

    public void InstanciarCartasDeLosMazosComoObjetos()
    {
        mazo1 = builder.GenerarMazo(mazoVersionString1);
        mazo2 = builder.GenerarMazo(mazoVersionString2);

        superestrella1 = builder.GenerarSuperestrella(mazoVersionString1);
        superestrella2 = builder.GenerarSuperestrella(mazoVersionString2);
    }

    
    public void RealizarTurnoJugador(Jugador jugador, JugadorData jugadorData)
    {
        if (endOfTheGame == false)
        {
            jugador.CheckIfThePlayerLostDuringHisOwnTurn();
            if (jugadorData.canKeepPlaying && jugadorData.oponenteData.canKeepPlaying)
            
            {
                Console.WriteLine("LLAMADA DESDE GAME");
                jugador.ExecuteTurn();
            }

            else
            {
                endOfTheGame = true;
            }
            
        }
        

    }


}

    


