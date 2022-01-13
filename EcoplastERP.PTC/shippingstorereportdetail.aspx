<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="shippingstorereportdetail.aspx.cs" Inherits="EcoplastERP.PTC.shippingstorereportdetail" %>

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
    <script type="text/javascript">
        function returnBack() {
            window.location.href = "shippingstorereport.aspx?rt=" + getParameter('rt') + "&dn=" + getParameter('dn');
        }
        function getParameter(name) {
            if (name = (new RegExp('[?&]' + encodeURIComponent(name) + '=([^&]*)')).exec(location.search))
                return decodeURIComponent(name[1]);
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div id="loading" style="display: none;">
        <img id="loading-image" src="images/spin.gif" alt="Yükleniyor..." />
    </div>
    <ul>
        <li><a onclick="returnBack();" style="cursor: pointer;">Geri</a></li>
    </ul>

    <asp:RadioButtonList ID="rblReportType2" runat="server" RepeatDirection="Horizontal" RepeatColumns="2" Width="100%" Font-Size="Large" AutoPostBack="true" OnSelectedIndexChanged="rblReportType2_SelectedIndexChanged">
        <asp:ListItem Value="palette" Selected="True">Palet Bazında</asp:ListItem>
        <asp:ListItem Value="barcode">Barkod Bazında</asp:ListItem>
    </asp:RadioButtonList>

    <table>
        <tr>
            <td>Toplam:</td>
            <td>
                <asp:Label runat="server" ID="lblTotalQuantity" Font-Bold="true"></asp:Label></td>
        </tr>
    </table>

    <asp:Repeater ID="rptPaletteReport" runat="server" DataSourceID="SqlDataSource1">
        <HeaderTemplate>
            <table class="table-fill">
                <thead>
                    <tr>
                        <th style="width: 40%;">Depo Kodu</th>
                        <th style="width: 40%;">Palet No</th>
                        <th style="width: 20%;">Miktar</th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%#Eval("WarehouseCode") %></td>
                <td><%#Eval("PaletteNumber")%></td>
                <td><%#Eval("Quantity", "{0:0.00}") %></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>

    <asp:Repeater ID="rptBarcodeReport" runat="server" DataSourceID="SqlDataSource2">
        <HeaderTemplate>
            <table class="table-fill">
                <thead>
                    <tr>
                        <th style="width: 20%;">Depo Kodu</th>
                        <th style="width: 30%;">Palet No</th>
                        <th style="width: 30%;">Barkod</th>
                        <th style="width: 20%;">Miktar</th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%#Eval("WarehouseCode") %></td>
                <td><%#Eval("PaletteNumber")%></td>
                <td><%#Eval("Barcode")%></td>
                <td><%#Eval("Quantity", "{0:0.00}") %></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="if((select Code from ProductGroup where Oid = (select ProductGroup from Product where Oid = (select Product from SalesOrderDetail where Oid = @salesOrderDetail))) = 'OM') begin select W.Code as WarehouseCode, S.PaletteNumber, SUM(S.Quantity) as Quantity from Store S inner join Warehouse W on W.Oid = S.Warehouse where S.GCRecord is null and S.SalesOrderDetail = @salesOrderDetail group by W.Code, S.PaletteNumber end else if((select Code from ProductGroup where Oid = (select ProductGroup from Product where Oid = (select Product from SalesOrderDetail where Oid = @salesOrderDetail))) = 'SM') begin select W.Code as WarehouseCode, S.PaletteNumber, SUM(S.Quantity) as Quantity from Store S inner join Warehouse W on W.Oid = S.Warehouse where S.GCRecord is null and S.Product = (select Product from SalesOrderDetail where Oid = @salesOrderDetail) group by W.Code, S.PaletteNumber end" OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:Parameter Name="salesOrderDetail" DbType="String" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="if((select Code from ProductGroup where Oid = (select ProductGroup from Product where Oid = (select Product from SalesOrderDetail where Oid = @salesOrderDetail))) = 'OM') begin select W.Code as WarehouseCode, S.PaletteNumber, S.Barcode, S.Quantity from Store S inner join Warehouse W on W.Oid = S.Warehouse where S.GCRecord is null and S.SalesOrderDetail = @salesOrderDetail end else if((select Code from ProductGroup where Oid = (select ProductGroup from Product where Oid = (select Product from SalesOrderDetail where Oid = @salesOrderDetail))) = 'SM') begin select W.Code as WarehouseCode, S.PaletteNumber, S.Barcode, S.Quantity from Store S inner join Warehouse W on W.Oid = S.Warehouse where S.GCRecord is null and S.Product = (select Product from SalesOrderDetail where Oid = @salesOrderDetail) end" OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:Parameter Name="salesOrderDetail" DbType="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
