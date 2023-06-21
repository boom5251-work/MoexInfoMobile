using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Custom.Html
{
    /// <summary>
    /// Элемент управления, представляющий ячейку таблицы.<br />
    /// Логика взаимодействия с HtmlTableCell.xaml
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HtmlTableCell : Frame
    {
        public HtmlTableCell()
        {
            InitializeComponent();
        }


        public HtmlTableCell(params View[] views) : base()
        {
            foreach (View view in views)
                AddView(view);
        }



        /// <summary>
        /// Свойство указывающее на то, есть ли дочерние элементы в ячейке.
        /// </summary>
        public bool HasChildren => _cell.Children.Count > 0;



        /// <summary>
        /// Добавляет элемент интерфейса в ячейку.
        /// </summary>
        /// <param name="view">Содержимое ячейки.</param>
        public void AddView(View view) =>
            _cell.Children.Add(view);
    }
}