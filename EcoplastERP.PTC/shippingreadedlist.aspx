<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="shippingreadedlist.aspx.cs" Inherits="EcoplastERP.PTC.shippingreadedlist" %>

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
        <li><a href="shippingtransfer.aspx">Geri</a></li>
    </ul>

    <asp:RadioButtonList ID="rblReportType1" runat="server" RepeatDirection="Horizontal" Width="100%" Font-Size="Large" AutoPostBack="true" OnSelectedIndexChanged="rblReportType1_SelectedIndexChanged">
        <asp:ListItem Value="expedition">Sevkiyat Belgesi Bazında</asp:ListItem>
        <asp:ListItem Value="delivery" Selected="True">Teslim Belgesi Bazında</asp:ListItem>
    </asp:RadioButtonList>
    <asp:RadioButtonList ID="rblReportType2" runat="server" RepeatDirection="Horizontal" RepeatColumns="2" Width="100%" Font-Size="Large" AutoPostBack="true" OnSelectedIndexChanged="rblReportType2_SelectedIndexChanged">
        <asp:ListItem Value="palette" Selected="True">Palet Bazında</asp:ListItem>
        <asp:ListItem Value="barcode">Barkod Bazında</asp:ListItem>
    </asp:RadioButtonList>

    <asp:Repeater ID="rptRecords" runat="server" DataSourceID="SqlDataSource3">
        <HeaderTemplate>
            <table class="table-fill">
                <thead>
                    <tr>
                        <th style="width: 15%;">Sipariş No</th>
                        <th style="width: 15%;">Barkod</th>
                        <th style="width: 15%;">Brüt</th>
                        <%--<th style="width: 15%;">Dara</th>--%>
                        <th style="width: 40%;">Müşteri</th>
                        <th style="width: 15%;">#</th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%#Eval("OrderNumber") %></td>
                <td><%#Eval("Barcode")%></td>
                <td><%#Eval("Quantity", "{0:0.00}") %></td>
                <%--<td><%#Eval("Tare") %></td>--%>
                <td><%#Eval("Contact") %></td>
                <td>
                    <asp:Button ID="btnDelete" runat="server" Text="Sil" CommandArgument='<%#Eval("Barcode")%>' OnClientClick="return confirm('Kayıt silinecek onaylıyor musunuz?');" OnClick="btnDelete_Click" />
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select O.OrderNumber+'/'+cast(Sd.LineNumber as varchar(5)) as OrderNumber, L.PaletteNumber as Barcode, isnull(SUM(L.Quantity), 0) as Quantity, isnull(Pp.Tare, 0) as Tare, SUBSTRING(C.Name, 0, CHARINDEX(' ', C.Name, 0) + 1) as Contact from DeliveryDetailLoading L inner join DeliveryDetail Dd on Dd.Oid = L.DeliveryDetail inner join Delivery D on D.Oid = Dd.Delivery inner join Expedition E on E.Oid = D.Expedition inner join SalesOrderDetail Sd on Sd.Oid = Dd.SalesOrderDetail inner join SalesOrder O on O.Oid = Sd.SalesOrder inner join Contact C on C.Oid = O.Contact left outer join ProductionPalette Pp on Pp.PaletteNumber = L.PaletteNumber where L.GCRecord is null and E.ExpeditionNumber = @expeditionNumber group by O.OrderNumber, Sd.LineNumber, L.PaletteNumber, Pp.Tare, C.Name" OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:Parameter Name="expeditionNumber" DbType="String" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select O.OrderNumber+'/'+cast(Sd.LineNumber as varchar(5)) as OrderNumber, L.Barcode, isnull(SUM(L.Quantity), 0) as Quantity, isnull(Pp.Tare, 0) as Tare, SUBSTRING(C.Name, 0, CHARINDEX(' ', C.Name, 0) + 1) as Contact from DeliveryDetailLoading L inner join DeliveryDetail Dd on Dd.Oid = L.DeliveryDetail inner join Delivery D on D.Oid = Dd.Delivery inner join Expedition E on E.Oid = D.Expedition inner join SalesOrderDetail Sd on Sd.Oid = Dd.SalesOrderDetail inner join SalesOrder O on O.Oid = Sd.SalesOrder inner join Contact C on C.Oid = O.Contact left outer join ProductionPalette Pp on Pp.PaletteNumber = L.PaletteNumber where L.GCRecord is null and E.ExpeditionNumber = @expeditionNumber group by O.OrderNumber, Sd.LineNumber, L.Barcode, Pp.Tare, C.Name" OnSelecting="SqlDataSource2_Selecting">
        <SelectParameters>
            <asp:Parameter Name="expeditionNumber" DbType="String" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSource3" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select O.OrderNumber+'/'+cast(Sd.LineNumber as varchar(5)) as OrderNumber, L.PaletteNumber as Barcode, isnull(SUM(L.Quantity), 0) as Quantity, isnull(Pp.Tare, 0) as Tare, SUBSTRING(C.Name, 0, CHARINDEX(' ', C.Name, 0) + 1) as Contact from DeliveryDetailLoading L inner join DeliveryDetail Dd on Dd.Oid = L.DeliveryDetail inner join Delivery D on D.Oid = Dd.Delivery inner join SalesOrderDetail Sd on Sd.Oid = Dd.SalesOrderDetail inner join SalesOrder O on O.Oid = Sd.SalesOrder inner join Contact C on C.Oid = O.Contact left outer join ProductionPalette Pp on Pp.PaletteNumber = L.PaletteNumber where L.GCRecord is null and D.DeliveryNumber = @deliveryNumber group by O.OrderNumber, Sd.LineNumber, L.PaletteNumber, Pp.Tare, C.Name" OnSelecting="SqlDataSource3_Selecting">
        <SelectParameters>
            <asp:Parameter Name="deliveryNumber" DbType="String" />
        </SelectParameters>
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSource4" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select O.OrderNumber+'/'+cast(Sd.LineNumber as varchar(5)) as OrderNumber, L.Barcode, isnull(SUM(S.Quantity), 0) as Quantity, SUBSTRING(C.Name, 0, CHARINDEX(' ', C.Name, 0) + 1) as Contact from DeliveryDetailLoading L inner join DeliveryDetail Dd on Dd.Oid = L.DeliveryDetail inner join Delivery D on D.Oid = Dd.Delivery inner join SalesOrderDetail Sd on Sd.Oid = Dd.SalesOrderDetail inner join SalesOrder O on O.Oid = Sd.SalesOrder inner join Contact C on C.Oid = O.Contact inner join Store S on S.Barcode = L.Barcode where L.GCRecord is null and D.DeliveryNumber = @deliveryNumber group by O.OrderNumber, Sd.LineNumber, L.Barcode, C.Name" OnSelecting="SqlDataSource4_Selecting">
        <SelectParameters>
            <asp:Parameter Name="deliveryNumber" DbType="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
