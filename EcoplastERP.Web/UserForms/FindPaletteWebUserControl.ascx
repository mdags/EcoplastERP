<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FindPaletteWebUserControl.ascx.cs" Inherits="EcoplastERP.Web.UserForms.FindPaletteWebUserControl" %>
<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<table style="width: 100%;">
    <tr>
        <td style="width: 10%;">Palet No:</td>
        <td style="width: 80%;">
            <dx:ASPxTextBox ID="txtBarcode" runat="server" Width="100%"></dx:ASPxTextBox>
        </td>
        <td style="width: 10%;">
            <dx:ASPxButton ID="btnReport" runat="server" Text="Listele" Width="100%" OnClick="btnReport_Click"></dx:ASPxButton>
        </td>
    </tr>
</table>
<table style="width: 100%;">
    <tr>
        <td style="width: 10%;">Son Ağırlık:</td>
        <td style="width: 40%;">
            <dx:ASPxLabel ID="lblLastWeight" runat="server" Text=""></dx:ASPxLabel>
        </td>
        <td style="width: 10%;">Dara:</td>
        <td style="width: 40%;">
            <dx:ASPxLabel ID="lblTare" runat="server" Text=""></dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td style="width: 10%;">Brüt:</td>
        <td style="width: 40%;">
            <dx:ASPxLabel ID="lblGross" runat="server" Text=""></dx:ASPxLabel>
        </td>
        <td style="width: 10%;">Net:</td>
        <td style="width: 40%;">
            <dx:ASPxLabel ID="lblNet" runat="server" Text=""></dx:ASPxLabel>
        </td>
    </tr>
</table>

<dx:ASPxGridView ID="gridView1" runat="server" Width="100%" DataSourceID="SqlDataSource1" AutoGenerateColumns="true"></dx:ASPxGridView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select (case when P.GCRecord is null then 'Aktif' else 'Silinmiş' end) as [Durumu], P.Barcode as [Barkod], P.WorkOrderNumber as [Üretim Siparişi No], M.Code as [Makine], O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as [Sipariş No], (case when S.Shift = 1 then 'A' when S.Shift = 2 then 'B' when S.Shift = 3 then 'C' else 'NULL' end) as [Vardiya], GrossQuantity as [Brüt], NetQuantity as [Net], ProductionDate as [Üretim Tarihi] from Production P inner join Machine M on M.Oid = P.Machine inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join ShiftStart S on S.Oid = P.Shift inner join Employee E on E.Oid = P.Employee where P.GCRecord is null and ProductionPalette in (select Oid from ProductionPalette where GCRecord is null and PaletteNumber = @barcode)"
    OnSelecting="SqlDataSource1_Selecting">
    <SelectParameters>
        <asp:Parameter Name="barcode" DbType="String" />
    </SelectParameters>
</asp:SqlDataSource>
