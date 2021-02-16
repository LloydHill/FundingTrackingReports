using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Web;
using System;
using System.Collections;
using System.Drawing;
using System.Web;
using System.Web.UI;

namespace FundingTrackingReports{
    public partial class EntityReport : System.Web.UI.Page
    {
        private ArrayList ParameterArrayList = new ArrayList();
        private ReportDocument ObjReportClientDocument = new ReportDocument();


        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.IsPostBack)
                return;
           string str = this.Request.QueryString["Entity"];
            ParameterArrayList.Add(0);
            ParameterArrayList.Add(str);
            GetReportDocument();
            ViewCrystalReport();
        }

        private void GetReportDocument()
        {
            var reportBase = new ReportBase();
            string empty1 = string.Empty;
            string empty2 = string.Empty;
            string ReportName = "Entity.rpt";
            string ReportFolder = this.Server.MapPath("~");
            ObjReportClientDocument = reportBase.PFSubReportConnectionMainParameter(ReportName, this.ParameterArrayList, ReportFolder);
            foreach (Section section in (SCRCollection)this.ObjReportClientDocument.ReportDefinition.Sections)
            {
                foreach (ReportObject reportObject1 in (SCRCollection)section.ReportObjects)
                {
                    if (this.ObjReportClientDocument.ReportDefinition.ReportObjects[reportObject1.Name] is FieldObject reportObject)
                    {
                        Font font1 = new Font("Arial Narrow", reportObject.Font.Size - 1f);
                        Font font2 = new Font("Arial", reportObject.Font.Size - 1f);
                        if (font1 != null)
                            reportObject.ApplyFont(font1);
                        else if (font2 != null)
                            reportObject.ApplyFont(font2);
                    }
                }
            } 
        }         private void ViewCrystalReport() => EntityReportViewer.ReportSource = (object)this.ObjReportClientDocument;
    }
}