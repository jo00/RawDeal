using RawDealView;

namespace RawDeal;

public class GameBuilder
{
    private bool endOfTheGame;

    private View _view;
    private string _deckFolder;


    private List<string> mazoVersionString1;
    private List<string> mazoVersionString2;

    private Dictionary<string, Card> mazo1;
    private Dictionary<string, Card> mazo2;

    private Superstar superestrella1;
    private Superstar superestrella2;

    private List<PlayerTurnsManager> playersManagers = new List<PlayerTurnsManager>();
    private List<Player> playersData = new List<Player>();

    private GameDataBuilder _gameDataBuilder;

    private DeckValidator deckValidation1;
    private DeckValidator deckValidation2;

    private Dictionary<string, Card> dictCards;
    private Dictionary<string, Superstar> dictSuperstars;

    public GameBuilder(GameDataBuilder gameDataBuilder, View view, string deckFolder)

    {
        _view = view;
        _deckFolder = deckFolder;
        _gameDataBuilder = gameDataBuilder;
    }

    public void CreateNecessaryElementsForDeckValidations()
    {
        GenerateDataFromJsons();
        CreateValidators();
    }

    public void GenerateDataFromJsons()
    {
        dictCards = _gameDataBuilder.GenerarDatosCartas();
        dictSuperstars = _gameDataBuilder.GenerarDatosSuperstars();

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
        string mazoAValidar = _view.AskUserToSelectDeck(_deckFolder);

        string rutaMazoAValidar = Path.Combine(mazoAValidar);
        List<string> mazo = deckValidator.ReadDeskFile(rutaMazoAValidar);
        return (mazo);
    }

    public void InicializarPartida()
    {
        InstanciarCartasDeLosMazosComoObjetos();

        DefinirTurnos();

        playersManagers[0].TakeHandSize(playersData[0].superstar.HandSize);
        playersManagers[1].TakeHandSize(playersData[1].superstar.HandSize);


    }

    public void InstanciarCartasDeLosMazosComoObjetos()
    {
        mazo1 = _gameDataBuilder.GenerarMazo(mazoVersionString1);
        mazo2 = _gameDataBuilder.GenerarMazo(mazoVersionString2);

        superestrella1 = _gameDataBuilder.GenerarSuperestrella(mazoVersionString1);
        superestrella2 = _gameDataBuilder.GenerarSuperestrella(mazoVersionString2);
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

        playersData.Add(new Player(mazo1, superestrella1));
        playersData.Add(new Player(mazo2, superestrella2));
        InstanciarJugadores();
    }

    public void HacerQueElSegundoJugadorEnIngresarDatosEmpiece()
    {
        playersData.Add(new Player(mazo2, superestrella2));
        playersData.Add(new Player(mazo1, superestrella1));
        InstanciarJugadores();

    }
    
    public void InstanciarJugadores()
    {
        playersManagers.Add(new PlayerTurnsManager(playersData[0], _view));
        playersManagers.Add(new PlayerTurnsManager(playersData[1], _view));
        CargarOponentes();

    }
    
    
    public void CargarOponentes()
    {
        playersData[0].LoadOponente(playersManagers[1]);
        playersData[1].LoadOponente(playersManagers[0]);
        playersData[0].LoadDataOponente(playersData[1]);
        playersData[1].LoadDataOponente(playersData[0]);

    }

    public List<PlayerTurnsManager> ObtainPlayersManagement()
    {
        return playersManagers;
    }

    public List<Player> ObtainPlayersInfo()
    {
        return playersData;
    }



}