using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class ReportWindow : Window
    {
        public ReportWindow()
        {
            InitializeComponent();
            txtDate.Text = "Дата формирования: " + DateTime.Now.ToString("dd.MM.yyyy HH:mm");
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var products = ProductService.GetAll();
                dgReport.ItemsSource = products;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog pd = new PrintDialog();
            if (pd.ShowDialog() == true)
            {
                var doc = new System.Windows.Documents.FlowDocument(
                    new System.Windows.Documents.Paragraph(
                        new System.Windows.Documents.Run("Отчёт по остаткам на складе"))
                    {
                        FontSize = 18,
                        FontWeight = System.Windows.FontWeights.Bold
                    });

                foreach (var item in (System.Collections.IEnumerable)dgReport.ItemsSource)
                {
                    Product p = (Product)item;
                    doc.Blocks.Add(new System.Windows.Documents.Paragraph(
                        new System.Windows.Documents.Run(
                            p.Naimenovanie + " | " + p.Kategoriya +
                            " | " + p.TsenaText + " руб. | Остаток: " + p.Ostatki)));
                }

                System.Windows.Documents.IDocumentPaginatorSource src = doc;
                pd.PrintDocument(src.DocumentPaginator, "Отчёт по остаткам");
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}