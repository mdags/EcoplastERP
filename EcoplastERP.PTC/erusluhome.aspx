<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="erusluhome.aspx.cs" Inherits="EcoplastERP.PTC.erusluhome" %>

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
        <li><a href="default.aspx">Geri</a></li>
    </ul>

    <h3>ERUSLU</h3>
    <table class="maintable">
        <tr>
            <td>
                <asp:Button ID="btnProduction" runat="server" Text="Üretime Çıkış" Width="100%" Height="50px" Font-Size="Small" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" PostBackUrl="~/erusluproduction.aspx" /></td>
        </tr>
    </table>
</asp:Content>
