namespace RawDealView.Formatters;

public interface IViewablePlayInfo
{
    IViewableCardInfo CardInfo { get; set; }
    String PlayedAs { get; set; }
}