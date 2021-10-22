using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoexInfoMobile.Custom
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TableCell : Frame
    {
        public TableCell()
        {
            InitializeComponent();
        }

        public TableCell(params View[] views) : base()
        {
            foreach (View view in views)
            {
                AddView(view);
            }
        }



        // Свойство указывающее на то, есть ли дочерние элементы в ячейке.
        public bool HasChildren
        {
            get { return _cell.Children.Count > 0; }
        }


        // Добавление элемента интерфейса в ячейку.
        public void AddView(View view)
        {
            _cell.Children.Add(view);
        }
    }
}