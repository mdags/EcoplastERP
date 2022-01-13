<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FindBarcodeWebUserControl.ascx.cs" Inherits="EcoplastERP.Web.UserForms.FindBarcodeWebUserControl" %>
<%@ Register Assembly="DevExpress.Web.v20.1, Version=20.1.7.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" Namespace="DevExpress.Web" TagPrefix="dx" %>

<table style="width: 100%;">
    <tr>
        <td style="width: 10%;">Barkod:</td>
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
        <td style="width: 10%;">Müşteri:</td>
        <td style="width: 90%;">
            <dx:ASPxLabel ID="lblContact" runat="server" Text=""></dx:ASPxLabel>
        </td>
    </tr>
</table>
<table style="width: 100%;">
    <tr>
        <td style="width: 10%;">İş Emri No:</td>
        <td style="width: 40%;">
            <dx:ASPxLabel ID="lblWorkOrderNumber" runat="server" Text=""></dx:ASPxLabel>
        </td>
        <td style="width: 10%;">Makine:</td>
        <td style="width: 40%;">
            <dx:ASPxLabel ID="lblMachine" runat="server" Text=""></dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td style="width: 10%;">Stok Kodu:</td>
        <td style="width: 40%;">
            <dx:ASPxLabel ID="lblProductCode" runat="server" Text=""></dx:ASPxLabel>
        </td>
        <td style="width: 10%;">Sip.No:</td>
        <td style="width: 40%;">
            <dx:ASPxLabel ID="lblOrderNumber" runat="server" Text=""></dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td style="width: 10%;">Vardiya:</td>
        <td style="width: 40%;">
            <dx:ASPxLabel ID="lblShift" runat="server" Text=""></dx:ASPxLabel>
        </td>
        <td style="width: 10%;">Operatör:</td>
        <td style="width: 40%;">
            <dx:ASPxLabel ID="lblEmployee" runat="server" Text=""></dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td style="width: 10%;">Palet No:</td>
        <td style="width: 40%;">
            <dx:ASPxLabel ID="lblPaletteNumber" runat="server" Text=""></dx:ASPxLabel>
        </td>
        <td style="width: 10%;">Üret.Tar.:</td>
        <td style="width: 40%;">
            <dx:ASPxLabel ID="lblProductionDate" runat="server" Text=""></dx:ASPxLabel>
        </td>
    </tr>
    <tr>
        <td style="width: 10%;">Brüt:</td>
        <td style="width: 40%;">
            <dx:ASPxLabel ID="lblGrossQuantity" runat="server" Text=""></dx:ASPxLabel>
        </td>
        <td style="width: 10%;">Net:</td>
        <td style="width: 40%;">
            <dx:ASPxLabel ID="lblNetQuantity" runat="server" Text=""></dx:ASPxLabel>
        </td>
    </tr>
</table>

<dx:ASPxGridView ID="gridView1" runat="server" Width="100%" DataSourceID="SqlDataSource1" AutoGenerateColumns="true"></dx:ASPxGridView>

<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="IF OBJECT_ID('tempdb..#movements') IS NOT NULL DROP TABLE #movements
            create table #movements([Barkod] nvarchar(100), [Hareket Türü] nvarchar(100), [Malzeme Adı] nvarchar(100), [Depo Kodu] nvarchar(100), [Birim] nvarchar(10), [Miktar] money)
            insert into #movements(Barkod, [Hareket Türü], [Malzeme Adı], [Depo Kodu], Birim, Miktar)
            select M.Barcode as [Barkod], T.Name as [Hareket Türü], P.Name as [Malzeme Adı], W.Code as [Depo Kodu], U.Code as [Birim], M.Quantity as [Miktar] from Movement M inner join MovementType T on T.Oid = M.MovementType inner join Warehouse W on W.Oid = M.Warehouse inner join Product P on P.Oid = M.Product inner join Unit U on U.Oid = M.Unit where M.GCRecord is null and HeaderId in (select top 1 HeaderId from Movement where GCRecord is null and MovementType = (select Oid from MovementType where GCRecord is null and Code = 'P110') and Barcode = @barcode) order by M.DocumentDate desc
            insert into #movements(Barkod, [Hareket Türü], [Malzeme Adı], [Depo Kodu], Birim, Miktar)
            select R.Barcode, 'Üretime Çıkış', (select Name from Product where GCRecord is null and Oid = (select Product from SalesOrderDetail where GCRecord is null and Oid = (select SalesOrderDetail from Production where GCRecord is null and Barcode = @barcode))), '', U.Code, M.Quantity from ReadResourceMovement M inner join ReadResource R on R.Oid = M.ReadResource inner join Unit U on U.Oid = R.Unit where M.GCRecord is null and M.ProductionBarcode = @barcode
            select * from #movements" OnSelecting="SqlDataSource1_Selecting">
    <SelectParameters>
        <asp:Parameter Name="barcode" DbType="String" />
    </SelectParameters>
</asp:SqlDataSource>
