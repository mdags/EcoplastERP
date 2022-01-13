<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="shippingcomplete.aspx.cs" Inherits="EcoplastERP.PTC.shippingcomplete" %>

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

        function CheckExpedition() {
            PageMethods.CheckExpedition(form1.txtExpeditionNumber.value, ProcessEndForCheckExpedition);
        }
        function ProcessEndForCheckExpedition(result) {
            if (result == '') {
                document.getElementById("lblerror").innerHTML = '';
                $('#txtDeliveryNumber').focus();
            }
            else {
                document.getElementById("lblerror").innerHTML = result;
                document.getElementById("txtExpeditionNumber").value = '';
                $('#txtExpeditionNumber').focus();
            }
        }

        function CheckDelivery() {
            PageMethods.CheckDelivery(form1.txtExpeditionNumber.value, form1.txtDeliveryNumber.value, ProcessEndForCheckDelivery);
        }
        function ProcessEndForCheckDelivery(result) {
            if (result == '') {
                document.getElementById("lblerror").innerHTML = '';
            }
            else {
                document.getElementById("lblerror").innerHTML = result;
                document.getElementById("txtDeliveryNumber").value = '';
                $('#txtDeliveryNumber').focus();
            }
        }
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
            <td style="width: 15%;">Sefer No:</td>
            <td style="width: 85%;">
                <asp:TextBox runat="server" ID="txtExpeditionNumber" ClientIDMode="Static" placeholder="sefer no" Width="99%" onkeydown="if (event.keyCode == 13) { CheckExpedition(); return false; }"></asp:TextBox></td>
        </tr>
        <tr>
            <td>Teslim No:</td>
            <td>
                <asp:TextBox runat="server" ID="txtDeliveryNumber" ClientIDMode="Static" placeholder="teslim no" Width="99%" MaxLength="11" onkeydown="if (event.keyCode == 13) { CheckDelivery(); return false; }"></asp:TextBox></td>
        </tr>
    </table>
    <br>
    <label id="lblerror" class="errortext"></label>
    <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
    <br>
    <asp:Button ID="btnUnComplete" ClientIDMode="Static" runat="server" Text="Teslimat Aç" Width="40%" Height="50px" Font-Size="Small" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" OnClick="btnUnComplete_Click" />
    <asp:Button ID="btnComplete" runat="server" Text="Teslimat Kapat" Width="40%" Height="50px" Font-Size="Small" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" Style="float: right;" OnClick="btnComplete_Click" />
</asp:Content>
