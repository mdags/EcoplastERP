<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="transferwarehouse.aspx.cs" Inherits="EcoplastERP.PTC.transferwarehouse" %>

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
            $('#txtWarehouse').focus();
        });
        function GetWarehouseInfo() {
            PageMethods.GetWarehouseInfo(form1.txtWarehouse.value, ProcessEndForGetWarehouseInfo);
        }
        function ProcessEndForGetWarehouseInfo(result) {
            if (result == '') {
                $('#txtWarehouseCell').focus();
                document.getElementById("lblerror").innerHTML = '';
            }
            else {
                form1.txtWarehouse.value = '';
                document.getElementById('lblerror').innerHTML = result;
            }
        }
        function GetWarehouseCellInfo() {
            $('#loading').show();
            PageMethods.GetWarehouseCellInfo(form1.txtWarehouseCell.value, ProcessEndForGetWarehouseCellInfo);
        }
        function ProcessEndForGetWarehouseCellInfo(result) {
            if (result[0].ErrorMessage == '') {
                $('#txtBarcode').focus();
                form1.txtWarehouse.value = result[0].WarehouseCode;
                document.getElementById("lblerror").innerHTML = '';
            }
            else {
                form1.txtWarehouseCell.value = '';
                document.getElementById('lblerror').innerHTML = result[0].ErrorMessage;
            }
            $('#loading').hide();
        }
        function GetBarcodeInfo() {
            PageMethods.BarcodeGrossInfo(form1.txtBarcode.value, ProcessEndForGrossQuantity);
            PageMethods.BarcodeNetInfo(form1.txtBarcode.value, ProcessEndForNetQuantity);
        }
        function ProcessEndForGrossQuantity(result) {
            document.getElementById('lblGrossQuantity').innerHTML = result;
        }
        function ProcessEndForNetQuantity(result) {
            document.getElementById('lblNetQuantity').innerHTML = result;
        }
        function BeginTransfer() {
            $('#loading').show();
            PageMethods.BeginTransfer(form1.txtWarehouse.value, form1.txtWarehouseCell.value, form1.txtBarcode.value, ProcessEndForBeginTransfer);
        }
        function ProcessEndForBeginTransfer(result) {
            if (result == '') {
                soundObject = document.createElement("embed");
                soundObject.setAttribute("src", "sounds/ding.wav");
                soundObject.setAttribute("hidden", true);
                soundObject.setAttribute("autostart", true);
                document.body.appendChild(soundObject);
                document.getElementById("lblerror").innerHTML = '';
            }
            else {
                soundObject = document.createElement("embed");
                soundObject.setAttribute("src", "sounds/error.wav");
                soundObject.setAttribute("hidden", true);
                soundObject.setAttribute("autostart", true);
                document.body.appendChild(soundObject);
                document.getElementById("lblerror").innerHTML = result;
            }

            $('#loading').hide();
            document.getElementById('txtBarcode').value = '';
            document.getElementById('lblGrossQuantity').innerHTML = '0,00';
            document.getElementById('lblNetQuantity').innerHTML = '0,00';
            $('#txtBarcode').focus();
        }
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
            <td style="width: 15%;"><strong>Depo: </strong></td>
            <td style="width: 85%;">
                <input id="txtWarehouse" type="text" onkeydown="if (event.keyCode == 13) { GetWarehouseInfo(); return false; }" style="width: 99%" />
            </td>
        </tr>
        <tr>
            <td><strong>Hücre: </strong></td>
            <td>
                <input id="txtWarehouseCell" type="text" onkeydown="if (event.keyCode == 13) { GetWarehouseCellInfo(); return false; }" style="width: 99%" />
            </td>
        </tr>
        <tr>
            <td><strong>Barkod: </strong></td>
            <td>
                <input id="txtBarcode" type="text" onkeydown="if (event.keyCode == 13) { GetBarcodeInfo(); return false; }" style="width: 99%" />
            </td>
        </tr>
        <tr>
            <td>Miktar: </td>
            <td>
                <asp:Label ID="lblGrossQuantity" ClientIDMode="Static" runat="server" Font-Size="Small"></asp:Label></td>
        </tr>
        <tr>
            <td>Çev.Mik.: </td>
            <td>
                <asp:Label ID="lblNetQuantity" ClientIDMode="Static" runat="server" Font-Size="Small"></asp:Label></td>
        </tr>
    </table>

    <label id="lblerror" class="errortext"></label>

    <br />

    <asp:Button ID="btnSave" ClientIDMode="Static" runat="server" Text="Kaydet" Width="50%" Height="30px" Font-Size="Medium" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" Style="float: right;" OnClientClick="BeginTransfer(); return false;" />

    <br />
    <br />
    <hr />
    <br />
    <br />

    <asp:Button ID="btnSalesOrderReport" ClientIDMode="Static" runat="server" Text="Sipariş Raporu" Width="100%" Height="30px" Font-Size="Medium" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" PostBackUrl="~/transfersalesorderreport.aspx" />
</asp:Content>
