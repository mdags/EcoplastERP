<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="shippingtransfer.aspx.cs" Inherits="EcoplastERP.PTC.shippingtransfer" %>

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

        function CheckDelivery() {
            $('#loading').show();
            PageMethods.CheckDelivery(form1.txtExpeditionNumber.value, form1.txtDeliveryNumber.value, ProcessEndForCheckDelivery);
        }
        function ProcessEndForCheckDelivery(result) {
            if (result == '') {
                document.getElementById("lblerror").innerHTML = '';
                GetLoadingInfo();
                $('#txtBarcode').focus();
            }
            else {
                document.getElementById("lblerror").innerHTML = result;
                $('#txtDeliveryNumber').value = '';
                $('#txtDeliveryNumber').focus();
            }
            $('#loading').hide();
        }

        function GetLoadingInfo() {
            PageMethods.GetLoading(form1.txtDeliveryNumber.value, ProcessEndForGetLoading);
        }
        function ProcessEndForGetLoading(result) {
            document.getElementById('lblTotalPaletteCount').innerHTML = result[0].PaletteCount;
            document.getElementById('lblTotalBobbinCount').innerHTML = result[0].BobbinCount;
            document.getElementById('lblTotalQuantity').innerHTML = result[0].Quantity;
            $('#loading').hide();
        }

        function GetBarcodeInfo() {
            $('#loading').show();
            PageMethods.GetBarcode(form1.txtBarcode.value, ProcessEndForGetBarcode);
        }
        function ProcessEndForGetBarcode(result) {
            document.getElementById('lblBobbinCount').innerHTML = result[0].BobbinCount;
            document.getElementById('lblGrossQuantity').innerHTML = result[0].GrossQuantity;
            $('#loading').hide();
        }

        function BeginTransfer() {
            $('#loading').show();
            PageMethods.BeginTransfer(form1.txtDeliveryNumber.value, form1.txtBarcode.value, ProcessEndForBeginTransfer);
        }
        function ProcessEndForBeginTransfer(result) {
            if (result == '') {
                soundObject = document.createElement("embed");
                soundObject.setAttribute("src", "sounds/ding.wav");
                soundObject.setAttribute("hidden", true);
                soundObject.setAttribute("autostart", true);
                document.body.appendChild(soundObject);
                document.getElementById("lblerror").innerHTML = '';
                document.getElementById('lblBobbinCount').innerHTML = '';
                document.getElementById('lblGrossQuantity').innerHTML = '';
            }
            else {
                soundObject = document.createElement("embed");
                soundObject.setAttribute("src", "sounds/error.wav");
                soundObject.setAttribute("hidden", true);
                soundObject.setAttribute("autostart", true);
                document.body.appendChild(soundObject);
                document.getElementById("lblerror").innerHTML = result;
            }

            document.getElementById("txtBarcode").value = '';
            $('#txtBarcode').focus();
            GetLoadingInfo();
            $('#loading').hide();
        }

        function readedList() {
            if (document.getElementById("txtExpeditionNumber").value !== '') {
                if (document.getElementById("txtDeliveryNumber").value !== '') {
                    document.getElementById("lblerror").innerHTML = '';
                    window.location.href = 'shippingreadedlist.aspx?exp=' + document.getElementById("txtExpeditionNumber").value + '&dlv=' + document.getElementById("txtDeliveryNumber").value;
                }
                else {
                    document.getElementById("lblerror").innerHTML = 'Teslim No boş olamaz.';
                }
            }
            else {
                document.getElementById("lblerror").innerHTML = 'Sefer No boş olamaz.';
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
            <td style="width: 15%;">Sefer No :</td>
            <td style="width: 85%;">
                <input id="txtExpeditionNumber" type="text" onkeydown="if (event.keyCode == 13) { $('#txtDeliveryNumber').focus(); return false; }" style="width: 99%;" /></td>
        </tr>
        <tr>
            <td>Teslimat No :</td>
            <td>
                <input id="txtDeliveryNumber" type="text" onkeydown="if (event.keyCode == 13) { CheckDelivery(); return false; }" style="width: 99%;" /></td>
        </tr>
        <tr>
            <td></td>
            <td><b>Toplam Okutulan</b></td>
        </tr>
        <tr>
            <td><b>Top.Palet :</b></td>
            <td>
                <asp:Label ID="lblTotalPaletteCount" ClientIDMode="Static" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td><b>Top.Bobin :</b></td>
            <td>
                <asp:Label ID="lblTotalBobbinCount" ClientIDMode="Static" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td><b>Top.Miktar :</b></td>
            <td>
                <asp:Label ID="lblTotalQuantity" ClientIDMode="Static" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td>Barkod :</td>
            <td>
                <input id="txtBarcode" type="text" onkeydown="if (event.keyCode == 13) { GetBarcodeInfo(); return false; }" style="width: 99%;" /></td>
        </tr>
        <tr>
            <td></td>
            <td><b>Barkod Bilgisi</b></td>
        </tr>
        <tr>
            <td><b>Bobin Say. :</b></td>
            <td>
                <asp:Label ID="lblBobbinCount" ClientIDMode="Static" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td><b>Miktar :</b></td>
            <td>
                <asp:Label ID="lblGrossQuantity" ClientIDMode="Static" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>

    <label id="lblerror" class="errortext"></label>

    <asp:Button ID="btnSave" ClientIDMode="Static" runat="server" Text="Kaydet" Width="50%" Height="30px" Font-Size="Medium" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" Style="float: right;" OnClientClick="BeginTransfer(); return false;" />

    <br />
    <br />
    <hr />
    <br />
    <br />

    <asp:Button ID="btnReadedList" ClientIDMode="Static" runat="server" Text="Okutulanlar Listesi" Width="100%" Height="30px" Font-Size="Medium" Font-Bold="true" ForeColor="#ffffff" BackColor="#5384BE" BorderStyle="Solid" BorderColor="#4980C1" OnClientClick="readedList(); return false;" />
</asp:Content>
