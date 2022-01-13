<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="shippingtruck.aspx.cs" Inherits="EcoplastERP.PTC.shippingtruck" %>

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
        $(document).ready(function () {
            $('#txtExpeditionNumber').focus();
        });
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
            <td style="width: 15%;">Sefer No:</td>
            <td style="width: 85%;">
                <asp:TextBox runat="server" ID="txtExpeditionNumber" ClientIDMode="Static" placeholder="sefer no" Width="99%" onkeydown="if (event.keyCode == 13) { $('#txtTruckPlate').focus(); return false; }"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Kamyon Plaka:</td>
            <td>
                <asp:TextBox runat="server" ID="txtTruckPlate" ClientIDMode="Static" placeholder="kamyon plaka" Width="99%" MaxLength="10" Style="text-transform: uppercase;" onkeydown="if (event.keyCode == 13) { $('#txtDorsePlate').focus(); return false; }"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Dorse Plaka:</td>
            <td>
                <asp:TextBox runat="server" ID="txtDorsePlate" ClientIDMode="Static" Style="text-transform: uppercase;" MaxLength="10" placeholder="dorse plaka" Width="99%"></asp:TextBox></td>
        </tr>
    </table>
    <br>
    <asp:Label runat="server" ID="lblerror" class="errortext" Visible="false"></asp:Label>
    <br>
    <asp:Button ID="btnSave" runat="server" Text="Kaydet" Width="100%" Height="50px" Font-Size="Small" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" OnClick="btnSave_Click" />
</asp:Content>
