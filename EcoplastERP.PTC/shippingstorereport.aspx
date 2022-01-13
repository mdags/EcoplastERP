<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="shippingstorereport.aspx.cs" Inherits="EcoplastERP.PTC.shippingstorereport" %>

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
        <li><a href="shippingstorereporthome.aspx">Geri</a></li>
    </ul>

    <asp:Repeater ID="rptRecords" runat="server" DataSourceID="SqlDataSource1">
        <HeaderTemplate>
            <table class="table-fill">
                <thead>
                    <tr>
                        <th style="width: 15%;">Sipariş No</th>
                        <th style="width: 15%;">Malzeme</th>
                        <th style="width: 40%;">Firma</th>
                        <th style="width: 15%;">#</th>
                        <th style="width: 15%;">Miktar</th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%#Eval("OrderNumber") %></td>
                <td><%#Eval("Product")%></td>
                <td><%#Eval("Contact") %></td>
                <td><%#Eval("Quantity", "{0:0.00}") %></td>
                <td>
                    <asp:Button ID="btnDetail" runat="server" Text="Detay" Font-Size="Medium" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" CommandArgument='<%#Eval("OrderNumber")%>' OnClick="btnDetail_Click" />
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select sum(L.Quantity) as Quantity, O.OrderNumber+'/'+cast(Sd.LineNumber as varchar(5)) as OrderNumber, P.Name as Product, C.Name as Contact from DeliveryDetailLoading L inner join DeliveryDetail Dd on Dd.Oid = L.DeliveryDetail inner join Delivery D on D.Oid = Dd.Delivery inner join Expedition E on E.Oid = D.Expedition inner join SalesOrderDetail Sd on Sd.Oid = Dd.SalesOrderDetail inner join SalesOrder O on O.Oid = Sd.SalesOrder inner join Product P on P.Oid = Sd.Product inner join Contact C on C.Oid = O.Contact where L.GCRecord is null and E.ExpeditionNumber = @documentNumber group by O.OrderNumber, Sd.LineNumber, P.Name, C.Name" OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:Parameter Name="documentNumber" DbType="String" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select sum(L.Quantity) as Quantity, O.OrderNumber+'/'+cast(Sd.LineNumber as varchar(5)) as OrderNumber, P.Name as Product, C.Name as Contact from DeliveryDetailLoading L inner join DeliveryDetail Dd on Dd.Oid = L.DeliveryDetail inner join Delivery D on D.Oid = Dd.Delivery inner join SalesOrderDetail Sd on Sd.Oid = Dd.SalesOrderDetail inner join SalesOrder O on O.Oid = Sd.SalesOrder inner join Product P on P.Oid = Sd.Product inner join Contact C on C.Oid = O.Contact where L.GCRecord is null and D.DeliveryNumber = @documentNumber group by O.OrderNumber, Sd.LineNumber, P.Name, C.Name" OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:Parameter Name="documentNumber" DbType="String" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select D.Quantity, O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as OrderNumber, P.Name as Product, C.Name as Contact from SalesOrderDetail D inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Product P on P.Oid = D.Product where D.GCRecord is null and O.OrderNumber = @documentNumber" OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:Parameter Name="documentNumber" DbType="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
