using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;

namespace SampleWPS
{
    class cAssetsData
    {
        public SqlConnection CreateConnection()
        {
            string connstr = ConfigurationManager.ConnectionStrings["Demo"].ToString();
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            return conn;
        }
     
        public cAssetsData()
        {
            //CODEGEN: This call is required by the ASP.NET Web Services Designer

            InitializeComponent();
        }

       
        private System.Data.SqlClient.SqlCommand sqlCmdSelAssetReturns;
        
        private System.Data.SqlClient.SqlCommand sqlCmdSelAssets;
        private System.Data.SqlClient.SqlCommand sqlCmdGetLookupValues;
       
        #region Component Designer generated code

        //Required by the Web Services Designer 
        //    private IContainer components = null;

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
           
            this.sqlCmdSelAssetReturns = new System.Data.SqlClient.SqlCommand();
            
            this.sqlCmdSelAssets = new System.Data.SqlClient.SqlCommand();
            this.sqlCmdGetLookupValues = new System.Data.SqlClient.SqlCommand();
            
            
             // 
            // sqlCmdSelAssetReturns
            // 
            this.sqlCmdSelAssetReturns.CommandText = "dbo.[usp_sel_AssetReturns]";
            this.sqlCmdSelAssetReturns.CommandType = System.Data.CommandType.StoredProcedure;
            this.sqlCmdSelAssetReturns.Connection = CreateConnection();
            this.sqlCmdSelAssetReturns.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(10)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
            this.sqlCmdSelAssetReturns.Parameters.Add(new System.Data.SqlClient.SqlParameter("@pintAssetID", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
            this.sqlCmdSelAssetReturns.Parameters.Add(new System.Data.SqlClient.SqlParameter("@pdtStart", System.Data.SqlDbType.DateTime, 8));
            this.sqlCmdSelAssetReturns.Parameters.Add(new System.Data.SqlClient.SqlParameter("@pdtEnd", System.Data.SqlDbType.DateTime, 8));
             // 
            // sqlCmdSelAssets
            // 
            this.sqlCmdSelAssets.CommandText = "dbo.[usp_sel_AssetsByType]";
            this.sqlCmdSelAssets.CommandType = System.Data.CommandType.StoredProcedure;
            this.sqlCmdSelAssets.Connection = CreateConnection();
            this.sqlCmdSelAssets.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(10)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
            this.sqlCmdSelAssets.Parameters.Add(new System.Data.SqlClient.SqlParameter("@pintClass", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
            this.sqlCmdSelAssets.Parameters.Add(new System.Data.SqlClient.SqlParameter("@pintType", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.Input, false, ((System.Byte)(10)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
            // 
            // sqlCmdGetLookupValues
            // 
            this.sqlCmdGetLookupValues.CommandText = "dbo.[usp_Get_Lookup_Values]";
            this.sqlCmdGetLookupValues.CommandType = System.Data.CommandType.StoredProcedure;
            this.sqlCmdGetLookupValues.Connection = CreateConnection();
            this.sqlCmdGetLookupValues.Parameters.Add(new System.Data.SqlClient.SqlParameter("@RETURN_VALUE", System.Data.SqlDbType.Int, 4, System.Data.ParameterDirection.ReturnValue, false, ((System.Byte)(10)), ((System.Byte)(0)), "", System.Data.DataRowVersion.Current, null));
            this.sqlCmdGetLookupValues.Parameters.Add(new System.Data.SqlClient.SqlParameter("@pstrSection", System.Data.SqlDbType.VarChar, 25));
            this.sqlCmdGetLookupValues.Parameters.Add(new System.Data.SqlClient.SqlParameter("@pstrShortValue", System.Data.SqlDbType.VarChar, 25));

            
        }


        #endregion
        /*==============================================================================
            ' Procedure:		usp_Get_Lookup_Values
            ' Purpose:     		Retrieve the appropriate Lookup values from the Lookup table
            ' Arguments:  	  	pstrSection - The value for the Section field
            '			        pstrShortValue - The value for the ShortValue field (not required)
            ' Returns:		Nothing	
         */

        public DataTable GetLookupValues(string Section, string Style = "")
        {
            
            using (CreateConnection())
            {
                sqlCmdGetLookupValues.Parameters["@pstrSection"].Value = Section;
                if (Style != "") // this is an optional parameter
                    sqlCmdGetLookupValues.Parameters["@pstrShortValue"].Value = Style;

                SqlDataReader reader = sqlCmdGetLookupValues.ExecuteReader();

                // make datatable with same structure as reader
                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("LookupID", typeof(int)));
                dt.Columns.Add(new DataColumn("LongValue", typeof(string)));
                dt.Columns.Add(new DataColumn("ShortValue", typeof(string)));
                //populate datatable with reader values
                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr["LookupID"] = reader.GetInt32(0); //cast to int and read into data row
                    dr["LongValue"] = reader.GetString(1);
                    dr["ShortValue"] = reader.GetString(2);
                    dt.Rows.Add(dr);

                }
                
                return dt;
            }

        }

        // Returns a DataTable of (AssetID, AssetName) pairs
        // of all managers in the given asset class

       
        public DataTable SelAssets(int TypeID)
        {
            using (CreateConnection())
            {
                sqlCmdSelAssets.Parameters["@pintType"].Value = TypeID;
                SqlDataReader reader = sqlCmdSelAssets.ExecuteReader();


                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("AssetID", typeof(int)));
                dt.Columns.Add(new DataColumn("AssetName", typeof(string)));
                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr["AssetID"] = reader.GetInt32(0);
                    dr["AssetName"] = reader.GetString(1).ToString();

                    dt.Rows.Add(dr);
                }

               
                return dt;
            }
        }

        
        // Returns a DataTable of (ReturnDate, ReturnPct) pairs
        // with all the historical returns data for the given manager

        public DataTable SelAssetReturns(int ManagerID)
        {
            using (CreateConnection())
            {
                sqlCmdSelAssetReturns.Parameters["@pintAssetID"].Value = ManagerID;
                sqlCmdSelAssetReturns.Parameters["@pdtStart"].Value = Convert.ToDateTime("1/1/1900");
                sqlCmdSelAssetReturns.Parameters["@pdtEnd"].Value = DateTime.Now;
                SqlDataReader reader = sqlCmdSelAssetReturns.ExecuteReader();

                DataTable dt = new DataTable();
                dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));
                dt.Columns.Add(new DataColumn("Return %", typeof(float)));

                while (reader.Read())
                {
                    DataRow dr = dt.NewRow();
                    dr["Date"] = reader.GetDateTime(0);
                    dr["Return %"] = (float)Math.Round(100 * reader.GetDouble(1), 2);
                    dt.Rows.Add(dr);
                }

                return dt;
            }

        }
       

    }

}
