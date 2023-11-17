
namespace RawDeal;


public class Player
{
    public bool canKeepPlaying;
    
    public Superstar superstar;
    public List<Card> arsenal;
    public List<Card> hand;
    public List<Card> ringside;
    public List<Card> ringarea;
    
    public int fortitudRating;
    public PlayerTurnsManager oponente;
    public Player oponenteData;
    public bool lost;
    public bool manKindType = false;

    public Hability hability;

    
    
    public Player(Dictionary<string, Card> mazo, Superstar superstar)
    {
        this.superstar = superstar;
        arsenal = mazo.Values.ToList();
        hand = new List<Card>();
        ringarea = new List<Card>();
        ringside = new List<Card>();
        canKeepPlaying = true;
        lost = false;

    }

    public void LoadHability(Hability hability)
    {
        this.hability = hability;
    }

    public void LoadOponente(PlayerTurnsManager oponente)
    {
        this.oponente = oponente;
    }

    public void LoadDataOponente(Player oponenteData)
    {
        this.oponenteData = oponenteData;

    }
    
}