<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="transfersalesorderreport.aspx.cs" Inherits="EcoplastERP.PTC.transfersalesorderreport" %>

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
    <asp:ScriptManager ID="scriptManager1" runat="server" EnablePageMethods="true"></asp:ScriptManager>
    <div id="loading" style="display: none;">
        <img id="loading-image" src="images/spin.gif" alt="Yükleniyor..." />
    </div>
    <ul>
        <li><a href="transferlogin.aspx">Geri</a></li>
    </ul>

    <table class="maintable">
        <tr>
            <td style="width: 15%;"><strong>Sip.No: </strong></td>
            <td style="width: 65%;">
                <asp:TextBox ID="txtSalesOrderNumber" runat="server" Width="95%" placeholder="Sipariş No"></asp:TextBox>
            </td>
            <td style="width: 20%;">
                <asp:Button ID="btnReport" ClientIDMode="Static" runat="server" Text="Rapor" Width="100%" Font-Size="Medium" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" OnClick="btnReport_Click" />
            </td>
        </tr>
    </table>

    <asp:Repeater ID="rptRecords" runat="server" DataSourceID="SqlDataSource1">
        <HeaderTemplate>
            <table class="table-fill">
                <thead>
                    <tr>
                        <th style="width: 15%;">Kalem No</th>
                        <th style="width: 20%;">Depo</th>
                        <th style="width: 20%;">Hücre</th>
                        <th style="width: 25%;">Barkod</th>
                        <th style="width: 20%;">Miktar</th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%#Eval("LineNumber") %></td>
                <td><%#Eval("Warehouse") %></td>
                <td><%#Eval("WarehouseCell") %></td>
                <td><%#Eval("Barcode")%></td>
                <td><%#Eval("cQuantity", "{0:0.00}") %></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>

        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select D.LineNumber, W.Code as Warehouse, WC.Name as WarehouseCell, S.Barcode, S.cQuantity from Store S inner join Warehouse W on W.Oid = S.Warehouse left outer join WarehouseCell WC on WC.Oid = S.WarehouseCell inner join SalesOrderDetail D on D.Oid = S.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder where S.GCRecord is null and O.OrderNumber = @salesOrderNumber order by D.LineNumber, S.Barcode" OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:Parameter Name="salesOrderNumber" DbType="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
