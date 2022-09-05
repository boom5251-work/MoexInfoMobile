using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Custom.Html
{
    /// <summary>
    /// Элемент управления, представляющий ячейку таблицы.
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
            {
                AddView(view);
            }
        }



        /// <summary>
        /// Свойство указывающее на то, есть ли дочерние элементы в ячейке.
        /// </summary>
        public bool HasChildren
        {
            get => _cell.Children.Count > 0;
        }


        /// <summary>
        /// Добавление элемента интерфейса в ячейку.
        /// </summary>
        /// <param name="view">Содержимое ячейки.</param>
        public void AddView(View view)
        {
            _cell.Children.Add(view);
        }
    }
}