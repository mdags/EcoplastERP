<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="shippingfastreadedlist.aspx.cs" Inherits="EcoplastERP.PTC.shippingfastreadedlist" %>
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
        <li><a href="shippingfast.aspx">Geri</a></li>
    </ul>

    <asp:RadioButtonList ID="rblReportType2" runat="server" RepeatDirection="Horizontal" RepeatColumns="2" Width="100%" Font-Size="Large" AutoPostBack="true" OnSelectedIndexChanged="rblReportType2_SelectedIndexChanged">
        <asp:ListItem Value="palette" Selected="True">Palet Bazında</asp:ListItem>
        <asp:ListItem Value="barcode">Barkod Bazında</asp:ListItem>
    </asp:RadioButtonList>

    <asp:Repeater ID="rptRecords" runat="server" DataSourceID="SqlDataSource1">
        <HeaderTemplate>
            <table class="table-fill">
                <thead>
                    <tr>
                        <th style="width: 15%;">Sipariş No</th>
                        <th style="width: 15%;">Barkod</th>
                        <th style="width: 15%;">Brüt</th>
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

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as OrderNumber, F.PaletteNumber as Barcode, isnull(sum(F.Quantity), 0) as Quantity, isnull(Avg(PP.Tare), 0) as Tare, C.Name as Contact from FastShipping F inner join Production P on P.Oid = F.Production inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact left outer join ProductionPalette PP on PP.PaletteNumber = F.PaletteNumber where F.GCRecord is null group by O.OrderNumber, D.LineNumber, F.PaletteNumber, C.Name">
    </asp:SqlDataSource>

    <asp:SqlDataSource ID="SqlDataSource2" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select O.OrderNumber+'/'+cast(D.LineNumber as varchar(5)) as OrderNumber, F.Barcode, F.Quantity, PP.Tare, C.Name as Contact from FastShipping F inner join Production P on P.Oid = F.Production inner join SalesOrderDetail D on D.Oid = P.SalesOrderDetail inner join SalesOrder O on O.Oid = D.SalesOrder inner join Contact C on C.Oid = O.Contact left outer join ProductionPalette PP on PP.PaletteNumber = F.PaletteNumber where F.GCRecord is null">
    </asp:SqlDataSource>
</asp:Content>
