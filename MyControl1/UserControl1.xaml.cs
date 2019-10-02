using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace MyControl1
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl, INotifyPropertyChanged
    {
        public UserControl1()
        {
            InitializeComponent();
            DataContext = this;
            Cells = new List<element>();
        }

        public static DependencyProperty RowProperty;
        public static DependencyProperty ColumnProperty;
        public static DependencyProperty CellsProperty;
        public static DependencyProperty TotalNumberProperty;
        //public static RoutedEvent ChooseCellEvent;

        static UserControl1()
        {
            var metadata = new FrameworkPropertyMetadata(new PropertyChangedCallback(OnRowChanged), new CoerceValueCallback(CoerceRow));
            RowProperty = DependencyProperty.Register("Row", typeof(int), typeof(UserControl1), metadata);
            metadata = new FrameworkPropertyMetadata(new PropertyChangedCallback(OnColumnChanged), new CoerceValueCallback(CoerceColumn));
            ColumnProperty = DependencyProperty.Register("Column", typeof(int), typeof(UserControl1), metadata);
            metadata = new FrameworkPropertyMetadata(new PropertyChangedCallback(OnCellsChanged));
            CellsProperty = DependencyProperty.Register("Cells", typeof(List<element>), typeof(UserControl1), metadata);
            metadata = new FrameworkPropertyMetadata(new PropertyChangedCallback(OnTotalNumberChanged), new CoerceValueCallback(CoerceTotalNumber));
            TotalNumberProperty = DependencyProperty.Register("Total Number", typeof(int), typeof(UserControl1), metadata);
            //ChooseCellEvent = EventManager.RegisterRoutedEvent("ChooseCell", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<List<Point>>), typeof(UserControl1));
            

        }


        public int Row
        {
            get { return (int)GetValue(RowProperty); }
            set { SetValue(RowProperty, value); }
        }
        public int Column
        {
            get { return (int)GetValue(ColumnProperty); }
            set { SetValue(ColumnProperty, value); }
        }
        public List<element> Cells
        {
            get { return (List<element>)GetValue(CellsProperty); }
            set { SetValue(CellsProperty, value); }
        }
        public int TotalNumber
        {
            get { return (int)GetValue(TotalNumberProperty); }
            set { SetValue(TotalNumberProperty, value); }
        }
        //public event RoutedPropertyChangedEventHandler<List<Point>> ChooseCell
        //{
        //    add { AddHandler(ChooseCellEvent, value); }
        //    remove { RemoveHandler(ChooseCellEvent, value); }
        //}


        private static List<Line> vLines = new List<Line>();
        private static List<Line> hLines = new List<Line>();
        private static List<ComboBox> comboBoxes = new List<ComboBox>();
        private static List<Label> numberingLabels = new List<Label>();
        private static readonly int width = 50;
        private static readonly int height = 30;
        private int sumWidth;
        public int SumWidth
        {
            get { return sumWidth; }
            set { sumWidth = value; OnPropertyChanged(); }
        }
        private int sumHeight;
        public int SumHeight
        {
            get { return sumHeight; }
            set { sumHeight = value; OnPropertyChanged(); }
        }
        private int leftIndent=30;
        public int LeftIndent
        {
            get { return leftIndent; }
            set { leftIndent = value; OnPropertyChanged(); }
        }
        private int topIndent=30;
        public int TopIndent
        {
            get { return topIndent; }
            set { topIndent = value; OnPropertyChanged(); }
        }
        private int widthOuter;
        public int WidthOuter
        {
            get { return widthOuter; }
            set { widthOuter = value; OnPropertyChanged(); }
        }
        private int heigthOuter;
        public int HeigthOuter
        {
            get { return heigthOuter; }
            set { heigthOuter = value; OnPropertyChanged(); }
        }


        #region Row && Column

        private static void OnColumnChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UserControl1 userControl = (UserControl1)d;
            int column = (int)e.NewValue;
            if (column == (int)e.OldValue) return;
            DrawGrid(userControl, userControl.Row, column);
        }

        private static object CoerceColumn(DependencyObject d, object baseValue)
        {
            UserControl1 userControl = (UserControl1)d;
            int current = (int)baseValue;
            if (current < 1) current = 1;
            return current;
        }


        private static void OnRowChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UserControl1 userControl = (UserControl1)d;
            int row = (int)e.NewValue;
            if (row == (int)e.OldValue) return;
            DrawGrid(userControl, row, userControl.Column);
        }

        private static object CoerceRow(DependencyObject d, object baseValue)
        {
            UserControl1 userControl = (UserControl1)d;
            int current = (int)baseValue;
            if (current < 1) current = 1;
            return current;
        }


        private static void DrawGrid(UserControl1 userControl, int row, int column)
        {
            if (row == 0 || column == 0) return;
            foreach (var p in comboBoxes)
            {
                userControl.MainField.Children.Remove(p);
            }
            comboBoxes.Clear();
            userControl.Cells.Clear();
            foreach (var line in vLines)
            {
                userControl.MainField.Children.Remove(line);
            }
            vLines.Clear();
            foreach (var line in hLines)
            {
                userControl.MainField.Children.Remove(line);
            }
            hLines.Clear();
            for (int i = 0; i < row + 1; ++i)
            {
                Line horL = new Line();
                horL.Y1 = i * height;
                horL.X1 = 0;
                horL.Y2 = i * height;
                horL.X2 = width*column;
                horL.Stroke = Brushes.Black;
                userControl.MainField.Children.Add(horL);
                hLines.Add(horL);
            }
            for (int i = 0; i < column + 1; ++i)
            {
                Line vertL = new Line();
                vertL.X1 = i * width;
                vertL.Y1 = 0;
                vertL.X2 = i * width;
                vertL.Y2 = height * row;
                vertL.Stroke = Brushes.Black;
                userControl.MainField.Children.Add(vertL);
                vLines.Add(vertL);
            }
            foreach (var l in numberingLabels)
            {
                userControl.Numbering.Children.Remove(l);
            }
            numberingLabels.Clear();
            for (int i = 0; i < column; ++i)
            {
                Label label = new Label();
                label.Width = width;
                label.Height = 30;
                label.Margin = new Thickness(30 + i * width, 0, 0, 0);
                label.Content = (i + 1).ToString();
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                userControl.Numbering.Children.Add(label);
                numberingLabels.Add(label);
            }
            for (int i = 0; i < row; ++i)
            {
                Label label = new Label();
                label.Width = 30;
                label.Height = height;
                label.Margin = new Thickness(0, 30 + i * height, 0, 0);
                label.Content = (i + 1).ToString();
                label.HorizontalContentAlignment = HorizontalAlignment.Center;
                label.VerticalContentAlignment = VerticalAlignment.Center;
                userControl.Numbering.Children.Add(label);
                numberingLabels.Add(label);
            }

            userControl.SumHeight = height * row;
            userControl.SumWidth = width * column;
            userControl.WidthOuter = userControl.SumWidth + userControl.LeftIndent;
            userControl.HeigthOuter = userControl.SumHeight + userControl.TopIndent;

        }

        #endregion



        #region Cells

        private static void OnCellsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UserControl1 userControl = (UserControl1)d;
            List<element> cells = (List<element>)e.NewValue;

            foreach (var comboBox in comboBoxes)
            {
                userControl.MainField.Children.Remove(comboBox);
            }
            comboBoxes.Clear();
            List<int> items = new List<int>();
            for (int i = 1; i <= userControl.TotalNumber; ++i) items.Add(i);
            foreach (var cell in cells)
            {
                ComboBox comboBox = new ComboBox();
                comboBox.ItemsSource = items;
                comboBox.Width = width;
                comboBox.Height = height;
                comboBox.Margin = new Thickness(cell.point.X * width, cell.point.Y * height, 0, 0);
                comboBox.SelectedItem = cell.number;
                userControl.MainField.Children.Add(comboBox);
                comboBoxes.Add(comboBox);
                Canvas.SetZIndex(comboBox, -100);
                comboBox.SelectionChanged += clickComboBox;
                comboBox.MouseRightButtonUp += clickDoubleComboBox;
            }

        }

        #endregion



        #region TotalNumber

        private static void OnTotalNumberChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            UserControl1 userControl = (UserControl1)d;
            int row = (int)e.NewValue;
            if (row == (int)e.OldValue) return;
            foreach (var p in comboBoxes)
            {
                userControl.MainField.Children.Remove(p);
            }
            comboBoxes.Clear();
            userControl.Cells.Clear();
        }

        private static object CoerceTotalNumber(DependencyObject d, object baseValue)
        {
            UserControl1 userControl = (UserControl1)d;
            int current = (int)baseValue;
            if (current < 0) current = 0;
            return current;
        }

        #endregion


        // удаление комбо-бокса
        private static void clickDoubleComboBox(object sender, MouseButtonEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            Canvas cv = (Canvas)comboBox.Parent;
            ScrollViewer scroll = (ScrollViewer)((Canvas)cv.Parent).Parent;
            UserControl1 userControl = (UserControl1)scroll.Parent;
            int cellX = (int)(comboBox.Margin.Left / width);
            int cellY = (int)(comboBox.Margin.Top / height);

            element el = userControl.Cells.Find(x => (x.point == new Point(cellX, cellY)));
            if (el != null)
            {
                comboBoxes.Remove(comboBox);
                cv.Children.Remove(comboBox);
                userControl.Cells.Remove(el);
            }

        }

        

        // Изменение выбраного элемента для комбо-бокса
        private static void clickComboBox(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            int ind = (int)e.AddedItems[0];
            Canvas cv = (Canvas)comboBox.Parent;
            ScrollViewer scroll = (ScrollViewer)((Canvas)cv.Parent).Parent;
            UserControl1 userControl = (UserControl1)scroll.Parent;
            int cellX = (int)(comboBox.Margin.Left / width);
            int cellY = (int)(comboBox.Margin.Top / height);

            element el = userControl.Cells.Find(x => (x.point == new Point(cellX, cellY)));
            if (el != null)
            {
                int index = userControl.Cells.IndexOf(el);
                userControl.Cells[index].number = ind;
            }
        }

        // Добавление комбо-бокса
        private void MainField_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point point = e.GetPosition(MainField);
            int cellX = (int)(point.X / width);
            int cellY = (int)(point.Y / height);
            if (Cells == null) Cells = new List<element>();

            List<int> items = new List<int>();
            for (int i = 1; i <= TotalNumber; ++i) items.Add(i);
            ComboBox comboBox = new ComboBox();
            comboBox.ItemsSource = items;
            comboBox.Width = width;
            comboBox.Height = height;
            comboBox.Margin = new Thickness(cellX * width, cellY * height, 0, 0);
            comboBox.SelectedItem = 1;
            comboBox.IsDropDownOpen = true;
            MainField.Children.Add(comboBox);
            comboBoxes.Add(comboBox);
            Canvas.SetZIndex(comboBox, -100);
            comboBox.SelectionChanged += clickComboBox;
            comboBox.MouseRightButtonUp += clickDoubleComboBox;
            Cells.Add(new element(new Point(cellX, cellY), 1));

        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }

    public class element
    {
        public element(Point p, int n)
        {
            point = p;
            number = n;
        }
        /// <summary>
        /// Координаты ячейки. Левая верхняя имеет координаты (0,0)
        /// </summary>
        public Point point;
        /// <summary>
        /// Выбранный элемент ComboBox-а для данной ячейки
        /// </summary>
        public int number;
    }

}
