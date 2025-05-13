using Foy5Wpf.Models;
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

namespace Foy5Wpf.Screens;
    public partial class FacultyWindow : Window
    {
        TextBox txtFacultyName;
        Button btnAdd;
        ListBox lstFaculties;
        OkulContext db = new OkulContext();
    public FacultyWindow()
    {
        InitializeComponent();
        var root = new StackPanel { Margin = new Thickness(20) };
        this.Content = root;

        txtFacultyName = new TextBox { Text = "Fakülte Adı", Margin = new Thickness(0, 0, 0, 10) };
        txtFacultyName.GotFocus += (s, e) => { if (txtFacultyName.Text == "Fakülte Adı") txtFacultyName.Text = ""; };
        txtFacultyName.LostFocus += (s, e) => { if (string.IsNullOrWhiteSpace(txtFacultyName.Text)) txtFacultyName.Text = "Fakülte Adı"; };

        btnAdd = new Button { Content = "Ekle", Width = 80, Height = 30, Margin = new Thickness(0, 0, 0, 10) };
        btnAdd.Click += btnAdd_Click;

        lstFaculties = new ListBox { Height = 150 };

        root.Children.Add(txtFacultyName);
        root.Children.Add(btnAdd);
        root.Children.Add(lstFaculties);

        LoadFaculties();
    }

    private void btnAdd_Click(object s, RoutedEventArgs e)
    {
        if (txtFacultyName.Text == "Fakülte Adı") return;
        db.tFakulteler.Add(new tFakulte { fakulteAd = txtFacultyName.Text });
        db.SaveChanges();
        LoadFaculties();
        txtFacultyName.Text = "Fakülte Adı";
    }

    private void LoadFaculties()
    {
        lstFaculties.ItemsSource = db.tFakulteler
            .Select(f => $"{f.fakulteID} - {f.fakulteAd}")
            .ToList();
    }
}