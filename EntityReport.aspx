
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EntityReport.aspx.cs" Inherits="FundingTrackingReports.EntityReport"  %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=9">
</head>
<body>
<form id="form1" runat="server">
    <div>
    </div>
    <CR:CrystalReportViewer ID="EntityReportViewer" runat="server" AutoDataBind="true" />
   
</form>
</body>
</html>