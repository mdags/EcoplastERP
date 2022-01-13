<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="shippinghome.aspx.cs" Inherits="EcoplastERP.PTC.shippinghome" %>

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
        <li><a href="shippinglogin.aspx">Geri</a></li>
    </ul>

    <h3>SEVKİYAT</h3>
    <table class="maintable">
        <tr>
            <td>
                <asp:Button ID="btnShippingList" runat="server" Text="Sevkiyat" Width="100%" Height="50px" Font-Size="Small" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" PostBackUrl="~/shippingtransfer.aspx" /></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnShippingFast" runat="server" Text="Hızlı Sevkiyat" Width="100%" Height="50px" Font-Size="Small" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" PostBackUrl="~/shippingfast.aspx" /></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnShippingTruck" runat="server" Text="Sefer Kamyon Atama" Width="100%" Height="50px" Font-Size="Small" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" PostBackUrl="~/shippingtruck.aspx" /></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnShippingEnd" runat="server" Text="Teslimat Kapat" Width="100%" Height="50px" Font-Size="Small" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" PostBackUrl="~/shippingcomplete.aspx" /></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnShippingAddPalette" runat="server" Text="Palet Transfer" Width="100%" Height="50px" Font-Size="Small" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" PostBackUrl="~/shippingpalettetransfer.aspx" /></td>
        </tr>
    </table>
    <h3>RAPORLAR</h3>
    <table class="maintable">
        <tr>
            <td>
                <asp:Button ID="btnShippingStoreReport" runat="server" Text="Stok Durum Durumu" Width="100%" Height="50px" Font-Size="Small" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" PostBackUrl="~/shippingstorereporthome.aspx" /></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnShippingFindOrder" runat="server" Text="Sipariş Durumu" Width="100%" Height="50px" Font-Size="Small" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" PostBackUrl="~/shippingfindorder.aspx" /></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnShippingFindPalette" runat="server" Text="Palet Takip" Width="100%" Height="50px" Font-Size="Small" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" PostBackUrl="~/shippingfindpalette.aspx" /></td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnShippingFindBarcode" runat="server" Text="Barkod Takip" Width="100%" Height="50px" Font-Size="Small" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" PostBackUrl="~/shippingfindbarcode.aspx" /></td>
        </tr>
    </table>
</asp:Content>
