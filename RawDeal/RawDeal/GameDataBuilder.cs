namespace RawDeal;
using System.Text.Json;
using System.Linq;



public class GameDataBuilder
{
    private Dictionary<string, Card> dictCards;
    private Dictionary<string, Superstar> dictSuperstars;
    


    public string LeerJson(string fileName)
    {
        string pathJson = Path.Combine("data", $"{fileName}");
        string jsonFile = File.ReadAllText(pathJson);
        return jsonFile;
    }


    public Dictionary <string , Card >  GenerarDatosCartas()
    {
        string jsonFileCards = LeerJson("cards.json");
        var cardsJson = JsonSerializer . Deserialize < List < Card > >( jsonFileCards ) ;
        dictCards = new Dictionary <string , Card >() ;
        
        foreach (Card card in cardsJson)
        {
            dictCards[card.Title] = card;
        }

        return dictCards;

    }



    public Dictionary<string, Superstar> GenerarDatosSuperstars()
    {
        string jsonFileSuperstars = LeerJson("superstar.json");
        var superstarsJson = JsonSerializer.Deserialize<List<Superstar>>(jsonFileSuperstars);

        dictSuperstars = new Dictionary<string, Superstar>();

        foreach (Superstar superstar in superstarsJson)
        {
            dictSuperstars[superstar.Name] = superstar;
        }

        return dictSuperstars;
    }


    public Dictionary<string, Card> GenerarMazo(List<string> mazoValidado)
    {
        Dictionary<string, Card> dictMazo = new Dictionary<string, Card>();
        
        var subMazoValidado = mazoValidado.Skip(1);
        int cont = 0;

        foreach (string strCard in subMazoValidado)
        {
            Card cardObj = dictCards[strCard];
            dictMazo[cont.ToString()] = cardObj;
            cont += 1;
        }

        return dictMazo;
    }

    public Superstar GenerarSuperestrella(List<string> mazoValidado)
    {
        String[] superstarNameArr = mazoValidado[0].Split('(');
        int largoSuperstarName = superstarNameArr[0].Length;
        string nombreSuperstar = superstarNameArr[0].Substring(0, largoSuperstarName - 1) ;
        Superstar superstarObj = dictSuperstars[nombreSuperstar];
        return superstarObj;
    }

}