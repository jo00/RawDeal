using RawDealView.Formatters;

namespace RawDeal;



using System.Collections.Generic;

public class Card : IViewableCardInfo 
{
    public string Title { get; set; }
    public string Fortitude { get; set; }
    public string Damage { get; set; }
    public string StunValue { get; set; }
    public List<string> Types { get; set; }
    public List<string> Subtypes { get; set; }
    public string CardEffect { get; set; }
}