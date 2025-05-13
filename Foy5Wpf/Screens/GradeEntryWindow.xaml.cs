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
using System.Windows.Shapes;

namespace Foy5Wpf.Screens
{
    public partial class GradeEntryWindow : Window
    {
        TextBox txtCourseId, txtMidterm, txtFinal;
        Button btnLoadAssignments, btnSaveGrades;
        ListBox lstAssignments;
        OkulContext db = new OkulContext();
        int selectedAssignmentId = -1;
        public GradeEntryWindow()
        {
            InitializeComponent();
            var root = new StackPanel { Margin = new Thickness(20) };
            this.Content = root;

            txtCourseId = CreateTextBox("Ders ID");
            btnLoadAssignments = new Button { Content = "Getir", Width = 75, Margin = new Thickness(0, 0, 0, 10) };
            btnLoadAssignments.Click += btnLoadAssignments_Click;
            root.Children.Add(txtCourseId);
            root.Children.Add(btnLoadAssignments);

            lstAssignments = new ListBox { Height = 150, Margin = new Thickness(0, 0, 0, 10) };
            lstAssignments.SelectionChanged += lstAssignments_SelectionChanged;
            root.Children.Add(lstAssignments);

            txtMidterm = CreateTextBox("Vize");
            txtFinal = CreateTextBox("Final");
            root.Children.Add(txtMidterm);
            root.Children.Add(txtFinal);

            btnSaveGrades = new Button { Content = "Kaydet", Width = 75 };
            btnSaveGrades.Click += btnSaveGrades_Click;
            root.Children.Add(btnSaveGrades);
        }

        private TextBox CreateTextBox(string watermark)
        {
            var tb = new TextBox { Text = watermark, Margin = new Thickness(0, 0, 0, 5) };
            tb.GotFocus += (s, e) => { if (tb.Text == watermark) tb.Text = ""; };
            tb.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(tb.Text)) tb.Text = watermark; };
            return tb;
        }

        private void btnLoadAssignments_Click(object s, RoutedEventArgs e)
        {
            if (!int.TryParse(txtCourseId.Text, out int cid)) return;
            lstAssignments.ItemsSource = db.tOgrenciDersler
                .Where(ad => ad.dersID == cid)
                .Select(ad => $"{ad.ID}: Ogr={ad.ogrenciID}, Vize={ad.vize}, Final={ad.@final}")
                .ToList();
        }

        private void lstAssignments_SelectionChanged(object s, SelectionChangedEventArgs e)
        {
            if (lstAssignments.SelectedItem == null) return;
            selectedAssignmentId = int.Parse(lstAssignments.SelectedItem.ToString().Split(':')[0]);
            var a = db.tOgrenciDersler.Find(selectedAssignmentId);
            txtMidterm.Text = a.vize?.ToString() ?? "";
            txtFinal.Text = a.@final?.ToString() ?? "";
        }

        private void btnSaveGrades_Click(object s, RoutedEventArgs e)
        {
            if (selectedAssignmentId < 0) return;
            var a = db.tOgrenciDersler.Find(selectedAssignmentId);
            if (int.TryParse(txtMidterm.Text, out int mv)) a.vize = mv;
            if (int.TryParse(txtFinal.Text, out int fv)) a.@final = fv;
            db.SaveChanges();
            MessageBox.Show("Notlar kaydedildi.");
        }
    }
}