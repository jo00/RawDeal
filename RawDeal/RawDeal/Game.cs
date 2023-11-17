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

    private List<PlayerTurnsManager> playersManagers = new List<PlayerTurnsManager>();
    private List<Player> playersData = new List<Player>();

    private GameDataBuilder _gameDataBuilder;
    private GameBuilder _gameBuilder;
    
    private DeckValidator deckValidation1;
    private DeckValidator deckValidation2;
    
    private Dictionary<string, Card> dictCards;
    private Dictionary<string, Superstar> dictSuperstars;

    
public Game(View view, string deckFolder)
    {
        this.view = view;
        this.deckFolder = deckFolder;
        _gameDataBuilder = new GameDataBuilder();
        _gameBuilder = new GameBuilder(_gameDataBuilder, view, deckFolder);
        
        endOfTheGame = false;
    }

    public void Play()
    {        
        _gameBuilder.CreateNecessaryElementsForDeckValidations();
        
        if (_gameBuilder.ValidateDecksForBothPlayers())
        {
            _gameBuilder.InicializarPartida();
            playersData = _gameBuilder.ObtainPlayersInfo();
            playersManagers = _gameBuilder.ObtainPlayersManagement();
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
            ExecutePlayerTurn(playersManagers[0], playersData[0]);
            if (endOfTheGame == false)
            {
                ExecutePlayerTurn(playersManagers[1], playersData[1]);

            }
        }

    }
    


public void ExecutePlayerTurn(PlayerTurnsManager playerTurnsManager, Player player)
    {
        
        playerTurnsManager.CheckIfThePlayerLostDuringHisOwnTurn();
        if (player.canKeepPlaying && player.oponenteData.canKeepPlaying)
        
        {
            playerTurnsManager.ExecuteTurn();
        }

        else
        {
            endOfTheGame = true;
        }
            
        
        

    }


}

    


