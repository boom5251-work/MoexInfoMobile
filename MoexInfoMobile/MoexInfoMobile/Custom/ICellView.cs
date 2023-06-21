namespace MoexInfoMobile.Custom
{
    /// <summary>
    /// Делегат события Tapped ячейки.
    /// </summary>
    /// <param name="sender">Ячейка вызвавшая событие.</param>
    public delegate void CellViewTapped(ICellView sender);



    /// <summary>
    /// Интерфейс ячейки.
    /// </summary>
    public interface ICellView
    {
        /// <summary>
        /// Событие нажатия на ячейку.
        /// </summary>
        public event CellViewTapped? Tapped;
    }
}