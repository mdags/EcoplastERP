<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="shippingfindbarcode.aspx.cs" Inherits="EcoplastERP.PTC.shippingfindbarcode" %>

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
        <li><a href="shippinghome.aspx">Geri</a></li>
    </ul>

    <table class="maintable">
        <tr>
            <td style="width: 15%;"><strong>Barkod: </strong></td>
            <td style="width: 75%;">
                <%--<input runat="server" id="txtBarcode" type="text" onkeydown="if (event.keyCode == 13) { GetBarcodeInfo(); return false; }" style="width: 99%" />--%>
                <asp:TextBox runat="server" ID="txtBarcode" Width="100%"></asp:TextBox>
            </td>
            <td style="width: 10%;">
                <asp:Button runat="server" ID="btnGet" Text="Getir" Width="95%" Font-Size="Small" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" OnClick="btnGet_Click" />
            </td>
        </tr>
        <tr>
            <td><strong>Palet No: </strong></td>
            <td>
                <label runat="server" id="lblPaletteNumber"></label>
            </td>
        </tr>
        <tr>
            <td><strong>Sipariş No: </strong></td>
            <td>
                <label runat="server" id="lblOrderNumber"></label>
            </td>
        </tr>
        <tr>
            <td><strong>Müşteri: </strong></td>
            <td>
                <label runat="server" id="lblContactName"></label>
            </td>
        </tr>
        <tr>
            <td><strong>Stok Adı: </strong></td>
            <td>
                <label runat="server" id="lblProductName"></label>
            </td>
        </tr>
        <tr>
            <td><strong>Brüt: </strong></td>
            <td>
                <label runat="server" id="lblGrossQuantity"></label>
            </td>
        </tr>
        <tr>
            <td><strong>Net: </strong></td>
            <td>
                <label runat="server" id="lblNetQuantity"></label>
            </td>
        </tr>
        <tr>
            <td><strong>Operatör: </strong></td>
            <td>
                <label runat="server" id="lblOperatorName"></label>
            </td>
        </tr>
        <tr>
            <td><strong>Üretim Tarihi: </strong></td>
            <td>
                <label runat="server" id="lblProductionDate"></label>
            </td>
        </tr>
    </table>

    <asp:Repeater ID="rptRecords" runat="server" DataSourceID="SqlDataSource1">
        <HeaderTemplate>
            <table class="table-fill">
                <thead>
                    <tr>
                        <th style="width: 20%;">Tarih</th>
                        <th style="width: 40%;">Har.Türü</th>
                        <th style="width: 25%;">Depo</th>
                        <th style="width: 15%;">Miktar</th>
                    </tr>
                </thead>
                <tbody>
        </HeaderTemplate>
        <ItemTemplate>
            <tr>
                <td><%#Eval("DocumentDate") %></td>
                <td><%#Eval("MovementTypeName")%></td>
                <td><%#Eval("WarehouseCode") %></td>
                <td><%#Eval("Quantity", "{0:0.00}") %></td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </tbody>
            </table>
        </FooterTemplate>
    </asp:Repeater>

    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="select M.DocumentDate, T.Name as MovementTypeName, W.Code as WarehouseCode, M.Quantity from Movement M inner join MovementType T on T.Oid = M.MovementType inner join Warehouse W on W.Oid = M.Warehouse where M.GCRecord is null and M.Barcode = @barcode order by M.DocumentDate desc" OnSelecting="SqlDataSource1_Selecting">
        <SelectParameters>
            <asp:Parameter Name="barcode" DbType="String" />
        </SelectParameters>
    </asp:SqlDataSource>
</asp:Content>
