<%@ Page Title="" Language="C#" MasterPageFile="~/Main.Master" AutoEventWireup="true" CodeBehind="shippingfindorder.aspx.cs" Inherits="EcoplastERP.PTC.shippingfindorder" %>

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
            $('#txtOrderNumber').focus();
        });
        function GetOrderInfo() {
            $('#loading').show();
            PageMethods.GetOrderInfo(form1.txtOrderNumber.value, form1.txtLineNumber.value, ProcessEndForGetOrder);
        }
        function ProcessEndForGetOrder(result) {
            document.getElementById('lblContactName').innerHTML = result[0].ContactName;
            document.getElementById('lblProductCode').innerHTML = result[0].ProductCode;
            document.getElementById('lblProductName').innerHTML = result[0].ProductName;
            document.getElementById('lblFilmingProduction').innerHTML = result[0].FilmingProduction;
            document.getElementById('lblPrintingProduction').innerHTML = result[0].PrintingProduction;
            document.getElementById('lblCuttingProduction').innerHTML = result[0].CuttingProduction;
            document.getElementById('lblShippingWarehouseQuantity').innerHTML = result[0].ShippingWarehouseQuantity;
            document.getElementById('lblLoadedQuantity').innerHTML = result[0].LoadedQuantity;
            $('#loading').hide();
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
            <td style="width: 15%;"><strong>Sipariş No: </strong></td>
            <td style="width: 85%;">
                <input id="txtOrderNumber" type="text" onkeydown="if (event.keyCode == 13) { $('#txtLineNumber').focus(); return false; }" style="width: 99%" />
            </td>
        </tr>
        <tr>
            <td><strong>Kalem No: </strong></td>
            <td>
                <input id="txtLineNumber" type="text" onkeydown="if (event.keyCode == 13) { GetOrderInfo(); return false; }" style="width: 99%" />
            </td>
        </tr>
        <tr>
            <td><strong>Müşteri: </strong></td>
            <td>
                <label id="lblContactName"></label>
            </td>
        </tr>
        <tr>
            <td><strong>Stok Kodu: </strong></td>
            <td>
                <label id="lblProductCode"></label>
            </td>
        </tr>
        <tr>
            <td><strong>Stok Adı: </strong></td>
            <td>
                <label id="lblProductName"></label>
            </td>
        </tr>
        <tr>
            <td><strong>Çekim Üret.: </strong></td>
            <td>
                <label id="lblFilmingProduction"></label>
            </td>
        </tr>
        <tr>
            <td><strong>Baskı Üret.: </strong></td>
            <td>
                <label id="lblPrintingProduction"></label>
            </td>
        </tr>
        <tr>
            <td><strong>Konf.Üret.: </strong></td>
            <td>
                <label id="lblCuttingProduction"></label>
            </td>
        </tr>
        <tr>
            <td><strong>Sevk Depo: </strong></td>
            <td>
                <label id="lblShippingWarehouseQuantity"></label>
            </td>
        </tr>
        <tr>
            <td><strong>Yüklenen: </strong></td>
            <td>
                <label id="lblLoadedQuantity"></label>
            </td>
        </tr>
    </table>
</asp:Content>
