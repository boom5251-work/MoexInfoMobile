namespace MoexInfoMobile.Custom
{
    public delegate void CellViewTapped(ICellView sender);



    public interface ICellView
    {
        event CellViewTapped Tapped;
    }
}