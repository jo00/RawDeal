namespace RawDeal;
using System.Linq;



public class DeckValidator
{
    private List<string> cardList;
    private Dictionary<string, Card> cardDict;
    private Dictionary<string, Superstar> dictSuperstars;
    private string ruta;
    private IEnumerable<string> subCardList;

    public DeckValidator(Dictionary<string, Card> cardDictInput,
        Dictionary<string, Superstar> dictSuperstarsInput)
    {
        cardDict = cardDictInput;
        dictSuperstars = dictSuperstarsInput;
        
    }
    public List<string> ReadDeskFile(string ruta)
    {
        
        //cambiar cardlist por un enumerate ver si es necesario
        cardList = new List<string>();
        subCardList = cardList.Skip(1);

        using (StreamReader sr = new StreamReader(ruta)) 
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                cardList.Add(line);
            }
        }
        
        return (cardList);
    }
    public bool ValidateDesk()
    {
        
        int validacionesNecesarias = 6;

        int validaciones = SumarValidaciones();

        if (validaciones == validacionesNecesarias)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int SumarValidaciones()
    {
        return (Validate60Cards() + ValidateNoMoreThan3Rep() + Validate1Unique() + ValidateHeelFace() +
                ValidateLogoSuperestrella() + ValidateOneSuperStar() 
                );
    }

    public int Validate60Cards()
    {
        int amountOfCards = cardList.Count();
        if (amountOfCards == 61)
        {

            return 1;
        }
        else
        {
            Console.WriteLine("Falta validacion1");
            return 0;
        }
    }

    public int ValidateNoMoreThan3Rep()
    {
        foreach (string card in subCardList)
        {
            int repeticiones = cardList.Count(item => item == card);
            bool esSetUp = true;

            if (repeticiones > 3)
            {
                esSetUp = VerSiLaCartaEsSetUp(card);
            }

            if (!esSetUp)
            {
                Console.WriteLine("Falta validacion2");
                return 0;
            }
        }
        return 1;
    }

    public bool VerSiLaCartaEsSetUp(string card)
    {
        
        Card cardObj = cardDict[card];

        if (!(cardObj.Subtypes.Contains("SetUp")))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    

    public int Validate1Unique()
    {
        List<string> uniquesList = new List<string>();
        foreach (string card in subCardList)
        {
            bool existeMasDeUnUnique = VerSiYaExisteOtraCartaUnique(card, uniquesList);
            if (existeMasDeUnUnique)
            {
                Console.WriteLine("Falta validacion3");
                return 0;
            }
        }

        return 1;
    }

    public bool VerSiYaExisteOtraCartaUnique(string card, List<string> uniquesList)
    {
        Card cardObj = cardDict[card];
        bool hayOtraUnique = false;
            
        if (cardObj.Subtypes.Contains("Unique"))
        {
            uniquesList.Add(card);
            if (uniquesList.Count(item => item == card) > 1)
            {
                hayOtraUnique = true;
            }
        }

        return hayOtraUnique;
    }

    public int ValidateHeelFace()
    {

        int suma = VerSiHayAlmenosUnHeelYUnFace();
        if (suma == 2)
        {
            Console.WriteLine("Falta validacion4");
            return 0;
        }
        
        return 1;
    }

    public int VerSiHayAlmenosUnHeelYUnFace()
    {
        int heel = 0;
        int face = 0;
        foreach (string card in subCardList)
        {
            Card cardObj = cardDict[card];

            if (cardObj.Subtypes.Contains("Heel") )
            {
                heel = 1;
            }

            else if (cardObj.Subtypes.Contains("Face"))
            {
                face = 1;
            }
        }
        return (heel + face);
    }


    public int ValidateLogoSuperestrella()
    {
        int subtiposIncorrectos = ContarSubtiposLogoIncorrectos();
        
        if (subtiposIncorrectos > 0)
        {
            Console.WriteLine("Falta validacion5");
            return 0;
        }

        return 1;
    }

    public int ContarSubtiposLogoIncorrectos()
    {
        int subtiposIncorrectos = 0;
        foreach (string card in subCardList)
        {
            Card cardObj = cardDict[card];
            foreach (Superstar superestrella in dictSuperstars.Values)
            {
                subtiposIncorrectos += VerificarSiSeTieneUnSubtipoIncorrecto(cardObj, superestrella);
            }
        }

        return subtiposIncorrectos;

    }

    
    public int VerificarSiSeTieneUnSubtipoIncorrecto(Card cardObj, Superstar superestrella)
    {
        if (cardObj.Subtypes.Contains(superestrella.Logo))
        {
            int tieneSuperestrella = VerificarSiSeTieneSuperestrella(superestrella);
            if (tieneSuperestrella == 0)
            {
                return 1;
            }
        }
        return 0;
    }

    public int VerificarSiSeTieneSuperestrella(Superstar superestrella)
    {
        String[] superstarNameArr = cardList[0].Split('(');
                    
        if (superestrella.Name + " " != superstarNameArr[0])
        {
            Console.WriteLine("Falta validacion7");
            return 0;
        }

        return 1;
    }
    
    public int ValidateOneSuperStar()
    {
        foreach (string card in subCardList)
        {

            string nombreSuperstar = ObtenerCartaComoNombreSuperstar(card);

            if (dictSuperstars.ContainsKey(nombreSuperstar))
            {
                Console.WriteLine("Falta validacion8");
                return 0;
            }
        }
        return 1;
    }

    public string ObtenerCartaComoNombreSuperstar(string card)
    {
        String[] superstarNameArr = card.Split('(');
        int largoSuperstarName = card.Length;
        string nombreSuperstar = card.Substring(0, largoSuperstarName - 1) ;
        return nombreSuperstar;
    }
    

}