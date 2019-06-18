<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" EnableEventValidation="false" AutoEventWireup="true" CodeBehind="admin.aspx.cs" Inherits="WebAssessment.admin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
      <!--================ Hero sm Banner start =================-->      
    
    <script type="text/javascript">
        function DisplayModal(id) {
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
      <div class="hero-banner--sm__content">
        <h1>Admin page</h1>
        <nav aria-label="breadcrumb" class="banner-breadcrumb">
          <ol class="breadcrumb">
            <li class="breadcrumb-item"><a href="index.aspx">Home</a></li>
            <li class="breadcrumb-item active" aria-current="page">Admin page</li>
          </ol>
        </nav>
      </div>

  <!--================ Hero sm Banner end =================-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="container centered-element">
    <asp:Repeater ID="rptUsers" runat="server" OnItemCommand="rptUsers_ItemCommand">
    
                <ItemTemplate>
                    <div class="container centered-element">
                    <table class="centered-element">
                        <tr>
                            <td>Name
                            </td>
                            <td>
                                <%# Eval("UserName") %>
                            </td>
                        </tr>
                        <tr>
                            <td>Place
                            </td>
                            <td>
                                <%# Eval("Email") %>
                            </td>
                        </tr>
                        <tr>
                            <td>Desription:
                            </td>
                            
                        </tr>
                        <tr>
                            <td>
                                <%# Eval("Description") %>
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2">
                                <input id="btnDeleteClient"  class="button blue-btn  ibvm" type="button" value="Delete" onclick="DisplayModal('<%# Eval("Id")%>')" />
                            </td>

                        </tr>
                    </table>
                    </div>
                    <%--Start Delete Modal Popup--%>

                    <div id='overlay-<%# Eval("Id")%>'></div>
                    <div id='modalMsg-<%# Eval("Id")%>' class="HideModal">

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
                                    <asp:Button  class="button blue-btn  ibvm" CausesValidation="False" CommandArgument='<%#Eval("Id")%>' ID="btnDelete" runat="server"  Text="Yes" OnClick="ConfirmButton_Click" />
                                    <input type="submit" value="No" class="button bluebg ibvm" onclick="RemoveModal('<%# Eval("Id")%>    ')" />
                                    <asp:Button  class="button blue-btn  ibvm" ID="btnHide" runat="server" Text="Back" Style="display: none" />
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
</asp:Content>
