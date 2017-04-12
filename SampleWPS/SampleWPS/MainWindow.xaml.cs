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
using System.Data;
using System.Windows.Controls.DataVisualization.Charting;


namespace SampleWPS
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Fill_lstManagers();
        }
        /*
        Method Name:Fill_lstManagers()
        Description"This method populates the managers in the ListView from the database
        Returns: Nothing
        */
        private void Fill_lstManagers()
        {
            //create an object of the class AssetsData.The class fetches all the data from the database
            cAssetsData Service = new cAssetsData();

            DataTable dt = Service.GetLookupValues("Asset", "Manager");
            //based on lookupID populate the datatable with AssetID and AssetName
            DataTable dtManagers = Service.SelAssets((int)(dt.Rows[0])["LookupID"]);

            //	Clear the Items from the listview
            this.lstManagers.Items.Clear();
            //bind the listview to the datasource
            lstManagers.ItemsSource = dtManagers.DefaultView;
            
        }


        //This event fires when the the selection of Asset changes in the ListView.It populates the corresponding returns datagrid 
        private void lstManagers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cAssetsData Service = new cAssetsData();
            DataRowView drv = (DataRowView)lstManagers.SelectedItem;
            int assetID = Convert.ToInt32(drv["AssetID"]);
            
            //store returns data in the datatable as per the assetID.
            DataTable dt = Service.SelAssetReturns(assetID);
            //bind the datatable to the datagrid uwgAssetReturns
            this.uwgAssetReturns.ItemsSource = dt.DefaultView;
            
            ((LineSeries)MyLineChart.Series[0]).ItemsSource = dt.DefaultView;
            ((ColumnSeries)MyColumnChart.Series[0]).ItemsSource = dt.DefaultView;
        }

    }
    
}
