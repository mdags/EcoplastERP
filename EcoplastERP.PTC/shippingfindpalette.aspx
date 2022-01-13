<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="shippingfindpalette.aspx.cs" Inherits="EcoplastERP.PTC.shippingfindpalette" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .ShadowNone {
            text-shadow: none;
            font-size: 14px;
            font-weight: 600;
            text-align: right;
        }

        #loading {
            width: 100%;
            height: 100%;
            top: 0;
            left: 0;
            position: fixed;
            display: block;
            opacity: 0.7;
            background-color: #fff;
            z-index: 999;
            text-align: center;
        }

        #loading-image {
            position: absolute;
            z-index: 1000;
            left: 45%;
            top: 45%;
        }
    </style>
    <script type="text/javascript">
        function disableButtons() {
            $('#loading').show();
        }
        function completed() {
            $('#loading').hide();
        }
        window.onbeforeunload = disableButtons;
        window.onload = completed;
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="loading" style="display: none;">
        <img id="loading-image" src="images/spin.gif" alt="Yükleniyor..." />
    </div>
    <ul>
        <li><a href="shippinghome.aspx">Geri</a></li>
    </ul>

    <table class="maintable">
        <tr>
            <td style="width: 15%;"><strong>Palet No: </strong></td>
            <td style="width: 75%;">
                <asp:TextBox ID="txtBarcode" runat="server" placeholder="Palet No" Width="95%"></asp:TextBox></td>
            <td style="width: 25%;">
                <asp:Button ID="btnGet" runat="server" Text="Getir" Width="95%" Font-Size="Small" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" OnClick="btnGet_Click" /></td>
        </tr>
        <tr>
            <td><strong>Brüt :</strong></td>
            <td>
                <asp:Label ID="lblTotalGross" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td><strong>Dara :</strong></td>
            <td>
                <asp:Label ID="lblTare" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td><strong>Net :</strong></td>
            <td>
                <asp:Label ID="lblTotalNet" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>

    <asp:Repeater ID="rptRecords" runat="server" DataSourceID="SqlDataSource1">
        <HeaderTemplate>
            <table class="table-fill">
                <thead>
                    <tr>
                        <th style="width: 25%;">Depo</th>
                        <th style="width: 25%;">Sipariş No</th>
                        <th style="width: 20%;">Barcode</th>
                        <th style="width: 15%;">Brüt</th>
                        <th style="width: 15%;">Net</th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%#Eval("WarehouseCode") %></td>
                <td><%#Eval("OrderNumber")%></td>
                <td><%#Eval("Barcode")%></td>
                <td><%#Eval("GrossQuantity", "{0:0.00}") %></td>
                <td><%#Eval("NetQuantity", "{0:0.00}") %></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select W.Code as [WarehouseCode], O.OrderNumber, S.Barcode, P.GrossQuantity, P.NetQuantity, E.NameSurname as [Operator], P.ProductionDate from Store S inner join Production P on P.Barcode = S.Barcode inner join Warehouse W on W.Oid = S.Warehouse inner join SalesOrderDetail D on D.Oid = S.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Employee E on E.Oid = P.Employee where S.GCRecord is null and S.PaletteNumber = @paletteNumber" OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:Parameter Name="paletteNumber" DbType="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
