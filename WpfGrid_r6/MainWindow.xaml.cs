using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using System.ComponentModel;

namespace WpfGrid_r5
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GridViewColumnHeader listViewSortCol = null;
        private SortAdorner listViewSortAdorner = null;

        public MainWindow()
        {
            InitializeComponent();
            //ListView1.ItemsSource = Process.GetProcesses();
            /*
            //Create current process list
            Process[] processlist = Process.GetProcesses();
            //Create List of ProcessOptions
            List<ProcessOptions> items = new List<ProcessOptions>();
            foreach (Process theprocess in processlist)
            {
                items.Add(new ProcessOptions()
                {
                    ProcessName = theprocess.ProcessName,
                    Id = theprocess.Id,
                    PagedMemorySize = theprocess.PagedMemorySize
                });
            }
            ListView1.ItemsSource = items;
            */
            List<ProcessOptions> items = new List<ProcessOptions>();
            for (int i = 0; i < 10000000; i++)
                items.Add(new ProcessOptions()
                {
                    ProcessName = "Hlam for filling" + " Number=" + i,
                    Id = Int32.Parse("1234567890"),
                    PagedMemorySize = Int32.Parse("1234567890")
                });
            ListView1.ItemsSource = items;
        }
        private void btnRefresh_Click(object sender, EventArgs e)
        {
            ListView1.ItemsSource = Process.GetProcesses();
        }
        private void lvUsersColumnHeader_Click(object sender, RoutedEventArgs e)
        {
            GridViewColumnHeader column = (sender as GridViewColumnHeader);
            string sortBy = column.Tag.ToString();
            if (listViewSortCol != null)
            {
                AdornerLayer.GetAdornerLayer(listViewSortCol).Remove(listViewSortAdorner);
                ListView1.Items.SortDescriptions.Clear();
            }

            ListSortDirection newDir = ListSortDirection.Ascending;
            if (listViewSortCol == column && listViewSortAdorner.Direction == newDir)
                newDir = ListSortDirection.Descending;

            listViewSortCol = column;
            listViewSortAdorner = new SortAdorner(listViewSortCol, newDir);
            AdornerLayer.GetAdornerLayer(listViewSortCol).Add(listViewSortAdorner);
            ListView1.Items.SortDescriptions.Add(new SortDescription(sortBy, newDir));
        }
        private void txtFilter_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListView1.ItemsSource);
            view.Filter = UserFilter;
            CollectionViewSource.GetDefaultView(ListView1.ItemsSource).Refresh();
        }
        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(txtFilter.Text))
                return true;
            else
                return ((item as ProcessOptions).ProcessName.IndexOf(txtFilter.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }


    }
    public class SortAdorner : Adorner
    {
        private static Geometry ascGeometry =
                Geometry.Parse("M 0 4 L 3.5 0 L 7 4 Z");

        private static Geometry descGeometry =
                Geometry.Parse("M 0 0 L 3.5 4 L 7 0 Z");

        public ListSortDirection Direction { get; private set; }

        public SortAdorner(UIElement element, ListSortDirection dir)
                : base(element)
        {
            this.Direction = dir;
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (AdornedElement.RenderSize.Width < 20)
                return;

            TranslateTransform transform = new TranslateTransform
                    (
                            AdornedElement.RenderSize.Width - 15,
                            (AdornedElement.RenderSize.Height - 5) / 2
                    );
            drawingContext.PushTransform(transform);

            Geometry geometry = ascGeometry;
            if (this.Direction == ListSortDirection.Descending)
                geometry = descGeometry;
            drawingContext.DrawGeometry(Brushes.Black, null, geometry);

            drawingContext.Pop();
        }
    }
    public class ProcessOptions
    {
        public string ProcessName { get; set; }

        public int Id { get; set; }

        public int PagedMemorySize { get; set; }
    }
}
