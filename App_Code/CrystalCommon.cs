
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections;
using System.Configuration;
using System.Data.SqlClient;

namespace FundingTrackingReports
{
    public class ReportBase
    {
        private static string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        private static SqlConnectionStringBuilder sConn = new SqlConnectionStringBuilder(ReportBase.connectionString);
        private string ServerName = ReportBase.sConn.DataSource;
        private string DataBaseName = ReportBase.sConn.InitialCatalog;
        private string UserID = string.Empty;
        private string Password = string.Empty;
        private ReportDocument crReportDocument = new ReportDocument();

      
        public ReportDocument PFSubReportConnectionMainParameter(
          string ReportName,
          ArrayList ParamArraylist,
          string ReportFolder)
        {
            int index1 = 0;
            string filename = ReportFolder + "\\" + ReportName;
            try
            {
                this.crReportDocument.Load(filename);
            }
            catch (Exception ex)
            {
                throw new Exception("The report file " + filename + " can not be loaded, ensure that the report exists or the path is correct.\nException:\n" + ex.Message + "\nSource:" + ex.Source + "\nStacktrace:" + ex.StackTrace);
            }
            if (!this.Logon(this.crReportDocument, this.ServerName, this.DataBaseName, this.UserID, this.Password))
                throw new Exception("Can not login to Report Server \nDatabase Server: " + this.ServerName + "\nDatabase:\n" + this.DataBaseName + "\nDBUser:" + this.UserID + "\nDBPassword:" + this.Password);
            this.Logon(this.crReportDocument, this.ServerName, this.DataBaseName, this.UserID, this.Password);
            if (ParamArraylist.Count != 0)
            {
                int num = ParamArraylist.Count / 2;
                num = this.crReportDocument.DataDefinition.ParameterFields.Count;
                for (int index2 = 0; index2 < ParamArraylist.Count / 2; ++index2)
                {
                    this.PassParameter((int)ParamArraylist[index1], (string)ParamArraylist[index1 + 1]);
                    index1 += 2;
                }
            }
            return this.crReportDocument;
        }

        public void PassParameter(int ParameterIndex, string ParameterValue)
        {
            ParameterFieldDefinition parameterField = this.crReportDocument.DataDefinition.ParameterFields[ParameterIndex];
            ParameterValues currentValues = parameterField.CurrentValues;
            currentValues.Add((ParameterValue)new ParameterDiscreteValue()
            {
                Value = (object)Convert.ToString(ParameterValue)
            });
            parameterField.ApplyCurrentValues(currentValues);
        }

        private bool Logon(
          ReportDocument cr,
          string server,
          string database,
          string user_id,
          string password)
        {
            ConnectionInfo ci = new ConnectionInfo();
            ci.ServerName = server;
            ci.DatabaseName = database;
            ci.IntegratedSecurity = true;
            if (!this.ApplyLogon(cr, ci))
                return false;
            foreach (ReportObject reportObject in (SCRCollection)cr.ReportDefinition.ReportObjects)
            {
                if (reportObject.Kind == ReportObjectKind.SubreportObject)
                {
                    SubreportObject subreportObject = (SubreportObject)reportObject;
                    if (!this.ApplyLogon(cr.OpenSubreport(subreportObject.SubreportName), ci))
                        return false;
                }
            }
            return true;
        }

        private bool ApplyLogon(ReportDocument cr, ConnectionInfo ci)
        {
            foreach (Table table in (SCRCollection)cr.Database.Tables)
            {
                TableLogOnInfo logOnInfo = table.LogOnInfo;
                logOnInfo.ConnectionInfo.ServerName = ci.ServerName;
                logOnInfo.ConnectionInfo.DatabaseName = ci.DatabaseName;
                logOnInfo.ConnectionInfo.UserID = ci.UserID;
                logOnInfo.ConnectionInfo.Password = ci.Password;
                table.ApplyLogOnInfo(logOnInfo);
                table.Location = ci.DatabaseName + ".dbo." + table.Name;
                if (!table.TestConnectivity())
                    return false;
            }
            return true;
        }
    }
}