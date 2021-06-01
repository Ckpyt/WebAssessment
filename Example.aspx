<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Example.aspx.cs" Inherits="_Default2" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title></title>
    <link href="css/style.css" rel="stylesheet" />
    <script type="text/javascript">
        function ShowModal(id) {
            document.getElementById("overlay-" + id).style.height = document.body.clientHeight + 'px';
            document.getElementById("overlay-" + id).className = "OverlayEffect";
            document.getElementById("modalMsg-" + id).className = "ShowModal";
        }

        function RemoveModal(id) {
            document.getElementById("overlay-" + id).style.height = "0px";
            document.getElementById("overlay-" + id).className = "";
            document.getElementById("modalMsg-" + id).className = "HideModal";
        }
    </script>
        <style>
        .OverlayEffect {
            background-color: black;
            filter: alpha(opacity=70);
            opacity: 0.7;
            width: 100%;
            height: 100%;
            z-index: 400;
            position: absolute;
            top: 0;
            left: 0;
        }

        .HideModal {
            display: none;
        }

        .modalPopup {
            z-index: 1 !important;
        }

        .ShowModal {
            top: 200px;
            z-index: 1000;
            position: absolute;
            background-color: lightblue;
            text-align: center;
            width: 300px;
            height: 200px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            Default form <br/>
            <asp:Repeater ID="rptFriends" runat="server">
                <ItemTemplate>
                    <table>
                        <tr>
                            <td>Name
                            </td>
                            <td>
                                <%# Eval("Name") %>
                            </td>
                        </tr>
                        <tr>
                            <td>Place
                            </td>
                            <td>
                                <%# Eval("Place") %>
                            </td>
                        </tr>
                        <tr>
                            <td>Mobile
                            </td>
                            <td>
                                <%# Eval("Mobile") %>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2">
                                <input id="btnDeleteClient" type="button" value="Delete" onclick="ShowModal('<%# Eval("FriendID")%>')" />
                            </td>

                        </tr>
                    </table>
                    <%--Start Delete Modal Popup--%>

                    <div id='overlay-<%# Eval("FriendID")%>'></div>
                    <div id='modalMsg-<%# Eval("FriendID")%>' class="HideModal">

                        <div class="popupbox">
                            <br />
                            <div class="popupttl bluetxt"><b>Delete Confirmation</b></div>
                            <hr />
                            <br />
                            <br />
                            <div class="popupdesc">
                                <!-- START POPUP BLOCK -->
                                <div class="popblock">
                                    <div class="poptxt">
                                        Are you sure you want to delete?
                                    </div>
                                </div>
                                <!-- END POPUP BLOCK -->
                                <!-- START POPUP BLOCK -->
                                <br />
                                <div class="popblock popblocksubbtn">
                                    <asp:Button ID="btnDelete" runat="server" Text="Yes" CausesValidation="False" CommandName="DeleteItem" />
                                    <input type="submit" value="No" class="button bluebg ibvm" onclick="RemoveModal('<%# Eval("FriendID")%>    ')" />
                                    <asp:Button ID="btnHide" runat="server" Text="Back" Style="display: none" />
                                </div>
                                <!-- END POPUP BLOCK -->
                            </div>
                        </div>
                    </div>

                    <%--End of Delete Modal Poup--%>
                </ItemTemplate>
                <SeparatorTemplate>
                    <hr />
                </SeparatorTemplate>
            </asp:Repeater>

        </div>
    </form>
</body>
</html>
