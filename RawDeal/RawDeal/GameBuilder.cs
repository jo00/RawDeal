using RawDealView;

namespace RawDeal;

public class GameBuilder
{
    private bool endOfTheGame;

    private View _view;
    private string _deckFolder;


    private List<List<string>> _strDecks = new List<List<string>>();
    private List<Dictionary<string, Card>> _decks = new List<Dictionary<string, Card>>();

    private List<Superstar> _superstars = new List<Superstar>();

    private List<PlayerTurnsManager> _playersManagers = new List<PlayerTurnsManager>();
    private List<Player> _playersData = new List<Player>();

    private GameDataBuilder _gameDataBuilder;

    private List<DeckValidator> _deckValidators = new List<DeckValidator>();
    

    private Dictionary<string, Card> _dictCards;
    private Dictionary<string, Superstar> _dictSuperstars;

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
        _dictCards = _gameDataBuilder.GenerarDatosCartas();
        _dictSuperstars = _gameDataBuilder.GenerarDatosSuperstars();

    }

    public void CreateValidators()
    {
        _deckValidators.Add(new DeckValidator(_dictCards, _dictSuperstars));
        _deckValidators.Add(new DeckValidator(_dictCards, _dictSuperstars));
    }

    public bool ValidateDecksForBothPlayers()
    {
        _strDecks.Add(ElegirMazo(_deckValidators[0]));
        bool validacionMazo1 = _deckValidators[0].ValidateDesk();

        if (validacionMazo1)
        {
            _strDecks.Add(ElegirMazo(_deckValidators[1]));
            bool validacionMazo2 = _deckValidators[1].ValidateDesk();

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

        _playersManagers[0].TakeHandSize(_playersData[0].superstar.HandSize);
        _playersManagers[1].TakeHandSize(_playersData[1].superstar.HandSize);


    }

    public void InstanciarCartasDeLosMazosComoObjetos()
    {
        _decks.Add(_gameDataBuilder.GenerarMazo(_strDecks[0]));
        _decks.Add(_gameDataBuilder.GenerarMazo(_strDecks[1]));

        _superstars.Add(_gameDataBuilder.GenerarSuperestrella(_strDecks[0]));
        _superstars.Add(_gameDataBuilder.GenerarSuperestrella(_strDecks[1]));
    }

    public void DefinirTurnos()
    {
        if (_superstars[0].SuperstarValue > _superstars[1].SuperstarValue)
        {
            HacerQueElPrimerJugadorEnIngresarDatosEmpiece();
        }
        else if (_superstars[0].SuperstarValue < _superstars[1].SuperstarValue)
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

        _playersData.Add(new Player(_decks[0], _superstars[0]));
        _playersData.Add(new Player(_decks[1], _superstars[1]));
        InstanciarJugadores();
    }

    public void HacerQueElSegundoJugadorEnIngresarDatosEmpiece()
    {
        _playersData.Add(new Player(_decks[1], _superstars[1]));
        _playersData.Add(new Player(_decks[0], _superstars[0]));
        InstanciarJugadores();

    }
    
    public void InstanciarJugadores()
    {
        _playersManagers.Add(new PlayerTurnsManager(_playersData[0], _view));
        _playersManagers.Add(new PlayerTurnsManager(_playersData[1], _view));
        CargarOponentes();

    }
    
    
    public void CargarOponentes()
    {
        _playersData[0].LoadOponente(_playersManagers[1]);
        _playersData[1].LoadOponente(_playersManagers[0]);
        _playersData[0].LoadDataOponente(_playersData[1]);
        _playersData[1].LoadDataOponente(_playersData[0]);

    }

    public List<PlayerTurnsManager> ObtainPlayersManagement()
    {
        return _playersManagers;
    }

    public List<Player> ObtainPlayersInfo()
    {
        return _playersData;
    }



}