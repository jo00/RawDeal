
namespace RawDeal;


public class JugadorData
{
    public bool canKeepPlaying;
    public Superstar superstar;
    public List<Card> arsenal;
    public List<Card> hand;
    public List<Card> ringside;
    public List<Card> ringarea;
    public int fortitudRating;
    public Jugador oponente;
    public JugadorData oponenteData;
    public bool lost;
    public bool manKindType = false;

    
    
    public JugadorData(Dictionary<string, Card> mazo, Superstar superstar)
    {
        this.superstar = superstar;
        arsenal = mazo.Values.ToList();
        hand = new List<Card>();
        ringarea = new List<Card>();
        ringside = new List<Card>();
        canKeepPlaying = true;
        lost = false;

    }

    public void CargarOponente(Jugador oponente)
    {
        this.oponente = oponente;
    }

    public void CargarDataOponente(JugadorData oponenteData)
    {
        this.oponenteData = oponenteData;

    }
    
}